Shader "Custom/FixedAngledGradient"
{
    Properties
    {
        _ColorTop ("Top Color", Color) = (0.267, 0.294, 0.498, 1)  // #444b7f
        _ColorBottom ("Bottom Color", Color) = (0.090, 0.149, 0.376, 1)  // #162660
        _Angle ("Gradient Angle (Degrees)", Range(0, 360)) = 30
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
         
        Pass
        {
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
            };

            fixed4 _ColorTop;
            fixed4 _ColorBottom;
            float _Angle;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Convert degrees to radians
                float rad = radians(_Angle);

                // Calculate blend factor by projecting UV onto the gradient direction vector
                float blend = i.uv.x * cos(rad) + i.uv.y * sin(rad);

                // Because uv in [0,1], blend will be in [0, sqrt(2)] range depending on angle,
                // so remap from [0, sqrt(2)] to [0,1]
                float maxBlend = sqrt(2.0);
                float blendRemapped = saturate(blend / maxBlend);

                // Interpolate colors
                fixed4 color = lerp(_ColorBottom, _ColorTop, blendRemapped);
                return color;
            }
            ENDCG
        }
    }
}
