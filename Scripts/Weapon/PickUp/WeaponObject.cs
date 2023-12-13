using UnityEngine;

public class WeaponObject : MonoBehaviour, IInteractable
{
    public WeaponData weapon;

    public int ammoCount;


    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", weapon.weaponName);
    }

    public void OnInteract()
    {
        WeaponManager.instance.PickupWeapon(gameObject, weapon.type);
        EquipManager.instance.EquipNew(weapon); // Arm ÇÁ¸®ÆÕ ÀåÂø

        if (weapon.type == WeaponType.Throw) // ¼ö·ùÅº ±âº» Åº¾Ë
        {
            ammoCount = 1;
        }

        foreach (var weaponBase in WeaponManager.instance.PlayerweaponsList)
        {
            if (weaponBase.weaponType == weapon.type)
            {
                weaponBase.currentAmmo = ammoCount;

            }
        }
        gameObject.SetActive(false);
    }
}
