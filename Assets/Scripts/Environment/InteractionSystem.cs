using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using LighthouseKeeper.GameStates;
using LighthouseKeeper.Player;

namespace LighthouseKeeper.Environment
{
    public class InteractionSystem : MonoBehaviour
    {
        public static InteractionSystem Instance;

        KeeperInputActions inputActions;

        List<InteractableBehaviour> actives = new List<InteractableBehaviour>(3);
        InteractableBehaviour active;

        [SerializeField] GameObject interactPrompt;


        void Awake()
        {
            inputActions = new KeeperInputActions();
            Instance = this;

            interactPrompt.SetActive(false);
        }

        void OnEnable()
        {
            inputActions.Player.Enable();
            inputActions.Player.Interact.performed += OnInteract;
            inputActions.Player.Interact.canceled += OnCancelInteract;
            inputActions.Player.Interact.started += OnStartInteract;

        }

        void OnStartInteract(InputAction.CallbackContext obj)
        {
            if (active == null) return;
            if (Time.time < interactCooldown) return;

            if (active.StartInteract(out int newState))
            {
                GameState.Set(active.gameState, newState);
            }
        }

        void OnCancelInteract(InputAction.CallbackContext obj)
        {
            if (active == null) return;

            if (active.StopInteract(out int newState))
            {
                GameState.Set(active.gameState, newState);
            }
        }

        void OnDisable()
        {
            inputActions.Player.Interact.performed -= OnInteract;
            inputActions.Player.Disable();

            if (interactPrompt != null) interactPrompt.SetActive(false);
        }

        public void Register(InteractableBehaviour interactable)
        {
            Debug.Log("Register");
            Debug.Assert(!actives.Contains(interactable));

            actives.Add(interactable);
            if (interactable.highlightTarget != null)
            {
                interactable.highlightTarget.enabled = true;
                interactable.highlightTarget.highlighted = false;
            }

            Update();
        }

        public void Unregister(InteractableBehaviour interactable)
        {
            Debug.Log("Unregister");
            Debug.Assert(actives.Contains(interactable));

            actives.RemoveSwapBack(interactable);
            if (interactable.highlightTarget != null)
            {
                interactable.highlightTarget.enabled = false;
                interactable.highlightTarget.highlighted = false;
            }

            if (active == interactable)
            {
                active = null;
                Update();
            }
        }

        void Update()
        {
            if (actives.Count == 0)
            {
                interactPrompt.SetActive(false);
                return;
            }

            var newActive = GetInteractable();

            if (newActive == active) return;

            if (active is not null && active.highlightTarget != null)
            {
                active.highlightTarget.highlighted = false;
            }
            active = newActive;
            if (active is not null && active.highlightTarget != null)
            {
                active.highlightTarget.highlighted = true;
            }
            interactPrompt.SetActive(active is not null);
        }



        float interactCooldown = 0f;


        void OnInteract(InputAction.CallbackContext context)
        {
            if (actives.Count == 0) return;
            if (Time.time < interactCooldown) return;

            if (active == null) return;

            interactCooldown = Time.time + 0.5f;

            if (active.Interact(out int newState))
            {
                GameState.Set(active.gameState, newState);
            }
        }

        InteractableBehaviour GetInteractable()
        {
            Transform playerTransform = PlayerController.Instance.cameraTransform;
            Vector3 playerForward = playerTransform.forward;
            Vector3 playerPosition = playerTransform.position;

            int bestIndex = -1;
            float bestDotProduct = 0.57f; // 35 degrees
            for (int i = 0; i < actives.Count; i++)
            {
                if (actives[i] == null)
                {
                    actives.RemoveAtSwapBack(i);
                    i--;
                }

                Vector3 direction = (actives[i].transform.position - playerPosition).normalized;
                float dotProduct = Vector3.Dot(playerForward, direction);
                if (dotProduct < bestDotProduct) continue;

                bestIndex = i;
                bestDotProduct = dotProduct;
            }

            return bestIndex < 0 ? null : actives[bestIndex];
        }
  }
}