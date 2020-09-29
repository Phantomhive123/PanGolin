Shader "Unlit/Bloom"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    //高斯模糊较亮的区域 
    _Bloom("Bloom(RGB)",2D) = "Black"{}
    //阔值，提取大于这个亮度的区域 后面将在 大于这个值 的区域里进行高斯模糊
    _luminanceThreshold("luminanceThreshold",Float) = 0.5
        //控制不同迭代之间高斯模糊的模糊区域范围  也就是uv偏移的范围
        _BlurSize("Blur Size",Float) = 1.0
    }
        SubShader
    {
        CGINCLUDE
         #include "UnityCG.cginc"  
        sampler2D _MainTex;
        half4 _MainTex_TexelSize;
        sampler2D _Bloom;
        Float _luminanceThreshold;
        float _BlurSize;



        ///提取交亮区域的 顶点.片元 着色器
        struct v2f {
        float4 pos :SV_POSITION;
        half2 uv :TEXCOORD0;
        };
        v2f vertExtractBright(appdata_img v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.texcoord;
            return o;
        }
        //通过主贴图得到一个灰度值
        fixed4 luminance(fixed4 color) {
            return 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
        }
        fixed4 fragExtractBright(v2f i) :SV_Target {
            ///我们降采样得到的亮度值减去阔值_luminanceThreshold，并把结果截取到0到1的范围内，然后
            ///我们把该值和原像素相乘，得到提取后的两部区域
            ///这样就是低于_luminanceThreshold显示为黑色

            fixed c = tex2D(_MainTex , i.uv);
        //clamp(x,a,b)，如果x<a,返回a，x>b，返回b，否则返回为x；
        fixed val = clamp(luminance(c) - _luminanceThreshold, 0.0, 1.0);

        return c * val;
    }



            ///使用Unity提供的_MainTex_Texel_TexelSize变量，计算相邻文理坐标的的偏移（也是高斯模糊的写法）
            //竖方向跟横方向的两个顶点着色器公用的v2f输出定义
            struct v2fBlur {
                float4 pos : SV_POSITION;
                half2 uv[5]:TEXCOORD0;
            };
    //竖直方向的
    v2fBlur vertBlurV(appdata_img v) {
        v2fBlur o;
        o.pos = UnityObjectToClipPos(v.vertex);
        half2 uv = v.texcoord;

        o.uv[0] = uv;
        o.uv[1] = uv + float2 (0.0,_MainTex_TexelSize.y * 1.0) * _BlurSize;
        o.uv[2] = uv - float2 (0.0,_MainTex_TexelSize.y * 1.0) * _BlurSize;
        o.uv[3] = uv + float2 (0.0,_MainTex_TexelSize.y * 2.0) * _BlurSize;
        o.uv[4] = uv - float2 (0.0,_MainTex_TexelSize.y * 2.0) * _BlurSize;

        return o;
    }

    //水平方向的
    v2fBlur vertBlurH(appdata_img v) {
        v2fBlur o;
        o.pos = UnityObjectToClipPos(v.vertex);
        half2 uv = v.texcoord;

        o.uv[0] = uv;
        o.uv[1] = uv + float2 (_MainTex_TexelSize.x * 1.0 , 0.0) * _BlurSize;
        o.uv[2] = uv - float2 (_MainTex_TexelSize.x * 1.0 , 0.0) * _BlurSize;
        o.uv[3] = uv + float2 (_MainTex_TexelSize.x * 2.0 , 0.0) * _BlurSize;
        o.uv[4] = uv - float2 (_MainTex_TexelSize.x * 2.0 , 0.0) * _BlurSize;

        return o;
    }

    //定义两个pass公用的片元着色器
    fixed4 fragBlur(v2fBlur i) : SV_Target{

    float weight[3] = {0.4026, 0.2442, 0.0545};

    fixed3 sum = tex2D(_MainTex,i.uv[0]).rgb * weight[0];

    for (int j = 1; j < 3; j++) {
        sum += tex2D(_MainTex,i.uv[j * 2 - 1]).rgb * weight[j];
        sum += tex2D(_MainTex,i.uv[j * 2]).rgb * weight[j];
    }
    return fixed4(sum,1.0);

    }


        ///混合亮部图像和原图像时使用的 顶点.片元 着色器
        struct v2fBloom {
            float4 pos :SV_POSITION;
            half4 uv :TEXCOORD0;
        };

        v2fBloom vertBloom(appdata_img v) {
            v2fBloom  o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv.xy = v.texcoord;
            o.uv.zw = v.texcoord;

            #if UNITY_UV_STARTS_AT_TOP
            if (_MainTex_TexelSize.y < 0.0)
                o.uv.w = 1.0 - o.uv.w;
            #endif

            return o;
        }
        fixed4 fragBloom(v2fBloom i) :SV_Target{
            return tex2D(_MainTex, i.uv.xy) + tex2D(_Bloom,i.uv.zw);

        }

        ENDCG



        ZTest Always Cull Off ZWrite Off

        pass {
            CGPROGRAM
            #pragma vertex vertExtractBright
            #pragma fragment  fragExtractBright
            ENDCG
        }

        pass {

            CGPROGRAM
            #pragma vertex vertBlurV
            #pragma fragment  fragBlur
            ENDCG
        }

        pass {

            CGPROGRAM
            #pragma vertex vertBlurH
            #pragma fragment  fragBlur
            ENDCG
        }

        pass {

            CGPROGRAM
            #pragma vertex vertBloom
            #pragma fragment  fragBloom
            ENDCG
        }

    }
        FallBack Off
}