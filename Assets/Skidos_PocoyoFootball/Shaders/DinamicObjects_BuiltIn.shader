Shader "Custom/DinamicObjects_BuiltIn"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1,1,1,1)
        _PatternMask ("Pattern Mask", 2D) = "white" {}
        _PatternColor ("Pattern Color", Color) = (1,1,1,1)
        _Shield ("Shield Texture", 2D) = "white" {}
        _ShieldColor ("Shield Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _SneakerPattern ("Sneaker Pattern", 2D) = "white" {}
        _SneakerColor ("Sneaker Color", Color) = (1,1,1,1)
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
        sampler2D _Shield;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_PatternMask;
            float2 uv_Shield;
        };
        
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _PatternColor;
        fixed4 _ShieldColor;
        
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
            
            // Apply shield if exists - use alpha channel for better blending
            fixed4 shieldTex = tex2D(_Shield, IN.uv_Shield);
            if (shieldTex.a > 0.01 || shieldTex.r > 0.01)
            {
                float shieldStrength = max(shieldTex.a, shieldTex.r);
                baseColor = lerp(baseColor, _ShieldColor * shieldStrength, shieldStrength * 0.9);
            }
            
            o.Albedo = baseColor.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = baseColor.a;
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
