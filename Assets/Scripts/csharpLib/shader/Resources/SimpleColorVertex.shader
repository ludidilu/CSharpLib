Shader "Custom/SimpleColorVertex" {

	Properties{

		_ZTest("ZTest", Float) = 0
		_ZWrite("ZWrite", Float) = 0
	}

	SubShader{

		Tags{ "RenderType" = "Transparent" }

		Pass{

			Blend SrcAlpha OneMinusSrcAlpha
			ZTest [_ZTest]
			ZWrite [_ZWrite]

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata {

				float4 vertex:POSITION;
				float4 color :COLOR;
			};

			struct v2f {

				float4 pos:POSITION;
				float4 color:COLOR;
			};

			half4 _Color;

			v2f vert(appdata v) {

				v2f o;

				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);

				o.color = v.color;

				return o;
			}

			half4 frag(v2f o) :COLOR{

				return o.color;
			}

			ENDCG
		}
	}

	FallBack "Mobile/Diffuse"
}
