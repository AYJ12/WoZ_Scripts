using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Vector3 _originPos;

    public ItemInfo itemInfo;
    //public int itemCount;
    public Image itemImage;

    [SerializeField]
    private Text _textCount;
    [SerializeField]
    private GameObject _goCountImage;

    private SlotTooltip _slotTooltip;

    private Inventory _inventory;

    [SerializeField] private GameObject _outLine;

    private Transform _playerTransform;

    private PlayerStatHandler _playerStatHandler;

    private UI_ShowText _showText;

    private ItemInventoryController _itemInventoryController;
    public void Initialize(Inventory inventory, ItemInventoryController itemInventoryController)
    {
        _itemInventoryController = itemInventoryController;
        _showText = GameObject.FindObjectOfType<UI_ShowText>();
        _inventory = inventory;
        _originPos = transform.position;
        _playerStatHandler = Player.Instance.playerStatHandler;
        _playerTransform = Player.Instance.transform;

        ClearSlot();
    }



    private void SelectItem()
    {
        if (_inventory.SelectedSlot(this))
        {
            Manager.Sound.Play("Click", Define.Sound.Effect);
            _outLine.SetActive(true);
        }
    }

    public void UnselectItem()
    {
        Manager.Sound.Play("Click", Define.Sound.Effect);
        _outLine.SetActive(false);
    }

    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템 사용 메서드
    public void UseItem(ItemInfo _itemInfo)
    {
        Item _item = _itemInfo.item;
        if (_item.itemType == ItemType.Health || _item.itemType == ItemType.Stamina)
        {
            switch (_item.soundType)
            {
                case SoundType.CanDrink:
                    Manager.Sound.Play("CanDrink", Define.Sound.Effect);
                    break;
                case SoundType.NomalDrink:
                    Manager.Sound.Play("NomalDrink", Define.Sound.Effect);
                    break;
                case SoundType.FreshVegetables:
                    Manager.Sound.Play("FreshVegetables", Define.Sound.Effect);
                    break;
                case SoundType.Snacks:
                    Manager.Sound.Play("Snacks", Define.Sound.Effect);
                    break;
                case SoundType.NomalFood:
                    Manager.Sound.Play("NomalFood", Define.Sound.Effect);
                    break;
                case SoundType.Recovery:
                    Manager.Sound.Play("Recovery", Define.Sound.Effect);
                    break;
                default:
                    Debug.Log("SoundType이 정해지지 않은 아이템 입니다.");
                    break;
            }
            _playerStatHandler.Add(_item.healthValue, _item.staminaValue);
            // PickUpActionController의 ItemUseAppear 메서드 호출하기 - 어떻게 하는게 맞는지 알아보기
            // 아이템 사용 Text 호출
            _showText.UseActionText(_item);
            _showText.OnUseActionText_2();
        }
        else if (_item.itemType == ItemType.Consumable)
        {
            Manager.Sound.Play("UpdateAmmo", Define.Sound.Effect);
            _playerStatHandler.UpdateAmmo(_item);
            _showText.UseActionText(_item);
            _showText.OnUseActionText_2();
        }
    }
    public void AddItem(ItemInfo _itemInfo)
    {
        itemInfo = _itemInfo;
        //itemCount = _count;
        itemImage.sprite = itemInfo.item.itemImage;

        _goCountImage.SetActive(true);

        //_textCount.text = itemCount.ToString();
        SetSlotCount(0);
        SetColor(1);
    }

    public void SetSlotCount(int _count)
    {
        itemInfo.amount += _count;
        // itemCount += _count;
        if (itemInfo.amount <= 0)
        {
            _itemInventoryController.RemoveItem(itemInfo);
            ClearSlot();
        }
        else
            _textCount.text = itemInfo.amount.ToString();
    }

    public void ClearSlot()
    {
        itemInfo = null;
        //itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        _textCount.text = "0";
        _goCountImage.SetActive(false);
        _outLine.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SelectItem();
        }
    }

    public void UseSlot()
    {
        if (itemInfo != null)
        {
            Manager.Sound.Play("Click", Define.Sound.Effect);
            UseItem(itemInfo);
            UnselectItem();
            SetSlotCount(-1);
        }
    }

    public void UnloadSlot()
    {
        if (itemInfo != null)
        {
            Manager.Sound.Play("Click", Define.Sound.Effect);
            DropItemInScene();
            _itemInventoryController.RemoveItem(itemInfo);
            ClearSlot();
        }
    }

    public void DropItemInScene()
    {
        if (itemInfo.item.itemPrefab != null)
        {
            Manager.Sound.Play("ItemDrop", Define.Sound.Effect);
            Vector3 dropPosition = _playerTransform.position + _playerTransform.forward * 2;
            dropPosition.y += 1;
            GameObject dropItem = Instantiate(itemInfo.item.itemPrefab, dropPosition, Quaternion.identity);

            ItemPickUp itemPickUp = dropItem.GetComponent<ItemPickUp>();
            itemPickUp.itemInfo = itemInfo;

            Rigidbody dropItemRigi = dropItem.GetComponent<Rigidbody>();
            dropItemRigi.AddForce(_playerTransform.forward * 2);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemInfo != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemInfo != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
    }

    private void ChangeSlot()
    {
        Manager.Sound.Play("SlotDrop", Define.Sound.Effect);
        ItemInfo _tempItem = itemInfo;
        //int _tempItemCount = itemInfo.amount;

        AddItem(DragSlot.instance.dragSlot.itemInfo);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInfo != null)
        {
            _inventory.CallTooltip(itemInfo.item, transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemInfo != null)
        {
            _inventory.CallHideTooltip();
        }
    }
}
