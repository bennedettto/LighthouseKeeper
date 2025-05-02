using UnityEngine;

namespace LighthouseKeeper.GameStates
{
    public class GeneratorAudio : MonoBehaviour
    {
      [SerializeField]AudioClip startupClip;
      [SerializeField]AudioClip runningClip;
      [SerializeField]AudioClip shutdownClip;

      [SerializeField]AudioSource audioSource;

      [SerializeReference]
      IConditionNode condition = new Condition();

      int hash;


      bool isMet;
      bool IsMet() => condition.IsMet();


      void Awake()
      {
        CalculateHash();

        audioSource.clip = runningClip;
        audioSource.loop = true;
        audioSource.Stop();


        GameState.OnStateChangeHash += OnStateChange;

        isMet = IsMet(); // Make sure we start / shutdown the generator
        if (isMet) audioSource.Play();
      }


      void CalculateHash() => hash = condition.GetHash();



      void OnStateChange(int hash)
      {
        if ((this.hash & hash) == 0) return;

        switch (IsMet())
        {
          case true when !isMet:
            isMet = true;
            StartGeneratorSound();
            break;
          case false when isMet:
            isMet = false;
            StopGeneratorSound();
            break;
        }
      }

      void StopGeneratorSound()
      {
        audioSource.Stop();
        audioSource.PlayOneShot(shutdownClip);
      }

      void StartGeneratorSound()
      {
        audioSource.PlayOneShot(startupClip);
        audioSource.PlayDelayed(startupClip.length);
      }


      void OnDestroy()
      {
        GameState.OnStateChangeHash -= OnStateChange;
      }
    }
}