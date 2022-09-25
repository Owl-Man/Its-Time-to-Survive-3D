using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public GameObject inventoryPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public void OnOpenInventoryButtonClick() => inventoryPanel.SetActive(true);

    public void OnCloseInventoryButtonClick() => inventoryPanel.SetActive(false);
}
