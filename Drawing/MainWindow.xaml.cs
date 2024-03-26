using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Drawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Entries { get; set; } = new ObservableCollection<string>() { "abc"};
        Model Model { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Model = new Model();
            this.DataContext = Model;
        }

        //public event PropertyChangedEventHandler? PropertyChanged;
        //private PointCollection _sendPointsList;

        //public PointCollection PolygonPoints
        //{
        //    get
        //    {
        //        if (_sendPointsList == null) _sendPointsList = new PointCollection();
        //        return _sendPointsList;
        //    }
        //    set
        //    {
        //        this._sendPointsList = value;
        //        OnPropertyChanged("PolygonPoints");
        //    }

        //}
        //private void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Point point = e.GetPosition(DrawingCanvas);
            Model.Add(point);
            //PolygonPoints.Add(point);
            //PolygonPoints = new PointCollection(Model.PolygonPoints);
            Entries.Add("abc");
        }
        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
        private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

    }
}