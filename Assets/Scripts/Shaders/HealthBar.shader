// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CircleBar" {
Properties {
_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
_Percentage("show the percentage", Range (0, 3.1416)) = 1 //传进来角度
}


SubShader {
Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
LOD 100

ZWrite Off
Blend SrcAlpha OneMinusSrcAlpha 

Pass {  
CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"


struct appdata_t 
{
float4 vertex : POSITION;
float2 texcoord : TEXCOORD0;
};


struct v2f 
{
float4 vertex : SV_POSITION;
half2 texcoord : TEXCOORD0;
};


sampler2D _MainTex;
float4 _MainTex_ST;
float _Percentage;

v2f vert (appdata_t v)
{
v2f o;
o.vertex = UnityObjectToClipPos(v.vertex);
o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
return o;
}

fixed4 frag (v2f i) : SV_Target
{
fixed4 col = tex2D(_MainTex, i.texcoord);
//col.a = col.a*ceil(_Percentage - i.texcoord.y);
//夹角
float jiaojiaoCos = dot(float2(i.texcoord.x - 0.5f, i.texcoord.y - 0.5f)/sqrt(pow(i.texcoord.x - 0.5f, 2) + pow(i.texcoord.y - 0.5f, 2)), float2(0, -1));
float jiajiao = acos(jiaojiaoCos);
col.a = col.a*ceil(saturate(_Percentage - jiajiao));
//if (jiajiao > _Percentage)
//{
// col.a = 0;
//}
return col;
}
ENDCG
}
}
FallBack "Diffuse"
}