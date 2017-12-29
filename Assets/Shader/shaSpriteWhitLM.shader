Shader "Custom/SpriteWithLM"
{
    Properties
    {
        _MainTex ("Tiled Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
			"OccType" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
				float2 cameraUV : TEXCOORD1;
            };

			uniform sampler2D lightMap;
			uniform float2 cameraLB;
			uniform float2 cameraSize;
            fixed4 _Color;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
				float2 worldPos = mul(unity_ObjectToWorld, IN.vertex).xy;
				OUT.cameraUV.x = (worldPos.x - cameraLB.x) / cameraSize.x;
				OUT.cameraUV.y = (worldPos.y - cameraLB.y) / cameraSize.y;

                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f IN) : COLOR
            {
                half4 texcol = tex2D(_MainTex, IN.texcoord);
				fixed4 light = tex2D(lightMap, IN.cameraUV);
                texcol = texcol * IN.color * light;
                return texcol;
            }
        ENDCG
        }
    }

    Fallback "Sprites/Default"
}