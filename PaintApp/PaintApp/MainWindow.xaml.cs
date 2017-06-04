using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static PaintApp.MyData;

namespace PaintApp
{
    public partial class MainWindow : Window
    {
        private Point down;
        private PathGeometry pathGeometry;
        private PathFigure pathFigure;
        private Path path;
        private bool whiteBackground;
        private string currentFile;
        MyData data;

        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
            data = new MyData();
            setDefaultValues();
            this.colorButtton.DataContext = data;
        }

        private void setDefaultValues()
        {
            whiteBackground = false;
            shapeImage.Source = data.RectangleImage;
            brushImage.Source = data.ThicknessImage1;
        }


        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainCanvas.CaptureMouse();
            if(!whiteBackground)
            {
                CanvasHelper.setWhitebackGround(MainCanvas);
                whiteBackground = true;
            }
            
            data.CurrentShape = getCurrentShape();
            Point position = e.GetPosition(MainCanvas);

            if (data.MShapeType == ShapeType.PENCIL)
            {   
                pathGeometry = new PathGeometry();
                pathFigure = new PathFigure();
                pathFigure.StartPoint = position;
                pathFigure.IsClosed = false;
                pathGeometry.Figures.Add(pathFigure);
                path = new Path();
                path.Stroke = data.PenBrush;
                path.StrokeThickness = data.PenThickness;
                path.Data = pathGeometry;
                MainCanvas.Children.Add(path);
            }
            else if (data.MShapeType == ShapeType.LINE)
            {
                MainCanvas.Children.Add(data.CurrentShape);
                down = new Point(position.X, position.Y);
            }
            else if(data.MShapeType == ShapeType.ELLIPSE || data.MShapeType == ShapeType.RECTANGLE)
            { 
                MainCanvas.Children.Add(data.CurrentShape);
                Canvas.SetLeft(data.CurrentShape, position.X);
                Canvas.SetTop(data.CurrentShape, position.Y);
                down = new Point(position.X, position.Y);
            }
            else if(data.MShapeType == ShapeType.RUBBER)
            {
                erasePosition(position);
            }
        }

        private void erasePosition(Point position)
        {
            data.CurrentShape = new Rectangle();
            data.CurrentShape.Stroke = data.WhiteBrush;
            data.CurrentShape.Fill = data.WhiteBrush;
            data.CurrentShape.Width = 32;
            data.CurrentShape.Height = 32;
            Canvas.SetLeft(data.CurrentShape, position.X);
            Canvas.SetTop(data.CurrentShape, position.Y);
            MainCanvas.Children.Add(data.CurrentShape);
        }

        

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainCanvas.ReleaseMouseCapture();
            if (data.CurrentShape != null)
            {
                data.CurrentShape = null;
            }
            if(pathFigure != null)
            {
                pathFigure = null;
                path = null;
                pathGeometry = null;
            }
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (data.CurrentShape == null && pathFigure == null)
            {
               return;
            }
            
            Point pos = e.GetPosition(MainCanvas);
            if(pos.X < 0 || pos.Y < 0)
            {
                return;
            }
            if(data.MShapeType == ShapeType.PENCIL && pathFigure != null)
            {
                LineSegment ls = new LineSegment();
                ls.Point = pos;
                pathFigure.Segments.Add(ls);
            }
            else if (data.MShapeType == ShapeType.LINE)
            {
                ((Line)data.CurrentShape).X1 = down.X;
                ((Line)data.CurrentShape).Y1 = down.Y;
                ((Line)data.CurrentShape).X2 = pos.X;
                ((Line)data.CurrentShape).Y2 = pos.Y;
            }
            else if (data.MShapeType == ShapeType.ELLIPSE || data.MShapeType == ShapeType.RECTANGLE)
            {   
                double x = Canvas.GetLeft(data.CurrentShape);
                double y = Canvas.GetTop(data.CurrentShape);
                data.CurrentShape.Width = Math.Abs(pos.X - x);
                data.CurrentShape.Height = Math.Abs(pos.Y - y);
            } else if(data.MShapeType==ShapeType.RUBBER)
            {
                erasePosition(pos);
            }
        }

        private Shape getCurrentShape()
        {
            Shape newShape;
            switch (data.MShapeType)
            {
                case ShapeType.RECTANGLE: newShape = new Rectangle(); break;
                case ShapeType.ELLIPSE: newShape = new Ellipse(); break;
                case ShapeType.LINE: newShape = new Line(); break;
                default: return null;
            }
            Brush brush = data.PenBrush;
            newShape.Stroke = brush;
            newShape.StrokeThickness = data.PenThickness;
            return newShape;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            if (shapeComboCox.SelectedIndex == 0)
            {
                data.MShapeType = ShapeType.RECTANGLE;
                shapeImage.Source = data.RectangleImage;
            }
            else if (shapeComboCox.SelectedIndex == 1)
            {
                data.MShapeType = ShapeType.ELLIPSE;
                shapeImage.Source = data.EllipseImage;

            }
            else if (shapeComboCox.SelectedIndex == 2)
            {
                data.MShapeType = ShapeType.LINE;
                shapeImage.Source = data.LineImage;

            }
        }

        private void brushComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (brushComboBox.SelectedIndex == 0)
            {
                data.PenThickness = 1;
                brushImage.Source = data.ThicknessImage1;
            }
            else if (brushComboBox.SelectedIndex == 1)
            {
                data.PenThickness = 3;
                brushImage.Source = data.ThicknessImage3;
            }
            else if (brushComboBox.SelectedIndex == 2)
            {
                data.PenThickness = 5;
                brushImage.Source = data.ThicknessImage5;
            }
            else if (brushComboBox.SelectedIndex == 3)
            {
                data.PenThickness = 7;
                brushImage.Source = data.ThicknessImage7;
            }
        }

        private void ShapeButtton_Click(object sender, RoutedEventArgs e)
        {
            shapeComboCox.IsDropDownOpen = true;
        }

        private void brushButtton_Click(object sender, RoutedEventArgs e)
        {
            brushComboBox.IsDropDownOpen = true;
        }

        private void ColorButtton_Click(object sender, RoutedEventArgs e)
        {
            ColorPicker colorPicker = new ColorPicker();
            colorPicker.ShowDialog();
            if(colorPicker.DialogResult == true)
            {
                data.PenBrush = colorPicker.selectedColor;
            }
        }

        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
            CanvasHelper.setWhitebackGround(MainCanvas);
            whiteBackground = true;
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            if(FileHelper.openFile(MainCanvas)) 
                MainCanvas.Children.Clear();
        }
        
        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {   
            if(currentFile!=null)
            {
                FileHelper.saveFile(MainCanvas, currentFile);
            }
        }

        private void MenuItem_Save_as_Click(object sender, RoutedEventArgs e)
        {
            currentFile = FileHelper.saveFileAs(MainCanvas);
            if(currentFile!=null)
            {
                saveItem.IsEnabled = true;
            }
        }

        private void rubberButton_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = new Cursor(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Cursors\\Rubber.cur");
            data.MShapeType = ShapeType.RUBBER;
        }

        private void penButton_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Pen;
            data.MShapeType = ShapeType.PENCIL;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
            CanvasHelper.setWhitebackGround(MainCanvas);
            whiteBackground = true;
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
