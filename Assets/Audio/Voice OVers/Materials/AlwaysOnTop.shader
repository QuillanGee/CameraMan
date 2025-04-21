Shader "Custom/AlwaysOnTop"
{
    SubShader
    {
        Tags { "Queue" = "Overlay" } // Renders above everything
        Pass
        {
            ZTest Always
        }
    }
}
