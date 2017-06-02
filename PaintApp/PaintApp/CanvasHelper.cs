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
        public static void setWhitebackGround(Canvas canvas)
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
    }
}
