Shader "Custom/GlitchEffectURP"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} 
        _GlitchIntensity ("Glitch Intensity", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }
        Pass
        {
            Name "GlitchPass"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float _GlitchIntensity;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float glitchOffset = sin(_Time.y * 30) * _GlitchIntensity;
                float2 glitchUV = IN.uv;
                glitchUV.x += glitchOffset * (frac(IN.uv.y * 100) - 0.5);

                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, glitchUV);
            }
            ENDHLSL
        }
    }
}
