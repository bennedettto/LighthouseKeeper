using UnityEngine;

namespace LighthouseKeeper.GameStates
{
  // Activates the GameObject this script when invoked
  public class ActivateGameObjectInvokable : InvokableBehaviour
  {
    enum TargetState
    {
      Active,
      Inactive,
    }

    [SerializeField]
    TargetState targetState = TargetState.Active;

    public override void Invoke() => gameObject.SetActive(targetState == TargetState.Active);
  }
}