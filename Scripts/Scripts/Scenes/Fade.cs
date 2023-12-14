using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image Panel;
    public Image Ending;
    float currentTime = 0f;
    float fadeTime = 3f;
    public ThePed.Credits creditsScript;

    private void Awake()
    {
        FadeInOut();
    }
    public void FadeInOut()
    {
        StartCoroutine(FadeFlow());
    }
    IEnumerator FadeFlow()
    {
        Panel.gameObject.SetActive(true);

        yield return FadePanel(0f, 1f, fadeTime);

        yield return new WaitForSeconds(1f);

        Ending.gameObject.SetActive(true);

        yield return FadePanel(1f, 0f, fadeTime);

        Panel.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);

        Panel.gameObject.SetActive(true);

        yield return FadePanel(0f, 1f, fadeTime + 1f);
        creditsScript.play = true;
        yield return null;
    }
    IEnumerator FadePanel(float startAlpha, float targetAlpha, float duration)
    {
        Color alpha = Panel.color;
        currentTime = 0f;
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime / duration;
            alpha.a = Mathf.Lerp(startAlpha, targetAlpha, currentTime);
            Panel.color = alpha;
            yield return null;
        }
    }
}