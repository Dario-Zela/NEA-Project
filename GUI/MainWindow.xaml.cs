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
using Models.World_Gen;

namespace GUI
{
    public partial class MainWindow : Window
    {
        private WorldCreator creator;

        public MainWindow()
        {
            InitializeComponent();
            Set();
        }

        unsafe private void CreateImage(byte[] imageData, int Height, int Width)
        {
            fixed (byte* ptr = imageData)
            {
                Bitmap image = new Bitmap(Width, Height, Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, new IntPtr(ptr));
                var hBitmap = image.GetHbitmap();
                BitmapSource bitSrc = null;
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                Image.Source = bitSrc;
                image.Dispose();
            }
        }

        private void CreateImage2(byte[] imageData, int Height, int Width)
        {
            WriteableBitmap bitmap = new WriteableBitmap(Width, Height, 96.8, 96.8, PixelFormats.Rgb48, null);
            bitmap.WritePixels(new Int32Rect(0, 0, Width/2, Height/2), imageData, 4 * Width, 0);
            Image.Source = bitmap;
        }

        static byte[] PlotPixel(int x, int y, byte[] _imageBuffer, float[,] initial, int Width)
        {
            int offset = ((Width * 4) * y) + (x * 4);

            _imageBuffer[offset] = initial[x, y] < 0.1 ? (byte)255 : initial[x, y] < 0.3 ? (byte)0 : initial[x, y] < 0.65 ? (byte)0 : initial[x, y] < 0.8 ? (byte)15 : (byte)255;
            _imageBuffer[offset + 1] = initial[x, y] < 0.1 ? (byte)0 : initial[x, y] < 0.3 ? (byte)255 : initial[x, y] < 0.65 ? (byte)(255-(int)((initial[x, y]*100)/0.35)) : initial[x, y] < 0.8 ? (byte)74 : (byte)255; ;
            _imageBuffer[offset + 2] = initial[x, y] < 0.1 ? (byte)0 : initial[x, y] < 0.3 ? (byte)255 : initial[x, y] < 0.65 ? (byte)0 : initial[x, y] < 0.8 ? (byte)140 : (byte)255; ;
            // Fixed alpha value (No transparency)
            _imageBuffer[offset + 3] = 255;

            return _imageBuffer;
        }

        public void Set()
        {
            int mapWidth = 700;
            int mapDepth = 700;

            creator = new WorldCreator(mapDepth, mapWidth);
            byte[] world = new byte[mapDepth * mapWidth * 4];
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapDepth; j++)
                {
                    int offset = ((mapWidth * 4) * j) + (i * 4);
                    world[offset] = creator.biomeMap[i, j].blue;
                    world[offset + 1] = creator.biomeMap[i, j].green;
                    world[offset + 2] = creator.biomeMap[i, j].red;
                    world[offset + 3] = creator.biomeMap[i, j].alpha;
                }
            }
            CreateImage(world, mapDepth, mapWidth);
        }

        private void SliderValue(object sender, RoutedEventArgs e)
        {
            Set();
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            var pos = Mouse.GetPosition(Image);
            char[] temp = creator.biomeMap[(int)pos.X, (int)pos.Y].Id;
            string Id = "";
            foreach (var item in temp)
            {
                Id += item;
            }
            string Biome;
            switch (Id)
            {
                case "10": Biome = "Sea"; break;
                case "11": Biome = "Frozen Sea"; break;
                case "20": Biome = "Beach"; break;
                case "21": Biome = "Frozen Beach"; break;
                case "30": Biome = "Plains"; break;
                case "31": Biome = "Taiga"; break;
                case "32": Biome = "Tundra"; break;
                case "33": Biome = "Forest"; break;
                case "40": Biome = "Hills"; break;
                case "41": Biome = "Frozen Hills"; break;
                case "42": Biome = "Tundra Hills"; break;
                case "43": Biome = "Forest Hills"; break;
                case "50": Biome = "Mountain"; break;
                case "51": Biome = "Forest Mountain"; break;
                case "60": Biome = "Peak"; break;
                case "61": Biome = "Snowy Peak"; break;
                default: Biome = null; break;
            }
            Console.WriteLine(Biome);
        }
    }
}