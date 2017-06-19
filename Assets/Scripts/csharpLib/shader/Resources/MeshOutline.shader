Shader "Custom/Mesh_Outline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		
		_TextureSize ("_TextureSize",Float) = 256
		//取值半径
		_BlurRadius ("_BlurRadius",Range(0,15) ) = 1
		
		_OutlineColor("OutlineColor",Color) = (1,0,0,0)
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque"}
		LOD 100
		
//		ZWrite Off
		
		//Blend SrcAlpha OneMinusSrcAlpha
		
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
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
			
			
			struct finalout{
			
				float4 color : SV_Target;
				float depth : SV_Depth;
			};
			

			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			float _TextureSize;
			
			float _BlurRadius;
			
			fixed4 _OutlineColor;
			
			sampler2D _CameraDepthTexture;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			void GetBlurColor(float2 uv, inout finalout result)
			{
			    float space = 1.0/_TextureSize; //算出一个像素的空间
			    int count = _BlurRadius * 2 +1; //取值范围
			    count *= count;
			    
			    float depth = 1;

			    //将以自己为中心，周围半径的所有颜色相加，然后除以总数，求得平均值
			    float4 colorTmp = float4(0,0,0,0);
			    
			    for( int x = -_BlurRadius ; x <= _BlurRadius ; x++ )
			    {
			        for( int y = -_BlurRadius ; y <= _BlurRadius ; y++ )
			        {
			        	float2 newUv = uv + float2(x * space,y * space);
			        
			            float4 color = tex2D(_MainTex,newUv);
			            
			            colorTmp += color;
			            
			            float tmpDepth = tex2D(_CameraDepthTexture,newUv).x;
			            
			            if(tmpDepth < depth){
			            
			            	depth = tmpDepth;
			            }
			        }
			    }
			    
			    fixed4 c = colorTmp / count;
			    
			    c.rgb = _OutlineColor.rgb;
			    
			    result.color = c;
			    
			    result.depth = depth;
			}
			
			finalout frag (v2f i)
			{
				float oldDepth = tex2D(_CameraDepthTexture,i.uv).x;
				
				finalout fo;
				
				GetBlurColor(i.uv,fo);
				
				if(oldDepth < 1){
				
					fo.depth = 1;
				}
				
				return fo;
			}
			ENDCG
		}
	}
}
