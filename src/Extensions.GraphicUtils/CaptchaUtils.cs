using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.IO;

namespace Extensions.GraphicUtils
{
    public static class CaptchaUtils
    {
        public static string GenerateCaptchaBase64Image(string code)
        {
            Guard.NotNullOrEmpty(code, nameof(code));

            var positions = new List<byte> { 15, 7, 16, 23, 13 };
            positions = ShuffleList(positions);

            var fonts = new FontCollection();
            var font = fonts.Install("arial.ttf");
            var _font = font.CreateFont(16, FontStyle.Italic);
            var stream = new MemoryStream();

            using (Image<Rgba32> img = new Image<Rgba32>(80, 40))
            {
                img.Mutate(x => x.DrawLines(Rgba32.Gray, 2, new PointF(0, 10), new PointF(80, 10)));
                img.Mutate(x => x.DrawLines(Rgba32.Gray, 2, new PointF(0, 30), new PointF(80, 30)));
                for (var i = 0; i < code.Length; ++i)
                {
                    img.Mutate(x => x.DrawText(code.Substring(i, 1), _font, Rgba32.Black, new PointF(i * 15 + 5, positions[i])));
                }

                img.SaveAsPng(stream);
            }

            return Convert.ToBase64String(stream.GetBuffer());
        }

        private static List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();
            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }
    }
}
