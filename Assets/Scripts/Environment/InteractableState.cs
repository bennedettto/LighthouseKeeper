using LighthouseKeeper.GameStates;
using UnityEngine;

namespace LighthouseKeeper.Environment
{
  public class InteractableState : InteractableBehaviour
  {
      [SerializeField] Condition turnOnCondition;
      [SerializeField] Condition turnOffCondition;

      int state;
      public override int Interact()
      {
          if (state == 0) TryTurnOn();
          else TryTurnOff();

          return state;
      }

      void TryTurnOff()
      {
          if (!turnOffCondition.IsMet()) return;

          state = 0;
      }

      void TryTurnOn()
      {
          if (!turnOnCondition.IsMet()) return;

          state = 1;
      }

      protected override void Initialize(int state)
      {
          this.state = state;
      }
  }
}
