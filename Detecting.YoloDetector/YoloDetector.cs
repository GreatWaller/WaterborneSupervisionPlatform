using Detecting.YoloDetector.YOLO;
using Detecting.YoloDetector.YOLO.Models;
using DL.Common;
using DL.Common.YLOL;
using Microsoft.ML.OnnxRuntime;
using System.Drawing;

namespace Detecting.YoloDetector
{
    public class YoloDetector
    {
        IPredictor predictor;
        private MatcherOption _matcherOption;

        public YoloDetector(MatcherOption matcherOption)
        {
            _matcherOption = matcherOption;

            predictor = ConstructPredictorFromOptions(matcherOption);

        }

        public IReadOnlyList<IPrediction> Predict(Bitmap image, params DetectionObjectType[] targetDetectionTypes)
        {
            var predictions = predictor.Predict(image, _matcherOption.TargetConfidence, targetDetectionTypes);
            return predictions;
        }

        private static IPredictor ConstructPredictorFromOptions(MatcherOption options)
        {
            if (string.IsNullOrEmpty(options.DetectorFilePath))
                throw new ArgumentNullException($"{nameof(options.DetectorFilePath)} was undefined.");

            IPredictor predictor = options.YoloVersion switch
            {
                YoloVersion.Yolo640 => new YoloScorer<Yolo640v5>(File.ReadAllBytes(options.DetectorFilePath), SessionOptions.MakeSessionOptionWithCudaProvider()),
                YoloVersion.Yolo1280 => new YoloScorer<Yolo1280v5>(File.ReadAllBytes(options.DetectorFilePath), SessionOptions.MakeSessionOptionWithCudaProvider()),
                YoloVersion.Yolov8 => new YoloScorer<Yolo640v8>(File.ReadAllBytes(options.DetectorFilePath), SessionOptions.MakeSessionOptionWithCudaProvider()),
                _ => throw new Exception("Yolo predictor cannot be constructed.")
            };

            return predictor;
        }
    }

    
}
