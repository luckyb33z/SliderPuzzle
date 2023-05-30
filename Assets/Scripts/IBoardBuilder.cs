using System;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardBuilder
{
    GameObject GameBoard { get; }

    void LinkUnityObjects(BoardManager boardManager, GameObject boardPrefab, GameObject tilePrefab, GameObject emptyTilePrefab, Canvas canvas);
    void SetSizes(int boardSize, float tileSize, float tileScale, float tileSpacing, float borderSpacing);
    void SetupGame();

    // These should not be the responsibility of IBoardBuilder
    void SwapEmptyTile(GameObject emptyTile, GameObject swapTile);
    List<List<GameObject>> GetTiles();

}

public abstract class BaseBoardBuilder: MonoBehaviour, IBoardBuilder
{

    // Unity linkages

    protected GameObject _boardPrefab;
    protected GameObject _tilePrefab;
    protected GameObject _emptyTilePrefab;

    protected List<List<GameObject>> _tilesList;
    protected BoardManager _boardManager;

    protected Canvas _canvas;

    public GameObject GameBoard { get; protected set; }

    // Size related

    protected int _boardSize;
    protected float _tileSize;
    protected float _tileScale;
    protected float _tileSpacing;
    protected float _borderSpacing;

    protected float _boardDimensions;

    private bool _unityLinked = false;
    private bool _sizesSet = false;

    public void LinkUnityObjects(BoardManager boardManager, GameObject boardPrefab, GameObject tilePrefab, GameObject emptyTilePrefab, Canvas canvas)
    {
        _boardManager = boardManager;
        _tilesList = new List<List<GameObject>>();
        _canvas = canvas;

        _boardPrefab = boardPrefab;
        _tilePrefab = tilePrefab;
        _emptyTilePrefab = emptyTilePrefab;

        _unityLinked = true;
    }

    public void SetSizes(int boardSize, float tileSize, float tileScale, float tileSpacing, float borderSpacing)
    {
        _boardSize = boardSize;
        _tileSize = tileSize;
        _tileScale = tileScale;
        _tileSpacing = tileSpacing;
        _borderSpacing = borderSpacing;

        _sizesSet = true;

    }

    public void SetupGame()
    {
        if (_sizesSet && _unityLinked)
        {
            BuildBoard();
            PlaceTiles();
        }
        else
        {
            throw new ArgumentException($"Sizes Set: {_sizesSet} | Unity Objects Linked: {_unityLinked}");
        }
    }

    protected abstract void BuildBoard();
    protected abstract void PlaceTiles();
    public abstract void SwapEmptyTile(GameObject emptyTile, GameObject swapTile);
    public abstract List<List<GameObject>> GetTiles();
    
    //public abstract Tile GetEmptyTile();

}

public class RandomBoardBuilder: BaseBoardBuilder
{

    protected override void BuildBoard()
    {
        // Size needs to accomodate all tiles, with tile spacing and borders taken into account.
        _boardDimensions = _boardSize + ((_boardSize - 1) * _tileSpacing) + (_borderSpacing * 2);
        
        GameBoard = GameObject.Instantiate(_boardPrefab, Vector2.zero, Quaternion.identity);
        if (GameBoard != null && _canvas != null)
        {
            GameBoard.transform.SetParent(_canvas.transform);
            GameBoard.transform.localScale *= _boardDimensions;
            GameBoard.transform.SetLocalPositionAndRotation(new Vector2(-6, 0), Quaternion.identity);
        }
    }

    protected override void PlaceTiles()
    {

        _tilesList = new List<List<GameObject>>();
        
        int tileNumber = 1;

        for (int col = 1; col <= _boardSize; col++)
        {
            List<GameObject> colTiles = new List<GameObject>();
            for (int row = 1; row <= _boardSize; row++)
            {

                GameObject newTile;

                if (tileNumber != ((_boardSize * _boardSize)))
                {
                    // Make a numbered tile if this isn't the last one

                    newTile = GameObject.Instantiate(_tilePrefab, Vector2.zero, Quaternion.identity);
                    if (newTile != null)
                    {
                        ValueTile vTileScript = Utilities.GetValueTileScript(newTile)!;
                        vTileScript.Value = tileNumber;
                    }
                    

                } else {

                    // Make an empty tile if this is the last one

                    newTile = GameObject.Instantiate(_emptyTilePrefab, Vector2.zero, Quaternion.identity);
                    //newTile.GetComponent<Tile>().SetEmpty(true);

                }
                
                if (newTile != null)
                {
                    newTile.transform.SetParent(GameBoard!.transform);

                    float tileX = ((_tileSpacing * (col - 1)) + (_tileSize * col)) / _boardDimensions;
                    float tileY = ((_tileSpacing * (row - 1)) + (_tileSize * row)) / _boardDimensions;

                    Vector2 tilePlacement = new Vector2(tileX, tileY);

                    Tile tileScript = Utilities.GetTileScript(newTile);
                    tileScript.LinkBoard(_boardManager);

                    ValueTile vTileScript = tileScript as ValueTile;

                    if (vTileScript != null)
                    {
                        vTileScript.Value = tileNumber;
                    }

                    tileScript.SetGridPos(col, row, tilePlacement);
                    RectTransform rect = newTile.GetComponent<RectTransform>();
                    rect.anchoredPosition = tilePlacement;

                    tileNumber++;
                    colTiles.Add(newTile);
                }
                
            }
            _tilesList.Add(colTiles);
        }

        RandomizeTiles();

    }

