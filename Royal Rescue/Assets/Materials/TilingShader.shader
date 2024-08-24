// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/TilingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TilingMultiplier ("Tiling Multiplier", Vector) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // Properties
            sampler2D _MainTex;
            float4 _TilingMultiplier;

            // Input structure
            struct appdata
            {
                float4 vertex : POSITION;  //  정점의 위치입니다. 이 값은 모델 공간의 정점 좌표를 나타냅니다.
                float2 uv : TEXCOORD0;  //  정점의 텍스처 좌표(UV)입니다. 텍스처 맵핑에 사용됩니다.
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;  //  버텍스 셰이더에서 계산된 텍스처 좌표를 프래그먼트 셰이더로 전달합니다.
                float4 vertex : SV_POSITION;  // 클립 공간에서의 정점 좌표입니다. 이 값은 화면에 렌더링될 위치를 결정합니다.
            };

            v2f vert (appdata v)
            {
                v2f o;

                // 월드 공간에서 오브젝트의 스케일을 추출합니다. _Object2World는 오브젝트의 월드 변환 행렬로, 
                // 이 행렬의 대각선 값인 _m00, _m11, _m22를 통해 x, y, z 축의 스케일 정보를 가져옵니다. 
                float3 objectScale = unity_ObjectToWorld._m00_m11_m22;

                // 오브젝트 크기에 따라 텍스처 타일링 조정
                // 정점의 UV 좌표를 오브젝트의 스케일과 _TilingMultiplier 값으로 곱하여 텍스처 타일링을 조정합니다.
                // objectScale.xy는 오브젝트의 x, y 스케일을 사용합니다.
                o.uv = v.uv * (objectScale.xy * _TilingMultiplier.xy);

                // 오브젝트의 정점 위치를 클립 공간 좌표로 변환합니다. 이는 화면에 렌더링될 위치를 결정하는 데 사용됩니다.
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target  // 프래그먼트 셰이더 함수로, 각 픽셀의 색상을 계산하여 반환합니다.
            {
                // 텍스처를 샘플링합니다.
                half4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
