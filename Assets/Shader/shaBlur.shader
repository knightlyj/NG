Shader "Custom/Blur" {
    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _TextureSize ("_TextureSize",Vector) = (256,256, 0, 0)
        _BlurRadius ("_BlurRadius",Range(1,15) ) = 5
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

	Pass {
        CGPROGRAM

          #pragma vertex vert
          #pragma fragment frag
        #include "UnityCG.cginc"


        sampler2D _MainTex;
        int _BlurRadius;
        float2 _TextureSize;

        struct v2f {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
        };


        v2f vert( appdata_img v ) 
        {
            v2f o;
            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
            o.uv = v.texcoord.xy;
            return o;
        } 
        
		float4 GetBlurColor(float2 uv)
		{

			float spaceX = 1.0 / _TextureSize.x; //算出一个像素的空间
			float spaceY = 1.0 / _TextureSize.y; //算出一个像素的空间
			int count = _BlurRadius * 2 + 1; //取值范围
			count *= count;

			//将以自己为中心，周围半径的所有颜色相加，然后除以总数，求得平均值
			float4 colorTmp = float4(0, 0, 0, 0);
			for (int x = -_BlurRadius; x <= _BlurRadius; x++)
			{
				for (int y = -_BlurRadius; y <= _BlurRadius; y++)
				{
					float4 color = tex2D(_MainTex, uv + float2(x * spaceX, y * spaceY));
					colorTmp += color;
				}
			}
			return colorTmp / count;
		}

        half4 frag(v2f i) : SV_Target 
        {
            //调用普通模糊
            return GetBlurColor(i.uv);
            //调用高斯模糊  
            //return GetGaussBlurColor(i.uv);
            //return tex2D(_MainTex,i.uv);
        }
        ENDCG
        }
    }
    FallBack "Diffuse"
}