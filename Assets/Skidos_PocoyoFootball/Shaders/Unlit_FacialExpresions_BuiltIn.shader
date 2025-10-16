Shader "Custom/Unlit_FacialExpresions_BuiltIn"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1,1,1,1)
        _ExpressionMask ("Expression Mask", 2D) = "white" {}
        _ExpressionColor ("Expression Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        
        sampler2D _MainTex;
        sampler2D _ExpressionMask;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_ExpressionMask;
        };
        
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _ExpressionColor;
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Sample base texture
            fixed4 baseTex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            
            // Sample expression mask and apply expression color
            fixed4 expressionMask = tex2D(_ExpressionMask, IN.uv_ExpressionMask);
            fixed4 expressionColor = _ExpressionColor * expressionMask.r;
            
            // Blend base and expression
            fixed4 finalColor = baseTex;
            if (expressionMask.r > 0.1) finalColor = lerp(finalColor, expressionColor, expressionMask.r);
            
            o.Albedo = finalColor.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = finalColor.a;
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
