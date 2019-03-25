// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ShaderDev/03MaskShader" 
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex("Main Texture",2D) = "white"{}
		_Alpha("Alpha (A)",2D) = "white"{}
	}

	Subshader
	{
	//     그리는 순서          , 프로젝터 사용 여부,      셰이더 교체용
	Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" ="Opaque"}
	  Pass
	  {
	  Blend SrcAlpha OneMinusSrcAlpha
	  ColorMask RGB
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		uniform half4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform sampler2D _Alpha;

		struct vertextInput
		{
			float4 vertex : POSITION;
			float4 texcoord : TEXCOORD0;
		};
		
		struct vertexOutput
		{
				float4 pos : SV_POSITION; 
				float4 texcoord : TEXCOORD0;
		};
		
		vertexOutput vert(vertextInput v)
		{
			vertexOutput o;
			o.pos = UnityObjectToClipPos(v.vertex);
			// 원래 uv * 타일크기  + 오프셋
			o.texcoord.xy = (v.texcoord.xy* _MainTex_ST.xy + _MainTex_ST.wz);
			return o;
		
		}

		half4 frag(vertexOutput i)  : COLOR
		{
			return tex2D(_MainTex,i.texcoord)*tex2D(_Alpha,i.texcoord)*_Color;
			
		}


//float - a 32bit floating point number
//half - a 16bit floating point number
//int - a 32bit integer
//fixed - a 12bit fixed point number
//bool - a boolean variable
	


		ENDCG
	  }
	}

}
