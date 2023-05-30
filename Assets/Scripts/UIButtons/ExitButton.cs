using UnityEngine;

public class ExitButton : BaseButton
{

    public override void Clicked()
    {
        Application.Quit();
    }

}