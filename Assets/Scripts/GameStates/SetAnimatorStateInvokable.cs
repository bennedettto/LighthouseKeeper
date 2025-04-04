using UnityEngine;

namespace LighthouseKeeper.GameStates
{
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
        [SerializeField] string key;
        [SerializeField] ParameterType parameterType;
        [SerializeField] bool boolValue;
        [SerializeField] int intValue;
        [SerializeField] float floatValue;

        public override void Invoke()
        {
            switch (parameterType)
            {
                case ParameterType.Bool:
                    animator.SetBool(key, boolValue);
                    break;
                case ParameterType.Trigger:
                    animator.SetTrigger(key);
                    break;
                case ParameterType.Float:
                    animator.SetFloat(key, floatValue);
                    break;
                case ParameterType.Int:
                    animator.SetInteger(key, intValue);
                    break;
            }
        }
    }
}