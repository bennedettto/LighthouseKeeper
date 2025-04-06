using System;
using UnityEngine;

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
    GameObject target;

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


    GameObject Target => target == null ? gameObject : target;


    void OnStateChange(int hash)
    {
      if ((this.hash & hash) == 0) return;

      bool isMet = IsMet();
      switch (action)
      {
        case Action.DestroyWhenMet:
          if (isMet) Destroy(Target);
          break;

        case Action.DestroyWhenNotMet:
          if (!isMet) Destroy(Target);
          break;

        case Action.DisableWhenMet:
          Target.SetActive(!isMet);
          break;

        case Action.EnableWhenMet:
          Target.SetActive(isMet);
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