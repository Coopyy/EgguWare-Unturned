Shader "chamsLit" {
	Properties {
		_ColorVisible ("Color Visible", Color) = (1,1,0)
		_ColorBehind ("Color Behind", Color) = (1,0,0)
		_Intensity ("Intensity", Color) = (0.5, 0.5, 0.5)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "Queue" = "Overlay+100" }
		ZWrite off
		ZTest LEqual
		CGPROGRAM
		#pragma surface surf Lambert
		struct Input {
			float4 color : COLOR;
		};
		fixed4 _ColorVisible;
		fixed4 _Intensity;
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo.rgb = _ColorVisible * _Intensity;
		}
		ENDCG

		
		ZWrite off
		ZTest Greater
		CGPROGRAM
		#pragma surface surf Lambert
		struct Input {
			float4 color : COLOR;
		};
		fixed4 _ColorBehind;
		fixed4 _Intensity;
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo.rgb = _ColorBehind * _Intensity;
		}
		ENDCG
    }
	FallBack "Diffuse"
}
