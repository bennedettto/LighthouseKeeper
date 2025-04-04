using System;
using UnityEngine;

namespace LighthouseKeeper
{
    public class GameManager : MonoBehaviour
    {
        public enum State
        {
            Undefined,
            MainMenu,
            Initializing,
            Playing,
            Paused,
            GameOver,
        }


        public static GameManager Instance;

        public static event Action<State> OnStateChanged;

        public State currentState = State.Undefined;


        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            SetState(State.MainMenu);
        }


        public void SetState(State newState)
        {
            if (currentState == newState) return;

            currentState = newState;
            OnStateChanged?.Invoke(newState);
        }
    }
}