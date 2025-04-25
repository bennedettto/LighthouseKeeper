using UnityEngine;

namespace LighthouseKeeper.Environment
{
  public class InteractableDoor : InteractableBehaviour
  {
      public override void Interact()
      {
          Debug.Log("Door interacted with!");
          // Add animation, sound, etc. here
      }
  }
}
