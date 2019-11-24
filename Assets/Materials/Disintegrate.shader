Shader "Unlit/Disintegrate"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PatternTex("Pattern", 2D) = "white" {}
		_Threshold("Threshold", Range(0,1)) = 0.3
		_TWidth("Threshold width", Float) = 0.01
		_STWidth("Secondary Threshold width", Float) = 0.05
		_ThresholdColor("Threshold Color", Color) = (1,0.4,0,1)
		_SThresholdColor("Secondary Threshold Color", Color) = (0.6,0.25,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
			sampler2D _PatternTex;

			float4 _MainTex_ST;
			float _Threshold;
			float _TWidth;
			float _STWidth;
			float4 _ThresholdColor;
			float4 _SThresholdColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 pcol = tex2D(_PatternTex, i.uv);
				fixed4 col;
				if (pcol.r > _Threshold)
					col = tex2D(_MainTex, i.uv);
				else if (pcol.r > _Threshold - _TWidth)
					col = _ThresholdColor;
				else if (pcol.r > _Threshold - _TWidth - _STWidth)
					col = _SThresholdColor;
				else 
					col = (0, 0, 0, 0);
				
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}