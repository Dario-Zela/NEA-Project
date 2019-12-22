using System;
using System.Collections.Generic;
using System.Drawing;
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

        public MainWindow()
        {
            Set();
            InitializeComponent();
        }

        unsafe private void CreateImage(byte[] imageData, int Height, int Width)
        {
            fixed (byte* ptr = imageData)
            {
                using (Bitmap image = new Bitmap(Width, Height, Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, new IntPtr(ptr)))
                {
                    image.Save(@"C:\Users\arben\Desktop\try.png");
                }
            }
        }

        static byte[] PlotPixel(int x, int y, byte[] _imageBuffer, float[,] initial, int Width)
        {
            int offset = ((Width * 4) * y) + (x * 4);

            _imageBuffer[offset] = initial[x, y] < 0.1 ? (byte)255 : initial[x, y] < 0.3 ? (byte)0 : initial[x, y] < 0.6 ? (byte)0 : initial[x, y] < 0.8 ? (byte)15 : (byte)255;
            _imageBuffer[offset + 1] = initial[x, y] < 0.1 ? (byte)0 : initial[x, y] < 0.3 ? (byte)255 : initial[x, y] < 0.6 ? (byte)255 : initial[x, y] < 0.8 ? (byte)74 : (byte)255; ;
            _imageBuffer[offset + 2] = initial[x, y] < 0.1 ? (byte)0 : initial[x, y] < 0.3 ? (byte)255 : initial[x, y] < 0.6 ? (byte)0 : initial[x, y] < 0.8 ? (byte)140 : (byte)255; ;
            // Fixed alpha value (No transparency)
            _imageBuffer[offset + 3] = 255;

            return _imageBuffer;
        }

        public void Set()
        {
            int mapWidth = 700;
            int mapDepth = 700;

            MapGen map = new MapGen();
            float[,] world = map.GenerateNoiseMap(mapWidth, mapDepth, 1f, 1219, 6, 0.5f, 2);
            byte[] world2 = new byte[mapDepth*mapWidth*4];
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapDepth; j++)
                {
                    world2 = PlotPixel(i,j,world2,world,mapWidth);
                }
            }
            CreateImage(world2, mapDepth, mapWidth);
        }

        private void SliderValue(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }
    }
}