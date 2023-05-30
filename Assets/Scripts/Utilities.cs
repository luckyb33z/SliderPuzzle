using UnityEngine;
using TMPro;

public static class Utilities
{

    public static ValueTile GetValueTileScript(GameObject tile)
    {
        return GetTileScript(tile) as ValueTile;    // this will return null if it's not a ValueTile
    }

    public static Tile GetTileScript(GameObject tile)
    {
        return tile.GetComponent<Tile>();
    }

    public static int ValueToRow(int value, int boardSize)
    {
        return value % boardSize;
    }

    public static int ValueToCol(int value, int boardSize)
    {
        return value / boardSize;
    }

    public static int GetRandom(int maxExclusiveRange)
    {
        return Random.Range(0, maxExclusiveRange);   
    }

    public static TextMeshProUGUI GetTileValueText(GameObject tile)
    {
        #nullable enable

        ValueTile? vTileScript = GetTileScript(tile) as ValueTile;

        if (vTileScript != null)
        {
            return vTileScript.GetValueText();
        }
        else
        {
            return null!;
        }

        #nullable disable
        
    }

}
