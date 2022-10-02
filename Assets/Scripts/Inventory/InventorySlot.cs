using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public ItemScriptableObject item;

    public GameObject icon;
    public TMP_Text itemAmount;

    public int amount;
    public bool isEmpty = true;
}
