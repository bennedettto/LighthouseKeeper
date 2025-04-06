using Sirenix.Utilities;
using UnityEngine;

namespace LighthouseKeeper.GameStates
{
  [RequireComponent(typeof(Collider))]
  public class InvokeOnEnter : MonoBehaviour
  {
    const int PLAYER_LAYER = 8;

    [SerializeField]
    InvokableBehaviour[] invokables;

    [SerializeReference]
    IConditionNode condition;

    InvokableBehaviour[] Invokables => invokables == null || invokables.Length == 0
                                         ? GetComponents<InvokableBehaviour>()
                                         : invokables;

    void OnTriggerEnter(Collider other)
    {
      var go = other.gameObject;
      if (go.layer != PLAYER_LAYER) return;

      if (!condition.IsMet()) return;

      Invokables.ForEach(invokable => invokable.Invoke());
    }
  }
}