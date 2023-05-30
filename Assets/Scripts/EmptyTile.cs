using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : Tile
{

    [SerializeField] private readonly Color brown = new Color(0.1698113f, 0.103267f, 0.01842293f); // lol

    void Start()
    {
        OriginalColor = brown; // yeah I know right lmao
    }

    public override int GetValue()
    {
        return -1;
    }

    public override void Highlight()
    {
        //PrintValue();
    }

    public override void PrintValue()
    {
        Debug.Log("Value is empty.");
    }
}
