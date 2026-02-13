Shader "Unlit/UI_ColorGlow"
{
     Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        // Which sprite color should be "selected" to glow (set per material)
        _TargetColor ("Target Color", Color) = (0,1,0,1)
        _Threshold ("Color Threshold", Range(0,1)) = 0.25

        // What color/intensity should be ADDED as glow (set per state / code)
        _GlowColor ("Glow Color", Color) = (0,1,1,1)
        _GlowStrength ("Glow Strength", Float) = 0

        // Unity UI boilerplate (Mask/RectMask2D/Stencil)
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        _ClipRect ("Clip Rect", Vector) = (-32767, -32767, 32767, 32767)
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
                float4 vertex : SV_POSITION;
                fixed4 color  : COLOR;
                float2 uv     : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            fixed4 _TargetColor;
            float _Threshold;

            fixed4 _GlowColor;
            float _GlowStrength;

            float4 _ClipRect;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                // UI clipping (Mask/RectMask2D)
                col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);

                // Select pixels similar to _TargetColor (distance in RGB space)
                float3 rgb = col.rgb;
                float3 tgt = _TargetColor.rgb;

                float dist = distance(rgb, tgt);

                // mask = 1 when dist < threshold, else 0
                float mask = 1.0 - step(_Threshold, dist);

                // Add glow only where mask is 1
                col.rgb += _GlowColor.rgb * _GlowStrength * mask;

                return col;
            }
            ENDCG
        }
    }
}