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
    [SuppressMessage("ReSharper", "SimplifyConditionalTernaryExpression")]
    public class LookAround : MonoBehaviour
    {
        [Header("Look Settings")]
        [Range(0, 90), SerializeField] float lookDownAngle = -30f;
        [Range(0, 90), SerializeField] float lookUpAngle = 60f;
        [Range(0, 180), SerializeField] float lookSideAngle = 90f;

        Vector3 lookDirection;

        KeeperInputActions inputActions;


        void Awake()
        {
            inputActions = new KeeperInputActions();
        }

        void Update()
        {
            HandleLook();

            transform.localRotation = Quaternion.Euler(lookDirection);
        }

        void HandleLook()
        {
            Vector2 input = inputActions.Player.Look.ReadValue<Vector2>();

            float multiplier = PlayerController.Instance.inputDevice switch
            {
                PlayerController.InputDevice.GamePad       => PlayerController.Instance.gamePadLookSensitivity,
                PlayerController.InputDevice.KeyboardMouse => PlayerController.Instance.mouseLookSensitivity,
                _                                          => throw new ArgumentOutOfRangeException(),
            };

            lookDirection += Time.deltaTime * multiplier * new Vector3(-input.y, input.x, 0);
            lookDirection.x = Mathf.Clamp(lookDirection.x, -lookUpAngle, lookDownAngle);
            lookDirection.y = Mathf.Clamp(lookDirection.y, -lookSideAngle, lookSideAngle);
        }
    }
}