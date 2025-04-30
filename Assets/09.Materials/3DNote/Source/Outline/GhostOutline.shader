Shader "Custom/OutlineOnly_TransparentSafe"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Float) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Cull Front       // 내부는 제거
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = normalize(v.normal);
                v.vertex.xyz += norm * _OutlineWidth; // 바깥쪽으로 확장
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}