using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using SixLabors.ImageSharp;
using System.Net.Http;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Formats.Gif;

namespace Extensions.GraphicUtils
{
    /// <summary>
    /// Determine the Type Of Image
    /// </summary>
    public enum ImageFormats
    {
        UnKnown = 1,
        Jpeg = 5,
        Png = 10,
        Gif = 15
    }

    public class GraphicUtils
    {
        /// <summary>
        /// Resizes an image in accordance with the given <see cref="ResizeOptions"/>.
        /// </summary>
        /// <param name="inputStream">Stream of Input(Original) Image</param>
        /// <param name="outputStream">Stream of Output(Resized) Image</param>
        /// <param name="options">The resize options.</param>
        /// <param name="format">Extention of Output(Resized) Image, if Passing as `UnKnown` it will Set Corresponding by outputPath Parameter</param>
        /// <param name="IsGrayscale">Specifies whether the output image is color or Grayscale</param>
        /// <remarks>Passing zero for one of height or width within the resize options will automatically preserve the aspect ratio of the original image or the nearest possible ratio.</remarks>
        public static void Resize(Stream inputStream, Stream outputStream, ResizeOptions options, ImageFormats format = ImageFormats.UnKnown, bool IsGrayscale = false)
        {
            using (Image<Argb32> image = Image.Load<Argb32>(inputStream))
            {
                if (IsGrayscale)
                {
                    image.Mutate(ctx => ctx
                                        .Resize(options)
                                        .Grayscale());
                }
                else
                {
                    image.Mutate(ctx => ctx
                                        .Resize(options));
                }
                FileStream fileStream = outputStream as FileStream;
                image.Save(outputStream, GetFormat(format, Path.GetExtension(fileStream.Name)));
            }
        }

        /// <summary>
        /// Resizes an image in accordance with the given <see cref="ResizeOptions"/>.
        /// </summary>
        /// <param name="inputPath">Physical path of Input(Original) Image</param>
        /// <param name="outputPath">Physical path of Output(Resized) Image</param>
        /// <param name="options">The resize options.</param>
        /// <param name="format">Extention of Output(Resized) Image, if Passing as `UnKnown` it will Set Corresponding by outputPath Parameter</param>
        /// <param name="IsGrayscale">Specifies whether the output image is color or Grayscale</param>
        /// <remarks>Passing zero for one of height or width within the resize options will automatically preserve the aspect ratio of the original image or the nearest possible ratio.</remarks>
        public static async Task Resize(string inputPath, string outputPath, ResizeOptions options, ImageFormats format = ImageFormats.UnKnown, bool IsGrayscale = false)
        {

            byte[] inputStream;
            using (FileStream SourceStream = File.Open(inputPath, FileMode.Open))
            {
                inputStream = new byte[SourceStream.Length];
                await SourceStream.ReadAsync(inputStream, 0, (int)SourceStream.Length);
            }
            Resize(new MemoryStream(inputStream), new FileStream(outputPath, FileMode.Create), options, format, IsGrayscale);
        }


        /// <summary>
        /// Resize Image to Special Size & Extension
        /// </summary>
        /// <param name="inputPath">Physical path of Input(Original) Image</param>
        /// <param name="outputPath">Physical path of Output(Resized) Image</param>
        /// <param name="targetWidth">Width of Output(Resized) Image</param>
        /// <param name="targetHeight">Height of Output(Resized) Image</param>
        /// <param name="format">Extention of Output(Resized) Image</param>
        /// <returns>Return is Void , It Save Image In Physical Output path</returns>
        public static async Task Resize(string inputPath, string outputPath, int targetWidth, int targetHeight, ImageFormats format = ImageFormats.UnKnown)
        {
            Guard.NotNullOrEmpty(inputPath, nameof(inputPath));
            Guard.NotNullOrEmpty(inputPath, nameof(outputPath));
            Guard.Positive(targetWidth, nameof(targetWidth));
            Guard.Positive(targetHeight, nameof(targetHeight));
            var options = new ResizeOptions
            {
                Size = new Size(targetWidth, targetHeight),
                Mode = ResizeMode.Min
            };

            await Resize(inputPath, outputPath, options);
        }

        private static IImageEncoder GetFormat(ImageFormats format, string extension = null)
        {
            if (format == ImageFormats.UnKnown)
            {
                switch (extension.ToLower())
                {
                    case "jpg":
                    case "jpeg":
                        return new JpegEncoder();

                    case "pnj":
                        return new PngEncoder();

                    case "gif":
                        return new GifEncoder();

                    default:
                        return new JpegEncoder();
                }
            }
            else
            {
                switch (format)
                {
                    case ImageFormats.Jpeg:
                        return new JpegEncoder();
                    case ImageFormats.Png:
                        return new PngEncoder();
                    case ImageFormats.Gif:
                        return new GifEncoder();

                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }
}
