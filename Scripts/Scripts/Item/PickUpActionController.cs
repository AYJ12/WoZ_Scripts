using UnityEngine;
using UnityEngine.UI;

public class PickUpActionController : MonoBehaviour
{
    [SerializeField]
    private float _range;

    private RaycastHit _hitInfo;

    [SerializeField]
    private LayerMask _layerMask;

    private UI_ShowText _showText;

    private ItemPickUp _itemPickUp;

    private Player _player;

    private ItemInventoryController _itemInventoryController;

    private void Start()
    {
        _itemInventoryController = GetComponent<ItemInventoryController>();
        _showText = Manager.UI.SceneUI.GetObject((int)UI_GameScene.GameObjects.ShowText).GetComponent<UI_ShowText>();
    }

    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_itemPickUp == null)
                return;
            // Health, Stamina, Quest 아이템이면
            if (!(_itemPickUp.itemInfo.item.itemType == ItemType.Consumable))
            {
                // 획득 메서드
                PickUpItem();
            }
            else
            {
                // Total, Sniper, RifleAmmoPack 아이템이면
                switch (_itemPickUp.itemInfo.item.ammoType)
                {
                    case AmmoType.Total:
                        PickUpItem();
                        break;
                    case AmmoType.Rifle:
                        ImmediatelyUseItem();
                        break;
                    case AmmoType.Sniper:
                        ImmediatelyUseItem();
                        break;
                }
            }
        }
    }

    // 아이템 획득 메서드
    private void PickUpItem()
    {
        if (_itemPickUp)
        {
            // 아이템 슬롯에 여유 공간이 있을 경우
            if (_itemInventoryController.Checkingemptyslots())
            {
                Manager.Sound.Play("PickUp", Define.Sound.Effect);
                _itemInventoryController.SetItemData(_itemPickUp.itemInfo);

                // 아이템 획득 Text 호출하기 
                ItemPickUpAppear();
                Destroy(_itemPickUp.gameObject);
            }
            else
            {
                ItemUnableInfoAppear();
            }
        }
    }

    // 획득 즉시 사용될 아이템 메서드
    private void ImmediatelyUseItem()
    {
        if (_itemPickUp.itemInfo.item.ammoType == AmmoType.Rifle || _itemPickUp.itemInfo.item.ammoType == AmmoType.Sniper)
        {
            Manager.Sound.Play("UpdateAmmo", Define.Sound.Effect);
            _itemInventoryController.SetItemData(_itemPickUp.itemInfo);
            ItemUseAppear();
        }
        Destroy(_itemPickUp.gameObject);
    }

    // 아이템 확인 메서드
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out _hitInfo, _range, _layerMask) &&
            _hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            _itemPickUp = _hitInfo.transform.GetComponent<ItemPickUp>();

            if (_itemPickUp)
            {
                // 아이템 획득 가능 알림 Text 활성화
                ItemInfoAppear();
                _itemPickUp.SetOutline(true);
            }

        }
        else
        {
            if (_itemPickUp)
            {
                _itemPickUp.SetOutline(false);
            }
            // 아이템 획득 가능 알림 Text 비활성화
            ItemInfoDisappear();
        }
    }

    // 아이템 획득 가능 알림 활성화 호출 메서드
    public void ItemInfoAppear()
    {
        // 획득 안내 문구 활성화 호출
        _showText.InfoActionText(_itemPickUp.itemInfo.item);
        _showText.ShowInfoActionText(true);
    }

    // 아이템 획득 가능 알림 비활성화 호출 메서드
    private void ItemInfoDisappear()
    {
        _itemPickUp = null;
        // 획득 안내 문구 비활성화 호출
        _showText.ShowInfoActionText(false);
    }

    // 아이템 획득 불가 알림
    private void ItemUnableInfoAppear()
    {
        _showText.UnablePickUpActionText(_itemPickUp.itemInfo.item);
        _showText.OnUnablePickUpActionText();
    }

    // 아이템 획득 알림 
    public void ItemPickUpAppear()
    {
        // 획득 문구 활성화 호출
        _showText.PickUpActionText(_itemPickUp.itemInfo.item);
        _showText.OnPickUpActionText();
    }

    // SniperAmmoPack or RifleAmmoPack 아이템 사용 알림 메서드
    public void ItemUseAppear()
    {
        // 사용 문구 활성화 호출
        if (_itemPickUp.itemInfo.item.ammoType == AmmoType.Total)
        {
            _showText.UseActionText(_itemPickUp.itemInfo.item);
            _showText.OnUseActionText_1();
        }
        else
        {
            _showText.UseActionText(_itemPickUp.itemInfo.item);
            _showText.OnUseActionText_2();
        }
    }
}
