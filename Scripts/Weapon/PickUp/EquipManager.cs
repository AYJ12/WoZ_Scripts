using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;
    public Transform dropPosition;
    private WeaponSwitchSystem _weaponSwitchSystem;

    // singleton
    public static EquipManager instance;

    private void Awake()
    {
        instance = this;
        _weaponSwitchSystem = GetComponent<WeaponSwitchSystem>();
    }

    public void EquipNew(WeaponData wepaonData)
    {
        WeaponType type = wepaonData.type;

        // ���� ���Ⱑ �ִ� ��� ��Ȱ��ȭ(���� Ÿ�� ���� ��Ȱ��ȭ, ����)
        if (WeaponManager.instance.PlayerweaponsList[(int)type] != null)
        {
            Destroy(WeaponManager.instance.PlayerweaponsList[(int)type].gameObject);
        }

        // ���ο� ���⸦ �ν��Ͻ�ȭ�ϰ� equipParent�� �߰�
        curEquip = Instantiate(wepaonData.equipPrefab, equipParent).GetComponent<Equip>();
        WeaponBase newWeapon = curEquip.gameObject.GetComponent<WeaponBase>();

        // ���ο� ���⸦ ����Ʈ�� �ش� Ÿ�� �ε����� �߰�
        WeaponManager.instance.PlayerweaponsList[(int)type] = newWeapon;

        // ���ο� ���� �������� ����(������Ʈ)
        WeaponInventory.instance.AddWeapon(WeaponManager.instance.PlayerweaponsList[(int)type]);

        _weaponSwitchSystem.GetCurrentWeaponType((int)type); // ���� ���� �ٷ� on

    }
}
