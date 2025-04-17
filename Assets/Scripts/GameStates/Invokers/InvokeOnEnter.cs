using UnityEngine;

namespace LighthouseKeeper.GameStates
{
  [RequireComponent(typeof(Collider))]
  public class InvokeOnEnter : InvokerBehaviour
  {
    const int PLAYER_LAYER = 8;

    void OnTriggerEnter(Collider other)
    {
      var go = other.gameObject;
      if (go.layer != PLAYER_LAYER) return;

      TryInvoke();
    }
  }
}