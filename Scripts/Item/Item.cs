using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemContent;
    public ItemType itemType;
    public AmmoType ammoType;
    public GameObject itemPrefab;
    [Header("Health")]
    public float healthValue;
    [Header("Stamina")]
    public float staminaValue;
    [Header("Ammo")]
    public int sniperAmmoValue;
    public int rifleAmmoValue;
    public Sprite itemImage;

}
    public enum ItemType
    {
        Quest,
        Health,
        Stamina,
        Consumable
    }

    public enum AmmoType
    {
        Sniper,
        Rifle,
        Total
    }
