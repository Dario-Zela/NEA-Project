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

        private WriteableBitmap CreateImage(byte[] imageData, int Height, int Width)
        {
            WriteableBitmap image = new WriteableBitmap(700, 700, 1, 1, PixelFormats.Indexed1, BitmapPalettes.Halftone64);
            image.WritePixels(new Int32Rect(0, 0, 700 , 700 ), imageData, 700*1, 0);
            return image;
        }

        public void Set(double seed = 10)
        {
            int mapWidth = 700;
            int mapDepth = 700;

            MapGen map = new MapGen();
            double[,] world = map.GenerateNoiseMap(mapDepth, mapWidth, 10 , (int)seed);
            Image image = new Image();
            byte[] world2 = new byte[mapDepth * mapWidth*4];
            int counter = -1;
            for (int i = 0; i < mapWidth; i++)
            {

                for (int j = 0; j < mapDepth; j++)
                {
                    counter++;
                    world2[counter] = world[j, i] < 0.5 ? (byte)0 : (byte)1;
                }
            }
            image.Source = CreateImage(world2, mapDepth, mapWidth);
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
