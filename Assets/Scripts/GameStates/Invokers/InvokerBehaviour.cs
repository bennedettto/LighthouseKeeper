using System;
using UnityEngine;

namespace LighthouseKeeper.GameStates
{
    public abstract class InvokerBehaviour : MonoBehaviour
    {

        [SerializeReference]
        IConditionNode condition = new Condition();

        [SerializeField]
        InvokableBehaviour[] invokables;


        InvokableBehaviour[] Invokables => invokables == null || invokables.Length == 0
                                               ? GetComponents<InvokableBehaviour>()
                                               : invokables;

        protected bool IsMet()
        {
            if (condition == null)
            {
                Debug.LogError("Condition is null", this);
                return false;
            }
            return condition.IsMet();
        }

        protected int GetHash()
        {
            if (condition == null)
            {
                Debug.LogError("Condition is null", this);
                return 0;
            }
            return condition.GetHash();
        }

        protected bool TryInvoke()
        {
            if (!condition.IsMet()) return false;

            Array.ForEach(Invokables, invokable => invokable.Invoke());
            return true;
        }
    }
}