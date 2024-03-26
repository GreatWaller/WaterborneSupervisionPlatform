using RtspPlayer.Polygon;
using RtspPlayer.Track;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using WaterborneSupervisionPlatform.Track;

namespace RtspPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private uint width = 1920;
        private uint height = 1080;

        private WriteableBitmap _writeableBitmap;
        private Int32Rect _dirtyRect;

        private VlcVideoProvider? vlcVideoProvider;
        private Tracker tracker;

        private PolygonViewModel? polygonViewModel;
        private long frameCounter = 0;
        private IEnumerable<BoatModel> lastBoatModels;
        Stopwatch stopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();

            _writeableBitmap = new WriteableBitmap(1920, 1080, 96, 96, PixelFormats.Bgr24, null);
            _dirtyRect = new Int32Rect(0, 0, 1920, 1080);
            VideoImage.Source = _writeableBitmap;

            UriTextBox.Text = "D:\\data\\boat\\river.ts";

            polygonViewModel = new PolygonViewModel();
            this.DataContext = polygonViewModel;

            tracker = new Tracker();
            lastBoatModels = new List<BoatModel>();
        }

        private void VlcVideoProvider_ImageChangeEvent(string deviceId, System.Drawing.Bitmap frame)
        {
            //var frame = (Bitmap)bitmap.Clone();

            Dispatcher.BeginInvoke(new Action(() =>
            {
                var mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(frame);
                if (frameCounter++ % 6 == 0)
                {
                    stopwatch.Start();
                    var tracks = tracker.Track(frame);
                    stopwatch.Stop();

                    // 输出经过的时间
                    Trace.TraceInformation("经过的时间: {0}", stopwatch.Elapsed);
                    // 重置计时器
                    stopwatch.Reset();
                    if (tracks.Count > 0)
                    {
                        lastBoatModels = tracks.Select(track => new BoatModel
                        {
                            Id = track.Id,
                            CurrentBoundingBox = track.CurrentBoundingBox,
                        });
                        
                    }
                }
                foreach (var bbox in lastBoatModels)
                {
                    Draw(mat, bbox.Id, bbox.CurrentBoundingBox.X, bbox.CurrentBoundingBox.Y,
                        bbox.CurrentBoundingBox.Width, bbox.CurrentBoundingBox.Height);

                }
                _writeableBitmap.Lock();
                try
                {
                    _writeableBitmap.WritePixels(_dirtyRect, mat.Data, mat.Height * (int)mat.Step(), (int)mat.Step());
                }
                catch (Exception ex)
                {
                    Trace.TraceError(deviceId, ex);
                }
                finally
                {
                    _writeableBitmap.Unlock();
                }
            }));
        }



        private async void OnStartClick(object sender, RoutedEventArgs e)
        {
            try
            {

                vlcVideoProvider?.Stop();
                vlcVideoProvider?.Dispose();
                vlcVideoProvider = new VlcVideoProvider(UriTextBox.Text, width, height);
                vlcVideoProvider.ImageChangeEvent += VlcVideoProvider_ImageChangeEvent;

                await vlcVideoProvider.Start().ConfigureAwait(false);
            }
            catch (Exception)
            {

            }
        }

        private void OnStopClick(object sender, RoutedEventArgs e)
        {
            try
            {
                vlcVideoProvider?.Stop();
                vlcVideoProvider?.Dispose();
                vlcVideoProvider = null;
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point point = e.GetPosition(DrawingCanvas);
            polygonViewModel.Add(point);
        }

        private static void Draw(OpenCvSharp.Mat image, int trackId, double upX, double upY, double width, double height)
        {
            //label formating
            var label = $"{trackId}";
            //Console.WriteLine($"confidence {confidence * 100:0.00}% {label}");
            //draw result
            image.Rectangle(new OpenCvSharp.Point(upX, upY), new OpenCvSharp.Point(upX + width, upY + height), new OpenCvSharp.Scalar(0, 255, 0), 2);
            OpenCvSharp.Cv2.PutText(image, label, new OpenCvSharp.Point(upX, upY - 5),
                OpenCvSharp.HersheyFonts.HersheyTriplex, 0.5, OpenCvSharp.Scalar.Red);
        }

    }
}