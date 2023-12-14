using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Player's Weapons")]
    [SerializeField] private Transform _weaponsParent;
    [SerializeField] private List<GameObject> _weaponPrefabs;
    public List<WeaponBase> PlayerweaponsList = new List<WeaponBase>();

    [Header("Weapon Data")]
    public Transform WeaponDropPosition; // ���� ������ ��ġ
    [SerializeField] private List<Transform> _pickupWeaponsParent; // �θ� ������Ʈ
    [SerializeField] private Transform _dropWeaponsParent; // �θ� ������Ʈ
    [SerializeField] private List<GameObject> _pickupWeaponPrefab; // �ֿ� ���� ������

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
    // �⺻ ���� ����
    private void DefaultPlayerWeapon()
    {
        foreach (var weaponPrefab in _weaponPrefabs)
        {
            if (weaponPrefab != null)
            {
                GameObject weaponObject = Instantiate(weaponPrefab); // ���� ������ ����
                WeaponBase weaponBase = weaponObject.GetComponent<WeaponBase>();

                PlayerweaponsList.Add(weaponBase);// ����Ʈ�� �߰�
                weaponObject.SetActive(false);
                weaponObject.transform.SetParent(_weaponsParent);
                weaponBase.currentAmmo = weaponBase.ammoToReload; // �Ѿ� �ʱ�ȭ
                AmmoManager.instance.UpdateAmmo(weaponBase.weaponType, weaponBase.currentAmmo);
            }
        }
    }

    // �⺻ ���� ����
    private void DefaultPickupWeapon()
    {
        for (int i = 0; i < _pickupWeaponPrefab.Count; i++)
        {
            GameObject weaponObject = Instantiate(_pickupWeaponPrefab[i]); // ���� ������ ����
            weaponObject.SetActive(false);
            weaponObject.transform.SetParent(_pickupWeaponsParent[i]);
        }

    }

    // ���� �ݱ�
    public void PickupWeapon(GameObject pickedupWeapon, WeaponType weaponType)
    {
        AddWeaponIntoActiveSlot(pickedupWeapon, weaponType);
    }

    // ���Կ� �ֿ� ���� �߰�
    private void AddWeaponIntoActiveSlot(GameObject pickedupWeapon, WeaponType weaponType)
    {
        Transform targetParent = _pickupWeaponsParent[(int)weaponType];

        // ���� Ÿ���� 0��° ���Ⱑ �ִ��� Ȯ�� �� ����߸���
        if (targetParent.childCount > 0)
        {
            var currentWeapon = targetParent.GetChild(0).gameObject.GetComponent<WeaponObject>();
            if (currentWeapon.weapon.type == weaponType)
            {
                DropWeapon(currentWeapon.gameObject, pickedupWeapon.transform.parent);
            }
        }

        pickedupWeapon.transform.SetParent(targetParent); // ���ο� ���� �߰�
        pickedupWeapon.SetActive(false);

    }

    // ������ ���Ⱑ ������ ��ġ�� ���� ź�� ��
    private void DropWeapon(GameObject weaponToDrop, Transform newParent)
    {
        // ���� ź�� ��
        WeaponObject weaponObject = weaponToDrop.GetComponent<WeaponObject>();
        WeaponType weaponType = weaponObject.weapon.type;
        Outline outline = weaponObject.GetComponent<Outline>();
        outline.enabled = false;

        // ���� ź�� �� ��������
        int ammoCount = AmmoManager.instance.GetAmmoCount(weaponType);
        ammoCounts[weaponType] = ammoCount;

        weaponObject.ammoCount = ammoCount; // ������ ���⿡ ���� ź�� �� ����

        // ������ ��ġ
        Transform dropWeaponsParentTransform = _dropWeaponsParent.transform;
        if (weaponType == WeaponType.Throw && ammoCount == 0) // ����ź ammo�� 0�� �� ������ ������ ����
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
