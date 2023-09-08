Shader"Custom/IDPhotoShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CropX ("Crop X", Range(0, 1)) = 0
        _CropY ("Crop Y", Range(0, 1)) = 0
        _CropWidth ("Crop Width", Range(0, 1)) = 1
        _CropHeight ("Crop Height", Range(0, 1)) = 1
    }

    SubShader
    {
        Pass
        {
            Tags { "Queue"="Transparent" "RenderType"="Transparent" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _CropX;
            float _CropY;
            float _CropWidth;
            float _CropHeight;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = float2(v.uv.x * _CropWidth + _CropX, v.uv.y * _CropHeight + _CropY);
                return o;
            }

            sampler2D _MainTex;

            half4 frag(v2f i) : SV_Target
            {
                half4 texColor = tex2D(_MainTex, i.uv);
                half grayscale = dot(texColor.rgb, half3(0.299, 0.587, 0.114));
                return half4(grayscale, grayscale, grayscale, texColor.a);
            }
            ENDCG
        }
    }
}