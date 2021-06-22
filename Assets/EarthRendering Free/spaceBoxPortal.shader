Shader "Unlit/spaceBoxPortal"
{

	SubShader
	{
		Tags{ "Queue" = "Geometry-1" }
		Lighting Off
		LOD 200
		ZWrite Off
		Pass{
			Stencil
			{
				Ref 1
				Comp always
				Pass replace
			}
		}
	}
}
