Shader "CustomRenderTexture/ColorObject"
{
      Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        LOD 200

        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;

            v2f Vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 Frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);
                return texColor * _Color;
            }
            ENDHLSL
        }
    }
}
