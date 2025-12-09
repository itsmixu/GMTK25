using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class CamSys : MonoBehaviour
{
    public List<CinemachineCamera> cameras; // Assign your virtual cameras in Inspector
    public int defaultPriority = 0;
    public int activePriority = 10;
    public float minSwitchTime = 10f;
    public float maxSwitchTime = 20f;

    private int currentCameraIndex = 0;
    private float switchTimer = 0f;
    private float switchInterval = 0f;

    void Start()
    {
        if (cameras == null || cameras.Count == 0)
        {
            Debug.LogError("No cameras assigned to AutoCameraSwitcher!");
            enabled = false;
            return;
        }

        // Initialize first active camera priority
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].Priority = (i == currentCameraIndex) ? activePriority : defaultPriority;
        }

        // Set initial random switch interval
        SetRandomInterval();
    }

    void Update()
    {
        switchTimer += Time.deltaTime;

        if (switchTimer >= switchInterval)
        {
            SwitchToNextCamera();
            SetRandomInterval();
            switchTimer = 0f;
        }
    }

    void SwitchToNextCamera()
    {
        // Lower priority of current camera
        cameras[currentCameraIndex].Priority = defaultPriority;

        // Move to next camera index in a circular manner
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Count;

        // Raise priority to make new camera active
        cameras[currentCameraIndex].Priority = activePriority;

        Debug.Log("Switched to camera: " + cameras[currentCameraIndex].name);
    }

    void SetRandomInterval()
    {
        switchInterval = Random.Range(minSwitchTime, maxSwitchTime);
        Debug.Log("Next camera switch in " + switchInterval.ToString("F2") + " seconds");
    }
}
