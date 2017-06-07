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
using static PaintApp.CanvasController;
using static PaintApp.CanvasControllerImpl;

namespace PaintApp
{
    public partial class MainWindow : Window
    {
        private bool whiteBackground;
        private string currentFile;
        CanvasControllerImpl controller;

        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
            controller = new CanvasControllerImpl();
            SetDefaultValues();
            this.colorButtton.DataContext = controller;
            this.statusBar.DataContext = controller;
        }

        private void SetDefaultValues()
        {
            whiteBackground = false;
            shapeImage.Source = controller.RectangleImage;
            brushImage.Source = controller.ThicknessImage1;
        }
        static int count = 0;
        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            count++;
            if(!whiteBackground)
            {
                CanvasHelper.SetWhitebackGround(MainCanvas);
                whiteBackground = true;
            }
            controller.MouseDown(MainCanvas, e.GetPosition(MainCanvas));
        }

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            controller.MouseUp(MainCanvas);
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(MainCanvas);
            controller.MousePositionText = String.Format("X:{0} Y:{1}", position.X, position.Y);
            controller.MouseMove(MainCanvas, position);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            controller.DrawingType1 = DrawingType.SHAPE;
            if (shapeComboCox.SelectedIndex == 0)
            {
                controller.MShapeType = ShapeType.RECTANGLE;
                shapeImage.Source = controller.RectangleImage;
            }
            else if (shapeComboCox.SelectedIndex == 1)
            {
                controller.MShapeType = ShapeType.ELLIPSE;
                shapeImage.Source = controller.EllipseImage;

            }
            else if (shapeComboCox.SelectedIndex == 2)
            {
                controller.MShapeType = ShapeType.LINE;
                shapeImage.Source = controller.LineImage;

            }
            else if (shapeComboCox.SelectedIndex == 3)
            {
                controller.MShapeType = ShapeType.POLYGON;
                shapeImage.Source = controller.TriangleImage;
            }
            else if (shapeComboCox.SelectedIndex == 4)
            {
                controller.MShapeType = ShapeType.STAR;
                shapeImage.Source = controller.StarImage;
            }
        }

        private void brushComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (brushComboBox.SelectedIndex == 0)
            {
                controller.PenThickness = 1;
                brushImage.Source = controller.ThicknessImage1;
            }
            else if (brushComboBox.SelectedIndex == 1)
            {
                controller.PenThickness = 3;
                brushImage.Source = controller.ThicknessImage3;
            }
            else if (brushComboBox.SelectedIndex == 2)
            {
                controller.PenThickness = 5;
                brushImage.Source = controller.ThicknessImage5;
            }
            else if (brushComboBox.SelectedIndex == 3)
            {
                controller.PenThickness = 7;
                brushImage.Source = controller.ThicknessImage7;
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
                controller.PenBrush = colorPicker.selectedColor;
            }
        }

        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
            CanvasHelper.SetWhitebackGround(MainCanvas);
            whiteBackground = true;
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            if(FileHelper.OpenFile(MainCanvas)) 
                MainCanvas.Children.Clear();
        }
        
        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {   
            if(currentFile!=null)
            {
                FileHelper.SaveFile(MainCanvas, currentFile);
            }
        }

        private void MenuItem_Save_as_Click(object sender, RoutedEventArgs e)
        {
            currentFile = FileHelper.SaveFileAs(MainCanvas);
            if(currentFile!=null)
            {
                saveItem.IsEnabled = true;
            }
        }

        private void rubberButton_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = new Cursor(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Cursors\\Rubber.cur");
            controller.DrawingType1 = DrawingType.ERASER;
        }

        private void penButton_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Pen;
            controller.DrawingType1 = DrawingType.PENCIL;
        }

        private void cutButton_Click(object sender, RoutedEventArgs e)
        {
            controller.DrawingType1 = DrawingType.CUT;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
            CanvasHelper.SetWhitebackGround(MainCanvas);
            whiteBackground = true;
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Size size = e.NewSize;
            controller.CanvasSizeText = String.Format("Width:{0} Height:{1}", size.Width,size.Height-100);
        }

        private void brushButton_Click(object sender, RoutedEventArgs e)
        {
            controller.DrawingType1 = DrawingType.BRUSH;
        }

        private void customBrushButton_Click(object sender, RoutedEventArgs e)
        {
            customBrushComboBox.IsDropDownOpen = true;
        }

        private void customBrushComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            controller.DrawingType1 = DrawingType.BRUSH;
            this.Cursor = Cursors.Hand;
            if (customBrushComboBox.SelectedIndex == 0)
            {
                controller.CustomBrush.ImageSource = controller.Heart;
            }
            else if (customBrushComboBox.SelectedIndex == 1)
            {
                controller.CustomBrush.ImageSource = controller.Ball;
            }
            else if (customBrushComboBox.SelectedIndex == 2)
            {
                controller.CustomBrush.ImageSource = controller.Czech;
            }
            else if (customBrushComboBox.SelectedIndex == 3)
            {
                controller.CustomBrush.ImageSource = controller.Slovakia;
            }
            customBrushImage.Source = controller.CustomBrush.ImageSource;
        }
    }
}
