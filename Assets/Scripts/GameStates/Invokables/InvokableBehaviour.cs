using UnityEngine;

namespace LighthouseKeeper.GameStates
{
  // offers an Interface to be Invoked when certain conditions are met
  public abstract class InvokableBehaviour : MonoBehaviour
  {
    public abstract void Invoke();
  }
}