using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private float reachDistance;
    public GameObject inventoryPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Update()
    {
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (hit.collider.gameObject.TryGetComponent<Item>(out var hitItem))
                {
                    AddItem(hitItem.item, hitItem.amount);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    private void AddItem(ItemScriptableObject _item, int _amount) 
    {
        foreach (InventorySlot slot in slots) 
        {
            if (slot.item == _item) 
            {
                slot.amount += _amount;
                slot.itemAmountText.text = slot.amount.ToString();
                return;
            }
        }

        foreach (InventorySlot slot in slots) 
        {
            if (slot.isEmpty == true) 
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.itemAmountText.text = _amount.ToString();
                break;
            }
        }
    }

    public void OnOpenInventoryButtonClick()
    {
        inventoryPanel.SetActive(true);
        Crosshair.instance.DisableCrosshair();
    }

    public void OnCloseInventoryButtonClick()
    {
        inventoryPanel.SetActive(false);
        Crosshair.instance.EnableCrosshair();
    }
}
