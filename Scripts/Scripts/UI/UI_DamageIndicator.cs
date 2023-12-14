using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_DamageIndicator : MonoBehaviour
{
    private UI_GameScene _uiGameScene;
    private Image _indicatorImage;
    private float _flashSpeed;

    private Coroutine coroutine;

    private void Start()
    {
        _uiGameScene = GetComponent<UI_GameScene>();
        _indicatorImage = _uiGameScene.GetImage((int)UI_GameScene.Images.DamageIndicator);
        _flashSpeed = 0.5f;
    }


    public void Flash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        _indicatorImage.enabled = true;
        _indicatorImage.color = Color.red;
        coroutine = StartCoroutine(FadeAway());
    }



    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0.0f)
        {
            a -= (startAlpha / _flashSpeed) * Time.deltaTime;
            _indicatorImage.color = new Color(1.0f, 0.0f, 0.0f, a);
            yield return null;
        }

        _indicatorImage.enabled = false;
    }

}