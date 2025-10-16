Shader "Custom/SimpleColorOnly_BuiltIn"
{
    Properties
    {
        _BodyColor ("Body Color", Color) = (1,1,1,1)
        _LegsColor ("Legs Color", Color) = (1,1,1,1)
        _PatternColor ("Pattern Color", Color) = (1,1,1,1)
        _ShieldColor ("Shield Color", Color) = (1,1,1,1)
        _SneakerColor ("Sneaker Color", Color) = (1,1,1,1)
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
        
        fixed4 _BodyColor;
        fixed4 _LegsColor;
        fixed4 _PatternColor;
        fixed4 _ShieldColor;
        fixed4 _SneakerColor;
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Apply colors directly without texture blending
            // This will show if the issue is with texture blending or color application
            fixed4 finalColor = _BodyColor;
            
            // Mix in other colors for testing
            finalColor = lerp(finalColor, _PatternColor, 0.5);
            finalColor = lerp(finalColor, _ShieldColor, 0.3);
            finalColor = lerp(finalColor, _LegsColor, 0.4);
            finalColor = lerp(finalColor, _SneakerColor, 0.2);
            
            o.Albedo = finalColor.rgb;
            o.Alpha = finalColor.a;
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
