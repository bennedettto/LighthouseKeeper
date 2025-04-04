using UnityEngine;

namespace LighthouseKeeper
{
    public static class Utility
    {
        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }
    }
}