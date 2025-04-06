using Sirenix.Utilities;
using UnityEngine;

namespace LighthouseKeeper.GameStates
{
    public class InvokeWhenGameState : MonoBehaviour
    {
        [SerializeField]
        InvokableBehaviour[] invokables;

        [SerializeReference]
        IConditionNode condition = new Condition();

        bool wasMet;
        int hash;

        InvokableBehaviour[] Invokables => invokables == null || invokables.Length == 0
                                               ? GetComponents<InvokableBehaviour>()
                                               : invokables;


        bool IsMet() => condition.IsMet();


        void Awake()
        {
            hash = condition.GetHash();
            wasMet = IsMet();

            if (wasMet) Invokables.ForEach(invokable => invokable.Invoke());

            GameState.OnStateChangeHash += OnStateChange;
        }


        void OnStateChange(int changedHash)
        {
            if ((hash & changedHash) == 0) return;
            if (wasMet == IsMet()) return;

            wasMet = !wasMet;
            if (wasMet) Invokables.ForEach(invokable => invokable.Invoke());
        }


        void OnDestroy()
        {
            GameState.OnStateChangeHash -= OnStateChange;
        }
    }
}