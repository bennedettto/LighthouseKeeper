using LighthouseKeeper.GameStates;
using UnityEngine;

namespace LighthouseKeeper.Environment
{
  public abstract class InteractableBehaviour : MonoBehaviour
  {
    static readonly int PLAYER_LAYER = 8;

    [SerializeField] public string gameState = "interactable.state";

    public abstract int Interact();
    protected abstract void Initialize(int state);


    void Awake()
    {
      #if UNITY_EDITOR
      Debug.Assert(LayerMask.LayerToName(PLAYER_LAYER).ToLower() == "player");
      #endif

      var sphere = gameObject.AddComponent<SphereCollider>();
      sphere.isTrigger = true;
      sphere.radius = 2.2f;
      sphere.includeLayers = 1 << PLAYER_LAYER;
    }

    void Start()
    {
      Initialize(GameState.Get(gameState));
    }

    void OnTriggerEnter(Collider other)
    {
      InteractionSystem.Instance.Register(this);
    }

    void OnTriggerExit(Collider other)
    {
      InteractionSystem.Instance.Unregister(this);
    }
  }
}
