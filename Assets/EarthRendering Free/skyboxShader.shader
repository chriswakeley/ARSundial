Shader "Custom/skyboxShader" {

	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader{
		Pass{
		Tags{ "Queue" = "1000" }
		//Cull Front
		ZWrite Off
		SetTexture[_MainTex]{
		ConstantColor[_Color]
		combine texture*constant
	}
	}
	}
}
