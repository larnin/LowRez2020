Shader "Unlit/PortalEffect"
{
    Properties
    {
        _MainTex ("Texture1", 2D) = "white" {}
		_SecondTex("Texture2", 2D) = "white" {}
		_Crack("Cracks", 2D) = "white" {}
		_CrackPower("CrackPower", float) = 1
		_BorderSize("BorderSize", float) = 0.1
		_EffectPower("Power", float) = 1
		_EffectOffset("Offset", float) = 0
		_Percent("Percent", Range(0, 1)) = 0
		_PortalPos("PortalPos", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
		Cull Off ZWrite Off ZTest Always

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

            sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _SecondTex;
			float4 _SecondTex_ST;
			sampler2D _Crack;
			float _CrackPower;
			float _BorderSize;
			float4 _Crack_ST;
			float _EffectPower;
			float _EffectOffset;
			float _Percent;
			float4 _PortalPos;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float x = i.uv.x - _PortalPos.x;
				float y = i.uv.y - _PortalPos.y;

				float2 crackUV = i.uv / 2;
				crackUV.x += (1 - _PortalPos.x) / 2;
				crackUV.y += (1 - _PortalPos.y) / 2;

				float offset = tex2D(_Crack, crackUV).r * _CrackPower;

				float dist = sqrt(x * x + y * y) + offset;

				float percent = _Percent * _EffectPower + _EffectOffset;

				float delta = abs(dist - percent);

                fixed4 col = tex2D(_MainTex, i.uv);
				if (dist < percent)
					col = tex2D(_SecondTex, i.uv);
				if (delta < _BorderSize)
					col = fixed4( 0, 0, 0, 1 );

                return col;
            }
            ENDCG
        }
    }
}
