// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SimpleShaderCullOffMove" {

	Properties {
	
		_MainTex("Main Texture", 2D) = ""{}
		_MoveSpeed ("MoveSpeed",Float) = 1
	}
	
	SubShader {
	
		Tags{"RenderType" = "Opaque"}
		
		Cull Off
		
		ZTest Off
		
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass{

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata{
			
				float4 vertex:POSITION;
				float4 uv2:TEXCOORD2;
				float2 uv:TEXCOORD;
			};
			
			struct v2f{
			
				float4 pos:POSITION;
				float2 uv:TEXCOORD;
			};

			sampler2D _MainTex;
			
			float _MoveSpeed;
			
			v2f vert(appdata v){
			
				v2f o;
			
				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.pos = UnityPixelSnap (o.pos);
				
				o.uv = v.uv;
				
				if(v.uv2.x > -1){
				
					float index = floor(fmod(_Time.y * _MoveSpeed,v.uv2.y));
					
					if(index != v.uv2.x){
					
						o.pos = float4(0,0,0,0);
					}
				}
				
				return o;
			}
			
			half4 frag(v2f o):COLOR{
			
				return tex2D(_MainTex,o.uv);
			}
			
			ENDCG
		
		}
	} 
	
	FallBack "Mobile/Diffuse"
}
