using System;
using LighthouseKeeper.GameStates;
using UnityEngine;

namespace LighthouseKeeper.Environment
{
  public class InteractableState : InteractableBehaviour
  {
      [SerializeField] Condition turnOnCondition;
      [SerializeField] Condition turnOffCondition;

      int state;
      public override bool Interact(out int newState)
      {
          newState = state;
          switch (state)
          {
              case 0:
                  if (!turnOnCondition.IsMet()) return false;

                  state = newState = 1;
                  return true;

              case 1:
                  if (!turnOffCondition.IsMet()) return false;

                  state = newState = 0;
                  return true;

                default:
                    throw new ArgumentOutOfRangeException();
          }
      }

      protected override void Initialize(int state)
      {
          this.state = state;
      }
  }
}
