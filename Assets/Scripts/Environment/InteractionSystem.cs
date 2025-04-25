using UnityEngine;
using UnityEngine.InputSystem;

namespace LighthouseKeeper.Environment
{
    public class InteractionSystem : MonoBehaviour
    {
        public static InteractionSystem Instance;

        private KeeperInputActions inputActions;

        InteractableBehaviour active = null;

        private void Awake()
        {
            inputActions = new KeeperInputActions();
            Instance = this;
        }

        private void OnEnable()
        {
            inputActions.Player.Enable();
            inputActions.Player.Interact.performed += OnInteract;
        }

        private void OnDisable()
        {
            inputActions.Player.Interact.performed -= OnInteract;
            inputActions.Player.Disable();
        }

        public void Register(InteractableBehaviour interactable)
        {
            active = interactable;
        }

        public void Unregister(InteractableBehaviour interactable)
        {
            if (active == interactable)
            {
                active = null;
            }
        }


        float interactCooldown = 0f;


        private void OnInteract(InputAction.CallbackContext context)
        {
            if (active == null || !active.isActiveAndEnabled) return;
            if (Time.time < interactCooldown) return;

            interactCooldown = Time.time + 0.5f;
            active.Interact();
        }
  }
}