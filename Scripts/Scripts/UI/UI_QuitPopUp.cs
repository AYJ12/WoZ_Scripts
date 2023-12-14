using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuitPopUp : UI_PopUp
{
    public void GoMainMenu()
    {
        Manager.Scene.LoadScene(Define.Scene.StartScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
