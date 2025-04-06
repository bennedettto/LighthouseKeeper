using UnityEngine;

namespace LighthouseKeeper.GameStates
{
    [DefaultExecutionOrder(-500)]
    public class InitialGameState : MonoBehaviour
    {
        [SerializeField]
        GameState.State[] initialStates;

        void Awake()
        {
            GameState.SetStates(initialStates);
        }
    }
}