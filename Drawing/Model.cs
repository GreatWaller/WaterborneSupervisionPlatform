using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Drawing
{
    public class Model: BaseNotifyModel
    {
        public ObservableCollection<Point> PolygonPoints { get; set; }= new ObservableCollection<Point>();

        public Model()
        {
            //PolygonPoints = new ObservableCollection<Point>() { new Point(10, 110), new Point(60, 10), new Point(110, 110), new Point(300,200) };

        }

        public void Add(Point point)
        {
            PolygonPoints.Add(point);
            OnPropertyChanged(nameof(PolygonPoints));
        }
    }
}
