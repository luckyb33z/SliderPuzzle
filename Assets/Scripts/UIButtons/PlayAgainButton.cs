using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAgainButton : BaseButton
{
    [SerializeField] private GameObject boardSizeButton;
    [SerializeField] private GameObject boardManager;

    public override void Clicked()
    {
        GetBoardManager().RestartGame(GetBoardSize());
    }

    private int GetBoardSize()
    {
        int newSize = boardSizeButton.GetComponent<BoardSizeButton>().BoardSize;
        return newSize;
    }

    private BoardManager GetBoardManager()
    {
        return boardManager.GetComponent<BoardManager>();
    }

}
