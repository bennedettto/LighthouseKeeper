using System;
using System.Collections;
using UnityEngine;

namespace LighthouseKeeper.Environment
{
  public class InteractableDoor : InteractableBehaviour
  {
      [Serializable]
      enum State
      {
          HalfOpen = 0,
          Open = 1,
          Closed = 2,
      }

      [NonSerialized] State state;
      [SerializeField] float openAngle = 90f;
      [SerializeField] float closeAngle = 0f;
      [SerializeField] float speed = 2f;
      [SerializeField] AudioClip openSound;
      [SerializeField] AudioClip closeSound;
      [SerializeField] Transform target;

      Transform Target => target == null ? transform : target;

      float Angle => state switch
      {
          State.HalfOpen => (1f * openAngle + 3f * closeAngle) / 4f,
          State.Open => openAngle,
          _ => closeAngle,
      };


      public override bool Interact(out int newState)
      {
          state = state switch
          {
              State.Open => State.Closed,
              _          => State.Open,
          };

          StartCoroutine(MoveCoroutine());
          AudioSystem.Instance.Play(state == State.Open ? openSound : closeSound, AudioSystem.Type.SFX);
          newState = (int)state;
          return true;
      }

      protected override void Initialize(int state)
      {
          this.state = (State)state;
          if (state == 0) return;
          transform.localRotation = Quaternion.Euler(0, Angle, 0);
      }

      IEnumerator MoveCoroutine()
      {
            Quaternion targetRotation = Quaternion.Euler(0, Angle, 0);
            Quaternion rotation = Target.localRotation;

            for (;;)
            {
                rotation = Quaternion.RotateTowards(rotation, targetRotation, speed * Time.deltaTime);
                Target.localRotation = rotation;

                if (Quaternion.Angle(rotation, targetRotation) < 0.1f) break;
                yield return null;
            }

            Target.localRotation = targetRotation;
      }
  }
}
