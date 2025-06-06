﻿using LighthouseKeeper.GameStates;
using UnityEngine;

namespace LighthouseKeeper.Environment
{
  [ExecuteAlways]
  public class TideManager : MonoBehaviour
  {

    [SerializeField] string key = "variables.tide";
    [SerializeField] float lowTide = 0.5f;
    [SerializeField] float highTide = 2.5f;

    public float tideHeight = 0f;

    int hashedKey;

    void Start()
    {
      hashedKey = key.GetHashCode();

      int tideLevel = GameState.Get(key);
      tideHeight = tideLevel == 0 ? lowTide : highTide;

      var myTransform = transform;
      var p = myTransform.position;
      p.y = tideHeight;
      myTransform.position = p;

      GameState.OnStateChangeKeyValue += OnGameStateChange;
    }

    void OnGameStateChange(int key, int value)
    {
      if (key != hashedKey) return;

      tideHeight = value == 0 ? lowTide : highTide;

      var myTransform = transform;
      var p = myTransform.position;
      p.y = tideHeight;
      myTransform.position = p;
    }

    void OnDestroy()
    {
      GameState.OnStateChangeKeyValue -= OnGameStateChange;
    }
  }
}