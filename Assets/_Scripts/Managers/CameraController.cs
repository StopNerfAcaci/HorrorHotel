using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera cutSceneCam;

    private const int PriorityValue = 5;
    private const int DefaultValue = 0;

    private void Awake()
    {
        SwitchCam(false);
    }

    internal void SwitchCam(bool isPlayer)
    {
        if(isPlayer) 
        {
            playerCam.Priority.Value = PriorityValue;
            playerCam.enabled = true;
            cutSceneCam.Priority.Value = DefaultValue;
            cutSceneCam.enabled = false;
        }
        else
        {
            cutSceneCam.Priority.Value = PriorityValue;
            cutSceneCam.enabled = true;
            playerCam.Priority.Value = DefaultValue;
            playerCam.enabled = false;
        }
    }
}
