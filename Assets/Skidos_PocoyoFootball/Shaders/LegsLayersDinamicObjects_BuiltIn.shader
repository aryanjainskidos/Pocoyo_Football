Shader "Custom/LegsLayersDinamicObjects_BuiltIn"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1,1,1,1)
        _PatternMask ("Pattern Mask", 2D) = "white" {}
        _PatternColor ("Pattern Color", Color) = (1,1,1,1)
        _SneakerPattern ("Sneaker Pattern", 2D) = "white" {}
        _SneakerColor ("Sneaker Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalStrength ("Normal Strength", Range(0,2)) = 1.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        
        sampler2D _MainTex;
        sampler2D _PatternMask;
        sampler2D _SneakerPattern;
        sampler2D _NormalMap;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_PatternMask;
            float2 uv_SneakerPattern;
            float2 uv_NormalMap;
        };
        
        half _Glossiness;
        half _Metallic;
        half _NormalStrength;
        fixed4 _Color;
        fixed4 _PatternColor;
        fixed4 _SneakerColor;
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Sample base texture with color tint
            fixed4 baseTex = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 baseColor = baseTex * _Color;
            
            // Apply pattern if exists - use alpha channel for better blending
            fixed4 patternMask = tex2D(_PatternMask, IN.uv_PatternMask);
            if (patternMask.a > 0.01 || patternMask.r > 0.01)
            {
                float patternStrength = max(patternMask.a, patternMask.r);
                baseColor = lerp(baseColor, _PatternColor * patternStrength, patternStrength * 0.8);
            }
            
            // Apply sneaker pattern if exists - use alpha channel for better blending
            fixed4 sneakerTex = tex2D(_SneakerPattern, IN.uv_SneakerPattern);
            if (sneakerTex.a > 0.01 || sneakerTex.r > 0.01)
            {
                float sneakerStrength = max(sneakerTex.a, sneakerTex.r);
                baseColor = lerp(baseColor, _SneakerColor * sneakerStrength, sneakerStrength * 0.8);
            }
            
            o.Albedo = baseColor.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = baseColor.a;
            
            // Apply normal mapping
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
            o.Normal.xy *= _NormalStrength;
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
