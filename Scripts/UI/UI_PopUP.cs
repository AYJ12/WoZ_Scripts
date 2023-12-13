using UnityEngine;

public class UI_PopUp : UI_Base
{
    public override void Init()
    {
        Manager.UI.SetCanvas(gameObject, true);
        RectTransform myRectTransform = this.gameObject.GetComponent<RectTransform>();
        myRectTransform = transform as RectTransform;
    }

    public virtual void ClosePopupUI()
    {
        Manager.UI.ClosePopupUI(this);
        Time.timeScale = 1.0f;
    }

    public virtual void HidePopupUI()
    {
        Manager.UI.HidePopupUI(this);
    }
}