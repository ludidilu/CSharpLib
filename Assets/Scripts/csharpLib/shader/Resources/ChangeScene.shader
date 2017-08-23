// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ChangeScene"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		
		lastRight("Right", Range(0,1)) = 0.9
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			ZWrite Off 
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			
			sampler2D _LastTex;
			
			float lastRight;
			
			v2f vert (appdata v)
			{
				v2f o;
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				o.uv = v.uv;

				#if UNITY_UV_STARTS_AT_TOP
				o.uv.y = 1 - o.uv.y;
				#endif
				
				return o;
			}
			
			fixed4 frag (v2f input) : SV_Target
			{
				fixed4 col = tex2D(_MainTex,input.uv);
				
				fixed4 last = tex2D(_LastTex,input.uv);
				
				col = col * (1 - lastRight) + last * lastRight;
				
				return col;
			}
			ENDCG
		}
	}
}
