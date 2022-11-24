using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private float reachDistance;

    public GameObject inventoryPanel, UIBG;

    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, reachDistance))
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
                if (slot.amount + _amount <= _item.maxAmount) 
                {
                    slot.amount += _amount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    return;
                }

                break;
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
        UIBG.SetActive(true);
        Crosshair.instance.DisableCrosshair();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnCloseInventoryButtonClick()
    {
        inventoryPanel.SetActive(false);
        UIBG.SetActive(false);
        Crosshair.instance.EnableCrosshair();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
