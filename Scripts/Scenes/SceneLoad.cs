using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    public Slider progressbar;
    public TextMeshProUGUI loadtext;
    public GameObject loadingObject;
    public GameObject PressObject;
    private Animator loadingAnimator;
    void Start()
    {
        StartCoroutine(LoadGameScene());
        loadingObject.SetActive(false);
        loadingAnimator = loadingObject.GetComponent<Animator>();
        PressObject.SetActive(false);
    }
    IEnumerator LoadGameScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");
        operation.allowSceneActivation = false;
        yield return null;
        while(!operation.isDone)
        {
            yield return null;
            if(progressbar.value<0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
                loadingObject.SetActive(true);
                PlayLoadingAnimation();
            }
            else if (progressbar.value >= 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }
            if (progressbar.value >= 1f)
            {
                loadtext.text = "Press SpaceBar";
                loadingObject.SetActive(false);
                PressObject.SetActive(true);
            }

            if(Input.GetKeyDown(KeyCode.Space) && progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
    void PlayLoadingAnimation()
    {
        if (loadingAnimator != null)
        {
            loadingAnimator.Play("Loading_Zombie");
        }
    }
}
