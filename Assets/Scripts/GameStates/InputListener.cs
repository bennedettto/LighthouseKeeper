using UnityEngine;
using UnityEngine.InputSystem;

namespace LighthouseKeeper.GameStates
{
    // While enabled, listens for a UI Click.
    // When done sets GameState
    public class InputListener : MonoBehaviour
    {
        [SerializeField] string key;
        [SerializeField] int value;

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

        void Click(InputAction.CallbackContext obj) => GameState.Set(key, value);

        void OnDisable()
        {
            inputActions.UI.Click.performed -= Click;
            inputActions.Disable();
        }
    }
}