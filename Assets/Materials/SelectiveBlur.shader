Shader "Custom/CameraBlur"
{
    Properties
    {
        _BlurSize ("Blur Strength", Range(0.001, 0.02)) = 0.005
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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

            sampler2D _MainTex;
            float _BlurSize;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // Use proper screen-space UV
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv); // Sample original color first
                int samples = 3;

                for (int x = -samples; x <= samples; x++)
                {
                    for (int y = -samples; y <= samples; y++)
                    {
                        float2 offset = float2(x, y) * _BlurSize;
                        color += tex2D(_MainTex, i.uv + offset);
                    }
                }

                color /= (samples * 2 + 1) * (samples * 2 + 1);
                return color;
            }
            ENDCG
        }
    }
}
