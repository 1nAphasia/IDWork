using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;

namespace ScriptableTest
{
    public class RadialBlurVolumeComponent : VolumeComponent
    {
        public ClampedFloatParameter BlurRadius = new ClampedFloatParameter(0f, 0f, 1f);
        public ClampedIntParameter Iteration = new ClampedIntParameter(10, 2, 30);
        public ClampedFloatParameter RadialCenterX = new ClampedFloatParameter(0.5f, 0f, 1f);
        public ClampedFloatParameter RadialCenterY = new ClampedFloatParameter(0.5f, 0f, 1f);
    }
}

namespace ScriptableTest
{
    public class RadialBlur02 : ScriptableRendererFeature
    {
        [System.Serializable]
        public class Setting
        {
            public Material material;
            public RenderPassEvent passEvent = RenderPassEvent.AfterRenderingTransparents;
            private FloatParameter BlurRadius = new ClampedFloatParameter(0f, 0f, 1f);
            private IntParameter Iteration = new ClampedIntParameter(10, 2, 30);
            private FloatParameter RadialCenterX = new ClampedFloatParameter(0.5f, 0f, 1f);
            private FloatParameter RadialCenterY = new ClampedFloatParameter(0.5f, 0f, 1f);
        }

        public Setting m_setting = new Setting();

        class RadialBlurPass : ScriptableRenderPass
        {
            readonly Setting _setting;
            //接入VolumeComponent
            private RadialBlurVolumeComponent _VolumeComponent;

            public RadialBlurPass(Setting setting)
            {
                this._setting = setting;
            }

            //申明一些Shader的属性ID，方便在Shader中访问
            static class ShaderIDs
            {
                internal static readonly string _BlurTextureName = "_BlurTexture";
                internal static readonly string _RadialPassName = "RadialBlurRenderPass";
                internal static readonly int Params = Shader.PropertyToID("_Params");
            }

            // PassData 类用于在 RenderGraph 中传递此 Pass 所需的数据
            private class PassData
            {
                internal TextureHandle _Source;
                internal TextureHandle _Target;
                internal Material _Material;
                internal Vector4 _Params;
            }

            // 重写 RecordRenderGraph 方法，这是 RenderGraph API 的核心
            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                // 访问 VolumeManager 以获取当前相机的 VolumeStack。
                // VolumeStack 包含所有活动 Volume Component 的组合状态。
                var strack = VolumeManager.instance.stack;
                // 从堆栈中获取我们的自定义 Volume 组件。
                // 这使得效果的参数可以通过场景中的Volume进行控制。
                _VolumeComponent = strack.GetComponent<RadialBlurVolumeComponent>();

                //设置材质参数
                Vector4 shaderParams = new Vector4(_VolumeComponent.BlurRadius.value * 0.02f, _VolumeComponent.Iteration.value, _VolumeComponent.RadialCenterX.value, _VolumeComponent.RadialCenterY.value);

                // 从 frameData 中获取 URP 的通用资源数据和相机数据
                UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
                UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

                // 获取相机目标纹理的描述符
                var desc = cameraData.cameraTargetDescriptor;
                desc.msaaSamples = 1;
                desc.depthBufferBits = 0;

                // 创建 PassData 实例，用于在 RenderGraph 中传递数据
                var passData = new PassData();

                // 设置输入源纹理为当前激活的相机颜色纹理
                passData._Source = resourceData.activeColorTexture;

                // 使用 RenderGraph 创建一个临时渲染纹理
                // ShaderIDs._BlurTextureName 是这个纹理在 RenderGraph 中的唯一标识名
                passData._Target = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc, ShaderIDs._BlurTextureName, false);
                passData._Material = _setting.material;

                renderGraph.AddBlitPass(passData._Source, passData._Target, Vector2.one, Vector2.zero, passName: "myBlit");

                //传递材质参数
                passData._Material.SetVector(ShaderIDs.Params, shaderParams);

                // 使用 RenderGraph 的 AddRasterRenderPass 添加一个光栅化渲染通道
                using (var builder = renderGraph.AddRasterRenderPass<PassData>(passName, out _))
                {
                    // 声明此 Pass 会读取 _Target 纹理
                    builder.UseTexture(passData._Target);
                    // 设置渲染目标为 _Source 纹理
                    builder.SetRenderAttachment(passData._Source, 0);

                    // 设置实际的渲染函数
                    builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
                    {
                        // 从 RasterGraphContext 获取 RasterCommandBuffer
                        // 拿到cmd后和之前类似，但有些功能缺失，比如DispatchCompute只能在renderGraph.AddComputePass的context中获取
                        RasterCommandBuffer cmd = context.cmd;

                        // 使用 Blitter.BlitTexture 执行全屏的材质绘制操作
                        Blitter.BlitTexture(cmd, passData._Target, new Vector4(1, 1, 0, 0), passData._Material, 0);
                    });
                }
            }
        }


        RadialBlurPass m_ScriptablePass;

        //验证材质有效性
        private bool IsMaterialValid =>
            m_setting.material && m_setting.material.shader && m_setting.material.shader.isSupported;

        public override void Create()
        {
            if (!IsMaterialValid)
            {
                Debug.Log("Return");
                return; ;
            }
            m_ScriptablePass = new RadialBlurPass(m_setting);
            m_ScriptablePass.renderPassEvent = m_setting.passEvent;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (m_ScriptablePass == null)
            {
                return;
            }
            renderer.EnqueuePass(m_ScriptablePass);
        }

    }
}