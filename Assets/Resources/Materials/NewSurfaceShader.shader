Shader "Custom/LootBeamAlphaByHeight"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,0,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Height ("Beam Height", Float) = 2
        _AlphaPower ("Alpha Power", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float yPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _Height;
            float _AlphaPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.yPos = v.vertex.y;
                return o;
            }

fixed4 frag (v2f i) : SV_Target
{
    float y01 = saturate((i.yPos + 1) / 2); // 0~1
    float alpha = (1.0 - y01)*0.5;                // 从下到上线性递减
    fixed4 col = tex2D(_MainTex, i.uv) * _Color;
    col.a *= alpha;
    return col;
}
            ENDCG
        }
    }
}