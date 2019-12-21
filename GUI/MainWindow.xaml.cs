using System;
using System.Collections.Generic;
using System.IO;
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
using Models.World_Gen;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Image Image;

        public MainWindow()
        {
            Set();
            InitializeComponent();
            GameBoard.Children.Add(Image);
        }

        private WriteableBitmap CreateImage(double[,] imageData, int Height, int Width)
        {
            WriteableBitmap image = new WriteableBitmap(Width, Height, 1, 1, PixelFormats.Gray32Float, null);
            image.WritePixels(new Int32Rect(0,0, Width, Height), imageData, (Width * image.Format.BitsPerPixel + 7) / 8, 0);
            return image;
        }

        public void Set(double scope = 10)
        {
            int mapWidth = 700;
            int mapDepth = 700;

            MapGen map = new MapGen();
            double[,] world = map.GenerateNoiseMap(mapDepth, mapWidth, scope);
            Image image = new Image();
            image.Source = CreateImage(world, mapDepth, mapWidth);
            image.Width = 700;
            image.Height = 700;
            Image = image;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Set(Slider.Value);
            GameBoard.Children.Remove(Image);
            GameBoard.Children.Add(Image);
        }
    }
}
