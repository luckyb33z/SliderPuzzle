using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#nullable enable

public class BoardManager : MonoBehaviour
{

    [SerializeField] Canvas? canvas;
    [SerializeField] GameObject? tilePrefab;
    [SerializeField] GameObject? emptyTilePrefab;
    [SerializeField] GameObject? boardPrefab;

    [SerializeField] TextMeshProUGUI? winnerText;

    private float tileSize = 1f;
    [SerializeField] private float tileScale = 1;
    [SerializeField] private int boardSize = 3;

    // private float boardDimensions = 1;

    [SerializeField] private float tileSpacing = 0.5f;
    [SerializeField] private float borderSpacing = 0.5f;

    private GameObject? emptyTile;

    private bool erasing = false;

    private Tile? highlightedTile;

    private IBoardBuilder? boardBuilder;

    [SerializeField] private GameObject? playingButtons;
    [SerializeField] private GameObject? victoryButtons;

    [SerializeField] private AudioButton? audioButton;
    [SerializeField] private AudioClip? tileMoveSound;
    [SerializeField] private AudioClip? victorySound;

    private int moveCount = 0;
    [SerializeField] TextMeshProUGUI? movesText;

    private bool _remaking = false;

    [SerializeField] private TimeKeeper? timeKeeper;

    bool _gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        StartGame(boardSize);
    }

    public void RestartGame(int newBoardSize)
    {
        timeKeeper!.Reset();
        victoryButtons!.SetActive(false);
        ResetMoveCount();
        winnerText!.gameObject.SetActive(false);
        _gameOver = false;
        boardSize = newBoardSize;
        EraseBoard();
    }

    private void StartGame(int newBoardSize)
    {
        if (boardBuilder == null)
        {
            boardBuilder = gameObject.AddComponent(typeof(RandomBoardBuilder)) as RandomBoardBuilder;
        }
        
        if (boardBuilder != null || _remaking) 
        {
            boardBuilder!.LinkUnityObjects(this, boardPrefab, tilePrefab, emptyTilePrefab, canvas);
            boardBuilder!.SetSizes(boardSize, tileSize, tileScale, tileSpacing, borderSpacing);
            boardBuilder!.SetupGame();
            playingButtons!.SetActive(true);
            _remaking = false;
        }
        else
        {
            Debug.Log("BoardBuilder is null.");
        }
    }

    public void SetHighlightedTile(Tile tile)
    {
        highlightedTile = tile;
    }

    private void EraseBoard()
    {
        erasing = true;
        if (boardBuilder?.GameBoard != null)
        {
            Destroy(boardBuilder.GameBoard);
            _remaking = true;
        }
    }

    enum Dir
    {
        Up,
        Down,
        Left,
        Right
    }

    // Update is called once per frame
    void Update()
    {

        if (erasing)
        {
            if (boardBuilder!.GameBoard == null)
            {
                StartGame(boardSize);
                erasing = false;
            }
        }

    }

    private void ResetMoveCount()
    {
        moveCount = 0;
        movesText!.SetText($"Moves: 0");
    }

    private void AddMove()
    {
        moveCount++;
        if (movesText != null)
        {
            movesText.SetText($"Moves: {moveCount}");
        }
    }

    public void TrySwapTile()
    {
        if (highlightedTile != null && _gameOver == false)
        {

            // Are any of the adjacent tiles the empty tile?

            List<Tile> tilesToCheck = GatherTilesToCheck();
            Tile? emptyTile = CheckTilesForEmptyTile(tilesToCheck);

            if (emptyTile != null)
            {
                if (audioButton!.On)
                {
                    AudioSource.PlayClipAtPoint(tileMoveSound, Vector3.zero);
                }
                highlightedTile.ToggleHighlight();
                // This should not be the responsibility of boardBuilder -- who will do it?
                boardBuilder!.SwapEmptyTile(emptyTile.gameObject, highlightedTile.gameObject);
                highlightedTile = emptyTile;
                emptyTile.Highlight();
                AddMove();
                if (CheckForWin())
                {
                    Victory();
                }
            }
            
        }
    }

    private void Victory()
    {
        _gameOver = true;

        playingButtons!.SetActive(false);
        victoryButtons!.SetActive(true);
        winnerText!.gameObject.SetActive(true);

        if (audioButton!.On)
        {
            AudioSource.PlayClipAtPoint(victorySound, Vector3.zero);
        }

        if (timeKeeper != null)
        {
            timeKeeper.StopTimer();
        }
    }

    private bool CheckForWin()
    {
        /*  The win condition is that all tiles are in order with
            the empty tile on the bottom right.

            A 3-size grid win condition:
                1   2   3
                4   5   6
                7   8   x
            where x is the empty tile.

            Do note that the origin is from the bottom left.

            Assume b = boardSize - 1;

            0/0 = bottom left
            0/b = top left
            b/b = top right
            b/0 = bottom right
        
            To check left to right, we must go from 0 to b.
            To check top to bottom, we must go from b to 0.
        */

        int max = boardSize - 1;

        int emptyTile = (boardSize * boardSize);
        int expected = 1;

        for (int row = max; row >= 0; row--)
        {
            for (int col = 0; col <= max; col++)
            {
                int tileValue = Utilities.GetTileScript(boardBuilder!.GetTiles()![col][row]).GetValue();
                if (tileValue == expected)
                {
                    expected++;
                    if (expected == emptyTile)
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        return false;

    }

    private void DoTestHighlight()
    {
        Utilities.GetTileScript(boardBuilder!.GetTiles()![0][boardSize - 1]).Highlight();
    }

    private Tile? CheckTilesForEmptyTile(List<Tile> tilesToCheck)
    {
        foreach (Tile tile in tilesToCheck)
        {
            if (tile is EmptyTile)
            {
                return tile;
            }
        }
        return null;
    }
    
    private List<Tile> GatherTilesToCheck()
    {
        List<Tile> tilesToCheck = new List<Tile>();

        tilesToCheck.Add(GetTileInDir(Dir.Left));
        tilesToCheck.Add(GetTileInDir(Dir.Right));
        tilesToCheck.Add(GetTileInDir(Dir.Up));
        tilesToCheck.Add(GetTileInDir(Dir.Down));

        return tilesToCheck;

    }

    private Tile GetTileInDir(Dir dir)
    {

        int currentX = highlightedTile!.GridX - 1;
        int currentY = highlightedTile!.GridY - 1;

        int x = 0;
        int y = 0;

        switch (dir)
        {
            case Dir.Left:
                x = -1;
                break;
            case Dir.Right:
                x = 1;
                break;
            case Dir.Up:
                y = 1;
                break;
            case Dir.Down:
                y = -1;
                break;
        }

        return Utilities.GetTileScript(boardBuilder!.GetTiles()![BoardClamp(currentX + x)][BoardClamp(currentY + y)]);
    }

    private int BoardClamp(int toClamp)
    {
        return Mathf.Clamp(toClamp, 0, boardSize - 1);
    }
}
