using UnityEngine;

namespace LighthouseKeeper.Environment
{
  public abstract class InteractableBehaviour : MonoBehaviour
  {
    static readonly int PLAYER_LAYER = LayerMask.NameToLayer("Player");
    
    public abstract void Interact();

    void Awake()
    {
      var collider = gameObject.AddComponent<SphereCollider>();
      collider.isTrigger = true;
      collider.radius = 2.4f;
    }

    void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.layer != PLAYER_LAYER) return;

      InteractionSystem.Instance.Register(this);
    }

    void OnTriggerExit(Collider other)
    {
      if (other.gameObject.layer != PLAYER_LAYER) return;

      InteractionSystem.Instance.Unregister(this);
    }
  }
}
