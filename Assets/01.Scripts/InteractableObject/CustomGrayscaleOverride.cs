using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomGrayscaleOverride : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private FilteringSettings filteringSettings;
        private string profilerTag;
        private RenderStateBlock renderStateBlock;
        private RenderTargetIdentifier cameraColorTarget;


        public CustomRenderPass(LayerMask layerMask)
        {
            filteringSettings = new FilteringSettings(RenderQueueRange.all, layerMask);
            renderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
        }

        public void Setup(RenderTargetIdentifier cameraColorTarget)
        {
            this.cameraColorTarget = cameraColorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);

            using (new ProfilingScope(cmd, new ProfilingSampler(profilerTag)))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                var drawingSettings = CreateDrawingSettings(new ShaderTagId("UniversalForward"), ref renderingData, SortingCriteria.CommonTransparent);
                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings, ref renderStateBlock);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [SerializeField] private LayerMask colorObjectsLayer;
    private CustomRenderPass customRenderPass;

    public override void Create()
    {
        customRenderPass = new CustomRenderPass(colorObjectsLayer)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //customRenderPass.Setup(renderer.cameraColorTargetHandle);
        renderer.EnqueuePass(customRenderPass);
    }
}