using UnityEngine;
[RequireComponent(typeof(Outline))]
public class ItemPickUp : MonoBehaviour
{
    public Item item;
    public ItemInfo itemInfo;

    private Outline _outLine;

    private void Awake()
    {
        _outLine = GetComponent<Outline>();

        if (_outLine == null)
        {
            Debug.LogWarning($"{gameObject.name} outline 표시");
        }

        SetOutline(false);
        itemInfo = new ItemInfo();
        itemInfo.item = item;
    }

    public void SetOutline(bool isActive)
    {
        _outLine.enabled = isActive;
    }
}
