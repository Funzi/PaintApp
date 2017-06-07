using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaintApp
{
    class CanvasControllerImpl : CanvasController, INotifyPropertyChanged
    {
        private PathGeometry pathGeometry;
        private PathFigure pathFigure;
        private PointCollection polygonPoints;
        private Path path;
        private Point down;
        private bool isDrawing;
        private Brush cutRectangleBrush;
        private PointCollection points = new PointCollection();

        public CanvasControllerImpl()
        {
            PenBrush = new SolidColorBrush(Colors.Black);
            RectangleImage = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Rectangle.png", UriKind.Absolute));
            EllipseImage = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Ellipse.png", UriKind.Absolute));
            LineImage = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Line.png", UriKind.Absolute));
            TriangleImage = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Triangle.png", UriKind.Absolute));
            StarImage = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Star.png", UriKind.Absolute));

            ThicknessImage1 = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Thickness1.png", UriKind.Absolute));
            ThicknessImage3 = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Thickness3.png", UriKind.Absolute));
            ThicknessImage5 = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Thickness5.png", UriKind.Absolute));
            ThicknessImage7 = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Thickness7.png", UriKind.Absolute));
            Heart = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Heart.png", UriKind.Absolute));
            Ball = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Ball.png", UriKind.Absolute));
            Slovakia = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Czech.png", UriKind.Absolute));
            Czech = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Images\\Slovakia.png", UriKind.Absolute));

            MShapeType = ShapeType.RECTANGLE;
            DrawingType1 = DrawingType.SHAPE;
            PenThickness = 1;

            cutRectangleBrush = new SolidColorBrush(Colors.Black);

            CustomBrush = new ImageBrush();
            CustomBrush.ImageSource = Heart;
        }
        
        public override void MouseDown(Canvas MainCanvas, Point position)
        {
            isDrawing = true;
            if(DrawingType1 == DrawingType.SHAPE)
            {
                CurrentShape = getShape();
                SetStroke(CurrentShape);
                down = new Point(position.X, position.Y);
                if (MShapeType == ShapeType.LINE)
                {
                    MainCanvas.Children.Add(CurrentShape);
                }
                else if (MShapeType == ShapeType.POLYGON)
                {
                    points = new PointCollection();
                    SetStroke(CurrentShape);
                    points.Add(down);
                    ((Polygon)CurrentShape).Points = points;
                    MainCanvas.Children.Add(CurrentShape);
                    down = new Point(position.X, position.Y);
                    
                }
                else if (MShapeType == ShapeType.ELLIPSE || MShapeType == ShapeType.RECTANGLE)
                {
                    MainCanvas.Children.Add(CurrentShape);
                    Canvas.SetLeft(CurrentShape, position.X);
                    Canvas.SetTop(CurrentShape, position.Y);
                }
                else if (MShapeType == ShapeType.STAR)
                {
                 
                    MainCanvas.Children.Add(CurrentShape);
                    down = new Point(position.X, position.Y);
                }
            }
            else if (DrawingType1 == DrawingType.PENCIL)
            {
                pathGeometry = new PathGeometry();
                pathFigure = new PathFigure();
                pathFigure.StartPoint = position;
                pathFigure.IsClosed = false;
                pathGeometry.Figures.Add(pathFigure);
                path = new Path();
                path.Stroke = PenBrush;
                path.StrokeThickness = PenThickness;
                path.Data = pathGeometry;
                MainCanvas.Children.Add(path);
            }
            
            else if (DrawingType1 == DrawingType.ERASER)
            {
                CanvasHelper.ErasePosition(MainCanvas,position);
            }
            else if (DrawingType1 == DrawingType.CUT)
            {
                CurrentShape = new Rectangle();
                SetStroke(CurrentShape);
                DoubleCollection collection = new DoubleCollection();
                collection.Add(4);
                collection.Add(4);
                CurrentShape.StrokeDashArray = collection;
                MainCanvas.Children.Add(CurrentShape);
                Canvas.SetLeft(CurrentShape, position.X);
                Canvas.SetTop(CurrentShape, position.Y);
                down = new Point(position.X, position.Y);
            }
            else if (DrawingType1 == DrawingType.BRUSH)
            {
                CanvasHelper.AddBrushImage(MainCanvas, CustomBrush, position);
            }
        }

        private Shape getShape()
        {
            switch(MShapeType)
            {
                case ShapeType.ELLIPSE: return new Ellipse();
                case ShapeType.RECTANGLE: return new Rectangle();
                case ShapeType.LINE: return new Line();
                case ShapeType.POLYGON: return new Polygon();
                case ShapeType.STAR: return new Polygon();
                default: return null;
            }
        }

        
        public override void MouseMove(Canvas MainCanvas, Point position)
        {
            if (!isDrawing)
            {
                return;
            }
            if (position.X < 0 || position.Y < 0)
            {
                return;
            }
            if(DrawingType1 == DrawingType.SHAPE)
            {
                if (MShapeType == ShapeType.LINE)
                {
                    ((Line)CurrentShape).X1 = down.X;
                    ((Line)CurrentShape).Y1 = down.Y;
                    ((Line)CurrentShape).X2 = position.X;
                    ((Line)CurrentShape).Y2 = position.Y;
                }
                else if (MShapeType == ShapeType.POLYGON)
                {
                   if(points.Count == 4)
                    {
                        points.RemoveAt(3);
                        points.RemoveAt(2);
                        points.RemoveAt(1);
                    }
                    points.Add(new Point(down.X, position.Y));
                    points.Add(new Point(position.X, down.Y));
                    points.Add(new Point(down.X, down.Y));
                }
                else if (MShapeType == ShapeType.ELLIPSE || MShapeType == ShapeType.RECTANGLE)
                {
                    
                    CurrentShape.Width = Math.Abs(position.X - down.X);
                    CurrentShape.Height = Math.Abs(position.Y - down.Y);
                    Canvas.SetLeft(CurrentShape, Math.Min(position.X, down.X));
                    Canvas.SetTop(CurrentShape, Math.Min(position.Y, down.Y));
                }
                else if (MShapeType == ShapeType.STAR)
                {
                    PointCollection tempPoints = new PointCollection();
                    SetStroke(CurrentShape);
                    double sizeX = Math.Abs(position.X-down.X)/3;
                    double sizeY = Math.Abs(position.Y - down.Y);
                    double xMin = Math.Min(position.X, down.X);
                    double yMin = Math.Min(position.Y, down.Y);

                    if ((yMin - sizeY) < 0) //OutOfBounds
                        return;
                    tempPoints.Add(new Point(xMin,yMin));
                    tempPoints.Add(new Point(xMin  + sizeX, yMin - sizeY / 3));
                    tempPoints.Add(new Point(xMin  + (float)3 / 2 * sizeX, yMin - sizeY));
                    tempPoints.Add(new Point(xMin  + 2 * sizeX, yMin - sizeY / 3));
                    tempPoints.Add(new Point(xMin  + 3 * sizeX, yMin));
                    tempPoints.Add(new Point(xMin  + 2 * sizeX, yMin + sizeY / 3));
                    tempPoints.Add(new Point(xMin  + (float)3 / 2 * sizeX, yMin + sizeY));
                    tempPoints.Add(new Point(xMin  + sizeX, yMin + sizeY / 3));
                    tempPoints.Add(new Point(xMin, yMin));
                    ((Polygon)CurrentShape).Points = tempPoints;
                }
                
            }
            if (DrawingType1 == DrawingType.PENCIL && pathFigure != null)
            {
                LineSegment ls = new LineSegment();
                ls.Point = position;
                pathFigure.Segments.Add(ls);
            }
            
            else if (DrawingType1 == DrawingType.ERASER)
            {
                CanvasHelper.ErasePosition(MainCanvas, position);
            }
            else if (DrawingType1 == DrawingType.CUT)
            {
                CurrentShape.Width = Math.Abs(position.X - down.X);
                CurrentShape.Height = Math.Abs(position.Y - down.Y);
                Canvas.SetLeft(CurrentShape, Math.Min(position.X, down.X));
                Canvas.SetTop(CurrentShape, Math.Min(position.Y, down.Y));
            }
            else if(DrawingType1 == DrawingType.BRUSH)
            {
                CanvasHelper.AddBrushImage(MainCanvas, CustomBrush, position);
            }
        }

        public override void MouseUp(Canvas mainCanvas)
        {   
            if(DrawingType1==DrawingType.CUT && CurrentShape != null)
            {
                ClipImage(mainCanvas);
            }
            if (CurrentShape != null)
            {
                CurrentShape = null;
            }
            if (pathFigure != null)
            {
                pathFigure = null;
                path = null;
                pathGeometry = null;
            }
            if(points !=null)
            {
                points = null;
            }
            isDrawing = false;
        }

        private void ClipImage(Canvas canvas)
        { 
            WriteableBitmap bmpImage = SaveAsWriteableBitmap(canvas);
            Image clippedImage = new Image();
            clippedImage.Source = bmpImage;
            EllipseGeometry clipGeometry = new EllipseGeometry(new Point(150, 160), CurrentShape.Width, CurrentShape.Height);
            clippedImage.Clip = clipGeometry;
            Canvas.SetLeft(clippedImage,0);
            Canvas.SetTop(clippedImage,0);
            CanvasHelper.SetWhitebackGround(canvas);
            Canvas tempCanvas = new Canvas();
            tempCanvas.Children.Add(clippedImage);
            tempCanvas.Clip = new RectangleGeometry(new Rect(Canvas.GetLeft(CurrentShape), Canvas.GetTop(CurrentShape), CurrentShape.Width, CurrentShape.Height));
            canvas.Children.Remove(CurrentShape);
            canvas.Clip = new RectangleGeometry(new Rect(Canvas.GetLeft(CurrentShape),Canvas.GetTop(CurrentShape),CurrentShape.Width,CurrentShape.Height));
            Canvas.SetZIndex(tempCanvas, 0);
            canvas.Children.Add(tempCanvas);
        }

        public WriteableBitmap SaveAsWriteableBitmap(Canvas surface)
        {   
            
            if (surface == null) return null;

            Transform transform = surface.LayoutTransform;
            surface.LayoutTransform = null;
            Size size = new Size(CurrentShape.ActualWidth, CurrentShape.ActualHeight);
            surface.Measure(size);
            surface.Arrange(new Rect(size));
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
            renderBitmap.Render(surface);
            surface.LayoutTransform = transform;

            return new WriteableBitmap(renderBitmap);

        }

        private void SetStroke(Shape shape)
        {
            shape.Stroke = PenBrush;
            shape.StrokeThickness = PenThickness;
        }
    }
}
