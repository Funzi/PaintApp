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

        public CanvasControllerImpl()
        {
            PenBrush = new SolidColorBrush(Colors.Black);
            RectangleImage = new BitmapImage(new Uri("Images/Rectangle.png", UriKind.Relative));
            EllipseImage = new BitmapImage(new Uri("Images/Ellipse.png", UriKind.Relative));
            LineImage = new BitmapImage(new Uri("Images/Line.png", UriKind.Relative));
            ThicknessImage1 = new BitmapImage(new Uri("Images/Thickness1.png", UriKind.Relative));
            ThicknessImage3 = new BitmapImage(new Uri("Images/Thickness3.png", UriKind.Relative));
            ThicknessImage5 = new BitmapImage(new Uri("Images/Thickness5.png", UriKind.Relative));
            ThicknessImage7 = new BitmapImage(new Uri("Images/Thickness7.png", UriKind.Relative));

            MShapeType = ShapeType.RECTANGLE;
            DrawingType1 = DrawingType.SHAPE;
            PenThickness = 1;

            cutRectangleBrush = new SolidColorBrush(Colors.Black);
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
                    MainCanvas.Children.Add(CurrentShape);
                    down = new Point(position.X, position.Y);
                    polygonPoints = new PointCollection();
                    polygonPoints.Add(down);
                    ((Polygon)CurrentShape).Points = polygonPoints;
                }
                else if (MShapeType == ShapeType.ELLIPSE || MShapeType == ShapeType.RECTANGLE)
                {
                    MainCanvas.Children.Add(CurrentShape);
                    Canvas.SetLeft(CurrentShape, position.X);
                    Canvas.SetTop(CurrentShape, position.Y);
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
        }

        private Shape getShape()
        {
            switch(MShapeType)
            {
                case ShapeType.ELLIPSE: return new Ellipse();
                case ShapeType.RECTANGLE: return new Rectangle();
                case ShapeType.LINE: return new Line();
                case ShapeType.POLYGON: return new Polygon();
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
                    polygonPoints.Add(position);
                    ((Polygon)CurrentShape).Points = polygonPoints;
                }
                else if (MShapeType == ShapeType.ELLIPSE || MShapeType == ShapeType.RECTANGLE)
                {
                    CurrentShape.Width = Math.Abs(position.X - down.X);
                    CurrentShape.Height = Math.Abs(position.Y - down.Y);
                    Canvas.SetLeft(CurrentShape, Math.Min(position.X, down.X));
                    Canvas.SetTop(CurrentShape, Math.Min(position.Y, down.Y));
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
            isDrawing = false;
        }

        private void ClipImage(Canvas canvas)
        {   
            // Create a BitmapImage
            WriteableBitmap bmpImage = SaveAsWriteableBitmap(canvas);
            // Clipped Image
            Image clippedImage = new Image();
            clippedImage.Source = bmpImage;
            EllipseGeometry clipGeometry = new EllipseGeometry(new Point(150, 160), CurrentShape.Width, CurrentShape.Height);
            clippedImage.Clip = clipGeometry;
            canvas.Children.Add(clippedImage);
        }

        public WriteableBitmap SaveAsWriteableBitmap(Canvas surface)
        {
            if (surface == null) return null;

            // Save current canvas transform
            Transform transform = surface.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            surface.LayoutTransform = null;

            // Get the size of canvas
            Size size = new Size(surface.ActualWidth, surface.ActualHeight);
            // Measure and arrange the surface
            // VERY IMPORTANT
            surface.Measure(size);
            surface.Arrange(new Rect(size));

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
              (int)size.Width,
              (int)size.Height,
              96d,
              96d,
              PixelFormats.Pbgra32);
            renderBitmap.Render(surface);


            //Restore previously saved layout
            surface.LayoutTransform = transform;

            //create and return a new WriteableBitmap using the RenderTargetBitmap
            return new WriteableBitmap(renderBitmap);

        }

        private void SetStroke(Shape shape)
        {
            shape.Stroke = PenBrush;
            shape.StrokeThickness = PenThickness;
        }
    }
}
