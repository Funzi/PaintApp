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

namespace PaintApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum ShapeType { PENCIL, LINE, ELLIPSE, RECTANGLE, RUBBER };
        private Shape currentShape;
        private Brush whiteBrush;
        private Point down, move;
        private ShapeType shapeType;
        private int penThickness;
        private PathGeometry pathGeometry;
        private PathFigure pathFigure;
        private Path path;
        private Color temp = Colors.Green;
        private bool whiteBackground;
        private string currentFile;
        MyData data;



        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
            setDefaultValues();
            data = new MyData();
            this.colorButtton.DataContext = data;
        }

        private void setDefaultValues()
        {
            shapeType = ShapeType.PENCIL;
            penThickness = 1;
            whiteBrush = new SolidColorBrush(Colors.White);
            whiteBackground = false;
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
            data.PenBrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Red;
            rect.Stroke = brush;
            MainCanvas.Children.Add(rect);
            Canvas.SetTop(rect, 100);
            Canvas.SetLeft(rect, 200);
        }

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {   
            if(!whiteBackground)
            {
                setWhitebackGround();
            }
            
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
                path.Stroke = data.PenBrush;
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
            else if(shapeType == ShapeType.ELLIPSE || shapeType == ShapeType.RECTANGLE)
            { 
                MainCanvas.Children.Add(currentShape);
                Canvas.SetLeft(currentShape, position.X);
                Canvas.SetTop(currentShape, position.Y);
                down = new Point(position.X, position.Y);
            }
            else
            {
                erasePosition(position);

            }
        }

        private void erasePosition(Point position)
        {
            currentShape = new Rectangle();
            currentShape.Stroke = whiteBrush;
            currentShape.Fill = whiteBrush;
            currentShape.Width = 32;
            currentShape.Height = 32;
            Canvas.SetLeft(currentShape, position.X);
            Canvas.SetTop(currentShape, position.Y);
            MainCanvas.Children.Add(currentShape);
        }

        private void setWhitebackGround()
        {
            currentShape = new Rectangle();
            Canvas.SetLeft(currentShape, 0);
            Canvas.SetTop(currentShape, 0);
            currentShape.Stroke = whiteBrush;
            currentShape.Fill = whiteBrush;
            Rect bounds = VisualTreeHelper.GetDescendantBounds(MainCanvas);
            currentShape.Height = MainCanvas.ActualHeight;
            currentShape.Width = MainCanvas.ActualWidth;
            MainCanvas.Children.Add(currentShape);
            whiteBackground = true;
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
            else if (shapeType == ShapeType.ELLIPSE || shapeType == ShapeType.RECTANGLE)
            {
                double x = Canvas.GetLeft(currentShape);
                double y = Canvas.GetTop(currentShape);
                currentShape.Width = Math.Abs(pos.X - x);
                currentShape.Height = Math.Abs(pos.Y - y);
            } else
            {
                erasePosition(pos);
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
            Brush brush = data.PenBrush;
            newShape.Stroke = brush;
            newShape.StrokeThickness = penThickness;
            return newShape;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
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
            shapeType = ShapeType.RUBBER;
        }

        private void penButton_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Pen;
            shapeType = ShapeType.PENCIL;
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
