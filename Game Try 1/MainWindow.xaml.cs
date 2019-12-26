using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
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

namespace Game_Try_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        City city;
        public MainWindow()
        {
            InitializeComponent();
            MaxO.Text = Max.Value.ToString();
            CreateMap();
        }

        static byte[] PlotPixel(int x, int y, byte[] _imageBuffer, int Width)
        {
            int offset = ((Width * 4) * y) + (x * 4);

            _imageBuffer[offset] = (byte)255;
            _imageBuffer[offset + 1] = (byte)0;
            _imageBuffer[offset + 2] = (byte)0;
            // Fixed alpha value (No transparency)
            _imageBuffer[offset + 3] = 255;

            return _imageBuffer;
        }

        private void CreateMap(double max = 200, double min = 20, int num = 200, Vector2 root = default)
        {
            city = new City((int)canvas.Height, (int)canvas.Width);
            var Con = city.roadMap.graph;
            foreach (var branch in Con)
            {
                if (branch.previous != null)
                {
                    Line line = new Line() { X1 = branch.previous.position.X, X2 = branch.position.X, Y1 = branch.previous.position.Y, Y2 = branch.position.Y };
                    line.Stroke = System.Windows.Media.Brushes.White;
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
            }

            var Cont = city.map;
            byte[] buffer = new byte[19000000];

            for (int i = 0; i < canvas.Height; i++)
            {
                for (int j = 0; j < canvas.Width; j++)
                {
                    if(Cont[i,j] != null)
                    {
                        buffer = PlotPixel(i, j, buffer, (int)canvas.Width);
                    }
                }
            }
            CreateImage(buffer, (int)canvas.Height, (int)canvas.Width);

            var Cont2 = city.Nodes;
            foreach (var item in Cont2)
            {
                Ellipse ellipse = new Ellipse { Height = 2, Width = 2, Fill = System.Windows.Media.Brushes.Red};
                Canvas.SetLeft(ellipse, item.X - 0.5);
                Canvas.SetTop(ellipse, item.Y - 0.5);
                canvas.Children.Add(ellipse);
            }
            

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

        private void Max_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MaxO.Text = Max.Value.ToString();
            Image.Width = Max.Value * 100;
            Image.Height = Max.Value * 100;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Vector2 closestNode = Vector2.Zero;
            double distance = double.MaxValue;
            Vector2 mouse = new Vector2((float)Mouse.GetPosition(canvas).X, (float)Mouse.GetPosition(canvas).Y);
            foreach (var node in city.Nodes)
            {
                if((mouse-node).Length() < distance)
                {
                    closestNode = node;
                    distance = (mouse - node).Length();
                }
            }
            foreach (var Node in city.roadMap.graph)
            {
                if(Node.position == closestNode)
                {
                    foreach (var item in Node.targets)
                    {
                        Ellipse ellipse = new Ellipse { Height = 4, Width = 4, Fill = System.Windows.Media.Brushes.Green };
                        Canvas.SetLeft(ellipse, item.Target.position.X - 2);
                        Canvas.SetTop(ellipse, item.Target.position.Y - 2);
                        canvas.Children.Add(ellipse);
                    }
                }
            }
        }
    }
}