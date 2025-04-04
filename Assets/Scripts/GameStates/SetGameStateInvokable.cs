using UnityEngine;

namespace LighthouseKeeper.GameStates
{
  public class SetGameStateInvokable : InvokableBehaviour
  {
    [SerializeField] string key;
    [SerializeField] int value;

    public override void Invoke()
    {
      GameState.Set(key, value);
    }
  }
}