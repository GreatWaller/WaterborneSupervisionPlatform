using OpenCvSharp;
using OpenCvSharp.Extensions;
using WaterborneSupervisionPlatform.Track;

namespace WaterborneSupervisionPlatform
{
    internal class Program
    {
        static WaterborneSupervisionPlatform.Track.Tracker tracker;
        static MotTraker motTraker;

        static VlcVideoProvider vlcVideoProvider;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //tracker= new WaterborneSupervisionPlatform.Track.Tracker("config.json");
            motTraker = new MotTraker();

            //VideoLoader videoLoader = new VideoLoader();
            //videoLoader.FrameReceived += VideoLoader_FrameReceived;
            //videoLoader.Open("D:\\data\\boat\\river.ts");
            //videoLoader.Play();

            vlcVideoProvider = new VlcVideoProvider("D:\\data\\boat\\river.ts", 1920, 1080);
            vlcVideoProvider.ImageChangeEvent += VlcVideoProvider_ImageChangeEvent;
            var task = vlcVideoProvider.Start();
            task.Wait();
            Console.ReadLine();
        }

        private static void VlcVideoProvider_ImageChangeEvent(string deviceId, System.Drawing.Bitmap image)
        {
            motTraker.Track(image);
        }

        private static void VideoLoader_FrameReceived(object? sender, OpenCvSharp.Mat e)
        {
            motTraker.Track(e.ToBitmap());
        }
    }
}
