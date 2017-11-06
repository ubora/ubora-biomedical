using System.Collections.Generic;
using TwentyTwenty.Storage;

namespace Ubora.Web.Infrastructure.ImageServices
{
    public class ImageSize
    {
        public int Width { get; }
        public int Height { get; }

        public ImageSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static ImageSize Thumbnail400x300 = new ImageSize(400, 300);
       // public static ImageSize Banner1500x1125 = new ImageSize(1500, 1125);

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
    }

    public class SizeOptions : List<ImageSize>
    {
        public bool IncludeOriginal { get; set; } = true;

        public static SizeOptions AllDefaultSizes = new SizeOptions
        {
            ImageSize.Thumbnail400x300,
          //  ImageSize.Banner1500x1125
        };
    }
}
