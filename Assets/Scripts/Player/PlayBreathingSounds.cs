using System;
using LighthouseKeeper.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LighthouseKeeper.Player
{
    public class PlayBreathingSounds : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;

        [SerializeField] AudioClip[] tiredBreathingSounds;
        [SerializeField] AudioClip[] exhaustedBreathingSounds;

        StaminaSystem.State currentState;

        void OnEnable()
        {
            StaminaSystem.OnStateChange += OnStaminaStateChanged;
        }
        void OnDisable()
        {
            StaminaSystem.OnStateChange -= OnStaminaStateChanged;
        }

        void OnStaminaStateChanged(StaminaSystem.State oldState, StaminaSystem.State newState)
        {
            currentState = newState;
        }

        void Update()
        {
            switch (currentState)
            {
                case StaminaSystem.State.Fresh:
                    audioSource.Stop();
                    break;

                case StaminaSystem.State.Tired:
                    audioSource.volume = StaminaSystem.Instance.TiredAmount;
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = tiredBreathingSounds[Random.Range(0, tiredBreathingSounds.Length)];
                        audioSource.Play();
                    }
                    break;

                case StaminaSystem.State.Exhausted:
                    audioSource.volume = 1f;
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = exhaustedBreathingSounds[Random.Range(0, exhaustedBreathingSounds.Length)];
                        audioSource.Play();
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}