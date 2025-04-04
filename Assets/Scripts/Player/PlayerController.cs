using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LighthouseKeeper
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
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

        bool isGrounded;
        public bool IsGrounded => isGrounded;


        [Header("Look Settings")]
        public Transform cameraTransform;

        [Range(0, 100), SerializeField] float mouseLookSensitivity = 2f;
        [Range(0, 100), SerializeField] float gamePadLookSensitivity = 2f;
        [Range(0, 90), SerializeField] float lookDownAngle = -30f;
        [Range(0, 90), SerializeField] float lookUpAngle = 60f;

        enum InputDevice
        {
            KeyboardMouse,
            GamePad,
        }
        InputDevice inputDevice = InputDevice.KeyboardMouse;


        KeeperInputActions inputActions;
        CharacterController characterController;
        Vector3 velocity;
        Vector3 lookDirection;
        PlayerInput playerInput;



        void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            Instance = this;
            inputActions = new KeeperInputActions();
            characterController = GetComponent<CharacterController>();
        }


        void OnEnable()
        {
          inputActions.Player.Enable();
          inputActions.Player.Jump.performed += Jump;

          playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
          playerInput.onControlsChanged += OnControlsChanged;
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
          inputActions.Player.Disable();
          playerInput.onControlsChanged -= OnControlsChanged;

        }


        void Update()
        {
            HandleGravity();
            HandleLook();
            HandleRotation();
            HandleMovement();

            characterController.Move(Time.deltaTime * velocity);
            cameraTransform.localRotation = Quaternion.Euler(lookDirection);
            Vector3 forward = transform.forward.WithY(0).normalized;

            animator.SetFloat(moveSpeedHash, Vector3.Dot(velocity.WithY(0), forward));
        }


        void HandleRotation()
        {
            if (isFrozen)
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

            transform.localRotation *= Quaternion.Euler(0, Time.deltaTime * rotationSpeed * Mathf.Sign(angle), 0);
            lookDirection.y = Mathf.Sign(angle) * (Mathf.Abs(angle) - Time.deltaTime * rotationSpeed);
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
          if (!isGrounded || isFrozen) return;

          velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        void HandleMovement()
        {
            if (!isGrounded) return;

            Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
            var inputLength = input.magnitude;

            if (isFrozen ||inputLength < 0.01f)
            {
                velocity = Vector3.MoveTowards(velocity, new Vector3(0, velocity.y, 0), deceleration * Time.deltaTime);
                return;
            }

            if (inputLength > 1f) input /= inputLength;

            Vector3 forward = transform.forward.WithY(0).normalized;
            Vector3 right = transform.right.WithY(0).normalized;

            Vector3 moveDirection = forward * input.y + right * input.x;
            velocity = Vector3.MoveTowards(velocity,
                                           new Vector3(moveSpeed * moveDirection.x, velocity.y, moveSpeed * moveDirection.z),
                                           acceleration * Time.deltaTime);
        }



        void HandleLook()
        {
            if (isVisionLocked) return;
            Vector2 input = inputActions.Player.Look.ReadValue<Vector2>();

            float multiplier = inputDevice switch
            {
                InputDevice.GamePad => gamePadLookSensitivity,
                InputDevice.KeyboardMouse => mouseLookSensitivity,
                _ => throw new ArgumentOutOfRangeException(),
            };

            lookDirection += Time.deltaTime * multiplier * new Vector3(-input.y, input.x, 0) ;
            lookDirection.x = Mathf.Clamp(lookDirection.x, -lookUpAngle, lookDownAngle);
        }

        bool isFrozen = false;
        public void Freeze() => isFrozen = true;
        public void Unfreeze() => isFrozen = false;

        bool isVisionLocked = false;
        public void LockVision() => isVisionLocked = true;
        public void UnlockVision() => isVisionLocked = false;

        public void Sit() => animator.SetBool(sittingHash, true);
        public void Stand() => animator.SetBool(sittingHash, false);

    }
}