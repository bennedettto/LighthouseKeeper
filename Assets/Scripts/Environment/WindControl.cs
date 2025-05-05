using UnityEngine;
using UnityEngine.VFX;

namespace LighthouseKeeper.Environment
{
    public class WindControl : MonoBehaviour
    {
        static readonly int windHash = Shader.PropertyToID("Wind");

        [SerializeField] VisualEffect vfx;

        // perlin is between 0.1 and 0.8
        static float WindDirectionComponent(float t) => Mathf.Clamp01(Mathf.InverseLerp(0.2f, 0.8f, t)) - 0.5f;

        static float GetWindStrength()
        {
            float x = Mathf.PerlinNoise1D(34.24f + 0.11f * Time.time);
            return x < 0.35f ? 0f : (x - 0.35f) / 0.65f;
        }

        static Vector3 GetWindDirection()
        {
            float strength = GetWindStrength();

            if (strength < 0.01f) return Vector3.zero;

            return strength * new Vector3(WindDirectionComponent(Mathf.PerlinNoise1D(-73.1f + 0.31f * Time.time)),
                                          0f,
                                          WindDirectionComponent(Mathf.PerlinNoise1D(92.3f + 0.21f * Time.time)));
        }


        void Update()
        {
            vfx.SetVector3(windHash, GetWindDirection());
        }
    }
}