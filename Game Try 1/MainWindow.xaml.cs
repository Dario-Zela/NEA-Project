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
        RoadNetwork network;

        public MainWindow()
        {
            InitializeComponent();
            network = new RoadNetwork(20, 400, ((int)canvas.Width / 2, (int)canvas.Width / 2), (int)canvas.Height, (int)canvas.Width, 400, 890132, 10);
            network.Grow();
            foreach (var road in network.Roads)
            {
                if(road.Parent != null)
                {
                    Line line = new Line() { X1 = road.Parent.Position.X, X2 = road.Position.X, Y1 = road.Parent.Position.Y, Y2 = road.Position.Y };
                    line.Stroke = System.Windows.Media.Brushes.White;
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
                else
                {
                    Line line = new Line() { X1 = canvas.Width / 2, X2 = road.Position.X, Y1 = canvas.Width / 2, Y2 = road.Position.Y };
                    line.Stroke = System.Windows.Media.Brushes.White;
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
            }

            foreach (var node in network.AttractionPoints)
            {
                Ellipse ellipse = new Ellipse() { Width = 4, Height = 4 };
                Canvas.SetLeft(ellipse, node.Position.X - 1);
                Canvas.SetTop(ellipse, node.Position.Y - 1);
                ellipse.Fill = System.Windows.Media.Brushes.Wheat;
                canvas.Children.Add(ellipse);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            for (int i = 0; i < 10; i++)
            {
                network.Grow();
            }
            foreach (var road in network.Roads)
            {
                if (road.Parent != null)
                {
                    Line line = new Line() { X1 = road.Parent.Position.X, X2 = road.Position.X, Y1 = road.Parent.Position.Y, Y2 = road.Position.Y };
                    line.Stroke = System.Windows.Media.Brushes.White;
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
                else
                {
                    Line line = new Line() { X1 = canvas.Width / 2, X2 = road.Position.X, Y1 = canvas.Width / 2, Y2 = road.Position.Y };
                    line.Stroke = System.Windows.Media.Brushes.White;
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
            }


            foreach (var node in network.AttractionPoints)
            {
                Ellipse ellipse = new Ellipse() { Width = 4, Height = 4 };
                Canvas.SetLeft(ellipse, node.Position.X - 1);
                Canvas.SetTop(ellipse, node.Position.Y - 1);
                ellipse.Fill = System.Windows.Media.Brushes.Wheat;
                canvas.Children.Add(ellipse);
            }
        }
    }
}