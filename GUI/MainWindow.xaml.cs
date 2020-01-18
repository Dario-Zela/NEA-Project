using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Models.WorldGen;

namespace GUI
{
    public partial class MainWindow : Window
    {
        private WorldCreator creator;
        int[] num = new int[9];

        public MainWindow()
        {
            InitializeComponent();
            Set();
        }

        private void CreateImage(byte[] imageData, int Height, int Width)
        {
            WriteableBitmap bitmap = new WriteableBitmap(Width, Height, 96.8, 96.8, PixelFormats.Bgra32, null);
            bitmap.WritePixels(new Int32Rect(0, 0, Width / 2, Height / 2), imageData, 4 * Width, 0);
            Image.Source = bitmap;
        }

        private byte[] PlotPixel(int x, int y, byte[] _imageBuffer, Map World, int Width)
        {
            int offset = ((Width * 4) * y) + (x * 4);
            byte B = 0;
            byte G = 0;
            byte R = 0;

            switch (World.landBlocks[World.idx(x, y)].type)
            {
                case 1: R = 0;
                    G = 0;
                    B = 255;
                    num[0]++;
                    break;
                case 2: R = 0;
                    G = 255;
                    B = 0;
                    num[1]++;
                    break;
                case 3: R = 255;
                    G = 0;
                    B = 0;
                    num[2]++;
                    break;
                case 4: R = 255;
                    G = 255;
                    B = 0;
                    num[3]++;
                    break;
                case 5: R = 0;
                    G = 255;
                    B = 255;
                    num[4]++;
                    break;
                case 6: R = 255;
                    G = 0;
                    B = 255;
                    num[5]++;
                    break;
                case 7: R = 255;
                    G = 255;
                    B = 255;
                    num[6]++;
                    break;
                case 8: R = 255;
                    G = 255;
                    B = 255;
                    num[7]++;
                    break;
                case 9: R = 0;
                    G = 0;
                    B = 0;
                    num[8]++;
                    break;
            }

            _imageBuffer[offset] = B;
            _imageBuffer[offset + 1] = G;
            _imageBuffer[offset + 2] = R;
            // Fixed alpha value (No transparency)
            _imageBuffer[offset + 3] = 255;

            return _imageBuffer;
        }

        public void Set(int seed1 = 9182)
        {
            int mapWidth = 700;
            int mapDepth = 700;
            creator = new WorldCreator(seed1);
            byte[] world = new byte[mapDepth * mapWidth * 4];
            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    PlotPixel(i, j, world, creator.World, 128);
                }
            }
            CreateImage(world, 128, 128);
            Image.Stretch = Stretch.Uniform;
            Image.Height = mapDepth;
            Image.Width = mapWidth;
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            var pos = Mouse.GetPosition(Image);
            int temp = creator.World.landBlocks[creator.World.idx((int)(pos.X / (float)700) * 120, (int)(pos.Y / (float)700) * 120)].type;
            string Biome;
            switch (temp)
            {
                case 1: Biome = "WATER"; break;
                case 2: Biome = "PLAINS"; break;
                case 3: Biome = "HILLS"; break;
                case 4: Biome = "MOUNTAINS"; break;
                case 5: Biome = "MARSH"; break;
                case 6: Biome = "PLATEAU"; break;
                case 7: Biome = "HIGHLANDS"; break;
                case 8: Biome = "COASTAL"; break;
                case 9: Biome = "SALT_MARSH"; break;
                default: Biome = null; break;
            }
            Console.WriteLine(Biome);
        }
    }
}