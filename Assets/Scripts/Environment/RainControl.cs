using LighthouseKeeper.GameStates;
using UnityEngine;
using UnityEngine.VFX;

namespace LighthouseKeeper.Environment
{
    public class RainControl : MonoBehaviour
    {
        static readonly int rainIsRainingHash = GameState.GetHash("rain.IsRaining");
        static readonly int rainAmountHash = GameState.GetHash("rain.amount");

        [SerializeField] VisualEffect rainVFX;

        [SerializeField] float lightRainFrequency;
        [SerializeField] float heavyRainFrequency;


        void Awake()
        {
            HandleVFX();

            GameState.OnStateChangeKeyValue += OnStateChangeKeyValue;
        }

        void OnDestroy()
        {
            GameState.OnStateChangeKeyValue -= OnStateChangeKeyValue;
        }

        void HandleVFX()
        {
            bool isRaining = GameState.Get(rainIsRainingHash) > 0;
            if (!isRaining)
            {
                rainVFX.Stop();
                rainVFX.enabled = false;
                return;
            }

            float amount = 0.01f * GameState.Get(rainAmountHash);
            rainVFX.SetFloat("SpawnRate", Mathf.Lerp(lightRainFrequency, heavyRainFrequency, amount));
            rainVFX.SetFloat("Wind", Mathf.Lerp(0.1f, 1f, amount));
            rainVFX.enabled = true;
            rainVFX.Play();
        }

        void OnStateChangeKeyValue(int key, int value)
        {
            if (key != rainIsRainingHash && key != rainAmountHash) return;

            HandleVFX();
        }
    }
}