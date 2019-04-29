// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UnderWater/Caustics" {
  Properties {
  	  _Color ("Caustic Color", Color) = (1,1,1,1)
  	  _Brightness("Caustic Brightness", Range(0.1, 1.0)) = 1
  	  _ScrollX ("Caustic Scroll Speed - X", Float) = 1.0
	  _ScrollY ("Caustic Scroll Speed - Y", Float) = 1.0   	
     _ShadowTex ("Cookie", 2D) = "" { }
     _Size ("Caustic Size", Float) = 0.01
     
  }

SubShader {
	Tags { "Queue"="Transparent+100"  "RenderType"="Transparent" }
	Lighting Off
	Fog { Color (0, 0, 0) } 
	ZWrite Off
	Blend SrcAlpha One

CGINCLUDE
#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
#include "UnityCG.cginc"
      
sampler2D _ShadowTex;
float4 _ShadowTex_ST;
float _ScrollX;
float _ScrollY;
float _Brightness;
float _Size;
float4 _Color;
float4x4 unity_Projector;
	
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		fixed4 color : TEXCOORD1;
	};

		v2f vert (appdata_full u) {
		v2f p;
		p.pos = UnityObjectToClipPos(u.vertex);
		p.uv = TRANSFORM_TEX (mul (unity_Projector, u.vertex).xyzx, _ShadowTex) / _Size + frac(float2(_ScrollX, _ScrollY) * _Time);
		p.color = fixed4(0,0,0,_Brightness) * _Color ;
		return p;
	}

		

ENDCG
	Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma target 2.0

	fixed4 frag (v2f i) : SV_Target
	{
		fixed4 tex = tex2D (_ShadowTex, i.uv)  * _Brightness * _Color;
		return tex;
	}
ENDCG
	}
}

  
  Subshader {
  Tags { "Queue" = "Transparent" }
     Pass {
        ZWrite off
        Fog { Color (0, 0, 0) }
        Color [_Color]
        ColorMask RGB
        Blend SrcAlpha One
		Offset -1, -1
        SetTexture [_ShadowTex] {
           constantColor (1,1,1,[_Brightness])
           combine texture * primary, constant
           //Matrix [_Projector]
        }
     
     }
  }
}