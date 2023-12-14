using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    public override void Init()
    {
        base.Init();

        SceneType = Define.Scene.StartScene;
    }

    public void OnStartGameButton()
    {
        LoadManager.nextSceneName = "GameScene";
        Manager.Scene.LoadScene(Define.Scene.LoadScene);
    }

    public override void Clear()
    {
        //throw new System.NotImplementedException();
    }

}
