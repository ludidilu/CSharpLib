Shader "Custom/SimpleSprite"
{
	Properties
	{
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_ZTest("ZTest", Float) = 0
		_ZWrite("ZWrite", Float) = 0
		_SrcAlpha("SrcAlpha", Float) = 0
		_OneMinusSrcAlpha("OneMinusSrcAlpha", Float) = 0
	}

	SubShader
	{
		ZTest [_ZTest]
		ZWrite [_ZWrite]
		Blend [_SrcAlpha] [_OneMinusSrcAlpha]

		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * i.color;
				return col;
			}
			ENDCG
		}
	}
}
