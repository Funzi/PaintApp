using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaintApp
{
    public static class CanvasHelper
    {
        public static void SetWhitebackGround(Canvas canvas)
        {
            Rectangle background = new Rectangle();
            SolidColorBrush whiteBrush = new SolidColorBrush(Colors.White);
            Canvas.SetLeft(background, 0);
            Canvas.SetTop(background, 0);
            background.Stroke = whiteBrush;
            background.Fill = whiteBrush;
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            background.Height = canvas.ActualHeight;
            background.Width = canvas.ActualWidth;
            canvas.Children.Add(background);
        }

        public static void ErasePosition(Canvas canvas, Point position)
        {
            Shape shape = new Rectangle();
            shape.Stroke = new SolidColorBrush(Colors.White);
            shape.Fill = new SolidColorBrush(Colors.White);
            shape.Width = 32;
            shape.Height = 32;
            Canvas.SetLeft(shape, position.X);
            Canvas.SetTop(shape, position.Y);
            canvas.Children.Add(shape);
        }

        internal static void AddBrushImage(Canvas canvas, ImageBrush brush, Point position)
        {
            Shape shape = new Rectangle();
            ImageBrush newBrush = new ImageBrush();
            newBrush.ImageSource = brush.ImageSource;
            shape.Fill = newBrush;
            shape.Width = 32;
            shape.Height = 32;
            Canvas.SetLeft(shape, position.X);
            Canvas.SetTop(shape, position.Y);
            canvas.Children.Add(shape);
        }
    }
}
