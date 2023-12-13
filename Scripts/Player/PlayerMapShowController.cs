using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapShowController : MonoBehaviour
{
    private PlayerEventController _playerEvent;
    private UI_Map _uiMap;
    private bool _mapActive = false;

    private void Start()
    {
        _playerEvent = GetComponent<PlayerEventController>();
        _playerEvent.OnMapUIEvent += ShowMapUIEvent;
    }

    private void ShowMapUIEvent()
    {
        if (!_mapActive)
            _uiMap = Manager.UI.ShowPopupUI<UI_Map>("WorldMapUI");
        else
            Manager.UI.ClosePopupUI();
        _mapActive = !_mapActive;
    }

    private void HideMapUI()
    {
        Manager.UI.HidePopupUI(_uiMap);
    }
}
