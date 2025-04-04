using UnityEngine;

namespace LighthouseKeeper.GameStates
{
    public class InitialGameState : MonoBehaviour
    {
        [SerializeField]
        Condition[] conditions;

        void Awake()
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                GameState.Set(conditions[i].key, conditions[i].value);
            }
        }
    }
}