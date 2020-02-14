Shader "Chams" {
	Properties{
		_ColorVisible("Color Visible", Color) = (1, 1, 0, 1)
		_ColorBehind("Color Behind", Color) = (1, 0, 0, 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}
		SubShader{
			Tags { "RenderType" = "Opaque" "Queue" = "Overlay+100" }
			Pass {
					ZTest LEqual
					ZWrite Off
					Color[_ColorVisible]
			}
			Tags { "RenderType" = "Opaque" "Queue" = "Overlay+100" }
			Pass {
					ZTest Greater
					ZWrite Off
					Color[_ColorBehind]
			}
	}
		FallBack "Diffuse"
}
