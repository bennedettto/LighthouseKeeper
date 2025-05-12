#define LOCKED_VIEW

using System;
using System.Diagnostics.CodeAnalysis;
using LighthouseKeeper.Player;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LighthouseKeeper.Player
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
    [SuppressMessage("ReSharper", "SimplifyConditionalTernaryExpression")]
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;

        static readonly int sittingHash = Animator.StringToHash("Sitting");
        static readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
        static readonly int rotationSpeedHash = Animator.StringToHash("RotationSpeed");

        [SerializeField] Animator animator;

        [Header("Movement Settings")]
        [Range(0, 10), SerializeField] float moveSpeed = 5f;

        [Range(0, 360), SerializeField] float rotationSpeed = 10f;
        [Range(0, 10), SerializeField] float jumpHeight = 2f;
        [Range(-20, 0), SerializeField] float gravity = -9.81f;
        [Range(0, 15), SerializeField] float acceleration = 4f;
        [Range(0, 15), SerializeField] float deceleration = 8f;

        [Header("Running")]
        [Range(0, 20), SerializeField] float sprintSpeed = 9f;
        [SerializeField] NoiseSettings runCameraNoiseSettings;
        [SerializeField] NoiseSettings targetNoiseSettings;
        [Range(0, 10), SerializeField] float staminaDrain = 10f;


        bool isGrounded;
        [ShowInInspector] public bool IsGrounded => isGrounded;

        bool isSprinting = false;


        [Header("Look Settings")]
        public Transform cameraTransform;

        [Range(0, 100), SerializeField]
        public float mouseLookSensitivity = 2f;
        [Range(0, 100), SerializeField]
        public float gamePadLookSensitivity = 2f;
        [Range(0, 90), SerializeField] float lookDownAngle = -30f;
        [Range(0, 90), SerializeField] float lookUpAngle = 60f;

        public enum InputDevice
        {
            KeyboardMouse,
            GamePad,
        }
        public InputDevice inputDevice = InputDevice.KeyboardMouse;


        KeeperInputActions inputActions;
        public CharacterController characterController;
        Vector3 velocity;
        Vector3 lookDirection;
        PlayerInput playerInput;

        StaminaSystem staminaSystem;



        void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            Instance = this;
            inputActions = new KeeperInputActions();
            characterController = GetComponent<CharacterController>();
            staminaSystem = GetComponent<StaminaSystem>();
        }


        void OnEnable()
        {
          inputActions.Player.Enable();
          inputActions.Player.Jump.performed += Jump;
          inputActions.Player.Sprint.started += StartSprint;
          inputActions.Player.Sprint.canceled += CancelSprint;

          playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
          playerInput.onControlsChanged += OnControlsChanged;

          StaminaSystem.OnStateChange += OnStaminaStateChanged;
        }

        void OnControlsChanged(PlayerInput newInput)
        {
            inputDevice = newInput.currentControlScheme switch
            {
                "Keyboard&Mouse" => InputDevice.KeyboardMouse,
                "Gamepad"        => InputDevice.GamePad,
                _                => inputDevice,
            };
        }


        void OnDisable()
        {
          inputActions.Player.Jump.performed -= Jump;
          inputActions.Player.Sprint.started -= StartSprint;
          inputActions.Player.Sprint.canceled -= CancelSprint;
          inputActions.Player.Disable();
          playerInput.onControlsChanged -= OnControlsChanged;
          StaminaSystem.OnStateChange -= OnStaminaStateChanged;
        }


        void Update()
        {
            HandleGravity();
            HandleLook();
            HandleRotation();
            HandleSprint();
            HandleMovement();
            HandleCameraShake();

            characterController.Move(Time.deltaTime * velocity);
            cameraTransform.localRotation = Quaternion.Euler(lookDirection);
            Vector3 forward = transform.forward.WithY(0).normalized;

            animator.SetFloat(moveSpeedHash, Vector3.Dot(velocity.WithY(0), forward));
        }


        void HandleSprint()
        {
            if (!isSprinting) return;

            staminaSystem.UseStamina(Time.deltaTime * staminaDrain);
        }


        void HandleRotation()
        {
            if (IsFrozen)
            {
                animator.SetFloat(rotationSpeedHash, 0f);
                return;
            }

            float angle = lookDirection.y;
            if (Mathf.Abs(angle) < 5)
            {
                animator.SetFloat(rotationSpeedHash, 0f);
                return;
            }

            #if LOCKED_VIEW
            transform.Rotate(0, angle, 0, Space.World);
            lookDirection.y = 0f;
            #else
            transform.localRotation *= Quaternion.Euler(0, Time.deltaTime * rotationSpeed * Mathf.Sign(angle), 0);
            lookDirection.y = Mathf.Sign(angle) * (Mathf.Abs(angle) - Time.deltaTime * rotationSpeed);
            #endif
            animator.SetFloat(rotationSpeedHash, rotationSpeed * angle);
        }


        void HandleGravity()
        {
          isGrounded = characterController.isGrounded;

          if (isGrounded && velocity.y < 0.01f)
          {
            velocity.y = -1f; // Small downward force to keep grounded
          }
          else
          {
            velocity.y += gravity * Time.deltaTime;
          }
        }


        void Jump(InputAction.CallbackContext obj)
        {
          if (!isGrounded || IsFrozen) return;

          velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }



        void OnStaminaStateChanged(StaminaSystem.State oldState, StaminaSystem.State newState)
        {
            if (newState == StaminaSystem.State.Exhausted)
            {
                CancelSprint();
            }
        }


        float cameraNoiseTargetAmplitude = 0f;
        void StartSprint(InputAction.CallbackContext obj)
        {
            if (!staminaSystem.CanUseStamina) return;

            isSprinting = true;
            cameraNoiseTargetAmplitude = 1f;
        }

        float cameraNoiseScale = 0f;
        void HandleCameraShake()
        {
            cameraNoiseScale =
                Mathf.MoveTowards(cameraNoiseScale, cameraNoiseTargetAmplitude, 5f * Time.deltaTime);

            var source = runCameraNoiseSettings.PositionNoise[0];
            var target = targetNoiseSettings.PositionNoise[0];
            if (source.X.Frequency != 0 && source.X.Amplitude != 0)
            {
                target.X.Frequency = source.X.Frequency;
                target.X.Amplitude = source.X.Amplitude * cameraNoiseScale;
            }
            if (source.Y.Frequency != 0 && source.Y.Amplitude != 0)
            {
                target.Y.Frequency = source.Y.Frequency;
                target.Y.Amplitude = source.Y.Amplitude * cameraNoiseScale;
            }
            if (source.Z.Frequency != 0 && source.Z.Amplitude != 0)
            {
                target.Z.Frequency = source.Z.Frequency;
                target.Z.Amplitude = source.Z.Amplitude * cameraNoiseScale;
            }
            targetNoiseSettings.PositionNoise[0] = target;


            source = runCameraNoiseSettings.OrientationNoise[0];
            target = targetNoiseSettings.OrientationNoise[0];
            if (source.X.Frequency != 0 && source.X.Amplitude != 0)
            {
                target.X.Frequency = source.X.Frequency;
                target.X.Amplitude = source.X.Amplitude * cameraNoiseScale;
            }
            if (source.Y.Frequency != 0 && source.Y.Amplitude != 0)
            {
                target.Y.Frequency = source.Y.Frequency;
                target.Y.Amplitude = source.Y.Amplitude * cameraNoiseScale;
            }
            if (source.Z.Frequency != 0 && source.Z.Amplitude != 0)
            {
                target.Z.Frequency = source.Z.Frequency;
                target.Z.Amplitude = source.Z.Amplitude * cameraNoiseScale;
            }
            targetNoiseSettings.OrientationNoise[0] = target;
        }


        void CancelSprint(InputAction.CallbackContext obj) => CancelSprint();

        void CancelSprint()
        {
            isSprinting = false;
            cameraNoiseTargetAmplitude = 0f;
        }


        void HandleMovement()
        {
            if (!isGrounded) return;

            Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
            var inputLength = input.magnitude;

            if (IsFrozen ||inputLength < 0.01f)
            {
                velocity = Vector3.MoveTowards(velocity, new Vector3(0, velocity.y, 0), deceleration * Time.deltaTime);
                return;
            }

            // get ground normal
            Vector3 groundNormal = characterController.isGrounded ? characterController.transform.up : Vector3.up;

            if (inputLength > 1f) input /= inputLength;

            Vector3 forward = transform.forward.WithY(0).normalized;
            Vector3 right = transform.right.WithY(0).normalized;

            Vector3 moveDirection = forward * input.y + right * input.x;
            var targetSpeed = isSprinting ? sprintSpeed : moveSpeed;
            velocity = Vector3.MoveTowards(velocity,
                                           new Vector3(targetSpeed * moveDirection.x, velocity.y, targetSpeed * moveDirection.z),
                                           acceleration * Time.deltaTime);
        }


        void HandleLook()
        {
            if (IsVisionLocked) return;
            Vector2 input = inputActions.Player.Look.ReadValue<Vector2>();

            float multiplier = inputDevice switch
            {
                InputDevice.GamePad => gamePadLookSensitivity,
                InputDevice.KeyboardMouse => mouseLookSensitivity,
                _ => throw new ArgumentOutOfRangeException(),
            };

            lookDirection += Time.deltaTime * multiplier * new Vector3(-input.y, input.x, 0);
            lookDirection.x = Mathf.Clamp(lookDirection.x, -lookUpAngle, lookDownAngle);
        }


        bool isFrozen = false;

        [ShowInInspector]
        bool IsFrozen =>
#if UNITY_EDITOR
            !Application.isPlaying ? false :
#endif
                (isFrozen || staminaSystem.state == StaminaSystem.State.Exhausted);
        public void Freeze() => isFrozen = true;
        public void Unfreeze() => isFrozen = false;


        bool isVisionLocked = false;

        [ShowInInspector] public bool IsVisionLocked =>
#if UNITY_EDITOR
            !Application.isPlaying ? false :
#endif
                (isVisionLocked || staminaSystem.state == StaminaSystem.State.Exhausted);
        public void LockVision() => isVisionLocked = true;
        public void UnlockVision() => isVisionLocked = false;

        public void Sit() => animator.SetBool(sittingHash, true);
        public void Stand() => animator.SetBool(sittingHash, false);
    }
}