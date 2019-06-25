using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingMapCreation
{
    public static class NoiseMap
    {
        public static Bitmap GetMap(this Bitmap img, Gradient grad)
        {
            Console.WriteLine("Getting Map ...");
            Console.WriteLine("Getting Noise Color range ...");
            int MX = 0;
            int MN = 665;
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Color pixel = img.GetPixel(x, y);
                    if(pixel.R>MX)
                    {
                        MX = pixel.R;
                    }
                    if(pixel.R<MN)
                    {
                        MN = pixel.R;
                    }
                }
            }
            double RAT = 255 / (MX - MN + 0.0f);
            Console.WriteLine($"Detirmined Range : [ {MN} - {MX} ]");
            Bitmap ret = new Bitmap(img.Width, img.Height);
            Console.WriteLine($"Creating Map ...");
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Color pixel = img.GetPixel(x, y);
                    ret.SetPixel(x, y, grad.ColorAt((int)(pixel.R * RAT)+MN));
                }
            }
            Console.WriteLine($"Map Created, Returning Map");
            return ret;
        }
        
        public static Bitmap AddNoise(this Bitmap bmp, int distance)
        {
            Console.WriteLine("Generating Noise ...");
            Random r = new Random();
            int wr = 0;
            int sk = 0;
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    if (distance == 0 || r.Next(distance) == 0)
                    {
                        int num = r.Next(0, 256);
                        bmp.SetPixel(x, y, Color.FromArgb(255, num, num, num));
                        wr++;
                    }
                    else
                    {
                        sk++;
                    }
                }
            }
            Console.WriteLine($"Noise Generated [ wrote: {wr}, skipped: {sk}, clarity: {((wr + 0.0f) / (bmp.Width * bmp.Height) * 100)}% ]");
            return bmp;

        }

        public static Bitmap GenerateNoise(int width, int height)
        {
            return GenerateNoise(width, height, 0);
        }

        public static Bitmap GenerateNoise(int width, int height, int distance)
        {
            Console.WriteLine("Generating Noise ...");
            Bitmap finalBmp = new Bitmap(width, height);
            Random r = new Random();
            int wr = 0;
            int sk = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (distance == 0 || r.Next(distance) == 0)
                    {
                        int num = r.Next(0, 256);
                        finalBmp.SetPixel(x, y, Color.FromArgb(255, num, num, num));
                        wr++;
                    } else
                    {
                        sk++;
                    }
                }
            }
            Console.WriteLine($"Noise Generated [ wrote: {wr}, skipped: {sk}, clarity: {((wr+0.0f)/(width*height)*100)}% ]");
            return finalBmp;
        }
    }
}
