using UnityEngine;

namespace LighthouseKeeper.Environment
{
  [ExecuteAlways]
  public class FloatOnWater : MonoBehaviour
  {
    [SerializeField] float waveSpeed;
    [SerializeField] float waveScale;
    [SerializeField] float waveFrequency;
    [SerializeField] float waveCircularFrequency;

    Transform _transform;

    [SerializeField] float maxRotationX;
    [SerializeField] float maxRotationZ;
    [SerializeField] float xRotationSpeed;
    [SerializeField] float zRotationSpeed;
    float rotationX;
    float rotationDirectionX = 1;
    float rotationZ;
    float rotationDirectionZ = 1;


    void Awake()
    {
      _transform = transform;
    }


    void Update()
    {
      Vector3 worldPosition = _transform.position;
      Vector2 uv = new Vector2(worldPosition.x, worldPosition.z);

      float length = uv.magnitude * waveFrequency;
      float atan = Mathf.Atan2(uv.x, uv.y) * waveCircularFrequency;

      float dt = Time.time * waveSpeed;
      length += dt;
      atan += dt;

      float delta = waveScale * (Mathf.Sin(length) +  Mathf.Sin(atan));

      rotationX += rotationDirectionX * xRotationSpeed * Time.deltaTime;
      rotationZ += rotationDirectionZ * zRotationSpeed * Time.deltaTime;

      if (rotationX >  maxRotationX) rotationDirectionX = -1;
      if (rotationX < -maxRotationX) rotationDirectionX =  1;
      if (rotationZ >  maxRotationZ) rotationDirectionZ = -1;
      if (rotationZ < -maxRotationZ) rotationDirectionZ =  1;

      _transform.localPosition = new Vector3(0, delta, 0);
      _transform.localRotation = Quaternion.Euler(rotationX, 0, rotationZ);
    }
  }
}
