using System;
using System.Collections.Generic;
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
    public partial class MainWindow : Window
    {
        City c;
        Image Image = new Image();
        public MainWindow()
        {
            InitializeComponent();
            CreateMap();
        }

        private void CreateImage2(byte[] imageData, int Height, int Width)
        {
            WriteableBitmap bitmap = new WriteableBitmap(Width, Height, 96.8, 96.8, PixelFormats.Bgra32, null);
            bitmap.WritePixels(new Int32Rect(0, 0, Width, Height), imageData, 4 * Width, 0);
            Image.Source = bitmap;
        }

        static byte[] PlotPixel(int x, int y, byte[] _imageBuffer, float[,] initial, int Width)
        {
            int offset = ((Width * 4) * y) + (x * 4);

            _imageBuffer[offset] = initial[x, y] == 0 ? (byte)0 : initial[x, y] < 0.25 ? (byte)255 : initial[x, y] < 0.75 ? (byte)0 : (byte)0;
            _imageBuffer[offset + 1] = initial[x, y] == 0 ? (byte)0 : initial[x, y] < 0.25 ? (byte)0 : initial[x, y] < 0.75 ? (byte)255 : (byte)0;
            _imageBuffer[offset + 2] = initial[x, y] == 0 ? (byte)0 : initial[x, y] < 0.25 ? (byte)0 : initial[x, y] < 0.75 ? (byte)0 : (byte)255;
            // Fixed alpha value (No transparency)
            _imageBuffer[offset + 3] = 255;

            return _imageBuffer;
        }

        private void CreateMap()
        {
            c = new City(canvas.Height, canvas.Width);
            Image.Height = canvas.Height;
            Image.Width = canvas.Width;
            byte[] image = new byte[(int)(canvas.Height * canvas.Width * 4)];
            for (int i = 0; i < canvas.Height; i++)
            {
                for (int j = 0; j < canvas.Width; j++)
                {
                    image = PlotPixel(i, j, image, c.x, (int)canvas.Width);
                }
            }
            CreateImage2(image, (int)canvas.Height, (int)canvas.Width);
            canvas.Children.Add(Image);
            foreach (var branch in c.t.Branches)
            {
                Ellipse el = new Ellipse() { Width = 2, Height = 2 };
                el.Fill = Brushes.Red;
                Canvas.SetLeft(el, branch.position.X - 1);
                Canvas.SetTop(el, branch.position.Y - 1);
                canvas.Children.Add(el);
                if (branch.parent != null)
                {
                    Line line = new Line() { X1 = branch.parent.position.X, X2 = branch.position.X, Y1 = branch.parent.position.Y, Y2 = branch.position.Y };
                    line.Stroke = Brushes.White;
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
            }

        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            c = new City(canvas.Height, canvas.Width);
            Image.Height = canvas.Height;
            Image.Width = canvas.Width;
            byte[] image = new byte[(int)(canvas.Height * canvas.Width * 4)];
            for (int i = 0; i < canvas.Height; i++)
            {
                for (int j = 0; j < canvas.Width; j++)
                {
                    image = PlotPixel(i, j, image, c.x, (int)canvas.Width);
                }
            }
            CreateImage2(image, (int)canvas.Height, (int)canvas.Width);
            canvas.Children.Add(Image);
            foreach (var branch in c.t.Branches)
            {
                Ellipse el = new Ellipse() { Width = 2, Height = 2 };
                el.Fill = Brushes.Red;
                Canvas.SetLeft(el, branch.position.X - 1);
                Canvas.SetTop(el, branch.position.Y - 1);
                canvas.Children.Add(el);
                if (branch.parent != null)
                {
                    Line line = new Line() { X1 = branch.parent.position.X, X2 = branch.position.X, Y1 = branch.parent.position.Y, Y2 = branch.position.Y };
                    line.Stroke = Brushes.White;
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Text.Text = Slider.Value.ToString();
        }

        private void Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            double x = 0;
            double.TryParse(Text.Text, out x);
            Slider.Value = x;
        }
    }
}