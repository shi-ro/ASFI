using RTS1_Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public static class NoiseMap
    {
        private static Random _r = new Random();
        public static int[,] GetNotSmoothedNoise(int w, int h, int variace)
        {
            int[,] map = new int[h,w];
            for(int y = 0; y < h; y++)
            {
                for(int x = 0; x < w; x++)
                {
                    map[y, x] = _r.Next(variace);
                }
            }
            return map;
        }
        
        //public static int[,] GetIntMap(this Bitmap bmp)
        //{
        //    int[,] map = new int[bmp.Height, bmp.Width];
        //    for(int y = 0; y < bmp.Height; y++)
        //    {
        //        for(int x = 0; x < bmp.Width; x++)
        //        {
                    
        //        }
        //    }
        //}

        public static int GetApproximateNoiseVariance(this int[,] noise)
        {
            int max = 0;
            for (int y = 0; y < noise.GetLength(0); y++)
            {
                for (int x = 0; x < noise.GetLength(1); x++)
                {
                    if (noise[y, x] > max) 
                    {
                        max = noise[y, x];
                    }
                }
            }
            return max;
        }

        public static int[,] ChangeVariance(this int[,] noise, int variance)
        {
            int var = noise.GetApproximateNoiseVariance();
            double sec = (var + 0.0f) / (variance + 0.0f);
            Console.WriteLine(sec);
            for (int y = 0; y < noise.GetLength(0); y++) 
            {
                for (int x = 0; x < noise.GetLength(1); x++)
                {
                    noise[y, x] = (int)(noise[y, x] / sec);
                }
            }
            return noise;
        }
        
        private static int GetValueUsingMatrix(this int[,] values, int[,] matrix, SelectionMethod method)
        {
            int i = 0;
            int tot = 0;
            List<int> all = new List<int>();
            for (int y = 0; y < values.GetLength(0); y++)
            {
                for (int x = 0; x < values.GetLength(1); x++)
                {
                    int cur = values[y, x] * matrix[y, x];
                    if(cur > 0)
                    {
                        if (method == SelectionMethod.Mean)
                        {
                            tot += cur;
                            i++;
                        }
                        else if (method == SelectionMethod.Median)
                        {
                            all.Add(cur);
                        }
                    }
                }
            }
            if(all.Count>0)
            {
                all.Sort();
                return all[all.Count / 2];
            }
            return tot / i;
        }

        private static int[,] GetInRange(this int[,] noise, int x, int y, int w, int h)
        {
            int[,] ret = new int[h, w];
            int left = (int)Math.Round(w / 2.0);
            int top = (int)Math.Round(h / 2.0);
            for(int wy = 0; wy < h; wy++)
            {
                for(int ex = 0; ex < w; ex++)
                {
                    int cur;
                    int cx = x - left + ex;
                    int cy = y - top + wy;
                    if (cx < 0 || cy < 0 || cx > noise.GetLength(1) - 1 || cy > noise.GetLength(1) - 1)
                    {
                        cur = 0;
                    } else
                    {
                        cur = noise[cy, cx];
                    }
                    ret[wy, ex] = cur;
                }
            }
            return ret;
        }

        public static int[,] ApplyMatrix(this int[,] noise, int [,] matrix, SelectionMethod method)
        {
            int[,] ret = new int[noise.GetLength(0), noise.GetLength(1)];
            for (int y = 0; y < noise.GetLength(0); y++)
            {
                for (int x = 0; x < noise.GetLength(1); x++)
                {
                    int[,] values = noise.GetInRange(x,y,matrix.GetLength(1),matrix.GetLength(0));
                    int cur = values.GetValueUsingMatrix(matrix, method);
                    ret[y, x] = cur;
                }
            }
            return ret;
        }

        public static int[,] MedianBlur(this int[,] noise, int range)
        {
            int[,] matrix = new int[range, range];
            for (int y = 0; y < range; y++)
            {
                for (int x = 0; x < range; x++)
                {
                    matrix[y, x] = 1;
                }
            }
            return noise.ApplyMatrix(matrix, SelectionMethod.Median);
        }

        public static int[,] MeanBlur(this int[,] noise, int range)
        {
            int[,] matrix = new int[range, range];
            for (int y = 0; y < range; y++)
            {
                for (int x = 0; x < range; x++)
                {
                    matrix[y, x] = 1;
                }
            }
            return noise.ApplyMatrix(matrix, SelectionMethod.Mean);
        }

        public static int[,] Quiver(this int[,] noise, int shuffle)
        {
            int var = noise.GetApproximateNoiseVariance();
            for (int y = 0; y < noise.GetLength(0); y++)
            {
                for (int x = 0; x < noise.GetLength(1); x++)
                {
                    noise[y, x] = Math.Abs(_r.Next(shuffle) * (_r.Next(2) == 0 ? -1 : 1) + noise[y, x]) % var;
                }
            }
            return noise;
        }
    }
}
