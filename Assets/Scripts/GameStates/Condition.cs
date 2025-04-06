using System;
using UnityEngine;

namespace LighthouseKeeper.GameStates
{
  [Serializable]
  public class Condition : IConditionNode
  {
    public enum Comparison
    {
      Equal = 0,
      GreaterEqual = 1,
      LessEqual = 2,
      Null = 3,
      NotNull = 4,
    }

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.HideLabel, Sirenix.OdinInspector.HorizontalGroup(0.4f)]
#endif
    [SerializeField] public string key = "some key";

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.HideLabel, Sirenix.OdinInspector.HorizontalGroup(0.3f)]
#endif
    [SerializeField] public Comparison comparison;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.HideLabel, Sirenix.OdinInspector.HorizontalGroup(0.3f)]
    [Sirenix.OdinInspector.HideIf(nameof(comparison), Comparison.Null)]
    [Sirenix.OdinInspector.HideIf(nameof(comparison), Comparison.NotNull)]
#endif
    [SerializeField] public int value = 0;

    int hashedKey = -1;
    int Key
    {
      get
      {
        if (hashedKey < 0) hashedKey = key.GetHashCode();
        return hashedKey;
      }
    }


    public bool IsMet() => comparison switch
    {
      Comparison.Equal        => value == GameState.Get(Key),
      Comparison.GreaterEqual => value <= GameState.Get(Key),
      Comparison.LessEqual    => value >= GameState.Get(Key),
      Comparison.Null         => !GameState.TryGet(Key, out int _),
      Comparison.NotNull      => GameState.TryGet(Key, out int _),
      _                       => throw new ArgumentOutOfRangeException(),
    };


    public int GetHash() => 1 << (key.GetHashCode() % 16);
  }
}