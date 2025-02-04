Shader "Custom/GridShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _GridColor ("Grid Color", Color) = (0,0,0,1)
        _GridSize ("Grid Size", Float) = 10
        _LineThickness ("Line Thickness", Float) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;
        fixed4 _GridColor;
        float _GridSize;
        float _LineThickness;

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Get the UV coordinates and scale them by the grid size
            float2 gridUV = frac(IN.uv_MainTex * _GridSize);
            
            // Create the grid lines
            float lineX = smoothstep(0.0, _LineThickness, gridUV.x) * smoothstep(0.0, _LineThickness, 1.0 - gridUV.x);
            float lineY = smoothstep(0.0, _LineThickness, gridUV.y) * smoothstep(0.0, _LineThickness, 1.0 - gridUV.y);

            // Combine horizontal and vertical lines
            float grid = lineX + lineY;

            // Set the color of the grid and the base material
            o.Albedo = lerp(_Color.rgb, _GridColor.rgb, grid);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
