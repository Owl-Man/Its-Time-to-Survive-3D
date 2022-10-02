using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;

    public static Crosshair instance;

    private void Awake() => instance = this;

    public void EnableCrosshair() => crosshair.SetActive(true);

    public void DisableCrosshair() => crosshair.SetActive(false);
}
