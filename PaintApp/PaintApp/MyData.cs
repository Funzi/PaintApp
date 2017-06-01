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
        public enum ShapeType { PENCIL, LINE, ELLIPSE, RECTANGLE, RUBBER };
        private Shape currentShape;
        private Brush penBrush, whiteBrush;
        private Point down, move;
        private ShapeType mShapeType;
        private int penThickness;
        private PathGeometry pathGeometry;
        private PathFigure pathFigure;
        private Path path;
        private Color temp = Colors.Green;
        private bool whiteBackground;
        private string currentFile;
        private string myDataProperty;
        private ImageSource rectangleImage, ellipseImage,lineImage;
        private ImageSource thicknessImage1, thicknessImage3, thicknessImage5, thicknessImage7;

        public ShapeType MShapeType { get => mShapeType; set => mShapeType = value; }
        public int PenThickness { get => penThickness; set => penThickness = value; }
        public Shape CurrentShape { get => currentShape; set => currentShape = value; }
        public Brush WhiteBrush { get => whiteBrush; set => whiteBrush = value; }
        public ImageSource EllipseImage { get => ellipseImage; set => ellipseImage = value; }
        public ImageSource LineImage { get => lineImage; set => lineImage = value; }
        public ImageSource RectangleImage { get => rectangleImage; set => rectangleImage = value; }
        public ImageSource ThicknessImage1 { get => thicknessImage1; set => thicknessImage1 = value; }
        public ImageSource ThicknessImage3 { get => thicknessImage3; set => thicknessImage3 = value; }
        public ImageSource ThicknessImage5 { get => thicknessImage5; set => thicknessImage5 = value; }
        public ImageSource ThicknessImage7 { get => thicknessImage7; set => thicknessImage7 = value; }

        public MyData()
        {
            penBrush = new SolidColorBrush(Colors.Black);
            rectangleImage = new BitmapImage(new Uri("Images/Rectangle.png", UriKind.Relative));
            ellipseImage = new BitmapImage(new Uri("Images/Ellipse.png", UriKind.Relative));
            lineImage = new BitmapImage(new Uri("Images/Line.png", UriKind.Relative));
            thicknessImage1 = new BitmapImage(new Uri("Images/Thickness1.png", UriKind.Relative));
            thicknessImage3 = new BitmapImage(new Uri("Images/Thickness3.png", UriKind.Relative));
            thicknessImage5 = new BitmapImage(new Uri("Images/Thickness5.png", UriKind.Relative));
            thicknessImage7 = new BitmapImage(new Uri("Images/Thickness7.png", UriKind.Relative));

            mShapeType = ShapeType.RECTANGLE;
            penThickness = 1;
            whiteBrush = new SolidColorBrush(Colors.White);
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
