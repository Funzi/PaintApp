using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintApp
{
    
    abstract class CanvasController : INotifyPropertyChanged
    {
        public enum ShapeType { LINE, ELLIPSE, RECTANGLE, POLYGON };
        public enum DrawingType { SHAPE, ERASER, CUT, PENCIL, BRUSH }
        private Shape currentShape;
        private Brush penBrush, whiteBrush;
        private ShapeType mShapeType;
        private DrawingType drawingType;
        private string mousePositionText;
        private string canvasSizeText;
        private int penThickness;
        private string myDataProperty;
        private ImageSource rectangleImage, ellipseImage, lineImage;
        private ImageSource thicknessImage1, thicknessImage3, thicknessImage5, thicknessImage7;
        private ImageSource heart,ball,slovakia,czech;
        private ImageBrush customBrush;
       

        public ShapeType MShapeType { get => mShapeType; set => mShapeType = value; }
        public DrawingType DrawingType1 { get => drawingType; set => drawingType = value; }

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
        public ImageSource Heart { get => heart; set => heart = value; }
        public ImageSource Ball { get => ball; set => ball = value; }
        public ImageSource Slovakia { get => slovakia; set => slovakia = value; }
        public ImageSource Czech { get => czech; set => czech = value; }

        public ImageBrush CustomBrush { get => customBrush; set => customBrush = value; }

        public abstract void MouseDown(Canvas mainCanvas, Point position);
        public abstract void MouseMove(Canvas mainCanvas, Point position);
        public abstract void MouseUp(Canvas mainCanvas);

        public String MyDataProperty
        {
            get { return MyDataProperty; }
            set
            {
                MyDataProperty = value;
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

        public string MousePositionText
        {
            get { return mousePositionText; }
            set
            {
                mousePositionText = value;
                OnPropertyChanged("MousePositionText");
            }
        }

        public string CanvasSizeText
        {
            get { return canvasSizeText; }
            set
            {
                canvasSizeText = value;
                OnPropertyChanged("CanvasSizeText");
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
