using UnityEngine;

namespace UniStorm
{
    public static class ShaderConstants
    {

        public static readonly int camToWorld = Shader.PropertyToID("_CamToWorld");
        public static readonly int cloudTex = Shader.PropertyToID("_CloudTex");
        public static readonly int cloudTexScale = Shader.PropertyToID("_CloudTexScale");
        public static readonly int bottomThreshold = Shader.PropertyToID("_BottomThreshold");
        public static readonly int topThreshold = Shader.PropertyToID("_TopThreshold");
        public static readonly int cloudShadowIntensity = Shader.PropertyToID("_CloudShadowIntensity");
        public static readonly int cloudMovementSpeed = Shader.PropertyToID("_CloudMovementSpeed");
        public static readonly int sunDirection = Shader.PropertyToID("_SunDirection");
        public static readonly int fade = Shader.PropertyToID("_Fade");


        public static string unistormCloudsRendererFeatureName = "UniStormCloudsRendererFeature";
        public static string cloudShadowFeatureName = "UniStormCloudShadowsFeature";
        public static string atmosphericFogFeatureName = "UniStormAtmosphericFogFeature";
        public static string sunShaftsFeatureName = "SunUniStormSunShaftsFeature";
        public static string moonShaftsFeatureName = "MoonUniStormSunShaftsFeature";


        public static readonly int frustumCornersWsArray = Shader.PropertyToID("_FrustumCornersWSArray");
        public static readonly int cameraWs = Shader.PropertyToID("_CameraWS");
        public static readonly int heightParams = Shader.PropertyToID("_HeightParams");
        public static readonly int distanceParams = Shader.PropertyToID("_DistanceParams");
        public static readonly int sunVector = Shader.PropertyToID("_SunVector");
        public static readonly int moonVector = Shader.PropertyToID("_MoonVector");
        public static readonly int sunIntensity = Shader.PropertyToID("_SunIntensity");
        public static readonly int moonIntensity = Shader.PropertyToID("_MoonIntensity");
        public static readonly int sunAlpha = Shader.PropertyToID("_SunAlpha");
        public static readonly int sunColor = Shader.PropertyToID("_SunColor");
        public static readonly int moonColor = Shader.PropertyToID("_MoonColor");
        public static readonly int upperColor = Shader.PropertyToID("_UpperColor");
        public static readonly int bottomColor = Shader.PropertyToID("_BottomColor");
        public static readonly int fogBlendHeight = Shader.PropertyToID("_FogBlendHeight");
        public static readonly int fogGradientHeight = Shader.PropertyToID("_FogGradientHeight");
        public static readonly int sunControl = Shader.PropertyToID("_SunControl");
        public static readonly int moonControl = Shader.PropertyToID("_MoonControl");
        public static readonly int enableDithering = Shader.PropertyToID("_EnableDithering");
        public static readonly int noiseTex = Shader.PropertyToID("_NoiseTex");
        public static readonly int sceneFogParams = Shader.PropertyToID("_SceneFogParams");
        public static readonly int sceneFogMode = Shader.PropertyToID("_SceneFogMode");

        public static readonly int normalY = Shader.PropertyToID("_normalY");


        public static readonly int uCloudsCoverage = Shader.PropertyToID("_uCloudsCoverage");
        public static readonly int uCloudsCoverageBias = Shader.PropertyToID("_uCloudsCoverageBias");
        public static readonly int uCloudsDensity = Shader.PropertyToID("_uCloudsDensity");
        public static readonly int uCloudsDetailStrength = Shader.PropertyToID("_uCloudsDetailStrength");
        public static readonly int uCloudsBaseEdgeSoftness = Shader.PropertyToID("_uCloudsBaseEdgeSoftness");
        public static readonly int uCloudsBottomSoftness = Shader.PropertyToID("_uCloudsBottomSoftness");
        public static readonly int uSimulatedCloudAlpha = Shader.PropertyToID("_uSimulatedCloudAlpha");
        public static readonly int mainTex = Shader.PropertyToID("_MainTex");


        public static readonly int frustumCornersWs = Shader.PropertyToID("_FrustumCornersWS");

        public static readonly int blurRadius4 = Shader.PropertyToID("_BlurRadius4");
        public static readonly int sunPosition = Shader.PropertyToID("_SunPosition");
        public static readonly int threshold = Shader.PropertyToID("_SunThreshold");
        public static readonly int skybox = Shader.PropertyToID("_Skybox");
        public static readonly int color = Shader.PropertyToID("_SunColor");
        public static readonly int colorBuffer = Shader.PropertyToID("_ColorBuffer");

        public static readonly int tintColor = Shader.PropertyToID("_TintColor");
        public static readonly int uLightning = Shader.PropertyToID("_uLightning");
    }
}