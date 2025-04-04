using UnityEngine;

namespace LighthouseKeeper.GameStates
{
  [RequireComponent(typeof(Collider))]
  public class InvokeOnEnter : MonoBehaviour
  {
    const int PLAYER_LAYER = 8;

    [SerializeField]
    Condition[] conditions;

    void OnTriggerEnter(Collider other)
    {
      var go = other.gameObject;
      if (go.layer != PLAYER_LAYER) return;

      for (int i = 0; i < conditions.Length; i++)
      {
        if (!conditions[i].IsMet) return;
      }

      var invokables = GetComponents<InvokableBehaviour>();
      for (int i = 0; i < invokables.Length; i++)
      {
        invokables[i].Invoke();
      }
    }
  }
}