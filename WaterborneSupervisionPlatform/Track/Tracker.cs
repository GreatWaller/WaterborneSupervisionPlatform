using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using DL.Common;
using Detecting.YoloDetector;
using Tracing.DeepSortTracker;
using Tracing.DeepSortTracker.Matchers.Abstract;
using DL.Common.YLOL;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;


namespace WaterborneSupervisionPlatform.Track
{
    public class Tracker: IDisposable
    {

        private readonly ServiceCollection _services;
        private readonly ServiceProvider _provider;

        private MatcherOption options;
        private float targetConfidence;



        private YoloDetector yoloDetector;
        private DeepSortTracer deepSortTracker;

        private Stopwatch stopwatch = new Stopwatch();
        public Tracker(string configPath="config.json")
        {
            options = ReadMatcherOptionFromJson(configPath);
            
            targetConfidence = float.Clamp(options.TargetConfidence, 0.0f, 1.0f);

            _services = new ServiceCollection();


            yoloDetector = new YoloDetector(new MatcherOption());
            deepSortTracker = new DeepSortTracer(new MatcherOption());

            //yoloDetector = CreateInstance<YoloDetector>("Detecting.YoloDetector.dll", "Detecting.YoloDetector.YoloDetector",
            //    new object?[] { new MatcherOption() });
            //_services.AddTransient<YoloDetector>(sp => yoloDetector);

            //deepSortTracker = CreateInstance<DeepSortTracer>("Tracing.DeepSortTracker.dll", "Tracing.DeepSortTracker.DeepSortTracer",
            //    new object?[] { new MatcherOption() });
            //_services.AddTransient<DeepSortTracer>(sp => deepSortTracker);
        }
        

        public IReadOnlyList<ITrack> Track(Bitmap frame)
        {
            stopwatch.Start();
            var predictions = yoloDetector.Predict(frame, DetectionObjectType.Boat);
            IReadOnlyList<ITrack> tracks = deepSortTracker.Track(frame, predictions.ToArray());
            stopwatch.Stop();
            Console.WriteLine($"Total time: {stopwatch.ElapsedMilliseconds}");
            stopwatch.Restart();
            return tracks;
        }


        private static T CreateInstance<T>(string assemblyFile, string fullQualifiedClassName, object?[] parameters = null)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyFile);
            Type type = assembly.GetType(fullQualifiedClassName);

            T instance = default;
            if (parameters == null)
            {
                instance = (T)Activator.CreateInstance(type);
            }
            else
            {
                instance = (T)Activator.CreateInstance(type, parameters);
            }

            return instance;
        }


        private static MatcherOption ReadMatcherOptionFromJson(string json)
        {
            try
            {
                var option = JsonConvert.DeserializeObject<MatcherOption>(File.ReadAllText(json));
                if (option == null)
                {
                    option = new MatcherOption();
                }
                return option;
            }
            catch (Exception ex)
            {
                return new MatcherOption();
            }
        }

        public void Dispose()
        {

        }

    }
}
