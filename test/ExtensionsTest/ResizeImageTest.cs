using System;
using Xunit;
using Extensions;
using System.Threading.Tasks;
using Extensions.GraphicUtils;
using System.IO;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace ExtensionsTest
{
    public class ResizeImageTest
    {
        [Fact]
        public async Task ResizeImageOrdinary()
        {
            var thumWidth = 150;
            var thumHeight = 120;
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var inputPath = Path.Combine(basePath, "files", "butterfly.jpg");
            var outputPath = Path.Combine(basePath, "files", "butterfly_tmb.jpg");
            
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            await GraphicUtils.Resize(inputPath, outputPath, thumWidth, thumHeight, Extensions.GraphicUtils.ImageFormats.UnKnown);

            var image = Image.Load(outputPath);
            Assert.True(File.Exists(outputPath) && image.Width== thumWidth && image.Height == thumHeight);
        }

        [Fact]
        public async Task ResizeImageWithOption()
        {
            var thumWidth = 152;
            var thumHeight = 164;
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var inputPath = Path.Combine(basePath, "files", "flower.jpg");
            var outputPath = Path.Combine(basePath, "files", "flower_tmb.jpg");

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            var options = new ResizeOptions
            {
                Size = new Size(thumWidth, thumHeight),
                Mode = ResizeMode.Crop,
                Compand=true,
                Position = AnchorPositionMode.TopLeft
            };


            await GraphicUtils.Resize(inputPath, outputPath, options, Extensions.GraphicUtils.ImageFormats.UnKnown,true);

            var image = Image.Load(outputPath);
            Assert.True(File.Exists(outputPath));
        }

    }
}
