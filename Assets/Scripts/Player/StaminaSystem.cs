using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LighthouseKeeper.Player
{
    public class StaminaSystem : MonoBehaviour
    {
        public enum RecoveryState
        {
            Full,
            Using,
            Depleted,
            Recovering,
        }

        public enum State
        {
            Fresh,
            Tired,
            Exhausted,
        }

        public static StaminaSystem Instance;
        public static event Action<State, State> OnStateChange;


        [ShowInInspector]
        RecoveryState recoveryState;

        [NonSerialized, ShowInInspector]
        public State state;

        [SerializeField, Min(0)]
        float maxStamina = 150;

        [SerializeField, Min(0)]
        float tiredStamina = 40;

        [ShowInInspector]
        float stamina;

        [SerializeField, Min(0)]
        float staminaRecoveryRate = 30f;

        [SerializeField, Min(0)]
        float recoveryCooldown = 2f;

        [SerializeField, Min(0)]
        float exhaustionDuration = 2f;

        float lastStateChangeTime;

        public bool CanUseStamina => recoveryState != RecoveryState.Depleted;


        public float TiredAmount => Mathf.Clamp01(Mathf.InverseLerp(tiredStamina, 0, stamina));



        void Awake()
        {
            Instance = this;

            stamina = maxStamina;
            recoveryState = RecoveryState.Full;
        }


        public void UseStamina(float x)
        {
            Debug.Assert(x >= 0);

            if (x == 0) return;

            if (stamina - x <= 0)
            {
                stamina = 0;
                SetState(RecoveryState.Depleted);
                return;
            }

            stamina -= x;
            SetState(RecoveryState.Using);
        }


        void SetState(RecoveryState newState)
        {
            if (recoveryState == newState) return;

            recoveryState = newState;
            lastStateChangeTime = Time.time;
        }

        void SetState(State newState)
        {
            if (state == newState) return;

            var oldState = state;
            state = newState;
            OnStateChange?.Invoke(oldState, newState);
            Debug.Log("StaminaSystem state changed: " + newState, this);
        }

        void Update()
        {
           switch (recoveryState)
           {
               case RecoveryState.Full:
                   break;

               case RecoveryState.Using:
                   if (Time.time > lastStateChangeTime + recoveryCooldown)
                   {
                       SetState(RecoveryState.Recovering);
                   }
                   break;

               case RecoveryState.Depleted:
                   if (Time.time > lastStateChangeTime + exhaustionDuration)
                   {
                       stamina = 1f;
                       SetState(RecoveryState.Using);
                   }
                   break;

               case RecoveryState.Recovering:
                   stamina += Time.deltaTime * staminaRecoveryRate;
                     if (stamina >= maxStamina)
                     {
                          stamina = maxStamina;
                          SetState(RecoveryState.Full);
                     }
                   break;

               default:
                   throw new ArgumentOutOfRangeException();
           }

           switch (state)
           {
               case State.Fresh when stamina < tiredStamina:
                   SetState(State.Tired);
                   break;


               case State.Tired when stamina > tiredStamina:
                   SetState(State.Fresh);
                   break;

               case State.Tired when stamina <= 0:
                   SetState(State.Exhausted);
                   break;


               case State.Exhausted when stamina > 0:
                   SetState(State.Tired);
                   break;

               case State.Fresh:
               case State.Tired:
               case State.Exhausted:
                   break;

               default:
                   throw new ArgumentOutOfRangeException();
           }
        }
    }
}