using System;
using UnityEngine;

namespace LighthouseKeeper.Player
{
    public class StaminaSystem : MonoBehaviour
    {
        public enum State
        {
            Fresh,
            Tired,
            Exhausted,
        }

        public State state;

        public static StaminaSystem Instance;
        public static event Action<State, State> OnStateChange;

        [SerializeField, Min(0)]
        float maxStamina = 150;

        [SerializeField, Min(0)]
        float tiredStamina = 50;

        float stamina;

        [SerializeField]
        float staminaRecoveryRate = 10f;

        float staminaDrainRate;

        [SerializeField]
        float exhaustionDuration = 2f;

        float exhaustionTimer;

        public bool CanUseStamina => state is State.Fresh or State.Tired;


        public float TiredAmount => Mathf.Clamp01(Mathf.InverseLerp(tiredStamina, 0, stamina));



        void Awake()
        {
            Instance = this;
        }

        public void SetStaminaDrain(float x)
        {
            Debug.Assert(x >= 0);
            staminaDrainRate = x;
        }



        void Start()
        {
            stamina = maxStamina;
            state = State.Fresh;
        }

        void SetState(State newState)
        {
            if (newState == state) return;

            switch (state, newState)
            {
                case (State.Fresh, State.Tired):
                case (State.Exhausted, State.Tired):
                case (State.Tired, State.Fresh):
                    break;

                case (State.Tired, State.Exhausted):
                    exhaustionTimer = Time.time + exhaustionDuration;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            OnStateChange?.Invoke(state, newState);
            state = newState;
        }

        void Update()
        {
            switch (state)
            {
                case State.Fresh:
                    if (staminaDrainRate > 0f)
                    {
                        stamina -= Time.deltaTime * staminaDrainRate;
                        if (stamina <= tiredStamina) SetState(State.Tired);
                    }
                    else
                    {
                        stamina += Time.deltaTime * staminaRecoveryRate;
                        if (stamina > maxStamina) stamina = maxStamina;
                    }
                    break;

                case State.Tired:
                    if (staminaDrainRate > 0f)
                    {
                        stamina -= Time.deltaTime * staminaDrainRate;
                        if (stamina <= 0)
                        {
                            stamina = 0f;
                            SetState(State.Exhausted);
                        }
                    }
                    else
                    {
                        stamina += Time.deltaTime * staminaRecoveryRate;
                        if (stamina > tiredStamina) SetState(State.Fresh);
                    }
                    break;

                case State.Exhausted:
                    exhaustionTimer -= Time.deltaTime;
                    if (exhaustionTimer <= 0f)
                    {
                        SetState(State.Tired);
                        stamina = 0.01f;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}