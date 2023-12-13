using UnityEngine;
using System.Collections.Generic;

public class Inventory : UI_PopUp
{
    public static bool inventoryActivated = false;

    [SerializeField]
    [Tooltip("인벤토리 베이스가 되는 프리팹")]
    private GameObject _goInventoryBase;
    [SerializeField]
    [Tooltip("모든 슬롯의 최상위 프리팹")]
    private GameObject _goSlotsParent;

    [Header("</color>" + "<color=#317F45>" + "Select Item")]
    [SerializeField]
    private GameObject _useButton;
    [SerializeField]
    private GameObject _unloadButton;

    private Slot[] _slots;
    private Slot _selectSlot;
    private SlotTooltip _slotTooltip;

    private Item item;

    public void Initialize(List<Item> _items)
    {
        _slots = _goSlotsParent.GetComponentsInChildren<Slot>();

        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].Initialize(this);
        }

        for (int i = 0; i < _items.Count; i++)
        {
            AcquireItem(_items[i]);
        }
    }
    public void OpenInventory()
    {
        _selectSlot = null;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inventoryActivated = true;
        //_goInventoryBase.SetActive(true);

    }

    public void CloseInventory()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inventoryActivated = false;
        //_goInventoryBase.SetActive(false);


        if (_selectSlot != null)
        {
            _selectSlot.UnselectItem(); // 아웃라인 끄기
        }
        CallHideTooltip(item); // 툴팁 끄기
    }



    public void AcquireItem(Item _item, int _count = 1)
    {
        item = _item;
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item != null)
            {
                if (_slots[i].item.itemName == _item.itemName)
                {
                    _slots[i].SetSlotCount(_count);
                    return;
                }
            }
        }
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item == null)
            {
                _slots[i].AddItem(_item, _count);
                return;
            }
        }
    }

    public bool SelectedSlot(Slot slot)
    {
        if (_selectSlot != null)
        {
            _selectSlot.UnselectItem();
        }
        if (_selectSlot == slot)
        {
            _selectSlot = null;
            return false;
        }
        else
        {
            _selectSlot = slot;
            return true;
        }
    }

    public void OnClicked_UseButton()
    {
        if (!_selectSlot)
        {
            return;
        }

        _selectSlot.UseSlot();
    }

    public void OnClicked_UnloadButton()
    {
        if (!_selectSlot)
        {
            return;
        }

        _selectSlot.UnloadSlot();
    }

    Slot GetEmptySlot()
    {
        return null;
    }

    // 툴팁
    public void CallTooltip(Item _item, Vector3 _pos)
    {
        _slotTooltip = Manager.UI.ShowPopupUI<SlotTooltip>("SlotTooltip");
        _slotTooltip.ShowTooltip(_item, _pos);
    }

    public void CallHideTooltip(Item _item)
    {
        if (_slotTooltip != null)
        {
            _slotTooltip.CloseTooltipProperty(_item);
            Manager.UI.ClosePopupUI();
            _slotTooltip = null;
        }
    }
}
