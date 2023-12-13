using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Vector3 _originPos;

    public Item item;
    public int itemCount;
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
    public void Initialize(Inventory inventory)
    {
        _showText = GameObject.FindObjectOfType<UI_ShowText>();
        _inventory = inventory;
        _originPos = transform.position;
        _playerStatHandler = Player.Instance.playerStatHandler;
        _playerTransform = Player.Instance.transform;

        ClearSlot();
    }

    private void SelectItem()
    {
        if(_inventory.SelectedSlot(this))
        {
        _outLine.SetActive(true);
        }
    }

    public void UnselectItem()
    {
        _outLine.SetActive(false);
    }

    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템 사용 메서드
    public void UseItem(Item _item)
    {
        if (_item.itemType == ItemType.Health || _item.itemType == ItemType.Stamina)
        {
            _playerStatHandler.Add(_item.healthValue, _item.staminaValue);
            // PickUpActionController의 ItemUseAppear 메서드 호출하기 - 어떻게 하는게 맞는지 알아보기
            // 아이템 사용 Text 호출
            _showText.UseActionText(_item);
            _showText.OnUseActionText_2();
        }
        else if (_item.itemType == ItemType.Consumable)
        {
            _playerStatHandler.UpdateAmmo(_item);
            _showText.UseActionText(_item);
            _showText.OnUseActionText_2();
        }
    }
    public void AddItem(Item _item, int _count = 1) 
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        _goCountImage.SetActive(true);

        _textCount.text = itemCount.ToString();

        SetColor(1);
    }

    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        _textCount.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
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
        if (item != null)
        {
            UseItem(item);
            UnselectItem();
            SetSlotCount(-1);
        }
    }

    public void UnloadSlot()
    {
        if (item != null)
        {
            DropItemInScene();

            ClearSlot();
        }
    }

    public void DropItemInScene()
    {
        if (item.itemPrefab != null)
        {
            Vector3 dropPosition = _playerTransform.position + _playerTransform.forward * 2;
            dropPosition.y += 1;
            GameObject dropItem = Instantiate(item.itemPrefab, dropPosition, Quaternion.identity);
            Rigidbody dropItemRigi = dropItem.GetComponent<Rigidbody>();
            dropItemRigi.AddForce(_playerTransform.forward * 2);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
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
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            _inventory.CallTooltip(item, transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            _inventory.CallHideTooltip(item);
        }
    }
}
