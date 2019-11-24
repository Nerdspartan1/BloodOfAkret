Shader "Custom/Disintegrate2"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		_PatternTex("Pattern", 2D) = "white" {}
		_Threshold("Threshold", Range(0,1)) = 0.3
		_TWidth("Threshold width", Float) = 0.01
		_STWidth("Secondary Threshold width", Float) = 0.05
		_ThresholdColor("Threshold Color", Color) = (1,0.4,0,1)
		_SThresholdColor("Secondary Threshold Color", Color) = (0.6,0.25,0,1)
    }
    SubShader
    {
		Tags {"RenderType" = "Opque" }
        LOD 200
		Cull Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
		

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _PatternTex;

		float _Threshold;
		float _TWidth;
		float _STWidth;
		float4 _ThresholdColor;
		float4 _SThresholdColor;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

			fixed4 pcol = tex2D(_PatternTex, IN.uv_MainTex);
			fixed4 col;
			
			if (pcol.r > _Threshold)
				col = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			else if (pcol.r > _Threshold - _TWidth)
				col = _ThresholdColor;
			else if (pcol.r > _Threshold - _TWidth - _STWidth)
				col = _SThresholdColor;
			else
				clip(-1);

            o.Albedo = col.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = col.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
