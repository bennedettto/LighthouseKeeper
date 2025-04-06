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
            GameState.OnStateChangeHash += OnGameStateChange;
            UpdateText();
        }

        void OnGameStateChange(int hash)
        {
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
            GameState.OnStateChangeHash -= OnGameStateChange;
        }
        #endif
    }
}