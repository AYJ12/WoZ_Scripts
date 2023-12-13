using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryController : MonoBehaviour
{
    private Inventory _inventory;

    private List<Item> _items;

    private bool inventoryActivated = false;

    private PlayerStatHandler _playerStatHeandler;

    private UI_ShowText _showText;

    private PickUpActionController _pickUpActionController;
    private void Start()
    {
        _pickUpActionController = GetComponent<PickUpActionController>();
        _showText = Manager.UI.SceneUI.GetObject((int)UI_GameScene.GameObjects.ShowText).GetComponent<UI_ShowText>();
        _playerStatHeandler = Player.Instance.playerStatHandler;

        _items = new List<Item>();
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
               _inventory.Initialize(_items);
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

    public void SetItemData(Item _item)
    {
        switch (_item.itemType)
        {
            case ItemType.Consumable:
                switch (_item.ammoType)
                {
                    case AmmoType.Rifle:
                    case AmmoType.Sniper:
                        SetAmmoItemData(_item);
                break;
                    case AmmoType.Total:
                        _items.Add(_item);
                        break;
                }
                break;
            default:
                _items.Add(_item);
                break;
        }
    }
}
