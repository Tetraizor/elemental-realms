Shader "Sprites/Sprite Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Float) = 1
        _CheckDiagonal ("Check Diagonal", Float) = 1
        _ShowOutline ("Show Outline", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineThickness;
            float _CheckDiagonal; // 1 = true, 0 = false
            float _ShowOutline; // 1 = true, 0 = false

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);

                if (col.a > 0)
                {
                    return col; // normal sprite
                }

                if(_ShowOutline) {
                    float outline = 0;

                    float2 offsets[8] = {
                        float2(-1, 0), float2(1, 0),
                        float2(0, -1), float2(0, 1),
                        float2(-1, -1), float2(1, -1),
                        float2(-1, 1), float2(1, 1)
                    };

                    for (int j = 0; j < 8; j++)
                    {
                        if (_CheckDiagonal < 0.5 && j >= 4)
                            continue;

                        float2 offsetUV = i.uv + offsets[j] * _OutlineThickness * _MainTex_TexelSize.xy;
                        float sampleAlpha = tex2D(_MainTex, offsetUV).a;
                        if (sampleAlpha > 0.01)
                        {
                            outline = 1;
                        }
                    }

                    if (outline > 0)
                        return _OutlineColor;
                }

                return float4(0,0,0,0);
            }
            ENDHLSL
        }
    }
}
