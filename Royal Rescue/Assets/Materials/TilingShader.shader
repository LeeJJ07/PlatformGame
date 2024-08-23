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
                float4 vertex : POSITION;  //  ������ ��ġ�Դϴ�. �� ���� �� ������ ���� ��ǥ�� ��Ÿ���ϴ�.
                float2 uv : TEXCOORD0;  //  ������ �ؽ�ó ��ǥ(UV)�Դϴ�. �ؽ�ó ���ο� ���˴ϴ�.
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;  //  ���ؽ� ���̴����� ���� �ؽ�ó ��ǥ�� �����׸�Ʈ ���̴��� �����մϴ�.
                float4 vertex : SV_POSITION;  // Ŭ�� ���������� ���� ��ǥ�Դϴ�. �� ���� ȭ�鿡 �������� ��ġ�� �����մϴ�.
            };

            v2f vert (appdata v)
            {
                v2f o;

                // ���� �������� ������Ʈ�� �������� �����մϴ�. _Object2World�� ������Ʈ�� ���� ��ȯ ��ķ�, 
                // �� ����� �밢�� ���� _m00, _m11, _m22�� ���� x, y, z ���� ������ ������ �����ɴϴ�. 
                float3 objectScale = unity_ObjectToWorld._m00_m11_m22;

                // ������Ʈ ũ�⿡ ���� �ؽ�ó Ÿ�ϸ� ����
                // ������ UV ��ǥ�� ������Ʈ�� �����ϰ� _TilingMultiplier ������ ���Ͽ� �ؽ�ó Ÿ�ϸ��� �����մϴ�.
                // objectScale.xy�� ������Ʈ�� x, y �������� ����մϴ�.
                o.uv = v.uv * (objectScale.xy * _TilingMultiplier.xy);

                // ������Ʈ�� ���� ��ġ�� Ŭ�� ���� ��ǥ�� ��ȯ�մϴ�. �̴� ȭ�鿡 �������� ��ġ�� �����ϴ� �� ���˴ϴ�.
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target  // �����׸�Ʈ ���̴� �Լ���, �� �ȼ��� ������ ����Ͽ� ��ȯ�մϴ�.
            {
                // �ؽ�ó�� ���ø��մϴ�.
                half4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
