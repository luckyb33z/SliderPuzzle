using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSizeButton : BaseButton
{
    private int _boardSize = 3;
    public int BoardSize { get {return _boardSize;} }

    public override void Clicked()
    {
        CycleSize();
    }

    private void CycleSize()
    {
        switch (_boardSize)
        {
            case 3:
                SetText("4x4");
                _boardSize = 4;
                break;
            case 4:
                SetText("5x5");
                _boardSize = 5;
                break;
            case 5:
                SetText("3x3");
                _boardSize = 3;
                break;
        }
    }

    
}
