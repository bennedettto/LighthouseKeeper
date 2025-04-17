using UnityEngine;
using UnityEngine.InputSystem;

namespace LighthouseKeeper.GameStates
{
    // While enabled, listens for a UI Click.
    // When done sets GameState
    public class SetGameStateWhenUIClickAction : MonoBehaviour
    {
        [SerializeField] GameState.State[] states;

        KeeperInputActions inputActions;

        void Awake()
        {
            inputActions = new KeeperInputActions();
        }

        void OnEnable()
        {
            inputActions.Enable();
            inputActions.UI.Click.performed += Click;
        }

        void Click(InputAction.CallbackContext obj) => GameState.SetStates(states);

        void OnDisable()
        {
            inputActions.UI.Click.performed -= Click;
            inputActions.Disable();
        }
    }
}