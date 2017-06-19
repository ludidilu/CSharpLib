Shader "Custom/SimpleColorDiffuse" {
	Properties {
		_Color("Color",Color) = (0,0,0,0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 150

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd

		half4 _Color;

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};


		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color.rgb;
		}
		ENDCG
	}

	Fallback "Mobile/VertexLit"
}
