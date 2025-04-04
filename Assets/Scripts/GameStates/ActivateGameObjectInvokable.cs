namespace LighthouseKeeper.GameStates
{
  public class ActivateGameObjectInvokable : InvokableBehaviour
  {
    public override void Invoke() => gameObject.SetActive(true);
  }
}