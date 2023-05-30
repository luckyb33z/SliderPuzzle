using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IClickable
{
    public void Clicked();
}

public abstract class BaseButton : MonoBehaviour, IClickable
{

    [SerializeField] private static Color baseColor = Color.black;
    [SerializeField] private static Color highlightColor = Color.red;
    [SerializeField] protected float shrinkScale = 0.95f;
    [SerializeField] protected Vector3 originalSize = new Vector3(1.0f, 1.0f, 1.0f);

    [SerializeField] private GameObject buttonParent;
    [SerializeField] private GameObject buttonBorder;
    [SerializeField] private GameObject textField;

    bool pressing = false;

    public abstract void Clicked();

    private void OnMouseEnter()
    {
        HighlightBorder();
    }

    private void OnMouseExit()
    {
        ResetBorderColor();
        ResetSize();
        pressing = false;
    }

    private void OnMouseDown()
    {
        Shrink();
        pressing = true;
    }

    private void OnMouseUp()
    {
        // This will help avoid clicking elsewhere and then letting go
        if (pressing)
        {
            ResetSize();
            Clicked();
        }
    }

    private void HighlightBorder()
    {
        GetBorderRenderer().color = highlightColor;
    }

    private void ResetBorderColor()
    {
        GetBorderRenderer().color = baseColor;
    }

    private void Shrink()
    {
        buttonParent.transform.localScale = originalSize * shrinkScale;
    }

    private void ResetSize()
    {
        buttonParent.transform.localScale = originalSize;
    }

    private SpriteRenderer GetBorderRenderer()
    {
        return buttonBorder.GetComponent<SpriteRenderer>();
    }

    protected TextMeshProUGUI GetText()
    {
        return textField.GetComponent<TextMeshProUGUI>();
    }

    protected void SetText(string newText)
    {
        GetText().SetText(newText);
    }

}
