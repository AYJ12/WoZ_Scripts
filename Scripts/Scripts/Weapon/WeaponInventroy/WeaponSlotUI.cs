using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    public Image icon;

    private WeaponSlot _curSlot;
    public TextMeshProUGUI weaponStatNames;
    public TextMeshProUGUI weaponStatValues1;
    public TextMeshProUGUI weaponStatValues2;
    public TextMeshProUGUI weaponStatValues3;
    public TextMeshProUGUI weaponStatValues4;

    public int index;
    public bool equipped;


    public void Set(WeaponSlot slot)
    {
        _curSlot = slot;

        if (_curSlot.weapon != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = _curSlot.weapon.weaponData.image;

            weaponStatNames.text = "Damage\nAttack Distance\nAttack Rate\nAmmo to Reload";
            weaponStatValues1.text = _curSlot.weapon.weaponData.Stats.damage.ToString() + "\n";
            weaponStatValues2.text = _curSlot.weapon.weaponData.Stats.attackDistance.ToString() + "\n";
            weaponStatValues3.text = _curSlot.weapon.weaponData.Stats.attackRate.ToString() + "\n";
            weaponStatValues4.text = _curSlot.weapon.ammoToReload.ToString() + "\n";
        }
        else
        {
            Clear();
        }
    }

    public void Clear()
    {
        _curSlot = null;
        icon.gameObject.SetActive(false);

        // 정보 텍스트 및 스탯 초기화
        weaponStatNames.text = string.Empty;
        weaponStatValues1.text = string.Empty;
        weaponStatValues2.text = string.Empty;
        weaponStatValues3.text = string.Empty;
        weaponStatValues4.text = string.Empty;
    }
}
