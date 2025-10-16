Shader "Custom/SimpleClothing_BuiltIn"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1,1,1,1)
        _PatternMask ("Pattern Mask", 2D) = "white" {}
        _PatternColor ("Pattern Color", Color) = (1,1,1,1)
        _Shield ("Shield Texture", 2D) = "white" {}
        _ShieldColor ("Shield Color", Color) = (1,1,1,1)
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
        sampler2D _PatternMask;
        sampler2D _Shield;
        sampler2D _SneakerPattern;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_PatternMask;
            float2 uv_Shield;
            float2 uv_SneakerPattern;
        };
        
        fixed4 _Color;
        fixed4 _PatternColor;
        fixed4 _ShieldColor;
        fixed4 _SneakerColor;
        half _Glossiness;
        half _Metallic;
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Start with base texture and color
            fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            
            // Apply pattern if exists
            fixed4 patternMask = tex2D(_PatternMask, IN.uv_PatternMask);
            if (patternMask.r > 0.1)
            {
                baseColor = lerp(baseColor, _PatternColor, patternMask.r * 0.8);
            }
            
            // Apply shield if exists
            fixed4 shieldTex = tex2D(_Shield, IN.uv_Shield);
            if (shieldTex.r > 0.1)
            {
                baseColor = lerp(baseColor, _ShieldColor, shieldTex.r * 0.8);
            }
            
            // Apply sneaker pattern if exists
            fixed4 sneakerTex = tex2D(_SneakerPattern, IN.uv_SneakerPattern);
            if (sneakerTex.r > 0.1)
            {
                baseColor = lerp(baseColor, _SneakerColor, sneakerTex.r * 0.8);
            }
            
            o.Albedo = baseColor.rgb;
            o.Alpha = baseColor.a;
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
