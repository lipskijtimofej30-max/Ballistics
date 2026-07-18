Shader "Custom/InfiniteGridXY"
{
    Properties
    {
        _MinorColor ("Minor Line Color", Color) = (1, 1, 1, 0.1)
        _MajorColor ("Major Line Color", Color) = (1, 1, 1, 0.4)
        
        _MinorSpacing ("Minor Spacing (Meters)", Float) = 1.0
        _MajorSpacing ("Major Spacing (Meters)", Float) = 10.0
        
        _LineWidth ("Line Width (Pixels)", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off // Сетка видна с обеих сторон

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Включаем поддержку fwidth (требуется версия шейдера 3.0)
            #pragma target 3.0 
            #include "UnityCG.cginc" // Стандартная библиотека Built-in

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _MinorColor;
            float4 _MajorColor;
            float _MinorSpacing;
            float _MajorSpacing;
            float _LineWidth;

            v2f vert(appdata v)
            {
                v2f o;
                // Переводим координаты для экрана
                o.pos = UnityObjectToClipPos(v.vertex);
                // Получаем мировые координаты точки
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            // Функция рисования сетки
            float DrawGrid(float2 uv, float spacing, float width)
            {
                float2 coord = uv / spacing;
                
                // Магия fwidth для четкости линий в зависимости от расстояния камеры
                float2 derivative = fwidth(coord);
                float2 grid = abs(frac(coord - 0.5) - 0.5) / derivative;
                float lineVal = min(grid.x, grid.y);
                
                return 1.0 - min(lineVal / width, 1.0);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Используем мировые X и Y
                float2 uv = i.worldPos.xy;

                // Мелкая сетка
                float minorGrid = DrawGrid(uv, _MinorSpacing, _LineWidth);
                
                // Крупная сетка (линии чуть толще)
                float majorGrid = DrawGrid(uv, _MajorSpacing, _LineWidth * 2);

                // Смешивание цветов
                float4 finalColor = lerp(float4(0,0,0,0), _MinorColor, minorGrid);
                finalColor = lerp(finalColor, _MajorColor, majorGrid);

                // Отбрасываем пустые пиксели (спасает FPS)
                if (finalColor.a <= 0.001)
                    discard;

                return finalColor;
            }
            ENDCG
        }
    }
}
