using UnityEngine;

public enum ItemType {Default, Food, Weapon, Instrument}
public class ItemScriptableObject : ScriptableObject
{
    public ItemType itemType;
    public string itemName;

    public GameObject itemPrefab;
    public Sprite icon;

    public int maxAmount;
    public string itemDescription;
}
