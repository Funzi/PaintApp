using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaintApp
{
    class MyData : INotifyPropertyChanged
    {
        private enum ShapeType { PENCIL, LINE, ELLIPSE, RECTANGLE };
        private Shape currentShape;
        private Brush penBrush, whiteBrush;
        private Point down, move;
        private ShapeType shapeType;
        private int penThickness;
        private PathGeometry pathGeometry;
        private PathFigure pathFigure;
        private Path path;
        private Color temp = Colors.Green;
        private bool whiteBackground;
        private string currentFile;
        private string myDataProperty;
        private BitmapImage shapeImage;

        public MyData()
        {
            penBrush = new SolidColorBrush(Colors.Black);
            myDataProperty = "Images/Rectangle.png";
            shapeImage = new BitmapImage(new Uri("Images/Rectangle.png", UriKind.Relative));
        }

        public String MyDataProperty
        {
            get { return myDataProperty; }
            set
            {
                myDataProperty = value;
                OnPropertyChanged("MyDataProperty");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        

        public Brush PenBrush
        {
            get { return penBrush; }
            set
            {
                penBrush = value;
                OnPropertyChanged("Penbrush");
            }
        }

        public BitmapImage ShapeImage
        {
            get { return shapeImage; }
            set
            {
                shapeImage = value;
                OnPropertyChanged("ShapeImage");
            }
        }


        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

    }
}
