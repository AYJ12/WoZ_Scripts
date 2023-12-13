using UnityEngine;
using System.Collections.Generic;

public class UIManager
{
    UI_Stack<UI_PopUp> popupStack = new UI_Stack<UI_PopUp>(); //보이는 중인 팝업 UI
    Dictionary<string, UI_PopUp> popUpDictionary = new Dictionary<string, UI_PopUp>();  //실행한 팝업 UI 저장

    public UI_Base SceneUI { get; private set; }

    int order = 10;

    public void Init()
    {
    }
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "UI_Root" };
            }

            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = ComponentEx.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Manager.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = ComponentEx.GetOrAddComponent<T>(go);
        SceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null, Transform parent = null) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        T popup;
        GameObject go;
        
        if (popUpDictionary.ContainsKey(name))
        {
            popup = (T)popUpDictionary[name];
            go = popup.gameObject;
            go.SetActive(true);
        }
        else
        {
            go = Manager.Resource.Instantiate($"UI/PopUp/{name}");
            popup = ComponentEx.GetOrAddComponent<T>(go);
            popUpDictionary.Add(name, popup);
        }

        popupStack.Push(popup);


        if (parent != null)
            go.transform.SetParent(parent);
        else
            go.transform.SetParent(Root.transform);

        go.transform.localScale = Vector3.one;

        SetCanvas(go);

        return popup;
    }


    public T PeekPopupUI<T>() where T : UI_PopUp
    {
        if (popupStack.IsEmpty())
            return null;

        return popupStack.Peek() as T;
    }

    public void ClosePopupUI(UI_PopUp popup)
    {
        if (popupStack.IsEmpty())
            return;

        if (popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (popupStack.IsEmpty())
            return;

        UI_PopUp popup = popupStack.Pop();  // popup set false로 변경
        popup.gameObject.SetActive(false);
        //Manager.Resource.Destroy(popup.gameObject);
        popup = null;
        order--;
    }

    public void HidePopupUI(UI_PopUp popup)
    {
        popup.gameObject.SetActive(false);
    }

    public void CloseAllPopupUI()
    {
        while (!popupStack.IsEmpty())
            ClosePopupUI();
    }

    public void Clear()
    {
        popUpDictionary.Clear();
        CloseAllPopupUI();
        SceneUI = null;
    }
}