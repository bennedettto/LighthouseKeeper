using UnityEngine;

namespace LighthouseKeeper
{
  [RequireComponent(typeof(CharacterController))]
  public class PlayerFootsteps : MonoBehaviour
  {
    const float SPEED_THRESHOLD = 0.05f;
    [SerializeField] CharacterController controller;

    Transform t;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    float footstepDistance = .3f;

    float distanceTraveled;

    [SerializeField, Range(0,1)]
    float volume = .1f;

    [SerializeField, Range(0, 1)]
    float randomizedVolume = 0.2f;

    [SerializeField, Range(0, 1)]
    float speedSoundScale = 0.1f;

    [SerializeField]
    AudioClip[] footstepClips;


    void Awake()
    {
      t = transform;
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


      footstepDistance += Time.deltaTime * speed;
      if (footstepDistance < distanceTraveled) return;

      distanceTraveled = 0;
      PlayFootstepSound(speed);
    }


    void PlayFootstepSound(float speed)
    {
      int index = Random.Range(0, footstepClips.Length);
      audioSource.PlayOneShot(footstepClips[index],
                              volume + volume * Random.Range(-randomizedVolume, randomizedVolume)
                                + speed * speedSoundScale);
    }
  }
}