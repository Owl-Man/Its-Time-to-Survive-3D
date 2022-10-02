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
            Debug.DrawRay(ray.origin, ray.direction * reachDistance, Color.green);
            if (hit.collider.gameObject.TryGetComponent<Item>(out var hitItem))
            {
                AddItem(hitItem.item, hitItem.amount);
                Destroy(hit.collider.gameObject);
            }
        }
        else Debug.DrawRay(ray.origin, ray.direction * reachDistance, Color.red);
    }

    private void AddItem(ItemScriptableObject _item, int _amount) 
    {
        foreach(InventorySlot slot in slots) 
        {
            if (slot.item == _item) 
            {
                slot.amount += _amount;
                return;
            }
        }

        foreach (InventorySlot slot in slots) 
        {
            if (slot.isEmpty == false) 
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
            }
        }
    }

    public void OnOpenInventoryButtonClick() => inventoryPanel.SetActive(true);

    public void OnCloseInventoryButtonClick() => inventoryPanel.SetActive(false);
}
