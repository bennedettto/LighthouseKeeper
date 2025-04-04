using UnityEngine;

namespace LighthouseKeeper.GameStates
{
  public class DependsOnState : MonoBehaviour
  {
    enum Action
    {
      DestroyWhenMet,
      DestroyUnlessMet,
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

    [SerializeField]
    Condition[] conditions;

    bool IsMet()
    {
      for (int i = 0; i < conditions.Length; i++)
      {
        if (!conditions[i].IsMet) return false;
      }
      return true;
    }


    void Awake()
    {
      OnStateChange(conditions[0].key, 0);

      if (check == Check.Once) return;
      GameState.OnStateChange += OnStateChange;
    }

    GameObject Target => target == null ? gameObject : target;


    void OnStateChange(string key, int value)
    {
      if (conditions.Length == 1 && conditions[0].key != key) return;

      bool isMet = IsMet();
      switch (action)
      {
        case Action.DestroyWhenMet:
          if (isMet) Destroy(Target);
          break;

        case Action.DestroyUnlessMet:
          if (!isMet) Destroy(Target);
          break;

        case Action.DisableWhenMet:
          Target.SetActive(!isMet);
          break;

        case Action.EnableWhenMet:
          Target.SetActive(isMet);
          break;
      }
    }


    void OnDestroy()
    {
      if (check == Check.Once) return;
      GameState.OnStateChange -= OnStateChange;
    }
  }
}