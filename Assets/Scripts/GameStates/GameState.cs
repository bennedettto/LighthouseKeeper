using System;
using System.Collections.Generic;

namespace LighthouseKeeper.GameStates
{
  public class GameState
  {
    internal static readonly Dictionary<string, int> state = new Dictionary<string, int>(20);

    public static event Action<string, int> OnStateChange;


    public static void Set(string key, int value)
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

      OnStateChange?.Invoke(key, value);
    }

    public static int Get(string key)
    {
      return state.TryGetValue(key, out int value) ? value : 0;
    }
  }
}


