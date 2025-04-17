using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LighthouseKeeper.GameStates
{
  #if ODIN_INSPECTOR
  [Sirenix.OdinInspector.HideMonoScript]
  #endif
  public class DependsOnState : MonoBehaviour
  {
    enum Action
    {
      DestroyWhenMet,
      DestroyWhenNotMet,
      DisableWhenMet,
      EnableWhenMet,
    }


    enum Check
    {
      Always,
      Once,
    }


    [SerializeField]
    Action action;

    [SerializeField]
    Check check;

    [SerializeField]
    Object target;

    [SerializeReference]
    IConditionNode condition = new Condition();

    int hash;


    bool IsMet() => condition.IsMet();


    void Awake()
    {
      CalculateHash();
      OnStateChange(~0);

      if (check == Check.Once) return;
      GameState.OnStateChangeHash += OnStateChange;
    }


    void CalculateHash() => hash = condition.GetHash();


    Object Target => target == null ? (Object)gameObject  : target;


    void OnStateChange(int hash)
    {
      if ((this.hash & hash) == 0) return;

      bool isMet = IsMet();
      Object _target = Target;
      switch (action)
      {
        case Action.DestroyWhenMet:
          if (isMet) Destroy(_target);
          break;

        case Action.DestroyWhenNotMet:
          if (!isMet) Destroy(_target);
          break;

        case Action.DisableWhenMet:
          switch (_target)
          {
            case GameObject goTarget:
              goTarget.SetActive(!isMet);
              break;

            case Behaviour behaviourTarget:
              behaviourTarget.enabled = !isMet;
              break;

            default:
              Debug.LogWarning($"Cannot disable target of type {_target.GetType()}");
              break;
          }
          break;

        case Action.EnableWhenMet:
          switch (_target)
          {
            case GameObject goTarget:
              goTarget.SetActive(isMet);
              break;

            case Behaviour behaviourTarget:
              behaviourTarget.enabled = isMet;
              break;

            default:
              Debug.LogWarning($"Cannot enable target of type {_target.GetType()}");
              break;
          }
          break;

        default:
          throw new ArgumentOutOfRangeException();
      }
    }


    void OnDestroy()
    {
      if (check == Check.Once) return;
      GameState.OnStateChangeHash -= OnStateChange;
    }
  }
}