Shader "UI/CustomGradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Gradient Properties)]
        _Angle ("Angle (Degrees)", Range(0, 360)) = 0

        [Header(Stop 1)]
        _Color1 ("Color 1", Color) = (0.25, 0.325, 0.314, 1.0) // #405350
        _Stop1 ("Position 1", Range(0, 1)) = 0.05

        [Header(Stop 2)]
        _Color2 ("Color 2", Color) = (0.85, 0.918, 0.91, 1.0)  // #D9EAE8
        _Stop2 ("Position 2", Range(0, 1)) = 0.49

        [Header(Stop 3)]
        _Color3 ("Color 3", Color) = (0.396, 0.518, 0.498, 1.0) // #65847F
        _Stop3 ("Position 3", Range(0, 1)) = 0.94

        [Header(Dither Noise Controls)]
        _DitherStrength ("Noise Strength", Range(0, 0.2)) = 0.05
        _DitherScale ("Noise Density / Scale", Range(1, 10)) = 1.0

        [Header(UI Masking Support)]
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 uv       : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            float _Angle;
            fixed4 _Color1, _Color2, _Color3;
            float _Stop1, _Stop2, _Stop3;
            float _DitherStrength;
            float _DitherScale;

            // 4x4 Bayer Matrix for ordered dithering noise
            static const float Bayer4x4[16] = {
                 0.0/16.0,  8.0/16.0,  2.0/16.0, 10.0/16.0,
                12.0/16.0,  4.0/16.0, 14.0/16.0,  6.0/16.0,
                 3.0/16.0, 11.0/16.0,  1.0/16.0,  9.0/16.0,
                13.0/16.0,  5.0/16.0, 15.0/16.0,  7.0/16.0
            };

            v2f vert(appdata_t v)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.uv = v.texcoord;
                OUT.color = v.color * _Color;
                OUT.screenPos = ComputeScreenPos(OUT.vertex);
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // 1. Calculate direction vector from angle
                float rad = _Angle * (3.14159265 / 180.0);
                float2 dir = float2(cos(rad), sin(rad));

                // 2. Base projection factor t (0.0 to 1.0)
                float2 centeredUV = IN.uv - 0.5;
                float t = dot(centeredUV, dir) + 0.5;

                // 3. Screen-space 4x4 Bayer Dither Noise calculation
                float2 pixelPos = (IN.screenPos.xy / IN.screenPos.w) * _ScreenParams.xy * _DitherScale;
                int x = ((int)pixelPos.x) % 4;
                int y = ((int)pixelPos.y) % 4;
                float noise = Bayer4x4[y * 4 + x] - 0.5; // Scale range to [-0.5, 0.5]

                // 4. Inject noise into gradient position 't'
                t = saturate(t + (noise * _DitherStrength));

                // 5. Multi-stop color interpolation
                fixed4 gradientColor;
                if (t <= _Stop1)
                {
                    gradientColor = _Color1;
                }
                else if (t <= _Stop2)
                {
                    float factor = (t - _Stop1) / max(_Stop2 - _Stop1, 0.0001);
                    gradientColor = lerp(_Color1, _Color2, factor);
                }
                else if (t <= _Stop3)
                {
                    float factor = (t - _Stop2) / max(_Stop3 - _Stop2, 0.0001);
                    gradientColor = lerp(_Color2, _Color3, factor);
                }
                else
                {
                    gradientColor = _Color3;
                }

                // 6. Combine with UI element texture and color
                return tex2D(_MainTex, IN.uv) * IN.color * gradientColor;
            }
            ENDCG
        }
    }
}