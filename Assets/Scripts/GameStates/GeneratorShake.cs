using UnityEngine;

namespace LighthouseKeeper.GameStates
{
    public class GeneratorShake : MonoBehaviour
    {

        new Transform transform;

        void Awake()
        {
            transform = GetComponent<Transform>();
        }

        [Range(0, 0.1f)][SerializeField] float amplitude = 0.001f;
        void Update()
        {
            float x = Mathf.PerlinNoise1D(10f * Time.time);
            float y = Mathf.PerlinNoise1D(-47f + 12f * Time.time);

            transform.localPosition = new Vector3(x, y, 0) * amplitude;
        }
    }
}