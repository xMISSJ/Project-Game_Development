Shader "Custom/projection" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		// Tags { "RenderType"="Opaque" }
		Tags {"QUEUE"="Transparent" "RenderType"="Transparent" }
		LOD 200
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard  vertex:vert fullforwardshadows alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		//sampler2D _MainTex; // removed in source 1 not sure what it does. test show that it removes texture and makes vertex shading work

		struct Input {
			float2 uv_MainTex;
			float3 vertexColor; // Vertex color stored here by vert() method // source 1
			float a;
		};
		
		// source 1
		struct v2f {
           float4 pos : SV_POSITION;
           fixed4 color : COLOR;
        };
        
        // source 1
        void vert (inout appdata_full v, out Input o)
         {
             UNITY_INITIALIZE_OUTPUT(Input,o);
             o.vertexColor = v.color; // Save the Vertex Color in the Input for the surf() method
			 o.a = v.color.w;
         }
         sampler2D _MainTex;

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		//UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here //  i do not use this 	
		//UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo =  c.rgb ; 
			
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a *IN.a; // change transeparantie based on vertex color//------------------------------ source 1
			clip(o.Alpha - 0.1);
		}
		ENDCG
	}
	FallBack "Diffuse"
}

// source 1
// taken from https://answers.unity.com/questions/923726/unity-5-standard-shader-support-for-vertex-colors.html