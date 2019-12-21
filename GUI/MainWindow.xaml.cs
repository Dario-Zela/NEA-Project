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

        private WriteableBitmap CreateImage(Single[] imageData, int Height, int Width)
        {
            WriteableBitmap image = new WriteableBitmap(Width, Height, 1, 1, PixelFormats.Rgba128Float, null);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    image.WritePixels(new Int32Rect(Width * i / 4, Height * j / 4, Width / 4, Height / 4), imageData, (Width * image.Format.BitsPerPixel + 7) / 8, 0);
                }
            }
            return image;
        }

        public float[] array = new float[1440000];

        public void Set()
        {
            Image image = new Image();
            BitmapImage im = new BitmapImage(new Uri(@"C:\Users\arben\Documents\logo.png"));
            image.Width = 600;
            image.Height = 600;
            FormatConvertedBitmap bitmap = new FormatConvertedBitmap(im, PixelFormats.Gray32Float, BitmapPalettes.Gray256, 1.0);
            bitmap.CopyPixels(array, 2400, 100);
            image.Source = bitmap;
            image.Stretch = Stretch.Uniform;
            image.StretchDirection = StretchDirection.Both;
            image.Height = 700;
            image.Width = 700;
            Image = image;
            Console.WriteLine(array[8778]);
        }

        public void Set(double scope = 10)
        {
            int mapWidth = 700;
            int mapDepth = 700;

            MapGen map = new MapGen();
            double[,] world = map.GenerateNoiseMap(mapDepth, mapWidth, scope, new Random().Next(0, 10000));
            Image image = new Image();
            Single[] world2 = new Single[mapDepth * mapWidth];
            int counter = -1;
            for (int i = 0; i < mapWidth; i++)
            {

                for (int j = 0; j < mapDepth; j++)
                {
                    counter++;
                    Single temp;
                    if ((world[j, i] + 0.5) <= 0)
                    {
                        temp = 0.0001F;
                    }
                    else if ((world[j, i] + 0.5) >= 1)
                    {
                        temp = 0.999F;
                    }
                    else
                    {
                        temp = (Single)(world[j, i] + 0.5);
                    }
                    world2[counter] = temp;
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
