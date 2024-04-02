using Detecting.YoloDetector.Utils.DataStructs;
using System.Drawing;

namespace Detecting.YoloDetector.Extensions
{
    public static class RectangleFExtensions
    {
        public static float Area(this RectangleF value) => value.Width * value.Height;
        public static Vector Center(this RectangleF value) => new Vector(value.X + value.Width / 2, value.Y + value.Height / 2);
    }
}
