using System;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Range(1, 1000)][SerializeField] private int FPS = 60;
    [SerializeField] private bool isVSyncActive;

    public static FPSController instance;

    private void Awake() => instance = this;

    private void Start()
    {
        QualitySettings.vSyncCount = !isVSyncActive ? 0 : 1;
        Application.targetFrameRate = FPS;
    }

    public void EnableVSync() => QualitySettings.vSyncCount = 1;

    public void DisableVSync() => QualitySettings.vSyncCount = 0;

    public void SetFPS(int fps)
    {
        if (fps > 0 && fps <= 1000) FPS = fps;
        else throw new ArgumentException();
    }
}