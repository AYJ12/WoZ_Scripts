//using UnityEngine;

//public class UI_ShowPopUp : MonoBehaviour
//{
//    private PlayerEventController _playerEventController;
//    private GameObject _uiObject;

//    private void Start()
//    {
//        _playerEventController = Player.Instance.playerEvent;
//        _playerEventController.OnGuideEvent += ShowPopUp;
//        _playerEventController.OnQuitUIEvent += HidePopUp;
//    }

//    private void ShowPopUp()
//    {
//        Manager.UI.ShowPopupUI<UI_PopUp>("ExplainPanel");
//    }

//    private void HidePopUp()
//    {
//        Manager.UI.ClosePopupUI();
//    }

//    private void HideAllPopUp()
//    {
//        Manager.UI.CloseAllPopupUI();
//    }
//}
