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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Brush whiteBrush;
        private Point down, move;
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
            if(!whiteBackground)
            {
                setWhitebackGround();
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
            else
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

        private void setWhitebackGround()
        {
            data.CurrentShape = new Rectangle();
            Canvas.SetLeft(data.CurrentShape, 0);
            Canvas.SetTop(data.CurrentShape, 0);
            data.CurrentShape.Stroke = whiteBrush;
            data.CurrentShape.Fill = whiteBrush;
            Rect bounds = VisualTreeHelper.GetDescendantBounds(MainCanvas);
            data.CurrentShape.Height = MainCanvas.ActualHeight;
            data.CurrentShape.Width = MainCanvas.ActualWidth;
            MainCanvas.Children.Add(data.CurrentShape);
            whiteBackground = true;
        }

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
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
            } else
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
            setWhitebackGround();
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Title = "Choose an image file";
            dlg.Filter = "Bitmap files (*.bmp)|*.bmp|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png|All files (*.*)|*.*";
            try
            {
                if (dlg.ShowDialog() == true)
                {
                    ImageBrush brush = new ImageBrush();
                    BitmapImage img = new BitmapImage(new Uri(dlg.FileName, UriKind.Relative));
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(img));
                    string tempPath = CreateTempFile();
                    using (var stream = System.IO.File.Open(tempPath, System.IO.FileMode.Open))
                    {
                        encoder.Save(stream);
                        stream.Close();
                    }
                    BitmapImage temp = new BitmapImage(new Uri(tempPath, UriKind.Relative));
                    brush.ImageSource = temp;
                    MainCanvas.Background = brush;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
        }

        private string CreateTempFile()
        {
            string fileName = string.Empty;

            try
            {
                fileName = System.IO.Path.GetTempFileName();

                // Create a FileInfo object to set the file's attributes
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);

                // Set the Attribute property of this file to Temporary. 
                fileInfo.Attributes = System.IO.FileAttributes.Temporary;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to create file." + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return fileName;
        }
        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            BitmapEncoder pngEncoder = GetBitmapEncoder();

            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                pngEncoder.Save(ms);
                ms.Close();
                System.IO.File.WriteAllBytes(currentFile, ms.ToArray());
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        

        private void MenuItem_Save_as_Click(object sender, RoutedEventArgs e)
        {
            BitmapEncoder pngEncoder = GetBitmapEncoder();

            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                pngEncoder.Save(ms);
                ms.Close();
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Title = "Save as";
                saveDialog.Filter = "Bitmap files (*.bmp)|*.bmp|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png|All files (*.*)|*.*";
                if (saveDialog.ShowDialog() == true)
                {
                    currentFile = saveDialog.FileName;
                    System.IO.File.WriteAllBytes(currentFile, ms.ToArray());
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private BitmapEncoder GetBitmapEncoder()
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(MainCanvas);
            double dpi = 96d;


            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);


            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(MainCanvas);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
            return pngEncoder;
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

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
