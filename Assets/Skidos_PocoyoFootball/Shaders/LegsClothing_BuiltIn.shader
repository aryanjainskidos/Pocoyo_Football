Shader "Custom/LegsClothing_BuiltIn"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _LegsColor ("Legs Color", Color) = (1,1,1,1)
        _SneakerPattern ("Sneaker Pattern", 2D) = "white" {}
        _SneakerColor ("Sneaker Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert
        
        sampler2D _MainTex;
        sampler2D _SneakerPattern;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_SneakerPattern;
        };
        
        fixed4 _LegsColor;
        fixed4 _SneakerColor;
        half _Glossiness;
        half _Metallic;
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Start with base texture and legs color
            fixed4 baseTex = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 baseColor = baseTex * _LegsColor;
            
            // Apply sneaker pattern if exists - use alpha channel for better blending
            fixed4 sneakerTex = tex2D(_SneakerPattern, IN.uv_SneakerPattern);
            if (sneakerTex.a > 0.01 || sneakerTex.r > 0.01) // Check both alpha and red channel
            {
                float sneakerStrength = max(sneakerTex.a, sneakerTex.r);
                baseColor = lerp(baseColor, _SneakerColor * sneakerStrength, sneakerStrength * 0.8);
            }
            
            // Ensure the base color has strong influence and maintain texture details
            o.Albedo = baseColor.rgb;
            o.Alpha = baseColor.a;
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
