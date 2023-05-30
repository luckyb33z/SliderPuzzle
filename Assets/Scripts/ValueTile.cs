using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValueTile : Tile
{

    private int _value;

    public int Value { 
        get {return _value; } 
        set
        {
            _value = value;
            UpdateValueText();
        }
    }

    void Start()
    {
        OriginalColor = Color.black;
    }

    public override int GetValue()
    {
        return Value;
    }

    public override void Highlight()
    {
        gameObject.GetComponent<SpriteRenderer>().color = _highlightColor;
        _highlighted = true;
        //PrintValue();
    }

    private void UpdateValueText()
    {
        GetValueText().text = System.Convert.ToString(_value);
    }

    public TextMeshProUGUI GetValueText()
    {
        return gameObject.transform.Find("InnerBG").Find("Value").GetComponent<TextMeshProUGUI>();
    }

    public override void PrintValue()
    {
        Debug.Log($"Value is {Value}");
    }

}