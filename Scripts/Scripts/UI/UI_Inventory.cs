using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    public GameObject inventoryPanel;
    private PlayerEventController _playerEvent;
    bool activeInventory = false;


    private void Start()
    {
        inventoryPanel.SetActive(activeInventory);
        _playerEvent.OnInventoryEvent += SetUI;
    }

    private void SetUI()
    {
        activeInventory = !activeInventory;
        inventoryPanel.SetActive(activeInventory);
    }
}
