using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlitchRenderPass : ScriptableRendererFeature
{
    class GlitchPass : ScriptableRenderPass
    {
        private Material glitchMaterial;
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;
        public float GlitchIntensity = 0.0f;  // Default is OFF

        public GlitchPass(Material material)
        {
            glitchMaterial = material;
            tempTexture.Init("_TempGlitchTex");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            source = renderingData.cameraData.renderer.cameraColorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (glitchMaterial == null || GlitchIntensity <= 0) return;  // Skip if disabled

            CommandBuffer cmd = CommandBufferPool.Get("GlitchEffect");
            cmd.GetTemporaryRT(tempTexture.id, renderingData.cameraData.cameraTargetDescriptor);
            
            glitchMaterial.SetFloat("_GlitchIntensity", GlitchIntensity);
            Blit(cmd, source, tempTexture.Identifier(), glitchMaterial);
            Blit(cmd, tempTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    [SerializeField] private Shader glitchShader;
    private Material glitchMaterial;
    private GlitchPass glitchPass;
    
    private static GlitchRenderPass instance; // Singleton reference for external control

    public override void Create()
    {
        if (glitchShader == null) return;

        glitchMaterial = CoreUtils.CreateEngineMaterial(glitchShader);
        glitchPass = new GlitchPass(glitchMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing
        };

        instance = this;  // Store reference for control
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(glitchPass);
    }

    // ✅ Public function to control glitch effect
    public static void SetGlitch(bool enabled)
    {
        if (instance?.glitchPass == null) return;
        instance.glitchPass.GlitchIntensity = enabled ? 0.5f : 0.0f;  // Adjust intensity
    }
}
