using System;
using HSM;
using Unity.Cinemachine;
using UnityEngine;

namespace Project.Player
{
    public class CameraManager : MonoBehaviour
    {

        [SerializeField] private PlayerStateDriver player;
        [SerializeField] private Transform cameraTarget;

        [Header("Look")] [SerializeField] private float mouseSensitivity = 0.08f;
        [SerializeField] private float gamepadSensitivity = 160f;
        [SerializeField] private bool invertY = false;
        [SerializeField] private float lookAcceleration = 18f;
        [SerializeField] private float lookDeceleration = 22f;
        [SerializeField] private float maxMouseLookSpeed = 8f;
        [SerializeField] private float maxGamepadLookSpeed = 180f;
        
        [Header("Pitch Limit")] [SerializeField]
        private float minPitch = -35f;

        [SerializeField] private float maxPitch = 70f;

        [Header("Target Offset")] [SerializeField]
        private Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);
        
        private InputReader inputReader;
        
        private Vector2 lookInput;
        private bool isMouseInput;
        private Vector2 currentLookVelocity;

        private float yaw;
        private float pitch;

        private float targetDistance;
        private float currentDistance;
        private float zoomVelocity;

        private void Awake()
        {
            if (cameraTarget != null)
            {
                Vector3 euler = cameraTarget.rotation.eulerAngles;
                yaw = euler.y;
                pitch = NormalizeAngle(euler.x);
            }
            inputReader = player.Reader;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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

        private void LateUpdate()
        {
            UpdateTargetPosition();
            UpdateRotation();
        }

        private void OnLook(Vector2 value, bool isMouse)
        {
            lookInput = value;
            isMouseInput = isMouse;
        }

        private void UpdateTargetPosition()
        {
            if (player == null || cameraTarget == null)
                return;

            cameraTarget.position = player.transform.position + targetOffset;
        }

        private void UpdateRotation()
        {
            if (cameraTarget == null || player.IsBusy)
                return;

            float deltaTime = Time.deltaTime;

            Vector2 targetVelocity;

            if (isMouseInput)
            {
                // Mouse delta is already frame-based input.
                targetVelocity = lookInput * mouseSensitivity;
                targetVelocity = Vector2.ClampMagnitude(targetVelocity, maxMouseLookSpeed);
            }
            else
            {
                // Gamepad stick is continuous input, so use degrees per second.
                targetVelocity = lookInput * gamepadSensitivity;
                targetVelocity = Vector2.ClampMagnitude(targetVelocity, maxGamepadLookSpeed);
            }

            float smoothRate = targetVelocity.sqrMagnitude > currentLookVelocity.sqrMagnitude
                ? lookAcceleration
                : lookDeceleration;

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

            if (invertY)
                pitch += y;
            else
                pitch -= y;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            player.transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
            // Mouse input only exists for one frame.
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
}