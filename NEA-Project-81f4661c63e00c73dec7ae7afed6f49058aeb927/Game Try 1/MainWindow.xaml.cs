﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Game_Try_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateMap();
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
            bitmap.WritePixels(new Int32Rect(0, 0, Width / 2, Height / 2), imageData, 4 * Width, 0);
            Image.Source = bitmap;
        }

        static byte[] PlotPixel(int x, int y, byte[] _imageBuffer, float[,] initial, int Width)
        {
            int offset = ((Width * 4) * y) + (x * 4);

            _imageBuffer[offset] = initial[x, y] < 0.1 ? (byte)255 : initial[x, y] < 0.3 ? (byte)0 : initial[x, y] < 0.65 ? (byte)0 : initial[x, y] < 0.8 ? (byte)15 : (byte)255;
            _imageBuffer[offset + 1] = initial[x, y] < 0.1 ? (byte)0 : initial[x, y] < 0.3 ? (byte)255 : initial[x, y] < 0.65 ? (byte)(255 - (int)((initial[x, y] * 100) / 0.35)) : initial[x, y] < 0.8 ? (byte)74 : (byte)255; ;
            _imageBuffer[offset + 2] = initial[x, y] < 0.1 ? (byte)0 : initial[x, y] < 0.3 ? (byte)255 : initial[x, y] < 0.65 ? (byte)0 : initial[x, y] < 0.8 ? (byte)140 : (byte)255; ;
            // Fixed alpha value (No transparency)
            _imageBuffer[offset + 3] = 255;

            return _imageBuffer;
        }

        private void CreateMap()
        {
            City c = new City(canvas.Height, canvas.Width);
            
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
            CreateMap();
        }
    }
}