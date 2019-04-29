// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UnderWater/UW_3"
{
	Properties 
	{
	_MainTex("MainTex", 2D) = "black" {}
	_BumpMap("BumpMap", 2D) = "black" {}
	_DistortionMap1("DistortionMap1", 2D) = "black" {}
	_DistortionMap2("DistortionMap2", 2D) = "black" {}
	_Color("Water Color", Color) = (1,1,1,1)
	_DistortPower("Distortion Power", Range(0,0.03) ) = 0
	_Opacity("Opacity", Range(-0.1,1.0) ) = 0
	_Cube("Reflection", Cube) = "black" {}
	_ReflectPower("Reflection Power", Range(0,1) ) = 0

	}
	
	SubShader 
	{
		Tags
		{
			"Queue"="Transparent-1"
			"IgnoreProjector"="False"
			"RenderType"="Transparent"
		}

		
		Cull Back
		ZWrite On
		ZTest LEqual
		ColorMask RGBA
		Blend SrcAlpha OneMinusSrcAlpha
		Fog{
		}
		
		
		CGPROGRAM
		#pragma surface surf Underwater decal:add nolightmap
		#pragma target 3.0
		
		
		uniform sampler2D _MainTex;
		uniform sampler2D _BumpMap;
		uniform sampler2D _DistortionMap1;
		uniform sampler2D _DistortionMap2;
		fixed4 _Color;
		half _DistortPower;
		float _Opacity;
		

		half4 LightingUnderwater (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
          half3 h = normalize (lightDir + viewDir);
          half diff = max (0, dot (s.Normal, lightDir));
          float nh = max (0, dot (s.Normal, h));
          float spec = pow (nh, s.Specular*128.0)* s.Gloss;
          half4 c;
          c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * (atten * 2);
          c.a = s.Alpha;
          return c;
      }
				
		
		struct Input {
			float3 viewDir : TEXCOORD4;
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_DistortionMap1;
			float2 uv_DistortionMap2;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			
			
			float2 DistortUV=(IN.uv_DistortionMap1.xy);
			float4 DistortNormal = tex2D(_DistortionMap1,DistortUV);
			float2 FinalDistortion = DistortNormal * _DistortPower;
			
			float2 MainTexUV=(IN.uv_MainTex.xy);			
			float4 DistortedMainTex=tex2D(_MainTex,MainTexUV + FinalDistortion);
			
			float2 BumpMapUV=(IN.uv_BumpMap.xy);			
			float4 DistortedBumpMap=tex2D(_BumpMap,BumpMapUV + FinalDistortion); 
			
			float4 TextureMix=DistortedMainTex * DistortedBumpMap;
			
			float4 FinalDiffuse = _Color * TextureMix; 			
			
			float2 Bump1UV=(IN.uv_DistortionMap1.xy) ;
			
			half4 DistortedBumpMap1=tex2D(_DistortionMap1,Bump1UV + FinalDistortion);			
			
			half2 Bump2UV=(IN.uv_DistortionMap2.xy);
			
			fixed4 DistortedBumpMap2=tex2D(_DistortionMap1,Bump2UV + FinalDistortion);
						
			fixed4 AvgBump= (DistortedBumpMap1 + DistortedBumpMap2) / 2;
			
			fixed4 UnpackNormal1=float4(UnpackNormal(AvgBump).xyz, 1.0);
			
			fixed FinalAlpha = _Opacity;			
			o.Albedo = FinalDiffuse;
			o.Normal = UnpackNormal1;
			o.Alpha = FinalAlpha;

			o.Normal = normalize(o.Normal);
		}
	ENDCG

Pass{
    Tags 
    {
    "Queue"="Transparent"
    "RenderType" = "Transparent" 
    }
	
	 Blend  SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc" 

        	uniform float4x4 _ProjMatrix;
            samplerCUBE _Cube;
            float _ReflectPower;
            uniform sampler2D _DistortionMap1;
            uniform sampler2D _DistortionMap2;
            float4 _DistortionMap1_ST;
            float4 _DistortionMap2_ST;
            fixed _DistortPower;
            float _Opacity;
            float _Flip;
            uniform float4x4 _Rotation;


            struct v2f {
                float4 pos : POSITION;
                float2 distortUV : TEXCOORD2;
                float2 distort2UV : TEXCOORD3;
                float3 I : TEXCOORD1;
                float3 viewDir : TEXCOORD4;
            }; 

            v2f vert( appdata_tan v ) {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                o.distortUV = TRANSFORM_TEX (v.texcoord, _DistortionMap1);
                o.distort2UV = TRANSFORM_TEX (v.texcoord, _DistortionMap2);
                o.viewDir = ObjSpaceViewDir( v.vertex );
                float3 I = reflect( o.viewDir, v.normal ) * _Flip;
                
				o.I = mul( (float3x3)unity_ObjectToWorld, I );  
                return o;
            }

        
        half4 frag( v2f IN ) : SV_Target {
        		
        		float4 DistortNormal =  tex2D(_DistortionMap1,IN.distortUV * 20 ); 
        		float4 DistortNormal2 =  tex2D(_DistortionMap2,IN.distort2UV  * 20); 
        		float FinalDistortion = (DistortNormal.x - DistortNormal2.x) * _DistortPower * 50;
                
                half4 reflection = texCUBE( _Cube, mul(_Rotation, IN.I +  FinalDistortion)); 
                
                half4 final = reflection;
                
                final.a =  _ReflectPower * _Opacity; 
                return final; 
            }   
        ENDCG
}
	
}
	Fallback "Diffuse"
}