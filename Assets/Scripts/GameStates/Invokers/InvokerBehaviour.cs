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

        protected bool IsMet() => condition.IsMet();
        protected int GetHash() => condition.GetHash();

        protected bool TryInvoke()
        {
            if (!condition.IsMet()) return false;

            Array.ForEach(Invokables, invokable => invokable.Invoke());
            return true;
        }
    }
}