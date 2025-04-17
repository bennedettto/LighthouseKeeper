using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LighthouseKeeper
{
  [RequireComponent(typeof(CharacterController))]
  public class PlayerFootsteps : MonoBehaviour
  {
    public enum GroundType
    {
      Concrete,
      Gravel,
      Sand,
    }

    [Serializable]
    public struct FootstepSound
    {
      [SerializeField]
      public AudioClip[] walkingClips;

      [SerializeField]
      public AudioClip[] runningClips;
    }

    public static PlayerFootsteps Instance;

    const float SPEED_THRESHOLD = 0.05f;
    [SerializeField] CharacterController controller;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    float footstepDistance = .3f;

    float distanceTraveled;

    [SerializeField, Range(-1,1)]
    float volume = .1f;

    [SerializeField, Range(0, 1)]
    float randomizedVolume = 0.2f;

    [SerializeField, Range(0, 1)]
    float speedSoundScale = 0.1f;

    public GroundType groundType = GroundType.Gravel;

    [SerializeField]
    FootstepSound gravelSounds;

    [SerializeField]
    FootstepSound concreteSounds;

    [SerializeField]
    FootstepSound sandSounds;


    void Awake()
    {
      Instance = this;
    }


    void Update()
    {
      var velocity = controller.velocity;
      velocity.y = 0;

      FootStepUpdate(velocity.magnitude);
    }


    void FootStepUpdate(float speed)
    {
      if (speed < SPEED_THRESHOLD) return;
      if (!controller.isGrounded) return;


      distanceTraveled += Time.deltaTime * speed;
      if (distanceTraveled < footstepDistance) return;

      distanceTraveled -= footstepDistance;
      PlayFootstepSound(speed);
    }


    void PlayFootstepSound(float speed)
    {
      var footSounds = groundType switch
      {
        GroundType.Concrete => concreteSounds,
        GroundType.Gravel   => gravelSounds,
        GroundType.Sand     => sandSounds,
        _                   => throw new ArgumentOutOfRangeException(),
      };

      var footstepClips = speed > 7.5f ? footSounds.runningClips : footSounds.walkingClips;

      if (footstepClips.Length == 0) return;

      int index = Random.Range(0, footstepClips.Length);
      float thisVolume = volume
                         + volume * Random.Range(-randomizedVolume, randomizedVolume)
                         + speed * speedSoundScale;
      if (thisVolume < 0f) return;

      audioSource.PlayOneShot(footstepClips[index],
                              thisVolume);
    }
  }
}