using System;
using UnityEngine;

namespace LighthouseKeeper.GameStates
{
  [Serializable]
  internal struct Condition
  {
    internal enum Comparison
    {
      Equal = 0,
      GreaterEqual = 1,
      LessEqual = 2,
    }

    [SerializeField] public string key;
    [SerializeField] public Comparison comparison;
    [SerializeField] public int value;

    public bool IsMet => comparison switch
    {
      Comparison.Equal        => GameState.Get(key) == value,
      Comparison.GreaterEqual => GameState.Get(key) >= value,
      Comparison.LessEqual    => GameState.Get(key) <= value,
      _                       => throw new ArgumentOutOfRangeException(),
    };
  }
}