Shader "Custom/LightMap"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	
	SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		LOD 100
		

		Pass 
		{
			ZTest Always
			Cull Off
			Lighting Off
			ZWrite Off
			Fog{ Mode off }

			Blend One One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct v2f
			{
				float2 cameraUV : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;


			uniform float3 lightWorldPos; //z为1时表示聚光灯
			uniform float lightRange;
			uniform float4 lightColor;

			uniform float2 cameraLB;
			uniform float2 cameraSize;
			#define PI 3.1415926f
			#define TwoPI (PI * 2)

			//以下是聚光灯使用的参数
			uniform float lightDirection;
			uniform float lightAngle;
			uniform float lightCenterAngle;

			

			#include "UnityCG.cginc"

			float AngleDiff(float a, float b)
			{
				a = a - floor(a / TwoPI) * TwoPI;
				b = b - floor(b / TwoPI) * TwoPI;
				float diff = abs(a - b);
				if (diff > PI)
					return TwoPI - diff;
				else
					return diff;
			}

			v2f vert(appdata_img v)
			{
				v2f o;
				o.cameraUV.x = v.texcoord.x * cameraSize.x + cameraLB.x;
				o.cameraUV.y = v.texcoord.y * cameraSize.y + cameraLB.y;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				float2 relativePos = i.cameraUV - lightWorldPos;
				float radian = atan2(relativePos.y, relativePos.x);
				if (radian < 0)  //为了采样shadowmap,范围调整为0~2π
					radian += PI * 2; 

				fixed4 col = lightColor;
				if (lightWorldPos.z == 1) { //spot light
					float toCenter = AngleDiff(radian, lightDirection);
					if (toCenter > lightAngle / 2)
					{  //范围之外,不受聚光灯影响
						return fixed4(0,0,0,1);
					}
					else if (toCenter > lightCenterAngle / 2)
					{  //外围区域随角度增加而衰减
						float atn = 1 - pow(toCenter*2 / lightAngle, 1);
						col.rgb *= atn;
						//return fixed4(1, 0, 0, 1);
					}
					else 
					{
						//return fixed4(0, 1, 0, 1);
					}
				}

				radian /= (PI * 2); //缩放到0~1
				float lightDst = DecodeFloatRGBA(tex2D(_MainTex, float2(radian, 1))) * lightRange;
				
				
				float dst = length(relativePos);
				if (lightDst < dst)
				{
					// 遮挡后一定距离有衰减的光照
					float rem = (lightRange - lightDst) * 0.3;
					float remDst = dst - lightDst;
					if (remDst > rem) 
					{
						col = fixed4(0, 0, 0, 1);
					}
					else 
					{
						float atn = (1 - pow(lightDst / lightRange, 3.5)) * 0.3;
						float atn2 = 1 - pow(remDst / rem, 2);
						col.rgb *= atn2 * atn;
					}
				}
				else 
				{
					float atn = 1 - pow(dst / lightRange, 3.5);
					col.rgb *= atn;
				}

				col.rgb;
				return col;
			}
			
			

			ENDCG
		}
	}
}