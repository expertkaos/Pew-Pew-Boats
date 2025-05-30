Shader "Unlit/WavyWaterRippleOpacity" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _RippleSpeed ("Ripple Speed", Float) = 2.0
        _RippleFrequency ("Ripple Frequency", Float) = 5.0
        _RippleAmplitude ("Ripple Amplitude", Float) = 0.1
        _WaveStrength ("Wave Strength", Float) = 0.05
        _WaveFrequency ("Wave Frequency", Float) = 10.0
        _Opacity ("Opacity", Range(0, 1)) = 1.0 // New opacity property
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" } // Important: Set RenderType to Transparent
        LOD 100

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha // Enable alpha blending
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            float4 _Color;
            float _RippleSpeed;
            float _RippleFrequency;
            float _RippleAmplitude;
            float _WaveStrength;
            float _WaveFrequency;
            float _Opacity; // Added opacity variable

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 center = float2(0.5, 0.5);
                float distance = length(i.worldPos.xy - center);

                float rippleValue = sin((distance * _RippleFrequency) + (_Time.y * _RippleSpeed)) * _RippleAmplitude;

                float waveOffset = sin(i.worldPos.x * _WaveFrequency + _Time.y * _RippleSpeed * 0.5) * _WaveStrength;
                rippleValue += waveOffset;

                fixed4 finalColor = _Color + fixed4(rippleValue, rippleValue, rippleValue, 0.0);

                finalColor.a = _Opacity; // Apply opacity

                return finalColor;
            }
            ENDCG
        }
    }
}