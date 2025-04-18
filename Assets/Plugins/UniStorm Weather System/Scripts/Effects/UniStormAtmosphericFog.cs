using System;
using UnityEngine;

namespace UniStorm.Effects
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    public class UniStormAtmosphericFog : UniStormPostEffectsBase
	{
        [HideInInspector]
        public Texture2D NoiseTexture = null;
        public enum DitheringControl { Enabled, Disabled };
        [HideInInspector]
        public DitheringControl Dither = DitheringControl.Enabled;

        [HideInInspector]
        public Light SunSource;
        [HideInInspector]
        public Light MoonSource;
        [HideInInspector]
        public bool  distanceFog = true;
        //[HideInInspector]
        public bool  useRadialDistance = false;
		[HideInInspector]
		public bool  heightFog = false;
        [HideInInspector]
        public float height = 1.0f;
        [HideInInspector]
        public float heightDensity = 2.0f;
        //[HideInInspector]
        public float startDistance = 0.0f;

        [HideInInspector]
        public Shader fogShader = null;
        public Material fogMaterial = null;
        [HideInInspector]
        public Color SunColor =  new Color(1, 0.63529f, 0);
        [HideInInspector]
        public Color MoonColor = new Color(1, 0.63529f, 0);
        [HideInInspector]
        public Color TopColor;
        [HideInInspector]
        public Color BottomColor;
        //[HideInInspector]
        [Range(0.0f, 1.0f)]
        public float BlendHeight = 0.03f;
        [HideInInspector]
        [Range(0.0f, 1.0f)]
        public float FogGradientHeight = 0.5f;
        //[HideInInspector]
        [Range(0.0f, 3.0f)]
        public float SunIntensity = 2;
        [HideInInspector]
        [Range(0.0f, 3.0f)]
        public float MoonIntensity = 1;
        //[HideInInspector]
        [Range(1.0f, 60.0f)]
        public float SunFalloffIntensity = 9.4f;
        //[HideInInspector]
        public float SunControl = 1;
        [HideInInspector]
        public float MoonControl = 1;

        public override bool CheckResources ()
		{
            CheckSupport (true);

            fogMaterial = CheckShaderAndCreateMaterial (fogShader, fogMaterial);

            if (!isSupported)
                ReportAutoDisable ();
            return isSupported;
        }

        [ImageEffectOpaque]
        void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
            if (CheckResources()==false || (!distanceFog && !heightFog))
            {
                Graphics.Blit (source, destination);
                return;
            }

			Camera cam = GetComponent<Camera>();
			Transform camtr = cam.transform;
			float camNear = cam.nearClipPlane;
			float camFar = cam.farClipPlane;
			float camFov = cam.fieldOfView;
			float camAspect = cam.aspect;

            Matrix4x4 frustumCorners = Matrix4x4.identity;

			float fovWHalf = camFov * 0.5f;

			Vector3 toRight = camtr.right * camNear * Mathf.Tan (fovWHalf * Mathf.Deg2Rad) * camAspect;
			Vector3 toTop = camtr.up * camNear * Mathf.Tan (fovWHalf * Mathf.Deg2Rad);

			Vector3 topLeft = (camtr.forward * camNear - toRight + toTop);
			float camScale = topLeft.magnitude * camFar/camNear;

            topLeft.Normalize();
			topLeft *= camScale;

			Vector3 topRight = (camtr.forward * camNear + toRight + toTop);
            topRight.Normalize();
			topRight *= camScale;

			Vector3 bottomRight = (camtr.forward * camNear + toRight - toTop);
            bottomRight.Normalize();
			bottomRight *= camScale;

			Vector3 bottomLeft = (camtr.forward * camNear - toRight - toTop);
            bottomLeft.Normalize();
			bottomLeft *= camScale;

            frustumCorners.SetRow (0, topLeft);
            frustumCorners.SetRow (1, topRight);
            frustumCorners.SetRow (2, bottomRight);
            frustumCorners.SetRow (3, bottomLeft);

			var camPos= camtr.position;
            float FdotC = camPos.y-height;
            float paramK = (FdotC <= 0.0f ? 1.0f : 0.0f);
            fogMaterial.SetMatrix(ShaderConstants.frustumCornersWs, frustumCorners);
            fogMaterial.SetVector(ShaderConstants.cameraWs, camPos);
            fogMaterial.SetVector(ShaderConstants.heightParams, new Vector4 (height, FdotC, paramK, heightDensity*0.5f));
            fogMaterial.SetVector(ShaderConstants.distanceParams, new Vector4 (-Mathf.Max(startDistance,0.0f), 0, 0, 0));

            fogMaterial.SetVector(ShaderConstants.sunVector, SunSource.transform.rotation * -Vector3.forward);
            fogMaterial.SetVector(ShaderConstants.moonVector, MoonSource.transform.rotation * -Vector3.forward);
            fogMaterial.SetFloat(ShaderConstants.sunIntensity, SunIntensity);
            fogMaterial.SetFloat(ShaderConstants.moonIntensity, MoonIntensity);
            fogMaterial.SetFloat(ShaderConstants.sunAlpha, SunFalloffIntensity);
            fogMaterial.SetColor(ShaderConstants.sunColor, SunColor);
            fogMaterial.SetColor(ShaderConstants.moonColor, MoonColor);

            fogMaterial.SetColor(ShaderConstants.upperColor, TopColor);
            fogMaterial.SetColor(ShaderConstants.bottomColor, BottomColor);
            fogMaterial.SetFloat(ShaderConstants.fogBlendHeight, BlendHeight);
            fogMaterial.SetFloat(ShaderConstants.fogGradientHeight, FogGradientHeight);

            fogMaterial.SetFloat(ShaderConstants.sunControl, SunControl);
            fogMaterial.SetFloat(ShaderConstants.moonControl, MoonControl);

            if (Dither == DitheringControl.Enabled)
            {
                fogMaterial.SetFloat(ShaderConstants.enableDithering, 1);
                fogMaterial.SetTexture(ShaderConstants.noiseTex, NoiseTexture);
            }
            else
            {
                fogMaterial.SetFloat(ShaderConstants.enableDithering, 0);
            }

            var sceneMode = RenderSettings.fogMode;
            var sceneDensity= RenderSettings.fogDensity;
            var sceneStart= RenderSettings.fogStartDistance;
            var sceneEnd= RenderSettings.fogEndDistance;
            Vector4 sceneParams;
            bool  linear = (sceneMode == FogMode.Linear);
            float diff = linear ? sceneEnd - sceneStart : 0.0f;
            float invDiff = Mathf.Abs(diff) > 0.0001f ? 1.0f / diff : 0.0f;
            sceneParams.x = sceneDensity * 1.2011224087f;
            sceneParams.y = sceneDensity * 1.4426950408f;
            sceneParams.z = linear ? -invDiff : 0.0f;
            sceneParams.w = linear ? sceneEnd * invDiff : 0.0f;
            fogMaterial.SetVector (ShaderConstants.sceneFogParams, sceneParams);
			fogMaterial.SetVector (ShaderConstants.sceneFogMode, new Vector4((int)sceneMode, useRadialDistance ? 1 : 0, 0, 0));

            int pass = 0;
            if (distanceFog && heightFog)
                pass = 0; // distance + height
            else if (distanceFog)
                pass = 1; // distance only
            else
                pass = 2; // height only
            CustomGraphicsBlit (source, destination, fogMaterial, pass);
        }

        static void CustomGraphicsBlit (RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
		{
            RenderTexture.active = dest;

            fxMaterial.SetTexture (ShaderConstants.mainTex, source);

            GL.PushMatrix ();
            GL.LoadOrtho ();

            fxMaterial.SetPass (passNr);

            GL.Begin (GL.QUADS);

            GL.MultiTexCoord2 (0, 0.0f, 0.0f);
            GL.Vertex3 (0.0f, 0.0f, 3.0f); // BL

            GL.MultiTexCoord2 (0, 1.0f, 0.0f);
            GL.Vertex3 (1.0f, 0.0f, 2.0f); // BR

            GL.MultiTexCoord2 (0, 1.0f, 1.0f);
            GL.Vertex3 (1.0f, 1.0f, 1.0f); // TR

            GL.MultiTexCoord2 (0, 0.0f, 1.0f);
            GL.Vertex3 (0.0f, 1.0f, 0.0f); // TL

            GL.End ();
            GL.PopMatrix ();
        }
    }
}
