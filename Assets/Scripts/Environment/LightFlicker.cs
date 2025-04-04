using UnityEngine;

namespace LighthouseKeeper.Environment
{
    [RequireComponent(typeof(Light))]
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] float minIntensity = 0.5f;
        [SerializeField] float maxIntensity = 1.5f;
        [SerializeField] float frequency = 1f;

        Light _light;
        float _targetIntensity;


        void Awake()
        {
            _light = GetComponent<Light>();
            _targetIntensity = _light.intensity;
        }


        void Update()
        {
            _light.intensity = Mathf.MoveTowards(_light.intensity, _targetIntensity, Time.deltaTime * frequency);

            if (!Mathf.Approximately(_light.intensity, _targetIntensity)) return;
            _targetIntensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}
