using System.Collections;
using System.Collections.Generic;
using UniStorm;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UniStormCloudShadowsPass : ScriptableRenderPass
{
    public UniStormCloudShadowsFeature.Settings settings;

    // Replace RenderTargetHandle and RenderTargetIdentifier with RTHandle
    RTHandle source;
    RTHandle destination;

    Material screenSpaceShadowsMaterial;

    string m_ProfilerTag;

    public UniStormCloudShadowsPass(string tag)
    {
        m_ProfilerTag = tag;
        Shader unistormCloudShadowsShader = Shader.Find("UniStorm/URP/UniStormCloudShadows");
#if UNITY_EDITOR
        if (unistormCloudShadowsShader == null) return;
#endif
        screenSpaceShadowsMaterial = new Material(unistormCloudShadowsShader);
    }

    [System.Obsolete]
    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        base.Configure(cmd, cameraTextureDescriptor);

        renderPassEvent = settings.renderPassEvent;

        // Allocate the RTHandle for the destination
        destination = RTHandles.Alloc(cameraTextureDescriptor, name: "_DestinationRT");
    }

    public void Setup(RTHandle cameraColorTargetHandle)
    {
        // Assign the source RTHandle
        source = cameraColorTargetHandle;
    }

    [System.Obsolete]
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
#if UNITY_EDITOR
        if (screenSpaceShadowsMaterial == null || UnityEditor.EditorApplication.isPaused) return;
#endif
        CameraData cameraData = renderingData.cameraData;
        Camera camera = cameraData.camera;

        //Skip execution for the Scene View camera or if not in runtime
        if (cameraData.cameraType == CameraType.SceneView || !Application.isPlaying) return;

        CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
        cmd.Clear();

        // Set shader properties
        screenSpaceShadowsMaterial.SetMatrix(ShaderConstants.camToWorld, camera.cameraToWorldMatrix);
        screenSpaceShadowsMaterial.SetTexture(ShaderConstants.cloudTex, settings.CloudShadowTexture);
        screenSpaceShadowsMaterial.SetFloat(ShaderConstants.cloudTexScale, settings.CloudTextureScale + (settings.m_CurrentCloudHeight * 0.000001f) * 2);
        screenSpaceShadowsMaterial.SetFloat(ShaderConstants.bottomThreshold, settings.BottomThreshold);
        screenSpaceShadowsMaterial.SetFloat(ShaderConstants.topThreshold, settings.TopThreshold);
        screenSpaceShadowsMaterial.SetFloat(ShaderConstants.cloudShadowIntensity, settings.ShadowIntensity);
        screenSpaceShadowsMaterial.SetFloat(ShaderConstants.cloudMovementSpeed, settings.CloudSpeed * -0.005f);
        screenSpaceShadowsMaterial.SetVector(ShaderConstants.sunDirection,
                                             new Vector3(settings.ShadowDirection.x, settings.ShadowDirection.y,
                                                         settings.ShadowDirection.z));
        screenSpaceShadowsMaterial.SetFloat(ShaderConstants.fade, settings.Fade);
        screenSpaceShadowsMaterial.SetFloat(ShaderConstants.normalY, settings.NormalY);

        // Execute the shader on input texture (source) and write to output (destination)
        Blitter.BlitCameraTexture(cmd, source, destination, screenSpaceShadowsMaterial, 0);
        Blitter.BlitCameraTexture(cmd, destination, source);

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        // Release the RTHandle
        if (destination != null)
        {
            destination.Release();
            destination = null;
        }
    }
}
