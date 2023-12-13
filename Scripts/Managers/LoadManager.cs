using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    [SerializeField]
    private Image _loadingBar;
    private AsyncOperation _op;
    public static string nextSceneName;    //load -> loadui 불러와 실행 구조

    public void Init()
    {
        StartCoroutine(LoadCoroutineScene());
    }
    private void Awake()
    {
        Init();
    }

    IEnumerator LoadCoroutineScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime; 

            if (op.progress < 0.9f) 
            {
                _loadingBar.fillAmount = Mathf.Lerp(_loadingBar.fillAmount, op.progress, timer); 
                
                if (_loadingBar.fillAmount >= op.progress) 
                {
                    timer = 0f; 
                } 
            } 
            else 
            {
                _loadingBar.fillAmount = Mathf.Lerp(_loadingBar.fillAmount, 1f, timer); 
                if (_loadingBar.fillAmount == 1.0f) 
                { 
                    op.allowSceneActivation = true; 
                    yield break; 
                } 
            }
        }
    }

}