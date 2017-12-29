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

			uniform float2 lightWorldPos;
			uniform float lightRange;
			uniform float4 lightColor;
			uniform float2 cameraLB;
			uniform float2 cameraSize;
			#define PI 3.1415926f

			#include "UnityCG.cginc"

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
				float radian = atan2(relativePos.y, relativePos.x) / (PI * 2);
				if (radian < 0)
					radian += 1;
				float lightDst = DecodeFloatRGBA(tex2D(_MainTex, float2(radian, 1))) * lightRange;
				
				fixed4 col = lightColor;
				float dst = length(relativePos);
				if (lightDst < dst) {
					col = fixed4(0, 0, 0, 1);
				}
				//TODO 遮挡后一定距离有光照
				float atn = 1 - pow(dst / lightRange, 2);
				col.rgb *= atn;

				return col;
			}
			
			

			ENDCG
		}
	}
}