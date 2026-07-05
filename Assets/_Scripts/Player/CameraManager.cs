using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Project.Player
{
    public class CameraManager : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private InputReader inputReader;

        [SerializeField] private Transform player;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private CinemachineCamera cinemachineCamera;

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

        [Header("Zoom")] [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private float minDistance = 2f;
        [SerializeField] private float maxDistance = 8f;
        [SerializeField] private float zoomSmoothTime = 0.08f;


        private CinemachineThirdPersonFollow thirdPersonFollow;

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
            if (cinemachineCamera != null)
            {
                thirdPersonFollow = cinemachineCamera.GetComponent<CinemachineThirdPersonFollow>();

                if (thirdPersonFollow != null)
                {
                    currentDistance = thirdPersonFollow.CameraDistance;
                    targetDistance = currentDistance;
                }
            }

            if (cameraTarget != null)
            {
                Vector3 euler = cameraTarget.rotation.eulerAngles;
                yaw = euler.y;
                pitch = NormalizeAngle(euler.x);
            }
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
            inputReader.Zoom += OnZoom;
        }

        private void OnDisable()
        {
            if (inputReader == null)
                return;

            inputReader.Look -= OnLook;
            inputReader.Zoom -= OnZoom;
        }

        private void LateUpdate()
        {
            UpdateTargetPosition();
            UpdateRotation();
            UpdateZoom();
        }

        private void OnLook(Vector2 value, bool isMouse)
        {
            lookInput = value;
            isMouseInput = isMouse;
        }

        private void OnZoom(float value)
        {
            if (Mathf.Abs(value) < 0.01f)
                return;

            targetDistance -= value * zoomSpeed;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }

        private void UpdateTargetPosition()
        {
            if (player == null || cameraTarget == null)
                return;

            cameraTarget.position = player.position + targetOffset;
        }

        private void UpdateRotation()
        {
            if (cameraTarget == null)
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

            cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);

            // Mouse input only exists for one frame.
            if (isMouseInput)
                lookInput = Vector2.zero;
        }

        private void UpdateZoom()
        {
            if (thirdPersonFollow == null)
                return;

            currentDistance = Mathf.SmoothDamp(
                currentDistance,
                targetDistance,
                ref zoomVelocity,
                zoomSmoothTime
            );

            thirdPersonFollow.CameraDistance = currentDistance;
        }

        private float NormalizeAngle(float angle)
        {
            if (angle > 180f)
                angle -= 360f;

            return angle;
        }
    }
}