    private void RandomizeTiles()
    {

        // Set up the loop

        if (_tilesList != null)
        {
            RandomizeTileValues();
            SwapEmptyTile(GetDefaultEmptyTile(), GetRandomTile());
        }
    }

    private GameObject GetDefaultEmptyTile()
    {
        // NEVER call this after initial randomization
        return _tilesList[_boardSize - 1][_boardSize - 1];
    }

    // GetRandom initially called tiles!.Count
    private GameObject GetRandomTile()
    {
        return _tilesList[Utilities.GetRandom(_tilesList.Count)][Utilities.GetRandom(_tilesList.Count)];
    }

    private void RandomizeTileValues()
    {
        List<int> tileValues = new List<int>();
        int tilesNeeded = (_boardSize * _boardSize) - 1;

        for (int i = 0; i < tilesNeeded; i++)
        {
            tileValues.Add(i);
        }

        while (tileValues.Count > 1)
        {
            // First, determine which tiles to swap.

            int tileAValue = UnityEngine.Random.Range(0, tileValues.Count);
            int tileBValue = UnityEngine.Random.Range(0, tileValues.Count);

            while (tileBValue == tileAValue)
            {
                tileBValue = UnityEngine.Random.Range(0, tileValues.Count);
            }

            // Now, find the matching tile in the tiles List.
            // Determine the column first, in intervals of boardSize.

            int tileAColumn = Utilities.ValueToCol(tileAValue, _boardSize);
            int tileARow = Utilities.ValueToRow(tileAValue, _boardSize);

            int tileBColumn = Utilities.ValueToCol(tileBValue, _boardSize);
            int tileBRow = Utilities.ValueToRow(tileBValue, _boardSize);

            // Next, get their corresponding tiles in the tiles list
            // and then swap their values.

            ValueTile tileA = Utilities.GetValueTileScript(_tilesList[tileAColumn][tileARow])!;
            ValueTile tileB = Utilities.GetValueTileScript(_tilesList[tileBColumn][tileBRow])!;
            SwapTileValue(tileA, tileB);

            // Remove the operated from tileValues so they don't get
            // swapped again.

            tileValues.RemoveAt(Mathf.Max(tileAValue, tileBValue));
            tileValues.RemoveAt(Mathf.Min(tileAValue, tileBValue));

        }
    }

    // This should not be a public method. Put it somewhere better!
    public override void SwapEmptyTile(GameObject emptyTile, GameObject swapTile)
    {
        // Now that the numbered tiles have been swapped around, let's 
        // randomize where the empty tile is.

        int emptyTileIndexX = Utilities.GetTileScript(emptyTile).GridX - 1;
        int emptyTileIndexY = Utilities.GetTileScript(emptyTile).GridY - 1;

        int destinationIndexX = Utilities.GetTileScript(swapTile).GridX - 1;
        int destinationIndexY = Utilities.GetTileScript(swapTile).GridY - 1;

        if (emptyTile != swapTile)
        {
            // First, swap their physical positions

            Tile emptyTileScript = Utilities.GetTileScript(emptyTile);
            Tile swapTileScript = Utilities.GetTileScript(swapTile);
            SwapTilePosition(emptyTileScript, swapTileScript);

            // Next, swap their data positions
            // Their physical positions are swapped, but...

            GameObject tileMedium = emptyTile;

            _tilesList[emptyTileIndexX][emptyTileIndexY] = swapTile;
            _tilesList[destinationIndexX][destinationIndexY] = tileMedium;
        }
    }
        
    private void SwapTilePosition(Tile tileA, Tile tileB)
    {
        /*
        int GridXMedium = tileA.GridX;
        int GridYMedium = tileA.GridY;
        Vector2 ActualPositionMedium = tileA.ActualPosition;

        tileA.SetGridPos(tileB.GridX, tileB.GridY, tileB.ActualPosition);
        tileB.SetGridPos(GridXMedium, GridYMedium, ActualPositionMedium);
        */

        int tileAGridX = tileA.GridX;
        int tileAGridY = tileA.GridY;
        Vector2 tileAActualPosition = tileA.ActualPosition;

        tileA.CopyPosition(tileB);
        tileB.SetGridPos(tileAGridX, tileAGridY, tileAActualPosition);

    }

    private void SwapTileValue(ValueTile tileA, ValueTile tileB)
    {
        
        int valueMedium = tileA.Value;
        tileA.Value = tileB.Value;
        tileB.Value = valueMedium;
        
    }

    public override List<List<GameObject>> GetTiles() {return _tilesList;}

}