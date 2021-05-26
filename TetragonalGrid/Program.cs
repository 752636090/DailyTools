using AnimatedGif;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TetragonalGrid
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Bitmap srcImage = ReadImageFile(@"image.png");
                Console.WriteLine("png存在");
                CreateTetragonalGrid(srcImage, true, true);
                CreateTetragonalGrid(srcImage, false, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("png不存在");
            }
            try
            {
                Bitmap srcImage = ReadImageFile(@"image_true.png");
                Console.WriteLine("单png存在");
                CreateTetragonalGrid(srcImage, true, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("单png不存在");
            }

            try
            {
                Image srcGif = Image.FromFile(@"gif.gif", true);
                Console.WriteLine("gif存在");
                CreateGif(srcGif, true);
                CreateGif(srcGif, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("gif不存在");
            }
            try
            {
                Image srcGif = Image.FromFile(@"gif_true.gif", true);
                Console.WriteLine("单gif存在");
                CreateGif(srcGif, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("单gif不存在");
            }

            try
            {
                Image bottomGif = Image.FromFile("bottom.gif", true);
                Image topPng = Image.FromFile("top.png", true);
                Console.WriteLine("合成素材存在");
                ComposeGif(bottomGif, topPng);
            }
            catch (Exception e)
            {
                Console.WriteLine("合成素材不存在");
            }

            Console.WriteLine("完成!");
        }

        private static void ComposeGif(Image bottomGif, Image topPng)
        {
            FrameDimension srcDim = new FrameDimension(bottomGif.FrameDimensionsList[0]);
            AnimatedGifCreator gif = AnimatedGif.AnimatedGif.Create("gif_out_compond.gif", GetDelay(bottomGif));
            Bitmap topMap = new Bitmap(topPng);
            int frameCount = bottomGif.GetFrameCount(srcDim);
            for (int i = 0; i < frameCount; i++)
            {
                bottomGif.SelectActiveFrame(srcDim, i);
                Bitmap bottomMap = new Bitmap(bottomGif);
                Bitmap map = ComposeImage(bottomMap, topMap);
                gif.AddFrame(map);
            }
        }

        private static Bitmap ComposeImage(Bitmap bottom, Bitmap top)
        {
            int bW = bottom.Width;
            int bH = bottom.Height;
            int tW = top.Width;
            int tH = top.Height;
            Bitmap map = new Bitmap(bottom);
            int startW = (bW - tW) / 2;
            int startH = (bH - tH) / 2;
            for (int iW = 0; iW < tW; iW++)
            {
                for (int iH = 0; iH < tH; iH++)
                {
                    Color color = top.GetPixel(iW, iH);
                    if (!(color.R < 60 && color.G > 180 && color.B < 60))
                    {
                        map.SetPixel(iW + startW, iH + startH, color); 
                    }
                }
            }
            return map;
        }

        private static void CreateGif(Image srcGif, bool isLeft)
        {
            FrameDimension srcDim = new FrameDimension(srcGif.FrameDimensionsList[0]);
            AnimatedGifCreator gif = AnimatedGif.AnimatedGif.Create(string.Format(@"gif_out_{0}.gif", isLeft), GetDelay(srcGif));
            int frameCount = srcGif.GetFrameCount(srcDim);
            for (int i = 0; i < frameCount; i++)
            {
                srcGif.SelectActiveFrame(srcDim, i);
                Bitmap map = new Bitmap(srcGif);
                map = CreateTetragonalGrid(map, isLeft, false);
                gif.AddFrame(map);
            }
        }

        private static Bitmap ReadImageFile(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = (int)fs.Length;
            byte[] image = new byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            Image result = Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }

        private static void CopyBitmap(Bitmap srcImage, Bitmap targetImage)
        {
            int width = srcImage.Width;
            int height = srcImage.Height;
            for (int iW = 0; iW < width; iW++)
            {
                for (int iH = 0; iH < height; iH++)
                {
                    targetImage.SetPixel(iW, iH, srcImage.GetPixel(iW, iH));
                }
            }
        }

        private static Bitmap CreateTetragonalGrid(Bitmap srcImage, bool isLeft, bool isSave)
        {
            int srcWidth = srcImage.Width;
            int srcHeight = srcImage.Height;
            Bitmap newImg = new Bitmap(srcWidth * 2, srcHeight * 2);
            for (int iW = 0; iW < srcWidth; iW++)
            {
                for (int iH = 0; iH < srcHeight; iH++)
                {
                    int[] targetWidth = new int[4]
                    {
                        isLeft ? srcWidth + iW : iW, // 不操作
                        isLeft ? srcWidth - 1 - iW : 2 * srcWidth - 1 - iW, // 水平翻转
                        isLeft ? srcWidth + iW : iW, // 垂直翻转
                        isLeft ? srcWidth - 1 - iW : 2 * srcWidth - 1 - iW // 斜翻转
                    };
                    int[] targetHeight = new int[4]
                    {
                        iH,
                        iH,
                        2 * srcHeight - 1 - iH,
                        2 * srcHeight - 1 - iH
                    };
                    for (int i = 0; i < 4; i++)
                    {
                        newImg.SetPixel(targetWidth[i], targetHeight[i], srcImage.GetPixel(iW, iH));
                    }
                }
            }
            if (isSave)
            {
                newImg.Save(string.Format(@"image_out_{0}.png", isLeft));
            }
            return newImg;
        }

        private static int GetDelay(Image img)
        {
            FrameDimension dim = new FrameDimension(img.FrameDimensionsList[0]);
            int framcount = img.GetFrameCount(dim);
            if (framcount <= 1)
                return 0;
            else
            {
                int delay = 0;
                bool stop = false;
                for (int i = 0; i < framcount; i++)//遍历图像帧
                {
                    if (stop == true)
                        break;
                    img.SelectActiveFrame(dim, i);//激活当前帧
                    for (int j = 0; j < img.PropertyIdList.Length; j++)//遍历帧属性
                    {
                        if ((int)img.PropertyIdList.GetValue(j) == 0x5100)//如果是延迟时间
                        {
                            PropertyItem pItem = (PropertyItem)img.PropertyItems.GetValue(j);//获取延迟时间属性
                            byte[] delayByte = new byte[4];//延迟时间，以1/100秒为单位
                            delayByte[0] = pItem.Value[i * 4];
                            delayByte[1] = pItem.Value[1 + i * 4];
                            delayByte[2] = pItem.Value[2 + i * 4];
                            delayByte[3] = pItem.Value[3 + i * 4];
                            delay = BitConverter.ToInt32(delayByte, 0) * 10; //乘以10，获取到毫秒
                            stop = true;
                            break;
                        }
                    }


                }
                return delay;
            }
        }
    }
}
