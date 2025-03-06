Shader "Hidden/GlitchEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlitchIntensity ("Glitch Intensity", Range(0,1)) = 0.5
    }
    SubShader
    {
        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _GlitchIntensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float glitch = sin(_Time.y * 30) * _GlitchIntensity;
                i.uv.x += glitch * (frac(i.uv.y * 100) - 0.5);
                return tex2D(_MainTex, i.uv);
            }
            ENDHLSL
        }
    }
}
