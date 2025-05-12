using LighthouseKeeper.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace LighthouseKeeper.Player
{
    public class ExhaustionCameraShake : MonoBehaviour
    {
        [SerializeField] NoiseSettings exhaustedCameraNoiseSettingsCameraNoiseSettings;
        [SerializeField] NoiseSettings targetNoiseSettings;

        StaminaSystem staminaSystem;


        void Awake()
        {
            if (targetNoiseSettings.PositionNoise.Length == 0)
            {
                targetNoiseSettings.PositionNoise = new NoiseSettings.TransformNoiseParams[1]
                {
                    new NoiseSettings.TransformNoiseParams
                    {
                        X = new NoiseSettings.NoiseParams { Frequency = 0, Amplitude = 0, Constant = true },
                        Y = new NoiseSettings.NoiseParams { Frequency = 0, Amplitude = 0, Constant = true },
                        Z = new NoiseSettings.NoiseParams { Frequency = 0, Amplitude = 0, Constant = true },
                    },
                };
            }
            if (targetNoiseSettings.OrientationNoise.Length == 0)
            {
                targetNoiseSettings.OrientationNoise = new NoiseSettings.TransformNoiseParams[1]
                {
                    new NoiseSettings.TransformNoiseParams
                    {
                        X = new NoiseSettings.NoiseParams { Frequency = 0, Amplitude = 0, Constant = true },
                        Y = new NoiseSettings.NoiseParams { Frequency = 0, Amplitude = 0, Constant = true },
                        Z = new NoiseSettings.NoiseParams { Frequency = 0, Amplitude = 0, Constant = true },
                    },
                };
            }
        }


        void Start() => staminaSystem = StaminaSystem.Instance;

        void Update()
        {
            float cameraNoiseScale = staminaSystem.TiredAmount;

            var source = exhaustedCameraNoiseSettingsCameraNoiseSettings.PositionNoise[0];
            var target = targetNoiseSettings.PositionNoise[0];
            if (source.X.Frequency != 0 && source.X.Amplitude != 0)
            {
                target.X.Frequency = source.X.Frequency;
                target.X.Amplitude = source.X.Amplitude * cameraNoiseScale;
            }
            if (source.Y.Frequency != 0 && source.Y.Amplitude != 0)
            {
                target.Y.Frequency = source.Y.Frequency;
                target.Y.Amplitude = source.Y.Amplitude * cameraNoiseScale;
            }
            if (source.Z.Frequency != 0 && source.Z.Amplitude != 0)
            {
                target.Z.Frequency = source.Z.Frequency;
                target.Z.Amplitude = source.Z.Amplitude * cameraNoiseScale;
            }
            targetNoiseSettings.PositionNoise[0] = target;


            source = exhaustedCameraNoiseSettingsCameraNoiseSettings.OrientationNoise[0];
            target = targetNoiseSettings.OrientationNoise[0];
            if (source.X.Frequency != 0 && source.X.Amplitude != 0)
            {
                target.X.Frequency = source.X.Frequency;
                target.X.Amplitude = source.X.Amplitude * cameraNoiseScale;
            }
            if (source.Y.Frequency != 0 && source.Y.Amplitude != 0)
            {
                target.Y.Frequency = source.Y.Frequency;
                target.Y.Amplitude = source.Y.Amplitude * cameraNoiseScale;
            }
            if (source.Z.Frequency != 0 && source.Z.Amplitude != 0)
            {
                target.Z.Frequency = source.Z.Frequency;
                target.Z.Amplitude = source.Z.Amplitude * cameraNoiseScale;
            }
            targetNoiseSettings.OrientationNoise[0] = target;
        }
    }
}