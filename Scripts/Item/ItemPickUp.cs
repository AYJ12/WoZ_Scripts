using UnityEngine;
[RequireComponent(typeof(Outline))]
public class ItemPickUp : MonoBehaviour
{
    public Item item;

    private Outline _outLine;

    private void Awake()
    {
        _outLine = GetComponent<Outline>();

        if (_outLine == null)
        {
            Debug.LogWarning($"{gameObject.name} outline 표시");
        }

        SetOutline(false);
    }

    public void SetOutline(bool isActive)
    {
        _outLine.enabled = isActive;
    }
}
