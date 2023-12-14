using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryController : MonoBehaviour
{
    private Inventory _inventory;

    private List<ItemInfo> _itemInfos;

    private bool inventoryActivated = false;

    private PlayerStatHandler _playerStatHeandler;

    private UI_ShowText _showText;

    private PickUpActionController _pickUpActionController;
    private void Start()
    {
        _pickUpActionController = GetComponent<PickUpActionController>();
        _showText = Manager.UI.SceneUI.GetObject((int)UI_GameScene.GameObjects.ShowText).GetComponent<UI_ShowText>();
        _playerStatHeandler = Player.Instance.playerStatHandler;

        _itemInfos = new List<ItemInfo>();
    }

    void Update()
    {
        OpenInventory();
    }
    private void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            inventoryActivated = !inventoryActivated;
            if (inventoryActivated)
            {
                _inventory = Manager.UI.ShowPopupUI<Inventory>("Inventory");
                _inventory.Initialize(_itemInfos, this);
                _inventory.OpenInventory();
            }
            else
            {
                Manager.UI.ClosePopupUI();
                _inventory.CloseInventory();
            }
        }
    }

    public void SetAmmoItemData(Item _item)
    {
        if (_item != null)
        {
            _playerStatHeandler.UpdateAmmo(_item);
        }
    }

    public void SetItemData(ItemInfo _itemInfo)
    {
        switch (_itemInfo.item.itemType)
        {
            case ItemType.Consumable:
                switch (_itemInfo.item.ammoType)
                {
                    case AmmoType.Rifle:
                    case AmmoType.Sniper:
                        SetAmmoItemData(_itemInfo.item);
                        break;
                    case AmmoType.Total:
                        {
                            // 같은 아이템을 찾고 있으면 갯수만 증가 아니면 새로 추가
                            ItemInfo find = FindSameItem(_itemInfo);
                            if (find != null)
                                find.amount += _itemInfo.amount;
                            else
                                _itemInfos.Add(_itemInfo);
                            break;
                        }
                }
                break;
            default:
                {
                    // 같은 아이템을 찾고 있으면 갯수만 증가 아니면 새로 추가
                    ItemInfo find = FindSameItem(_itemInfo);
                    if (find != null)
                        find.amount += _itemInfo.amount;
                    else
                        _itemInfos.Add(_itemInfo);
                }
                break;
        }
    }

    // 같은 이름의 아이템이 잇는지 찾는다
    public ItemInfo FindSameItem(ItemInfo _itemInfo)
    {
        for (int i = 0; i < _itemInfos.Count; i++)
        {
            if (_itemInfos[i].item.itemName == _itemInfo.item.itemName)
            {
                return _itemInfos[i];
            }
        }

        return null;
    }

    // 아이템 슬롯이 여유 공간 확인
    public bool Checkingemptyslots()
    {
        int slotcount = _itemInfos.Count;

        if (slotcount == 8)
        {
            return false;
        }
        else
        {

            return true;
        }
    }

    // 아이템 지우기
    public void RemoveItem(ItemInfo _item)
    {
        _itemInfos.Remove(_item);
    }
}


