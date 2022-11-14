Shader "Custom/Pixelize" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NumPixel ("Pixel Count", Range(10, 200)) = 200
	}
	SubShader {
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"		
			uniform sampler2D _MainTex;

			fixed _NumPixel;
			
			fixed4 frag(v2f_img i) : COLOR
			{
				float stepSize = 1.0 / _NumPixel;
				float2 fragment = float2(stepSize * floor(i.uv.x * _NumPixel), stepSize * floor(i.uv.y * _NumPixel));
				fixed4 finalColor = tex2D(_MainTex, fragment);  
				return finalColor;
			}
	
			ENDCG
			}
	} 
	FallBack "Diffuse"
}
