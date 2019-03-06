using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;
using System;
using System.IO;

namespace Extensions.GraphicUtils
{
    public class CaptchaUtils
    {
        public string GenerateCaptchaBase64Image(string code)
        {
            var stream = new MemoryStream();
            using (Image<Rgba32> img = new Image<Rgba32>(80, 40))
            {
                PathBuilder pathBuilder = new PathBuilder();
                pathBuilder.SetOrigin(new PointF(30, 0));
                pathBuilder.AddBezier(new PointF(5, 35), new PointF(20, 5), new PointF(20, 5), new PointF(35, 35));
                IPath path = pathBuilder.Build();
                var font = SystemFonts.CreateFont("Arial", 7, FontStyle.Italic);
                using (var img2 = img.Clone(ctx => ctx.ApplyScalingWaterMark(font, code, Rgba32.Black, 2, true)))
                {
                    img2.SaveAsBmp(stream);
                }
            }

            return Convert.ToBase64String(stream.GetBuffer());
        }
    }
}
