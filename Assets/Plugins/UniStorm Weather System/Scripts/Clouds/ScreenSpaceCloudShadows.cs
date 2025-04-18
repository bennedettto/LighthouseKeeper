using UnityEngine;
using System.Collections;

namespace UniStorm.Effects
{
    //[ExecuteInEditMode]
    public class ScreenSpaceCloudShadows : MonoBehaviour
    {
        [HideInInspector]
        public float Fade = 0.33f;
        [HideInInspector]
        public RenderTexture CloudShadowTexture;
        [HideInInspector]
        public Color ShadowColor = Color.white;
        [HideInInspector]
        public float CloudTextureScale = 0.1f;
        [HideInInspector]
        [Range(0, 1)]
        public float BottomThreshold = 0f;
        [HideInInspector]
        [Range(0, 1)]
        public float TopThreshold = 1f;
        [HideInInspector]
        public float ShadowIntensity = 1f;
        [HideInInspector]
        public Material ScreenSpaceShadowsMaterial;
        [HideInInspector]
        public Vector3 ShadowDirection;


        void OnEnable()
        {
            //Dynamically create a material that will use the Screenspace cloud shader
            ScreenSpaceShadowsMaterial = new Material(Shader.Find("UniStorm/Celestial/Screen Space Cloud Shadows"));

            //Set the camera to render depth and normals
            GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
        }

        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (Application.isPlaying && UniStormSystem.Instance.UniStormInitialized)
            {
                //Set shader properties
                ScreenSpaceShadowsMaterial.SetMatrix(ShaderConstants.camToWorld, UniStormSystem.Instance.PlayerCamera.cameraToWorldMatrix);
                ScreenSpaceShadowsMaterial.SetTexture(ShaderConstants.cloudTex, CloudShadowTexture);
                ScreenSpaceShadowsMaterial.SetFloat(ShaderConstants.cloudTexScale, CloudTextureScale + (UniStormSystem.Instance.m_CurrentCloudHeight * 0.000001f) * 2);
                ScreenSpaceShadowsMaterial.SetFloat(ShaderConstants.bottomThreshold, BottomThreshold);
                ScreenSpaceShadowsMaterial.SetFloat(ShaderConstants.topThreshold, TopThreshold);
                ScreenSpaceShadowsMaterial.SetFloat(ShaderConstants.cloudShadowIntensity, ShadowIntensity);
                ScreenSpaceShadowsMaterial.SetFloat(ShaderConstants.cloudMovementSpeed, UniStormSystem.Instance.CloudSpeed * -0.005f);
                ScreenSpaceShadowsMaterial.SetVector(ShaderConstants.sunDirection, new Vector3(ShadowDirection.x, ShadowDirection.y, ShadowDirection.z));
                ScreenSpaceShadowsMaterial.SetFloat(ShaderConstants.fade, Fade);

                //Execute the shader on input texture (src) and write to output (dest)
                Graphics.Blit(src, dest, ScreenSpaceShadowsMaterial);
            }
        }
    }
}