using System.Collections.Generic;
using System.Linq;
using LighthouseKeeper.GameStates;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LighthouseKeeper.Environment
{
    public class InteractionSystem : MonoBehaviour
    {
        public static InteractionSystem Instance;

        KeeperInputActions inputActions;

        List<InteractableBehaviour> actives = new List<InteractableBehaviour>(3);

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
        }

        void OnDisable()
        {
            inputActions.Player.Interact.performed -= OnInteract;
            inputActions.Player.Disable();

            if (interactPrompt != null) interactPrompt.SetActive(false);
        }

        public void Register(InteractableBehaviour interactable)
        {
            actives.Add(interactable);
            interactPrompt.SetActive(true);
        }

        public void Unregister(InteractableBehaviour interactable)
        {
            actives.RemoveSwapBack(interactable);
            if (actives.Count == 0) interactPrompt.SetActive(false);
        }

        void Update()
        {
            if (actives.Count == 0) return;

            var interactable = GetInteractable();
            interactPrompt.SetActive(interactable != null);
        }


        float interactCooldown = 0f;


        void OnInteract(InputAction.CallbackContext context)
        {
            if (actives.Count == 0) return;
            if (Time.time < interactCooldown) return;

            var interactable = GetInteractable();
            if (interactable == null) return;

            interactCooldown = Time.time + 0.5f;
            int newState = interactable.Interact();
            GameState.Set(interactable.gameState, newState);
        }

        InteractableBehaviour GetInteractable()
        {
            Transform playerTransform = PlayerController.Instance.cameraTransform;
            Vector3 playerForward = playerTransform.forward;
            Vector3 playerPosition = playerTransform.position;

            int bestIndex = -1;
            float bestDotProduct = 0.7f;
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