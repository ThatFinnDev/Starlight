Shader "UI/Starlight/Rounded"
{
    Properties
    {
        [HideInInspector]_MainTex("Texture", 2D) = "white" {}
        _CornerRadius("Corner Radius", Float) = 20
        _HalfSize("Half Size", Vector) = (0,0,0,0)
        _OuterUV("Outer UV", Vector) = (0,0,1,1)

        // Required for UI.Mask
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            // Your custom properties
            float _CornerRadius;
            float4 _HalfSize;
            float4 _OuterUV;

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(o.worldPosition);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color;
                return o;
            }

            float RoundCornerAlpha(float2 p, float2 halfSize, float radius)
            {
                // Edge case: if radius is 0, just return solid alpha
                if(radius <= 0) return 1.0;

                float2 d = abs(p) - (halfSize - radius);
                float dist = length(max(d, 0.0)) + min(max(d.x, d.y), 0.0) - radius;
                return saturate(1.0 - dist);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Remap UVs based on atlas/outer UVs
                float2 uv = (i.texcoord - _OuterUV.xy) / (_OuterUV.zw - _OuterUV.xy);
                float2 pos = uv * 2.0 - 1.0;
                pos *= _HalfSize.xy;

                half4 color = (tex2D(_MainTex, i.texcoord) + _TextureSampleAdd) * i.color;

                float alpha = RoundCornerAlpha(pos, _HalfSize.xy, _CornerRadius);
                color.a *= alpha;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
}
