using System.Collections.Generic;
using UnityEngine;

public class bloom : PostEffectsBase
{
    public Shader bloomShader;
    private Material bloomMaterial;
    public Material material
    {
        get
        {
            //根据PostEffectsBase中的方法检测，第一个参数指定了该特效需要使用的Shader，第二个参数则是用于后期处理的材质；
            //该函数首先检查Shader的可用性，检查通过后返回一个使用了该shader的材质，否则返回Null.
            bloomMaterial = CheckShaderAndCreateMaterial(bloomShader, bloomMaterial);
            return bloomMaterial;
        }
    }

    //高斯模糊的叠带次数
    [Range(0, 4)]
    public int iterations = 3;

    //高斯模糊的叠带范围
    [Range(0.2f, 4.0f)]
    public float blurSpread = 0.6f;

    //降采样的数值
    [Range(1, 8)]
    public int downSample = 2;

    //luminanceThreshold，大多数情况下图像亮度不会超过1.但如果我们开启了HDR，硬件会允许我们把颜色值储存在一个更高精度范围的缓冲中，
    //此时像素的亮度就会超过1.
    [Range(0.0f, 4.0f)]
    public float luminanceThreshold = 0.6f;



    //OnRenderImage函数
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            material.SetFloat("_luminanceThreshold", luminanceThreshold);

            //将图像进行降采样不仅可以减少需要处理的像素，提高性能，而且适当的降采样旺旺还可以得到更好的模糊效果
            int rtW = src.width / downSample;
            int rtH = src.height / downSample;

            //定义第一个缓存buffer0，并吧src中的图像缩放后储存到buffer0中。
            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            buffer0.filterMode = FilterMode.Bilinear;

            //调用shader中的第一个Pass提取图像中较亮的区域，提到的较亮区域将储存在buffer0 中。
            Graphics.Blit(src, buffer0, material, 0);

            for (int i = 0; i < iterations; i++)
            {
                material.SetFloat("_BlurSize", 1.0f + i * blurSpread);
                //定义buffer1
                RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                //调用第二个pass，输入buffer0，输出buffer1.
                Graphics.Blit(buffer0, buffer1, material, 1);

                RenderTexture.ReleaseTemporary(buffer0);
                //将输出的buffer1重新赋值给buffer0
                buffer0 = buffer1;
                buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);

                //调用第三个pass，输入buffer0(上面输出的buffer1),输出buffer1（新的buffer1)
                Graphics.Blit(buffer0, buffer1, material, 2);
                //将新的buffer1再次给buffer0赋值
                buffer0 = buffer1;
            }
            //第四个Pass，将buffer0赋值给贴图_Bloom
            material.SetTexture("_Bloom", buffer0);
            Graphics.Blit(src, dest, material, 3);

            RenderTexture.ReleaseTemporary(buffer0);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

}