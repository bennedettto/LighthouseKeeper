using System;
using System.Collections.Generic;
using UnityEngine;

namespace LighthouseKeeper.GameStates
{
  public static class GameState
  {
    [Serializable]
    public struct State
    {
      [SerializeField] public string key;
      [SerializeField] public int value;
    }


    internal static readonly Dictionary<int, int> state = new Dictionary<int, int>(200);

    public static event Action<int> OnStateChangeHash;
    public static event Action<int, int> OnStateChangeKeyValue;


    public static int GetHash(string key) => key.GetHashCode();



    public static void Set(int key, int value)
    {
      if (state.TryGetValue(key, out int previousValue))
      {
        if (previousValue == value) return;

        state[key] = value;
      }
      else
      {
        state.Add(key, value);
      }

      OnStateChangeHash?.Invoke(1 << (key % 16));
      OnStateChangeKeyValue?.Invoke(key, value);
    }
    public static void Set(string key, int value) => Set(key.GetHashCode(), value);


    public static int Get(int key) => state.TryGetValue(key, out int value) ? value : 0;
    public static int Get(string key) => Get(key.GetHashCode());


    public static bool TryGet(int key, out int value) => state.TryGetValue(key, out value);
    public static bool TryGet(string key, out int value) => TryGet(key.GetHashCode(), out value);


    public static void SetStates(State[] states)
    {
      int hash = 0;
      for (int i = 0; i < states.Length; i++)
      {
        int key = states[i].key.GetHashCode();
        if (state.TryGetValue(key, out int previousValue))
        {
          if (previousValue == states[i].value) continue;

          state[key] = states[i].value;
        }
        else
        {
          state.Add(states[i].key.GetHashCode(), states[i].value);
        }

        OnStateChangeKeyValue?.Invoke(key, states[i].value);
        hash |= 1 << (key % 16);
      }

      if (hash != 0) OnStateChangeHash?.Invoke(hash);
    }
  }
}


