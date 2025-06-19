//Shader部分跟之前的URP14版本保持一致
Shader "CustomEffects/Blur"
{
    HLSLINCLUDE
    
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        // The Blit.hlsl file provides the vertex shader (Vert),
        // the input structure (Attributes), and the output structure (Varyings)
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

        half4 _Params;
    
        #define _BlurRadius _Params.x
        #define _Iteration _Params.y
        #define _RadialCenter _Params.zw

        half4 GetScreenColor(float2 uv)
        {
            return SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);
        }
    
        float4 RadialBlur (Varyings i) : SV_Target
        {
            half4 acumulateColor = half4(0, 0, 0, 0);
            float2 blurVector = (_RadialCenter - i.texcoord) * _BlurRadius;
            float2 uv = i.texcoord;
            
            [unroll(30)]
            for (int j = 0; j < _Iteration; j++)
            {
                acumulateColor += GetScreenColor(uv);
                uv += blurVector;
            }
            
            return acumulateColor / _Iteration;
        }
        
    ENDHLSL
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Name "BlurPassVertical"

            HLSLPROGRAM
            
            #pragma vertex Vert
            #pragma fragment RadialBlur
            
            ENDHLSL
        }
        
    }
}