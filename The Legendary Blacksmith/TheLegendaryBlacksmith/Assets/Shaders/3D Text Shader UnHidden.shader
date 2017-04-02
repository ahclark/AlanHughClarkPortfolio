﻿Shader "GUI/3D Text Shader UnHidden" {
	Properties{
		_MainTex("Font Texture", 2D) = "white" {}
	_Color("Text Color", Color) = (1,1,1,1)
	}

		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Lighting Off Cull Back ZWrite Off Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		Stencil{
		Ref 1
		Comp NotEqual
	}

		Pass{
		Color[_Color]
		ColorMaterial AmbientAndDiffuse
		SetTexture[_MainTex]{
		combine primary, texture * primary
	}
	}
	}
}