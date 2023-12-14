using UnityEngine;

public class WeaponSlot
{
    public WeaponBase weapon;
}
public class WeaponInventory : MonoBehaviour
{
    public WeaponSlotUI[] uiSlots;
    public WeaponSlot[] slots;

    public GameObject inventoryWindow;

    private PlayerEventController _playerEvent;

    public static WeaponInventory instance;

    private void Awake()
    {
        instance = this;
        _playerEvent = GetComponent<PlayerEventController>();
    }

    private void Start()
    {
        _playerEvent.OnWeaponInvenEvent += Toggle;

        inventoryWindow.SetActive(false);

        slots = new WeaponSlot[WeaponManager.instance.PlayerweaponsList.Count];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new WeaponSlot();
            uiSlots[i].index = i;
            uiSlots[i].Clear();
        }

        UpdateUI();
    }

    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }
    public void UpdateUI()
    {
        for (int i = 0; i < WeaponManager.instance.PlayerweaponsList.Count; i++)
        {
            if (WeaponManager.instance.PlayerweaponsList[i] != null)
            {
                slots[i].weapon = WeaponManager.instance.PlayerweaponsList[i];
                uiSlots[i].Set(slots[i]);
            }
        }
    }

    public void AddWeapon(WeaponBase weapon)
    {
        WeaponData weaponData = weapon.weaponData;

        int slotIndex = (int)weaponData.type;

        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            slots[slotIndex].weapon = weapon;
            uiSlots[slotIndex].Set(slots[slotIndex]);
        }
    }

}

