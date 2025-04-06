using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace LighthouseKeeper.GameStates
{
    // Sets Animator State when Invoked
    public class SetAnimatorStateInvokable : InvokableBehaviour
    {
        internal enum ParameterType
        {
            Bool,
            Trigger,
            Float,
            Int,
        }
        [SerializeField] Animator animator;
        [SerializeField] string animatorParameterName;
        [SerializeField] ParameterType parameterType;
#if ODIN_INSPECTOR
        [EnableIf(nameof(parameterType), ParameterType.Bool)]
#endif
        [SerializeField] bool boolValue;
#if ODIN_INSPECTOR
        [EnableIf(nameof(parameterType), ParameterType.Int)]
#endif
        [SerializeField] int intValue;
#if ODIN_INSPECTOR
        [EnableIf(nameof(parameterType), ParameterType.Float)]
#endif
        [SerializeField] float floatValue;



        public override void Invoke()
        {
            if (animator == null) animator = GetComponentInChildren<Animator>();
            switch (parameterType)
            {
                case ParameterType.Bool:
                    animator.SetBool(animatorParameterName, boolValue);
                    break;
                case ParameterType.Trigger:
                    animator.SetTrigger(animatorParameterName);
                    break;
                case ParameterType.Float:
                    animator.SetFloat(animatorParameterName, floatValue);
                    break;
                case ParameterType.Int:
                    animator.SetInteger(animatorParameterName, intValue);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}