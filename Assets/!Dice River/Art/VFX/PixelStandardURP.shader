Shader "Custom/URP_Sprite_Z_Perfect" {
    Properties {
        [MainTexture] [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [MainColor] _Color ("Tint", Color) = (1,1,1,1)
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
    }

    SubShader {
        Tags { 
            "Queue" = "AlphaTest" 
            "RenderType" = "TransparentCutout" 
            "RenderPipeline" = "UniversalPipeline" 
        }

        // Включаем Z-буфер
        ZWrite On
        ZTest LEqual
        Cull Off 

        // Offset помогает избежать "мигания" (Z-fighting), 
        // когда спрайт находится очень близко к другой поверхности.
        Offset -1, -1

        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float4 color      : COLOR; 
                float2 uv         : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float4 color      : COLOR;
                float2 uv         : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _Color;
                half _Cutoff;
            CBUFFER_END

            Varyings vert(Attributes input) {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                output.color = input.color * _Color;
                return output;
            }

            half4 frag(Varyings input) : SV_TARGET {
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                half4 finalColor = texColor * input.color;

                // Используем небольшое смещение (0.001), чтобы при значении 1 
                // объект гарантированно исчезал, а не мигал.
                clip(finalColor.a - (_Cutoff + 0.0001));

                return finalColor;
            }
            ENDHLSL
        }
    }
    Fallback "Universal Render Pipeline/Unlit"
}
