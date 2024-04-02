using System.Drawing;

namespace DL.Common.YLOL
{
    public interface IPrediction
    {
        public DetectionObjectType DetectionObjectType { get; }
        public Rectangle CurrentBoundingBox { get; }
        public float Confidence { get; }
    }
}
