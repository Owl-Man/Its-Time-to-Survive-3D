using UnityEngine;

public class InventoryEvents : MonoBehaviour
{
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private GameObject DescriptionItemPanel;

    public static InventoryEvents Instance;

    private void Awake() => Instance = this;

    public void OnInventoryButtonClick() => InventoryPanel.SetActive(true);

    public void OnBackForInventoryButtonClick() => InventoryPanel.SetActive(false);

    public void IsOnItemButtonClick(bool state) => DescriptionItemPanel.SetActive(state);
}
