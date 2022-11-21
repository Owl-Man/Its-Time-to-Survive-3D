using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemScriptableObject item;

    public Image icon;
    public TMP_Text itemAmountText;

    public GameObject trashZoneHelpText;

    public int amount;
    public bool isEmpty = true;

    public void SetIcon(Sprite _icon) 
    {
        icon.color = new Color(1f, 1f, 1f, 255);
        icon.sprite = _icon;
    }
}
