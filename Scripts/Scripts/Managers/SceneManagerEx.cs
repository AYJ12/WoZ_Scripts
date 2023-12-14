using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public void Init() { }
    public BaseScene CurrentScene 
    { 
        get { return GameObject.FindObjectOfType<BaseScene>(); } 
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }
    public void LoadScene(Define.Scene type)
    {
        Manager.Clear();
        SceneManager.LoadScene(GetSceneName(type)); // SceneManager´Â UnityEngineÀÇ SceneManager
    }
    public void Clear()
    {
        CurrentScene.Clear();
    }

}
