using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuideController : MonoBehaviour
{
    private PlayerEventController _playerEventController;
    private UI_GuidePopUp _uiGuidePopUp;
    private UI_QuitPopUp _uiQuitPopUp;
    private bool _guideActive = false;
    private bool _quitActive = false;

    private void Awake()
    {
        _playerEventController = GetComponent<PlayerEventController>();
        _playerEventController.OnGuideEvent += ShowGuideEvent;
        _playerEventController.OnQuitUIEvent += ShowQuitEvent;
    }

    private void ShowGuideEvent()
    {
        if (!_guideActive)
        {
            _uiGuidePopUp = Manager.UI.ShowPopupUI<UI_GuidePopUp>("Help");
            Player.Instance.playerInput.ToggleCursor(true);
        }
        else
        {
            Manager.UI.ClosePopupUI(_uiGuidePopUp);
            Player.Instance.playerInput.ToggleCursor(false);
        }
        _guideActive = !_guideActive;
    }

    private void ShowQuitEvent()
    {
        if (!_quitActive)
        {
            _uiQuitPopUp = Manager.UI.ShowPopupUI<UI_QuitPopUp>("QuitGame");
            Player.Instance.playerInput.ToggleCursor(true);
        }
        else
        {
            Manager.UI.ClosePopupUI(_uiQuitPopUp);
            Player.Instance.playerInput.ToggleCursor(false);
        }
        _quitActive = !_quitActive;
    }
}
