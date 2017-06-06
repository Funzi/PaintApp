using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaintApp
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        public Brush selectedColor { get; set; }

        public ColorPicker()
        {
            InitializeComponent();
            //this.colorList.ItemsSource = typeof(Brushes).GetProperties();
            var properties = typeof(Colors).GetProperties();
            var colors = new ArrayList();
            foreach (PropertyInfo prop in properties)
            {
                // get the value of this static property
                var color = (Color)prop.GetValue(null, null);
                // skip colors that are not interesting
                if (color == Colors.Transparent) continue;
                // create a solid brush of this color
                Brush brush = new SolidColorBrush(color);
                colors.Add(brush);
            }
            this.colorList.ItemsSource = colors;
        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.colorList.ItemsSource = typeof(Brushes).GetProperties();
        }

        private void colorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedColor = (e.AddedItems[0] as Brush);
            rtlfill.Fill = selectedColor;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
