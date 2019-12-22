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
    public partial class MainWindow : Window
    {
        Image Image;

        public MainWindow()
        {
            Set();
            InitializeComponent();
            GameBoard.Children.Add(Image);
        }

        private BitmapSource CreateImage(float[] imageData, int Height, int Width)
        {
            WriteableBitmap image = new WriteableBitmap(Width, Height, 1, 1, PixelFormats.Cmyk32, null);
            image.WritePixels(new Int32Rect(0, 0, Width, Height), imageData, Width * 4, 0);
            return image;
        }

        public void Set()
        {
            int mapWidth = 700;
            int mapDepth = 700;

            MapGen map = new MapGen();
            float[,] world = map.GenerateNoiseMap(mapWidth, mapDepth, 1f, 1219, 6, 0.5f, 2);
            float[] world2 = new float[mapDepth*mapWidth*4];
            int counter = 0;
            for (int j = 0; j < mapWidth; j++)
            {
                for(int i =0; i < mapDepth; i++)
                {
                    world2[counter]  = world[i, j] < 0.1 ? 100f : world[i, j] < 0.3 ? 0f : world[i, j] < 0.6 ? 100f : world[i, j] < 0.8 ? 0f : 0f;
                    world2[counter++] = world[i, j] < 0.1 ? 100f : world[i, j] < 0.3 ? 0f : world[i, j] < 0.6 ? 0f : world[i, j] < 0.8 ? 48f : 0f;
                    world2[counter++] = world[i, j] < 0.1 ? 0f : world[i, j] < 0.3 ? 100f : world[i, j] < 0.6 ? 100f : world[i, j] < 0.8 ? 88f : 0f;
                    world2[counter++] = world[i, j] < 0.1 ? 0f : world[i, j] < 0.3 ? 0f : world[i, j] < 0.6 ? 0f : world[i, j] < 0.8 ? 60f : 0f;
                    counter++;
                }
            }
            Image image = new Image();
            image.Source = CreateImage(world2, mapDepth, mapWidth);
            image.Width = 700;
            image.Height = 700;
            Image = image;
        }

        private void SliderValue(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Image.Height = Image.Width = Slider.Value * 100;
        }
    }
}