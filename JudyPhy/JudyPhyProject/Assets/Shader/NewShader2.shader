Shader "Custom/NewShader2" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
		_Cutoff ("Cutoff", Range(0,1)) = 0
	}
	SubShader {
		Tags { "Queue"="Transparent" }
		Lighting Off
		ZWrite Off
		Blend Off
		AlphaTest GEqual[_Cutoff]
		Pass
		{
			SetTexture[_Mask]{Combine texture}
			SetTexture[_MainTex]{Combine texture, previous}
		}
	} 
}
