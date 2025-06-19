using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;


public class FrostedGlassBlurFeature : ScriptableRendererFeature
{

    public class FrostedGlassBlurPass : ScriptableRenderPass
    {

        private RTHandle tgtHandle;
        private RTHandle tmpHandle;

        private Material blurMaterial;
        public FrostedGlassBlurPass(Settings settings)
        {
            blurMaterial = settings.blurMaterial;
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
            tgtHandle = RTHandles.Alloc(settings.targetTexture);
            tmpHandle = RTHandles.Alloc(settings.tempTexture);
        }

        private class PassData
        {
            internal TextureHandle source;
            internal TextureHandle target;
            public float blurSize;
            public Vector2 blurDirection;
            public Material blurMaterial;

        }
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (UIManager.Instance == null)
                return;
            var resources = frameData.Get<UniversalResourceData>();
            var cameraData = frameData.Get<UniversalCameraData>();

            var desc = cameraData.cameraTargetDescriptor;
            desc.msaaSamples = 1;

            // TextureHandle targetHandle = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc, "Blur Texture", false);
            TextureHandle targetHandle = renderGraph.ImportTexture(tgtHandle);
            TextureHandle tempHandle = renderGraph.ImportTexture(tmpHandle);
            Vector4 texelSize = new Vector4(
               1.0f / Screen.width,
               1.0f / Screen.height,
               Screen.width,
               Screen.height
           );


            renderGraph.AddBlitPass(resources.activeColorTexture, targetHandle, Vector2.one, Vector2.zero, passName: "MyBlit");

            using (var builder = renderGraph.AddRasterRenderPass<PassData>("Horizontal Blur Pass", out var passData))
            {
                passData.source = targetHandle;
                passData.target = tempHandle;
                passData.blurMaterial = blurMaterial;
                passData.blurSize = UIManager.Instance.blurSize;
                passData.blurDirection = new Vector2(1, 0);
                builder.UseTexture(passData.source);
                builder.SetRenderAttachment(passData.target, 0);
                builder.SetRenderFunc((PassData data, RasterGraphContext ctx) =>
                {
                    data.blurMaterial.SetFloat("_BlurSize", passData.blurSize);
                    data.blurMaterial.SetVector("_BlurDirection", passData.blurDirection);
                    data.blurMaterial.SetVector("_MainTex_TexelSize", texelSize);
                    // 执行模糊操作
                    Blitter.BlitTexture(
                        ctx.cmd,
                        data.source,
                        new Vector4(1, 1, 0, 0),
                        data.blurMaterial,
                        0
                    );

                });
            }

            using (var builder = renderGraph.AddRasterRenderPass<PassData>("Vertical Blur Pass", out var passData))
            {
                passData.source = tempHandle;
                passData.target = targetHandle;
                passData.blurMaterial = blurMaterial;
                passData.blurSize = UIManager.Instance.blurSize;
                passData.blurDirection = new Vector2(0, 1);
                builder.UseTexture(passData.source);
                builder.SetRenderAttachment(passData.target, 0);
                builder.SetRenderFunc((PassData data, RasterGraphContext ctx) =>
                {
                    data.blurMaterial.SetFloat("_BlurSize", passData.blurSize);
                    data.blurMaterial.SetVector("_BlurDirection", passData.blurDirection);
                    data.blurMaterial.SetVector("_MainTex_TexelSize", texelSize);
                    // 执行模糊操作
                    Blitter.BlitTexture(
                        ctx.cmd,
                        data.source,
                        new Vector4(1, 1, 0, 0),
                        data.blurMaterial,
                        0
                    );
                });
            }


        }
    }

    [System.Serializable]
    public class Settings
    {
        public Material blurMaterial; // 把自己写好的“Unlit/Blur” Shader 做成 Material，拖到这里
        public RenderTexture targetTexture;
        public RenderTexture tempTexture;
    }
    public Settings settings = new Settings();
    FrostedGlassBlurPass blurPass;
    // 在 Pipeline Asset 加载 / 每次脚本编译后调用
    public override void Create()
    {
        blurPass = new FrostedGlassBlurPass(settings);
    }

    // 每帧渲染时，URP 会调用这里把 Pass 挂进去
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (UIManager.Instance != null)
        {
            renderer.EnqueuePass(blurPass);
        }
    }

}
