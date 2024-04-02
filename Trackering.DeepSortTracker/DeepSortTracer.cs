using Tracing.DeepSortTracker.Matchers.Deep;
using Tracing.DeepSortTracker.ReID.Models.Fast_Reid;
using Tracing.DeepSortTracker.ReID.Models.OSNet;
using Tracing.DeepSortTracker.ReID;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.OnnxRuntime;
using Tracing.DeepSortTracker.Matchers.Abstract;
using System.Diagnostics;
using System.Drawing;
using DL.Common.YLOL;
using DL.Common;

namespace Tracing.DeepSortTracker
{
    public class DeepSortTracer
    {
        private readonly Matcher matcher;
        private readonly MatcherOption _options;
        public DeepSortTracer(MatcherOption options)
        {
            _options = options;
            matcher = new DeepSortMatcher(
                    ConstructAppearanceExtractorFromOptions(options),
                    options.AppearanceWeight ?? 0.775f,
                    options.Threshold ?? 0.5f,
                    options.MaxMisses ?? 50,
                    options.FramesToAppearanceSmooth ?? 40,
                    options.SmoothAppearanceWeight ?? 0.875f,
                    options.MinStreak ?? 8);
        }
        public IReadOnlyList<ITrack> Track(Bitmap bitmap, IPrediction[] detectedObjects)
        {
            IReadOnlyList<ITrack> tracks = matcher.Run(bitmap, detectedObjects);

            //Trace.TraceInformation($"Tracks count: {tracks.Count}; DetectedObject TrackId: {detectedObjects[0].TrackingId}") ;
            return tracks;
        }

        private static IAppearanceExtractor ConstructAppearanceExtractorFromOptions(MatcherOption options)
        {
            if (string.IsNullOrEmpty(options.AppearanceExtractorFilePath))
                throw new ArgumentNullException($"{nameof(options.AppearanceExtractorFilePath)} was undefined.");

            if (options.AppearanceExtractorVersion == null)
                throw new ArgumentNullException($"{nameof(options.AppearanceExtractorVersion)} was undefined.");

            const int DefaultExtractorsCount = 4;

            IAppearanceExtractor appearanceExtractor = options.AppearanceExtractorVersion switch
            {
                AppearanceExtractorVersion.OSNet => new ReidScorer<OSNet_x1_0>(File.ReadAllBytes(options.AppearanceExtractorFilePath),
                    options.ExtractorsInMemoryCount ?? DefaultExtractorsCount, SessionOptions.MakeSessionOptionWithCudaProvider()),
                AppearanceExtractorVersion.FastReid => new ReidScorer<Fast_Reid_mobilenetv2>(File.ReadAllBytes(options.AppearanceExtractorFilePath),
                    options.ExtractorsInMemoryCount ?? DefaultExtractorsCount, SessionOptions.MakeSessionOptionWithCudaProvider()),
                _ => throw new Exception("Appearance extractor cannot be constructed.")
            };

            return appearanceExtractor;
        }
    }
}
