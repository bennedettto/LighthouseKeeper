using UnityEngine;
using UnityEngine.VFX;

namespace LighthouseKeeper.Player
{
    public class VisualEffectWhenTired : MonoBehaviour
    {
        [SerializeField] VisualEffect fx;



        void Awake()
        {
            fx.enabled = false;
            StaminaSystem.OnStateChange += OnStaminaStateChanged;
        }


        void OnDestroy()
        {
            StaminaSystem.OnStateChange -= OnStaminaStateChanged;
        }


        void OnStaminaStateChanged(StaminaSystem.State oldState, StaminaSystem.State newState)
        {
            enabled = fx.enabled = newState is StaminaSystem.State.Tired or StaminaSystem.State.Exhausted;
        }
    }
}