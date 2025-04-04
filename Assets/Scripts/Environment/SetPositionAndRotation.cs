using UnityEngine;

namespace LighthouseKeeper.Environment
{
    public class OverridePositionRotation : MonoBehaviour
    {
        [SerializeField] Transform constrainedObject;
        [SerializeField] Transform target;

        void OnEnable()
        {
            constrainedObject.position = target.position;
            constrainedObject.rotation = target.rotation;
        }
    }
}