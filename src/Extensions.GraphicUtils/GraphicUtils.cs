using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Extensions.GraphicUtils
{
    public static class GraphicUtils
    {
        /// <summary>
        /// Get size of image from byte array image .
        /// </summary>
        /// <param name="binaryImage">Byte array of image</param>
        public static Size GetImageSizeFromByteArray(byte[] binaryImage)
        {
            var stream = new MemoryStream(binaryImage);
            using var image = Image.Load<Argb32>(stream);

            return image.Size();
        }

        /// <summary>
        /// GenerateThumbnail from memorystream of image .
        /// </summary>
        /// <param name="inputStream">stream of image</param>
        /// <param name="width">width of thumbnail </param>
        /// <param name="height">height of thumbnail</param>
        public static MemoryStream GenerateThumbnail(MemoryStream inputStream, int width, int height)
        {
            var thumbStream = new MemoryStream();
            using (var image = Image.Load<Argb32>(inputStream))
            {
                image.Mutate(ctx => ctx
                                    .Resize(new ResizeOptions
                                    {
                                        Size = new Size
                                        {
                                            Height = height,
                                            Width = width
                                        }
                                    }));
                image.SaveAsJpeg(thumbStream);
            }

            return thumbStream;
        }

        /// <summary>
        /// ReduseQuality from memorystream of image .
        /// </summary>
        /// <param name="inputStream">stream of image</param>
        /// <param name="quality">quality of output image</param>
        public static MemoryStream ReduseQuality(MemoryStream inputStream, int quality)
        {
            if (quality < 0 || quality > 100)
            {
                new ArgumentException(nameof(quality));
            }

            var thumbStream = new MemoryStream();
            using (var image = Image.Load<Argb32>(inputStream))
            {
                var encoder = new JpegEncoder
                {
                    Quality = quality
                };

                image.SaveAsJpeg(thumbStream, encoder);
            }

            return thumbStream;
        }

        /// <summary>
        /// Resizes an image in accordance with the given <see cref="ResizeOptions"/>.
        /// </summary>
        /// <param name="inputStream">Stream of Input(Original) Image</param>
        /// <param name="outputStream">Stream of Output(Resized) Image</param>
        /// <param name="options">The resize options.</param>
        /// <param name="format">Extention of Output(Resized) Image, if Passing as `UnKnown` it will Set Corresponding by outputPath Parameter</param>
        /// <param name="IsGrayscale">Specifies whether the output image is color or Grayscale</param>
        /// <remarks>Passing zero for one of height or width within the resize options will automatically preserve the aspect ratio of the original image or the nearest possible ratio.</remarks>
        public static void Resize(Stream inputStream, Stream outputStream, ResizeOptions options, ImageFormats format = ImageFormats.UnKnown, bool isGrayscale = false)
        {
            using var image = Image.Load<Argb32>(inputStream);

            if (isGrayscale)
            {
                image.Mutate(ctx => ctx.Resize(options).Grayscale());
            }
            else
            {
                image.Mutate(ctx => ctx.Resize(options));
            }

            var fileStream = outputStream as FileStream;
            image.Save(outputStream, GetFormat(format, Path.GetExtension(fileStream.Name)));
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
        public static async Task Resize(string inputPath, string outputPath, ResizeOptions options, ImageFormats format = ImageFormats.UnKnown, bool isGrayscale = false)
        {
            //byte[] inputStream;
            using (var sourceStream = File.Open(inputPath, FileMode.Open))
            {
                var inputStream = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(inputStream, 0, (int)sourceStream.Length);

                using (var outStream = new FileStream(outputPath, FileMode.Create))
                {
                    Resize(new MemoryStream(inputStream), outStream, options, format, isGrayscale);
                }
            }
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
        public static Task Resize(string inputPath, string outputPath, int targetWidth, int targetHeight, ImageFormats format = ImageFormats.UnKnown)
        {
            var options = new ResizeOptions
            {
                Size = new Size(targetWidth, targetHeight),
                Mode = ResizeMode.Stretch
            };

            return Resize(inputPath, outputPath, options);
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

                    case "png":
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
