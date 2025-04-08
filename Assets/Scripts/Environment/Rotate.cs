using System;
using UnityEngine;

namespace LighthouseKeeper.Environment
{
    [ExecuteAlways]
    public class Rotate : MonoBehaviour
    {
        public enum Axis
        {
            X,
            Y,
            Z,
        }

        [SerializeField]
        float rotationSpeed = 1f;

        [SerializeField]
        Axis rotationAxis = Axis.X;

        // Update is called once per frame
        void Update()
        {
            Vector3 rotation = Vector3.zero;
            switch (rotationAxis)
            {
                case Axis.X:
                    rotation.x = rotationSpeed * Time.deltaTime;
                    break;

                case Axis.Y:
                    rotation.y = rotationSpeed * Time.deltaTime;
                    break;

                case Axis.Z:
                    rotation.z = rotationSpeed * Time.deltaTime;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            transform.Rotate(rotation);
        }
    }
}