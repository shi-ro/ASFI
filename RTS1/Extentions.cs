using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RTS1.MonoGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public static class Extensions
    {
        public static Random R = new Random();
        public static Vector2 ToVector2(this Position position)
        {
            return new Vector2(position.X, position.Y);
        }

        public static Position ToTuple(Vector2 vector)
        {
            return new Position((int)vector.X,(int)vector.Y);
        }

        public static string AsString(this int[,] noise)
        {
            string ret = "";
            for (int y = 0; y < noise.GetLength(0); y++)
            {
                for (int x = 0; x < noise.GetLength(1); x++)
                {
                    ret += (char)(noise[y, x] + 100);
                }
                if(y< noise.GetLength(0)-1)
                {
                    ret += "\n";
                }
            }
            return ret;
        }

        

        public static int RoundUp(this double d)
        {
            return RoundDown(d) + 1;
        }

        public static int RoundDown(this double d)
        {
            return Int32.Parse(($"{d}").Split('.')[0]);
        }



        public static Bitmap AddToTopRandom(this Bitmap bmp, Bitmap top)
        {
            int Rx = R.Next(bmp.Width - top.Width);
            int Ry = R.Next(bmp.Height - top.Height);
            Bitmap ret = new Bitmap(bmp.Width, bmp.Height);
            for (int y = Ry; y < Rx + top.Height; y++)
            {
                for (int x = Rx; x < Rx + top.Width; x++)
                {
                    int R = top.GetPixel(x - Rx, y - Ry).R;
                    int G = top.GetPixel(x - Rx, y - Ry).G;
                    int B = top.GetPixel(x - Rx, y - Ry).B;
                    if ( R== 0 && 
                         G== 0 && 
                         B== 0)
                    {
                        ret.SetPixel(x, y, bmp.GetPixel(x, y));
                    }
                    else
                    {
                        ret.SetPixel(x, y, top.GetPixel(x - Rx, y - Ry));
                    }
                }
            }
            return ret;
        }

        public static Bitmap AddToTop(this Bitmap bmp, Bitmap top)
        {
            Bitmap ret = new Bitmap(bmp.Width, bmp.Height);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    if (top.GetPixel(y, x).R == 0 && top.GetPixel(y, x).G == 0 && top.GetPixel(y, x).B == 0)
                    {
                        ret.SetPixel(y, x, bmp.GetPixel(y, x));
                    }
                    else
                    {
                        ret.SetPixel(y, x, top.GetPixel(y, x));
                    }
                }
            }
            return ret;
        }

        public static long GetMS()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static Bitmap ResizeImage(this Bitmap image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static void SetRow(this Bitmap bitmap, int index, System.Drawing.Color c)
        {
            for (int i = 0; i < bitmap.Width; i++)
            {
                bitmap.SetPixel(i, index, c);
            }
        }
        
        public static int Shake(int input, int range)
        {
            return input + R.Next(-Math.Abs(range), Math.Abs(range));
        }

        public static int IndexOf(this List<PlayerNodeDataContainer> list, PlayerIndex index)
        {
            for(int i = 0; i < list.Count; i++)
            {
                if(list[i].Index == index)
                {
                    return i;
                }
            }
            return -1;
        }

        public static Position AveragePosition(Position a, Position b)
        {
            return new Position((int)((a.X+b.X)/2.0f), (int)((a.Y + b.Y) / 2.0f));
        }

        public static float Distance(this GameObject t, GameObject o)
        {
            return (float)Math.Sqrt(Math.Pow(t.Position.Y - o.Position.Y, 2) + Math.Pow(t.Position.X - o.Position.X, 2));
        }

        public static bool Equals(this Direction d1, Direction d2)
        {
            return $"{d1}".Contains($"{d2}") || $"{d2}".Contains($"{d1}");
        }

        public static bool RandomBool()
        {
            return R.Next(2) == 0;
        }

        public static T RandomIndex<T>(this T[] source)
        {
            return source[R.Next(0, source.Length)];
        }

        public static T RandomItem<T>(this IEnumerable<T> source)
        {
            return source.RandomItems(1).Single();
        }
        
        public static IEnumerable<T> RandomItems<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        public static bool WasKeyReleased(this KeyboardState ks, Keys key)
        {
            return ks.IsKeyUp(key) && Program.LastKeyboard.IsKeyDown(key);
        }
        
        public static bool WasKeyPressed(this KeyboardState ks, Keys key)
        {
            if(ks.IsKeyDown(key)&&Program.LastKeyboard.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }

        public static Bitmap CopyToSquareCanvas(this Bitmap sourceBitmap, int canvasWidthLenght)
        {
            float ratio = 1.0f;
            int maxSide = sourceBitmap.Width > sourceBitmap.Height ?
                          sourceBitmap.Width : sourceBitmap.Height;

            ratio = (float)maxSide / (float)canvasWidthLenght;

            Bitmap bitmapResult = (sourceBitmap.Width > sourceBitmap.Height ?
                                    new Bitmap(canvasWidthLenght, (int)(sourceBitmap.Height / ratio))
                                    : new Bitmap((int)(sourceBitmap.Width / ratio), canvasWidthLenght));

            using (Graphics graphicsResult = Graphics.FromImage(bitmapResult))
            {
                graphicsResult.CompositingQuality = CompositingQuality.HighQuality;
                graphicsResult.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsResult.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphicsResult.DrawImage(sourceBitmap,
                                        new System.Drawing.Rectangle(0, 0,
                                            bitmapResult.Width, bitmapResult.Height),
                                        new System.Drawing.Rectangle(0, 0,
                                            sourceBitmap.Width, sourceBitmap.Height),
                                            GraphicsUnit.Pixel);
                graphicsResult.Flush();
            }

            return bitmapResult;
        }

        public static Bitmap ImageBlurFilter(this Bitmap sourceBitmap,
                                                    BlurType blurType)
        {
            Bitmap resultBitmap = null;

            switch (blurType)
            {
                case BlurType.Mean3x3:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                                       Matrix.Mean3x3, 1.0 / 9.0, 0);
                    }
                    break;
                case BlurType.Mean5x5:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                                       Matrix.Mean5x5, 1.0 / 25.0, 0);
                    }
                    break;
                case BlurType.Mean7x7:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                                       Matrix.Mean7x7, 1.0 / 49.0, 0);
                    }
                    break;
                case BlurType.Mean9x9:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                                       Matrix.Mean9x9, 1.0 / 81.0, 0);
                    }
                    break;
                case BlurType.GaussianBlur3x3:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                                Matrix.GaussianBlur3x3, 1.0 / 16.0, 0);
                    }
                    break;
                case BlurType.GaussianBlur5x5:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                               Matrix.GaussianBlur5x5, 1.0 / 159.0, 0);
                    }
                    break;
                case BlurType.MotionBlur5x5:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                                   Matrix.MotionBlur5x5, 1.0 / 10.0, 0);
                    }
                    break;
                case BlurType.MotionBlur5x5At45Degrees:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur5x5At45Degrees, 1.0 / 5.0, 0);
                    }
                    break;
                case BlurType.MotionBlur5x5At135Degrees:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur5x5At135Degrees, 1.0 / 5.0, 0);
                    }
                    break;
                case BlurType.MotionBlur7x7:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur7x7, 1.0 / 14.0, 0);
                    }
                    break;
                case BlurType.MotionBlur7x7At45Degrees:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur7x7At45Degrees, 1.0 / 7.0, 0);
                    }
                    break;
                case BlurType.MotionBlur7x7At135Degrees:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur7x7At135Degrees, 1.0 / 7.0, 0);
                    }
                    break;
                case BlurType.MotionBlur9x9:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur9x9, 1.0 / 18.0, 0);
                    }
                    break;
                case BlurType.MotionBlur9x9At45Degrees:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur9x9At45Degrees, 1.0 / 9.0, 0);
                    }
                    break;
                case BlurType.MotionBlur9x9At135Degrees:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur9x9At135Degrees, 1.0 / 9.0, 0);
                    }
                    break;
                case BlurType.Median3x3:
                    {
                        resultBitmap = sourceBitmap.MedianFilter(3);
                    }
                    break;
                case BlurType.Median5x5:
                    {
                        resultBitmap = sourceBitmap.MedianFilter(5);
                    }
                    break;
                case BlurType.Median7x7:
                    {
                        resultBitmap = sourceBitmap.MedianFilter(7);
                    }
                    break;
                case BlurType.Median9x9:
                    {
                        resultBitmap = sourceBitmap.MedianFilter(9);
                    }
                    break;
                case BlurType.Median11x11:
                    {
                        resultBitmap = sourceBitmap.MedianFilter(11);
                    }
                    break;
            }

            return resultBitmap;
        }

        private static Bitmap ConvolutionFilter(this Bitmap sourceBitmap,
                                                  double[,] filterMatrix,
                                                       double factor = 1,
                                                            int bias = 0)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new System.Drawing.Rectangle(0, 0,
                                     sourceBitmap.Width, sourceBitmap.Height),
                                                       ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;

            int filterWidth = filterMatrix.GetLength(1);
            int filterHeight = filterMatrix.GetLength(0);

            int filterOffset = (filterWidth - 1) / 2;
            int calcOffset = 0;

            int byteOffset = 0;

            for (int offsetY = filterOffset; offsetY <
                sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                    sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;

                    byteOffset = offsetY *
                                 sourceData.Stride +
                                 offsetX * 4;

                    for (int filterY = -filterOffset;
                        filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);

                            blue += (double)(pixelBuffer[calcOffset]) *
                                    filterMatrix[filterY + filterOffset,
                                                        filterX + filterOffset];

                            green += (double)(pixelBuffer[calcOffset + 1]) *
                                     filterMatrix[filterY + filterOffset,
                                                        filterX + filterOffset];

                            red += (double)(pixelBuffer[calcOffset + 2]) *
                                   filterMatrix[filterY + filterOffset,
                                                      filterX + filterOffset];
                        }
                    }

                    blue = factor * blue + bias;
                    green = factor * green + bias;
                    red = factor * red + bias;

                    blue = (blue > 255 ? 255 :
                           (blue < 0 ? 0 :
                            blue));

                    green = (green > 255 ? 255 :
                            (green < 0 ? 0 :
                             green));

                    red = (red > 255 ? 255 :
                          (red < 0 ? 0 :
                           red));

                    resultBuffer[byteOffset] = (byte)(blue);
                    resultBuffer[byteOffset + 1] = (byte)(green);
                    resultBuffer[byteOffset + 2] = (byte)(red);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new System.Drawing.Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                                      ImageLockMode.WriteOnly,
                                                 PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }

        public enum BlurType
        {
            Mean3x3,
            Mean5x5,
            Mean7x7,
            Mean9x9,
            GaussianBlur3x3,
            GaussianBlur5x5,
            MotionBlur5x5,
            MotionBlur5x5At45Degrees,
            MotionBlur5x5At135Degrees,
            MotionBlur7x7,
            MotionBlur7x7At45Degrees,
            MotionBlur7x7At135Degrees,
            MotionBlur9x9,
            MotionBlur9x9At45Degrees,
            MotionBlur9x9At135Degrees,
            Median3x3,
            Median5x5,
            Median7x7,
            Median9x9,
            Median11x11
        }

        public static Bitmap MedianFilter(this Bitmap sourceBitmap,
                                                    int matrixSize)
        {
            BitmapData sourceData =
                       sourceBitmap.LockBits(new System.Drawing.Rectangle(0, 0,
                       sourceBitmap.Width, sourceBitmap.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride *
                                          sourceData.Height];

            byte[] resultBuffer = new byte[sourceData.Stride *
                                           sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,
                                       pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            int filterOffset = (matrixSize - 1) / 2;
            int calcOffset = 0;

            int byteOffset = 0;

            List<int> neighbourPixels = new List<int>();
            byte[] middlePixel;

            for (int offsetY = filterOffset; offsetY <
                sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                    sourceBitmap.Width - filterOffset; offsetX++)
                {
                    byteOffset = offsetY *
                                 sourceData.Stride +
                                 offsetX * 4;

                    neighbourPixels.Clear();

                    for (int filterY = -filterOffset;
                        filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {

                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);

                            neighbourPixels.Add(BitConverter.ToInt32(
                                             pixelBuffer, calcOffset));
                        }
                    }

                    neighbourPixels.Sort();

                    middlePixel = BitConverter.GetBytes(
                                       neighbourPixels[filterOffset]);

                    resultBuffer[byteOffset] = middlePixel[0];
                    resultBuffer[byteOffset + 1] = middlePixel[1];
                    resultBuffer[byteOffset + 2] = middlePixel[2];
                    resultBuffer[byteOffset + 3] = middlePixel[3];
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);

            BitmapData resultData =
                       resultBitmap.LockBits(new System.Drawing.Rectangle(0, 0,
                       resultBitmap.Width, resultBitmap.Height),
                       ImageLockMode.WriteOnly,
                       PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                       resultBuffer.Length);

            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }
    }
}
