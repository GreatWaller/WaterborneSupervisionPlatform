using Tracing.DeepSortTracker.Utils.DataStructs;
using System;
using System.Collections.Generic;
using System.Drawing;
using DL.Common.YLOL;

namespace Tracing.DeepSortTracker.ReID
{
    public interface IAppearanceExtractor : IDisposable
    {
        public abstract IReadOnlyList<Vector> Predict(Bitmap image, IPrediction[] detectedBounds);
    }
}
