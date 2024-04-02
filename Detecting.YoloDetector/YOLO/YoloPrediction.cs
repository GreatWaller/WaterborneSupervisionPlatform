using DL.Common.YLOL;
using System.Drawing;

namespace Detecting.YoloDetector.YOLO
{
    public class YoloPrediction : IPrediction
    {
        public YoloPrediction(DetectionObjectType detectedObject, float confidence, Rectangle rectangle)
        {
            DetectionObjectType = detectedObject;
            Confidence = confidence;
            CurrentBoundingBox = rectangle;
        }

        public DetectionObjectType DetectionObjectType { get; private set; }
        public Rectangle CurrentBoundingBox { get; private set; }
        public float Confidence { get; private set; }
    }
}
