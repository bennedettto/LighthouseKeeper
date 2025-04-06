using UnityEngine;

namespace LighthouseKeeper.GameStates
{
  // Sets GameState when Invoked

  #if ODIN_INSPECTOR
  [Sirenix.OdinInspector.HideMonoScript]
  #endif
  public class SetGameStateInvokable : InvokableBehaviour
  {
    [SerializeField]
    GameState.State[] states;


    public override void Invoke()
    {
      GameState.SetStates(states);
    }
  }
}