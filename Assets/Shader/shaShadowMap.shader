Shader "Custom/ShadowMap"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass 
		{
			ZTest Always
			Cull Off
			ZWrite Off
			Fog{ Mode off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct v2f
			{
				float theta : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			uniform float lightResolution;
			uniform float2 lightWorldPos;
			uniform float lightRange;
			
			uniform float2 leftBottom;
			uniform float2 rightTop;

			#define PI 3.1415926f

			#include "UnityCG.cginc"

			//inline by default
			float2 WorldPosToUV(float2 pos)
			{
				float2 uv;
				uv.x = (pos.x - leftBottom.x) / (rightTop.x - leftBottom.x);
				uv.y = (pos.y - leftBottom.y) / (rightTop.y - leftBottom.y);
				return uv;
			}

			bool UVOutOfRange(float2 uv)
			{
				if (uv.x >= 0 && uv.x <= 1 && uv.y >= 0 && uv.y <= 1)
					return false;

				return true;
			}

			v2f vert(appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.theta = v.texcoord.x * 2 * PI;
				return o;
			}

			float4 frag(float theta : TEXCOORD0) : SV_Target
			{
				float distance = 0.999f; //a large number
				for (float step = 0.0f; step < lightResolution; step += 1.0f)
				{
					float rate = step / lightResolution;
					float radius = rate * lightRange;
					float2 relativePos = float2(cos(theta) * radius, sin(theta) * radius);
					float2 worldPos = relativePos + lightWorldPos;
					float2 uv = WorldPosToUV(worldPos);

					if (!UVOutOfRange(uv)) 
					{
						float alpha = tex2Dlod(_MainTex, float4(uv,0,0)).a;
						if (alpha > 0.5f) {
							distance = rate;
							break;
						}
					}
					else
					{
						break;
					}
				}

				float4 col = EncodeFloatRGBA(distance);

				return col;
			}
			
			

			ENDCG
		}
	}
}