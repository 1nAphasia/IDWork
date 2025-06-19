Shader "Unlit/GaussianBlurSeparable"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 1.0
        // _BlurDirection ("Blur Direction", Vector) = (1,0,0,0) // 在脚本中设置
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Name "GAUSSIAN_BLUR_PASS"
            // 不需要深度写入或测试，因为我们是全屏后处理
            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl" // For ConvertSRGBToLinear, etc. if needed

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // _MainTex_TexelSize: (1/width, 1/height, width, height)
            // Unity 会自动提供这个，如果你的纹理名为 _MainTex
            // 但在 RenderGraph 中手动设置更可靠
            float4 _MainTex_TexelSize;
            float  _BlurSize;
            float2 _BlurDirection; // (1,0) for horizontal, (0,1) for vertical

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            // 简单的高斯权重 (可以预计算或使用更精确的分布)
            // 这里为了简单，我们用一个固定的权重分布，实际应用中可能会更复杂
            // 对于5个样本点 (中心点 + 两边各2个)
            // const float weights[3] = { 0.227027, 0.316216, 0.070270 }; // 权重可以调整
            // 对于9个样本点 (中心点 + 两边各4个)
            // 权重应该加起来约等于1 (或通过归一化因子调整)
            // 这里我们使用一个简单的盒状模糊的权重近似，或者直接取平均
            // 真实高斯模糊的权重会从中心向外递减

            half4 Frag(Varyings input) : SV_Target
            {
                half4 finalColor = half4(0,0,0,0);
                float2 texelSize = _MainTex_TexelSize.xy; // 1/width, 1/height

                // 5x1 or 1x5 Kernel (示例，你可以扩展到更多tap，如7x1, 9x1)
                // 权重可以根据高斯函数计算，这里为了简单直接平均
                // 或者使用预定义的权重数组
                // const int KERNEL_RADIUS = 2; // 2 samples on each side + center
                // const int NUM_SAMPLES = KERNEL_RADIUS * 2 + 1;
                // float totalWeight = 0.0;

                // 中心点
                finalColor += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv) * 0.2270270270; // w0

                // 向一个方向采样
                finalColor += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv + _BlurDirection * texelSize * _BlurSize * 1.0) * 0.3162162162; // w1
                finalColor += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv - _BlurDirection * texelSize * _BlurSize * 1.0) * 0.3162162162; // w1

                // 更远一点
                finalColor += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv + _BlurDirection * texelSize * _BlurSize * 2.0) * 0.0702702703; // w2
                finalColor += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv - _BlurDirection * texelSize * _BlurSize * 2.0) * 0.0702702703; // w2

                // 如果权重没有预先归一化，你需要除以总权重
                // finalColor /= totalWeight;

                return finalColor;
            }
            ENDHLSL
        }
    }
    Fallback "Hidden/Universal Render Pipeline/FallbackError"
}