Shader "Custom/Blured"
{
    Properties
    {
        _BlurSize ("Blur Strength", Range(0.001, 0.02)) = 0.005
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }

        // 🔹 GrabPass MUST be outside of the Pass block
        GrabPass { "_BackgroundTexture" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _BackgroundTexture;
            float _BlurSize;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = ComputeGrabScreenPos(o.pos).xy; // Get screen-space UVs
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = fixed4(0, 0, 0, 1);
                int samples = 5; // More samples = stronger blur

                for (int x = -samples; x <= samples; x++)
                {
                    for (int y = -samples; y <= samples; y++)
                    {
                        float2 offset = float2(x, y) * _BlurSize;
                        color += tex2D(_BackgroundTexture, i.uv + offset);
                    }
                }
                color /= (samples * 2 + 1) * (samples * 2 + 1); // Normalize blur
                return fixed4(color.rgb, 0.5); // Adjust alpha for transparency
            }
            ENDCG
        }
    }
}
