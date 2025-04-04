using System.Linq;
using UnityEngine;

namespace LighthouseKeeper.GameStates
{
    public class GameStateDebugManager : MonoBehaviour
    {
        #if UNITY_ASSERTIONS
        [Multiline]
        public string debug;

        void Awake()
        {
            GameState.OnStateChange += OnGameStateChange;
            UpdateText();
        }

        void OnGameStateChange(string key, int value)
        {
            Debug.Log($"State Changed: {key}={value}");
            UpdateText();
        }

        void UpdateText()
        {
            var list = GameState.state.Keys.ToList();
            list.Sort();

            debug = "";
            foreach (var key in list)
            {
                debug += $"{key}: {GameState.Get(key)}\n";
            }
        }

        void OnDestroy()
        {
            GameState.OnStateChange -= OnGameStateChange;
        }
        #endif
    }
}