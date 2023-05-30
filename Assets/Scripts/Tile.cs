using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Tile : MonoBehaviour
{

    // This exists because of a big whoopsie
    [SerializeField] private BoardManager _board;

    public int GridX { get; set; } = 0;
    public int GridY { get; set; } = 0;
    public Vector2 ActualPosition { get; protected set; } = Vector2.zero;

    public Color OriginalColor { get; protected set; }
    protected Color _highlightColor = Color.red;
    protected bool _highlighted;

    public void CopyPosition(Tile other, bool update = true)
    {

        GridX = other.GridX;
        GridY = other.GridY;
        ActualPosition = other.ActualPosition;

        if (update)
        {
            UpdatePosition();
        }
        
    }

    public abstract int GetValue();

    public abstract void PrintValue();

    void Start()
    {
        _highlighted = false;
    }

    public void LinkBoard(BoardManager board)
    {
        _board = board;
    }

    public void ToggleHighlight()
    {
        if (!_highlighted)
        {
            Highlight();
        }
        else
        {
            ResetOriginalColor();
        }
    }

    public void ResetOriginalColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = OriginalColor;
        _highlighted = false;
    }

    public abstract void Highlight();

    private Color GetCurrentColor()
    {
        Color current = gameObject.GetComponent<SpriteRenderer>().color;
        return gameObject.GetComponent<SpriteRenderer>().color;
    }

    public void SetGridPos(int newX, int newY, Vector2 newPos)
    {
        GridX = newX;
        GridY = newY;
        ActualPosition = newPos;

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = ActualPosition;
        //gameObject.transform.position = ActualPosition;
    }

    void OnMouseOver()
    {
        if (!_highlighted) {
            Highlight();
            _board.SetHighlightedTile(this);
        }
    }

    void OnMouseExit()
    {
        if (_highlighted)
        {
            ResetOriginalColor();
            _board.SetHighlightedTile(null);
        }
    }

    void OnMouseDown()
    {
        _board.TrySwapTile();
    }

}