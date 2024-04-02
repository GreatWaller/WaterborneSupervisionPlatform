using Tracing.DeepSortTracker.Matchers.Abstract;
using Tracing.DeepSortTracker.Utils.DataStructs;
using System.Drawing;

namespace Tracing.DeepSortTracker.Matchers.Deep
{
    public class DeepSortTrack : DeepTrack
    {
        public DeepSortTrack(ITrack track, Vector appearance, int medianAppearancesCount) : base(track, appearance, medianAppearancesCount) { }

        public RectangleF PredictedBoundingBox { get; set; }

        protected override void RegisterTrackedInternal(RectangleF trackedRectangle)
        {
            base.RegisterTrackedInternal(trackedRectangle);
        }
    }
}
