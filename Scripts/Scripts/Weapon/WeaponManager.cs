using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Player's Weapons")]
    [SerializeField] private Transform _weaponsParent;
    [SerializeField] private List<GameObject> _weaponPrefabs;
    public List<WeaponBase> PlayerweaponsList = new List<WeaponBase>();

    [Header("Weapon Data")]
    public Transform WeaponDropPosition; // 무기 버리는 위치
    [SerializeField] private List<Transform> _pickupWeaponsParent; // 부모 오브젝트
    [SerializeField] private Transform _dropWeaponsParent; // 부모 오브젝트
    [SerializeField] private List<GameObject> _pickupWeaponPrefab; // 주운 무기 프리팹

    private Dictionary<WeaponType, int> ammoCounts = new Dictionary<WeaponType, int>();

    public static WeaponManager instance { get; set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DefaultPlayerWeapon();
        DefaultPickupWeapon();
    }

    private void Start()
    {
        GameScene go = (GameScene)Manager.Scene.CurrentScene;

        if (go != null)
        {
            _dropWeaponsParent = go.weaponDrop.transform;
        }

    }
    // 기본 무기 장착
    private void DefaultPlayerWeapon()
    {
        foreach (var weaponPrefab in _weaponPrefabs)
        {
            if (weaponPrefab != null)
            {
                GameObject weaponObject = Instantiate(weaponPrefab); // 무기 프리팹 생성
                WeaponBase weaponBase = weaponObject.GetComponent<WeaponBase>();

                PlayerweaponsList.Add(weaponBase);// 리스트에 추가
                weaponObject.SetActive(false);
                weaponObject.transform.SetParent(_weaponsParent);
                weaponBase.currentAmmo = weaponBase.ammoToReload; // 총알 초기화
                AmmoManager.instance.UpdateAmmo(weaponBase.weaponType, weaponBase.currentAmmo);
            }
        }
    }

    // 기본 소유 무기
    private void DefaultPickupWeapon()
    {
        for (int i = 0; i < _pickupWeaponPrefab.Count; i++)
        {
            GameObject weaponObject = Instantiate(_pickupWeaponPrefab[i]); // 무기 프리팹 생성
            weaponObject.SetActive(false);
            weaponObject.transform.SetParent(_pickupWeaponsParent[i]);
        }

    }

    // 무기 줍기
    public void PickupWeapon(GameObject pickedupWeapon, WeaponType weaponType)
    {
        AddWeaponIntoActiveSlot(pickedupWeapon, weaponType);
    }

    // 슬롯에 주운 무기 추가
    private void AddWeaponIntoActiveSlot(GameObject pickedupWeapon, WeaponType weaponType)
    {
        Transform targetParent = _pickupWeaponsParent[(int)weaponType];

        // 같은 타입의 0번째 무기가 있는지 확인 후 떨어뜨리기
        if (targetParent.childCount > 0)
        {
            var currentWeapon = targetParent.GetChild(0).gameObject.GetComponent<WeaponObject>();
            if (currentWeapon.weapon.type == weaponType)
            {
                DropWeapon(currentWeapon.gameObject, pickedupWeapon.transform.parent);
            }
        }

        pickedupWeapon.transform.SetParent(targetParent); // 새로운 무기 추가
        pickedupWeapon.SetActive(false);

    }

    // 버려진 무기가 떨어진 위치와 남은 탄약 수
    private void DropWeapon(GameObject weaponToDrop, Transform newParent)
    {
        // 남은 탄약 수
        WeaponObject weaponObject = weaponToDrop.GetComponent<WeaponObject>();
        WeaponType weaponType = weaponObject.weapon.type;
        Outline outline = weaponObject.GetComponent<Outline>();
        outline.enabled = false;

        // 남은 탄약 수 가져오기
        int ammoCount = AmmoManager.instance.GetAmmoCount(weaponType);
        ammoCounts[weaponType] = ammoCount;

        weaponObject.ammoCount = ammoCount; // 버려진 무기에 남은 탄약 수 저장

        // 떨어진 위치
        Transform dropWeaponsParentTransform = _dropWeaponsParent.transform;
        if (weaponType == WeaponType.Throw && ammoCount == 0) // 수류탄 ammo가 0일 때 버리면 프리팹 제거
        {
            Destroy(weaponToDrop);
        }
        else
        {
            weaponToDrop.transform.SetParent(dropWeaponsParentTransform);
            weaponToDrop.transform.position = WeaponDropPosition.position;
            weaponToDrop.transform.rotation = WeaponDropPosition.rotation;

            weaponToDrop.SetActive(true);
        }
    }
}
