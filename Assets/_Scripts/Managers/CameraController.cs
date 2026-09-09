using System;
using Unity.Cinemachine;
using UnityEngine;
using Utils.Extensions;

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public struct LookConfig
    {
        public float mouseSensitivity;
        public float gamepadSensitivity;
        public bool invertY;
        public float lookAcceleration;
        public float lookDeceleration;
        public float maxMouseLookSpeed;
        public float maxGamepadLookSpeed;

        // Static default instance
        public static LookConfig Default => new LookConfig
        {
            mouseSensitivity = 0.08f,
            gamepadSensitivity = 160f,
            invertY = false,
            lookAcceleration = 18f,
            lookDeceleration = 22f,
            maxMouseLookSpeed = 8f,
            maxGamepadLookSpeed = 180f
        };
    }

    [SerializeField] private InputReader inputReader;
    [SerializeField] CinemachineBrain brain;
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera cutSceneCam;
    [SerializeField] private float blendingTime = 1f;
    [SerializeField] private LookConfig lookConfig;

    [Header("Pitch Limit")] [SerializeField]
    private float minPitch = -35f;

    [SerializeField] private float maxPitch = 70f;

    [Header("Target Offset")] [SerializeField]
    private Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);

    private const int PriorityValue = 5;
    private const int DefaultValue = 0;

    private Transform playerTarget;
    private Transform playerTransform;
    private Vector2 lookInput;
    private bool isMouseInput;
    private Vector2 currentLookVelocity;

    private float yaw;
    private float pitch;
    
    private void Awake()
    {
        if (brain == null) brain = GetComponentInChildren<CinemachineBrain>();
        brain.DefaultBlend = new CinemachineBlendDefinition(
            CinemachineBlendDefinition.Styles.EaseInOut, blendingTime);

        playerTarget = playerCam.Target.TrackingTarget;
        playerTransform = playerTarget.parent;
    }
    private void OnEnable()
    {
        if (inputReader == null)
            return;

        inputReader.Look += OnLook;
    }

    private void OnDisable()
    {
        if (inputReader == null)
            return;

        inputReader.Look -= OnLook;
    }
    private void OnLook(Vector2 value, bool isMouse)
    {
        lookInput = value;
        isMouseInput = isMouse;
    }
    internal void SwitchCam(bool isPlayer)
    {
        if (isPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerCam.Priority.Value = PriorityValue;
            if(cutSceneCam) cutSceneCam.Priority.Value = DefaultValue;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if(cutSceneCam) cutSceneCam.Priority.Value = PriorityValue;
            playerCam.Priority.Value = DefaultValue;
        }
    }

    public void OnLateUpdate(float deltaTime)
    {
        UpdateRotation(deltaTime);
    }

    private void UpdateRotation(float deltaTime)
    {
        if (playerTarget == null)
            return;
        
        Vector2 targetVelocity;

        if (isMouseInput)
        {
            // Mouse delta is already frame-based input.
            targetVelocity = lookInput * lookConfig.mouseSensitivity;
            targetVelocity = Vector2.ClampMagnitude(targetVelocity, lookConfig.maxMouseLookSpeed);
        }
        else
        {
            // Gamepad stick is continuous input, so use degrees per second.
            targetVelocity = lookInput * lookConfig.gamepadSensitivity;
            targetVelocity = Vector2.ClampMagnitude(targetVelocity, lookConfig.maxGamepadLookSpeed);
        }

        float smoothRate = targetVelocity.sqrMagnitude > currentLookVelocity.sqrMagnitude
            ? lookConfig.lookAcceleration
            : lookConfig.lookDeceleration;

        currentLookVelocity = Vector2.Lerp(
            currentLookVelocity,
            targetVelocity,
            1f - Mathf.Exp(-smoothRate * deltaTime)
        );

        float x = currentLookVelocity.x;
        float y = currentLookVelocity.y;

        if (!isMouseInput)
        {
            x *= deltaTime;
            y *= deltaTime;
        }

        yaw += x;

        if (lookConfig.invertY)
            pitch += y;
        else
            pitch -= y;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        playerTransform.rotation = Quaternion.Euler(0f, yaw, 0f);
        playerTarget.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        if (isMouseInput)
            lookInput = Vector2.zero;
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;

        return angle;
    }
}