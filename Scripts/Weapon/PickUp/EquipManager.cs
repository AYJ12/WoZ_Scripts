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

        // 현재 무기가 있는 경우 비활성화(같은 타입 무기 비활성화, 제거)
        if (WeaponManager.instance.PlayerweaponsList[(int)type] != null)
        {
            Destroy(WeaponManager.instance.PlayerweaponsList[(int)type].gameObject);
        }

        // 새로운 무기를 인스턴스화하고 equipParent에 추가
        curEquip = Instantiate(wepaonData.equipPrefab, equipParent).GetComponent<Equip>();
        WeaponBase newWeapon = curEquip.gameObject.GetComponent<WeaponBase>();

        // 새로운 무기를 리스트의 해당 타입 인덱스에 추가
        WeaponManager.instance.PlayerweaponsList[(int)type] = newWeapon;

        // 새로운 무기 슬롯으로 변경(업데이트)
        WeaponInventory.instance.AddWeapon(WeaponManager.instance.PlayerweaponsList[(int)type]);

        _weaponSwitchSystem.GetCurrentWeaponType((int)type); // 현재 무기 바로 on

    }
}
