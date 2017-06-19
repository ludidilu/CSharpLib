Shader "Custom/RpgMapShadow"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		
		ZTest Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			
			#include "UnityCG.cginc"
			#pragma target 3.0

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 worldPosition : TEXCOORD1;
			};
			
			float4 hole[30];

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldPosition = v.vertex;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed alpha = 1;
			
				for(int m = 0 ; m < 30 ; m++){
				
					float4 h = hole[m];
					
					if(h.z > 0){
					
						float dis = distance(float2(i.worldPosition.x,i.worldPosition.y),float2(h.x,h.y));
				
						if(dis < h.z){
						
							alpha = alpha * pow(dis / h.z,0.5);
						}
					}
				}
			
				return fixed4(0,0,0,alpha);
			}
			ENDCG
		}
	}
}
