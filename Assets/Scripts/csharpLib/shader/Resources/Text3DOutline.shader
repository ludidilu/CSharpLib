// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Text3DOutline" {
	Properties {
	
		_MainTex("Main Texture", 2D) = ""{}
		_Color ("Text Color", Color) = (1,1,1,1)
		_OutlineColor ("Text OutlineColor", Color) = (1,1,1,1)
		_OutLineWidth ("_OutLineWidth", float) = 0.01  
		_XFix("XFix", float) = 0
		_YFix("YFix", float) = 0
	}
	
	SubShader {
	
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
			"DisableBatching"="true"
		}
		
		Pass{
		
			Lighting Off Cull Off ZTest Off 
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
		
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform fixed4 _Color;
			uniform fixed4 _OutlineColor;
			half _OutLineWidth;  
			float _XFix;
			float _YFix;
			
			struct appdata{
			
				float4 vertex:POSITION;
				float2 texcoord:TEXCOORD;
			};
			
			struct v2f{
			
				float4 pos:POSITION;
				float2 texcoord:TEXCOORD;
			};


			v2f vert(appdata v){
			
				v2f o;
				
//				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);

				float4 vx = float4(unity_ObjectToWorld[0].x,unity_ObjectToWorld[1].x,unity_ObjectToWorld[2].x,unity_ObjectToWorld[3].x);
				
				float scaleX = length(vx);
				
				float4 vy = float4(unity_ObjectToWorld[0].y,unity_ObjectToWorld[1].y,unity_ObjectToWorld[2].y,unity_ObjectToWorld[3].y);
				
				float scaleY = length(vy);
				
               	o.pos = mul(UNITY_MATRIX_P, (mul(UNITY_MATRIX_MV , float4(0,0,0,1)) + v.vertex * float4(scaleX, scaleY, 0, 1) + float4(_XFix,_YFix,0,0)));
               	
               	o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
               	
				return o;
			}
			
			half4 frag(v2f i):COLOR{
			
				fixed4 col = tex2D(_MainTex, i.texcoord);
				
				fixed ba = col.a;
				
				col.a *= _Color.a;
				
				fixed alpha = tex2D(_MainTex, i.texcoord.xy + float2(_OutLineWidth , 0)).a;
				ba = min(ba,alpha);
                alpha = tex2D(_MainTex, i.texcoord.xy + float2(-_OutLineWidth , 0)).a;
                ba = min(ba,alpha);
                alpha = tex2D(_MainTex, i.texcoord.xy + float2( 0, _OutLineWidth)).a;
                ba = min(ba,alpha);
                alpha = tex2D(_MainTex, i.texcoord.xy + float2( 0, -_OutLineWidth)).a;
                ba = min(ba,alpha);
                alpha = tex2D(_MainTex, i.texcoord.xy + float2(_OutLineWidth , _OutLineWidth)).a;
                ba = min(ba,alpha);
                alpha = tex2D(_MainTex, i.texcoord.xy + float2(_OutLineWidth , -_OutLineWidth)).a;
                ba = min(ba,alpha);
                alpha = tex2D(_MainTex, i.texcoord.xy + float2(-_OutLineWidth , _OutLineWidth)).a;
                ba = min(ba,alpha);
                alpha = tex2D(_MainTex, i.texcoord.xy + float2(-_OutLineWidth , -_OutLineWidth)).a;
                ba = min(ba,alpha);
                
                col.rgb = _OutlineColor.rgb * (1 - ba) + _Color.rgb * ba;
                
				return col;
			}
			
			ENDCG
		
		}
	} 
	
	FallBack "Mobile/Diffuse"
}
