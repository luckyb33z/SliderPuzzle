using UnityEngine;

public class ResignButton : BaseButton
{

    private bool confirm = false;

    public override void Clicked()
    {
        if (!confirm)
        {
            GetText().fontSize = 0.075f;
            SetText("Are you sure?");
            confirm = true;
        }
        else
        {
            Application.Quit();
        }
    }
    
}
