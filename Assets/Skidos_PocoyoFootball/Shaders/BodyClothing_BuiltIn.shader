Shader "Custom/BodyClothing_BuiltIn"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _BodyColor ("Body Color", Color) = (1,1,1,1)
        _PatternMask ("Pattern Mask", 2D) = "white" {}
        _PatternColor ("Pattern Color", Color) = (1,1,1,1)
        _Shield ("Shield Texture", 2D) = "white" {}
        _ShieldColor ("Shield Color", Color) = (1,1,1,1)
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
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_PatternMask;
            float2 uv_Shield;
        };
        
        fixed4 _BodyColor;
        fixed4 _PatternColor;
        fixed4 _ShieldColor;
        half _Glossiness;
        half _Metallic;
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Start with base texture and body color - make body color VERY dominant for high contrast
            fixed4 baseTex = tex2D(_MainTex, IN.uv_MainTex);
            
            // Use lerp to make body color VERY dominant over the base texture
            // This ensures high contrast, vibrant colors while preserving texture details
            fixed4 baseColor = lerp(baseTex * 0.1, _BodyColor, 0.95);
            
            // Apply pattern if exists - use alpha channel for better blending
            fixed4 patternMask = tex2D(_PatternMask, IN.uv_PatternMask);
            if (patternMask.a > 0.01 || patternMask.r > 0.01)
            {
                float patternStrength = max(patternMask.a, patternMask.r);
                baseColor = lerp(baseColor, _PatternColor * patternStrength, patternStrength * 0.7);
            }
            
            // Apply shield if exists - use alpha channel for better blending
            fixed4 shieldTex = tex2D(_Shield, IN.uv_Shield);
            if (shieldTex.a > 0.01 || shieldTex.r > 0.01)
            {
                float shieldStrength = max(shieldTex.a, shieldTex.r);
                baseColor = lerp(baseColor, _ShieldColor * shieldStrength, shieldStrength * 0.8);
            }
            
            // Ensure the body color has proper influence and maintain texture details
            o.Albedo = baseColor.rgb;
            o.Alpha = baseColor.a;
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
