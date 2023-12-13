using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

	protected void Bind<T>(Type type) where T : UnityEngine.Object
	{
		string[] names = Enum.GetNames(type);
		UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
		_objects.Add(typeof(T), objects); // Dictionary �� �߰�

		// T �� ���ϴ� ������Ʈ���� Dictionary�� Value�� objects �迭�� ���ҵ鿡 �ϳ��ϳ� �߰�
		for (int i = 0; i < names.Length; i++)
		{
			if (typeof(T) == typeof(GameObject))
				objects[i] = ComponentEx.FindChild(gameObject, names[i], true);
			else
				objects[i] = ComponentEx.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

	public GameObject GetObject(int idx) { return Get<GameObject>(idx); } // ������Ʈ�μ� ��������
    public TextMeshProUGUI GetTextMesh(int idx) { return Get<TextMeshProUGUI>(idx); }
    public Text GetText(int idx) { return Get<Text>(idx); } // Text�μ� ��������
    public Button GetButton(int idx) { return Get<Button>(idx); } // Button�μ� ��������
    public Image GetImage(int idx) { return Get<Image>(idx); } // Image�μ� ��������

    public static void BindEvent(GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = ComponentEx.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Pressed:
                evt.OnPressedHandler -= action;
                evt.OnPressedHandler += action;
                break;
            case Define.UIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;
            case Define.UIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
        }
    }
}
