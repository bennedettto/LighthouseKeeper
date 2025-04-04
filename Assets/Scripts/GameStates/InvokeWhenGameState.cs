using UnityEngine;

namespace LighthouseKeeper.GameStates
{
    public class InvokeWhenGameState : MonoBehaviour
    {
        [SerializeField]
        InvokableBehaviour invokable;

        [SerializeField]
        Condition[] conditions;

        bool isMet;


        bool IsMet()
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                if (!conditions[i].IsMet) return false;
            }
            return true;
        }


        void Awake()
        {
            isMet = IsMet();

            if (isMet) invokable.Invoke();

            GameState.OnStateChange += OnStateChange;
        }


        void OnStateChange(string key, int value)
        {
            if (isMet == IsMet()) return;

            isMet = !isMet;
            if (isMet) invokable.Invoke();
        }


        void OnDestroy()
        {
            GameState.OnStateChange -= OnStateChange;
        }
    }
}