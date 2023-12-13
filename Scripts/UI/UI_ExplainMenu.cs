//using UnityEngine;
//using UnityEngine.UI;

//public class UI_ExplainMenu : UI_Base
//{
//    private GameObject _explainPanel;
//    private PlayerEventController _playerEventController;

//    public enum ExplainUIElement
//    {
//        Button,
//        Image
//    }

//    public override void Init()
//    {
//        Bind<Button>(typeof(ExplainUIElement));
//        Bind<Image>(typeof(ExplainUIElement));
//        Bind<Text>(typeof(ExplainUIElement));
//    }


//    private void Start()
//    {
//        _playerEventController = GameObject.FindWithTag("Player").GetComponent<PlayerEventController>();
//        _playerEventController.OnGuideEvent += SetExUI;
//        _playerEventController.OnQuitUIEvent += CloseExUI;
//        Init();
//        Button myButton = GetButton((int)ExplainUIElement.Button);
//        BindEvent(myButton.gameObject, OnButtonClick, Define.UIEvent.Click);
//    }


//    public void SetExUI()
//    {
//        Debug.Log("Guide");
//        Manager.UI.ShowPopupUI<UI_PopUp>("ExplainPanel");
//    }

//    public void CloseExUI()
//    {
//        Manager.UI.ClosePopupUI();
//    }

//    private void OnButtonClick()
//    {
//        gameObject.SetActive(!gameObject.activeSelf);
//    }
//}
