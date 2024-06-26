﻿using Detecting.YoloDetector.Utils.DataStructs;
using System.Drawing;

namespace Detecting.YoloDetector.Extensions
{
    public static class RectangleExtensions
    {
        public static float Area(this Rectangle value) => value.Width * value.Height;
        public static Vector Center(this Rectangle value) => new Vector(value.X + value.Width / 2, value.Y + value.Height / 2);
    }
}
