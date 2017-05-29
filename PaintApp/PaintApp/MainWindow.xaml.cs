using System;
using System.Collections.Generic;
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

namespace PaintApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum ShapeType { PENCIL, LINE, ELLIPSE, RECTANGLE };
        private Shape currentShape;
        private Brush penColor;
        private Point down, move;
        private ShapeType shapeType;
        private int penThickness;
        private PathGeometry pathGeometry;
        private PathFigure pathFigure;
        private Path path;

        public MainWindow()
        {
            InitializeComponent();
            setDefaultValues();
        }

        private void setDefaultValues()
        {
            shapeType = ShapeType.PENCIL;
            penThickness = 1;
            penColor = new SolidColorBrush(Colors.Black);
        }

        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            drawObject(new Ellipse());
        }

        private void drawObject(Shape shape)
        {
            //current.Stroke = color;
            Rectangle rect = new Rectangle();
            rect.Width = 100;
            rect.Height = 100;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Red;
            rect.Stroke = brush; 
            MainCanvas.Children.Add(rect);
            Canvas.SetTop(rect, 100);
            Canvas.SetLeft(rect, 200);
        }

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentShape = getCurrentShape();
            Point position = e.GetPosition(MainCanvas);

            if (shapeType == ShapeType.PENCIL)
            {   
                pathGeometry = new PathGeometry();
                pathFigure = new PathFigure();
                pathFigure.StartPoint = position;
                pathFigure.IsClosed = false;
                pathGeometry.Figures.Add(pathFigure);
                path = new Path();
                path.Stroke = penColor;
                path.StrokeThickness = penThickness;
                path.Data = pathGeometry;
                MainCanvas.Children.Add(path);
            }
            else if (shapeType == ShapeType.LINE)
            {
                MainCanvas.Children.Add(currentShape);
               
                //Canvas.SetLeft(currentShape, position.X);
                //Canvas.SetTop(currentShape, position.Y);
                down = new Point(position.X, position.Y);
            }
            else
            { 
                MainCanvas.Children.Add(currentShape);
                Canvas.SetLeft(currentShape, position.X);
                Canvas.SetTop(currentShape, position.Y);
                down = new Point(position.X, position.Y);
            }
        }

       

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (currentShape != null)
            {
                currentShape = null;
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
            if (currentShape == null && pathFigure == null)
            {
               return;
            }
            
            Point pos = e.GetPosition(MainCanvas);
            if(shapeType == ShapeType.PENCIL && pathFigure != null)
            {
                LineSegment ls = new LineSegment();
                ls.Point = pos;
                pathFigure.Segments.Add(ls);
            }
            else if (shapeType==ShapeType.LINE)
            {
                ((Line)currentShape).X1 = down.X;
                ((Line)currentShape).Y1 = down.Y;
                ((Line)currentShape).X2 = pos.X;
                ((Line)currentShape).Y2 = pos.Y;
            }
            else
            {
                double x = Canvas.GetLeft(currentShape);
                double y = Canvas.GetTop(currentShape);
                currentShape.Width = Math.Abs(pos.X - x);
                currentShape.Height = Math.Abs(pos.Y - y);
            }
        }

        private Shape getCurrentShape()
        {
            Shape newShape;
            switch (shapeType)
            {
                case ShapeType.RECTANGLE: newShape = new Rectangle(); break;
                case ShapeType.ELLIPSE: newShape = new Ellipse(); break;
                case ShapeType.LINE: newShape = new Line(); break;
                default: return null;
            }
            Brush brush = penColor;
            newShape.Stroke = brush;
            newShape.StrokeThickness = penThickness;
            return newShape;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (shapeComboCox.SelectedIndex == 0)
            {
                shapeType = ShapeType.RECTANGLE;
            }
            else if (shapeComboCox.SelectedIndex == 1)
            {
                shapeType = ShapeType.ELLIPSE;
            }
            else if (shapeComboCox.SelectedIndex == 2)
            {
                shapeType = ShapeType.LINE;
            }
            else if (shapeComboCox.SelectedIndex == 3)
            {
                shapeType = ShapeType.PENCIL;
            }
        }

        private void brushComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (brushComboBox.SelectedIndex == 0)
            {
                penThickness = 1;
            }
            else if (brushComboBox.SelectedIndex == 1)
            {
                penThickness = 3;
            }
            else if (brushComboBox.SelectedIndex == 2)
            {
                penThickness = 5;
            }
            else if (brushComboBox.SelectedIndex == 3)
            {
                penThickness = 7;
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
                penColor = colorPicker.selectedColor;
            }
        }

    }
}
