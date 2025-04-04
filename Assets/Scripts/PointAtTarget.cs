using UnityEngine;

namespace LighthouseKeeper
{
    [ExecuteAlways]
    public class PointAtTarget : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset;

        Transform _transform;

        void Awake()
        {
            _transform = transform;
            if (target == null) enabled = false;
        }

        void Update()
        {
            _transform.rotation = Quaternion.LookRotation(target.position + offset - _transform.position, Vector3.up);
        }
    }
}