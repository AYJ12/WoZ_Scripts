using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : BaseScene
{
    public override void Init()
    {
        base.Init();

        SceneType = Define.Scene.LoadScene;
    }

    public override void Clear()
    {
    }
}