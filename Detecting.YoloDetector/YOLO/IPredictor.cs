using System.Drawing;
using System.Collections.Generic;
using System;
using DL.Common.YLOL;

namespace Detecting.YoloDetector.YOLO
{
    public interface IPredictor : IDisposable
    {
        public abstract IReadOnlyList<IPrediction> Predict(Bitmap image, float targetConfidence, params DetectionObjectType[] targetDetectionTypes);
    }
}
