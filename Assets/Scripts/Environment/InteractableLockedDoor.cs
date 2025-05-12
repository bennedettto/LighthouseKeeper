using System;
using System.Collections;
using LighthouseKeeper.GameStates;
using UnityEngine;

namespace LighthouseKeeper.Environment
{
  public class InteractableLockedDoor : InteractableBehaviour
  {
      [Serializable]
      enum State
      {
          Locked = 0,
          Unlocked = 1,
          Open = 2,
      }

      [NonSerialized] State state;
      [SerializeField] float openAngle = 90f;
      [SerializeField] float closeAngle = 0f;
      [SerializeField] float speed = 2f;
      [SerializeField] AudioClip unlockSound;
      [SerializeField] AudioClip lockedSound;
      [SerializeField] Transform lockTransform;
      [SerializeField] AudioClip openSound;
      [SerializeField] AudioClip closeSound;
      [SerializeField] Transform target;

      [SerializeField] Condition unlockCondition;

      Transform Target => target == null ? transform : target;

      float Angle => state switch
      {
          State.Locked   => closeAngle,
          State.Unlocked => closeAngle,
          State.Open     => openAngle,
          _              => throw new ArgumentOutOfRangeException()
      };


      public override bool Interact(out int newState)
      {
          switch (state)
          {
              case State.Locked when unlockCondition.IsMet():
                  state = State.Unlocked;
                  StartCoroutine(DelayedOpenCoroutine());
                  AudioSystem.Instance.Play(unlockSound, AudioSystem.Type.SFX);
                  break;

              case State.Locked: // when !unlockCondition.IsMet()
                  StartCoroutine(VibrateLockCoroutine());
                  AudioSystem.Instance.Play(lockedSound, AudioSystem.Type.SFX);
                  newState = (int)state;
                  return false;

              case State.Unlocked:
                  Open();
                  break;

              case State.Open:
                  state = State.Unlocked;
                  StartCoroutine(MoveDoorCoroutine());
                  AudioSystem.Instance.Play(closeSound, AudioSystem.Type.SFX);
                  break;

              default:
                  throw new ArgumentOutOfRangeException();
          }

          newState = (int)state;
          return true;
      }


      void Open()
      {
          state = State.Open;
          StartCoroutine(MoveDoorCoroutine());
          AudioSystem.Instance.Play(openSound, AudioSystem.Type.SFX);
      }


      IEnumerator DelayedOpenCoroutine()
      {
          yield return new WaitForSeconds(0.3f);

          Open();
      }


      IEnumerator VibrateLockCoroutine()
      {
          Vector3 start = lockTransform.localRotation.eulerAngles;
          Vector3 end = start;
          start.z -= 13f;
          end.z += 13f;

          for (float t = 0; t <= 1.1f; t += Time.deltaTime)
          {
              lockTransform.localRotation = Quaternion.Euler(
                    Vector3.Lerp(start, end, Mathf.PingPong(10 * t, 1)));
              yield return null;
          }
          lockTransform.localRotation = Quaternion.identity;
      }


      protected override void Initialize(int state)
      {
          this.state = (State)state;
          if (state == 0) return;
          transform.localRotation = Quaternion.Euler(0, Angle, 0);
      }


      IEnumerator MoveDoorCoroutine()
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
