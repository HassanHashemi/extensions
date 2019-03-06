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
        public static string GenerateCaptchaBase64Image(string code)
        {
            FontCollection fonts = new FontCollection();
            FontFamily font = fonts.Install(@"arial.ttf");
            var _font = font.CreateFont(7, FontStyle.Italic);
            var stream = new MemoryStream();
            using (Image<Rgba32> img = new Image<Rgba32>(80, 40))
            {
                using (var img2 = img.Clone(ctx => ctx.ApplyScalingWaterMark(_font, code, Rgba32.Black, 2, true)))
                {
                    img2.SaveAsPng(stream);
                }
            }

            return Convert.ToBase64String(stream.GetBuffer());
        }
    }
}
