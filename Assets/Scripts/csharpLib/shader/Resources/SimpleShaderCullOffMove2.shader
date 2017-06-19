// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Custom/SimpleShaderCullOffMove2" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_MoveSpeed("MoveSpeed",float) = 1
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float4 texcoord2:TEXCOORD2;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			float fTextTileWidth;
			float fTextTilePartHeight;
			
			float _MoveSpeed;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				
				float2 tmpUv = TRANSFORM_TEX(v.texcoord, _MainTex);
				
				if(v.texcoord2.x == 1){
				
					float uOffset = fTextTileWidth * floor(fmod(_Time.y * _MoveSpeed,3)) * 2;
					
					o.texcoord = float2(tmpUv.x + uOffset,tmpUv.y);
					
				}else if(v.texcoord2.x == 2){
				
					float frame = floor(fmod(_Time.y * _MoveSpeed,3));
					
					frame = frame + floor((v.texcoord2.y - 1) / 2);
				
                    float vOffset = (v.texcoord2.y - frame) * fTextTilePartHeight * 2;
						
					o.texcoord = float2(tmpUv.x,tmpUv.y + vOffset);
						
				}else{
				
					o.texcoord = tmpUv;
				}
				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				
				return col;
			}
		ENDCG
	}
}

}
