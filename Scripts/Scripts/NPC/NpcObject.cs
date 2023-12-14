using UnityEngine;

public class NpcObject : MonoBehaviour, IInteractable
{
    private NpcInteractAI _npcInteractAI;
    private static NpcObject _instance;

    private void Init()
    {
        if(_instance == null)
            _instance = this;
        _npcInteractAI = GetComponent<NpcInteractAI>();
    }

    public static NpcObject Instance
    {
        get
        {
            if (_instance == null)
                return null;
            return _instance;
        }
    }

    private void Awake()
    {
        Init();
    }

    public string GetInteractPrompt()
    {
        return string.Format("Interact ! {0}", gameObject.tag);   //TODO ��ȭ�ϱ� or ����Ʈ �ؽ�Ʈ�� ����
    }
    public void OnInteract()
    {
        _npcInteractAI.isActive = true;
        Manager.Sound.Play("Doctor Speech", Define.Sound.Effect);
    }
}
