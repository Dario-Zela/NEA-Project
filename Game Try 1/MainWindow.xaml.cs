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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Vector point1 = Vector.Max;
        Vector point2 = Vector.Max;
        Vector point3 = Vector.Max;
        RoadNetwork network;
        LinkedListNode<(Vector, Vector)> toDel;

        public MainWindow()
        {
            InitializeComponent();
            network = new RoadNetwork((int)canvas.Height, (int)canvas.Width, 5, 290821);
            foreach (Vector Key in network.keys)
            {
                foreach (Vector node in network.Graph[Key])
                {
                    Line line = new Line() { X1 = Key.X, X2 = node.X, Y1 = Key.Y, Y2 = node.Y };
                    line.Stroke = Brushes.White;
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
            }

            foreach (var Key in network.keys)
            {
                Ellipse e = new Ellipse() { Width = 4, Height = 4 };
                Canvas.SetLeft(e, Key.X - 2);
                Canvas.SetTop(e, Key.Y - 2);
                e.Fill = Brushes.Red;
                canvas.Children.Add(e);
            }
        }
        /*
        private void Button_Click(object sender, RoutedEventArgs el)
        {
            canvas.Children.Clear();
            network.Graph.removeEdge(toDel.Value.Item1, toDel.Value.Item2);
            toDel = toDel.Next;
            foreach (Vector Key in network.keys)
            {
                foreach (Vector node in network.Graph[Key])
                {
                    Line line = new Line() { X1 = Key.X, X2 = node.X, Y1 = Key.Y, Y2 = node.Y };
                    line.Stroke = network.toDel.First.Value == (Key, node) ? Brushes.Red : Brushes.White;
                    line.StrokeThickness = toDel.Value == (Key, node) ? 1.5 : 0.5;
                    canvas.Children.Add(line);
                }
            }

            foreach (var Key in network.keys)
            {
                Ellipse e = new Ellipse() { Width = 4, Height = 4 };
                Canvas.SetLeft(e, Key.X - 2);
                Canvas.SetTop(e, Key.Y - 2);
                e.Fill = Brushes.Red;
                canvas.Children.Add(e);
            }
        }
        */
        /*
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(point1 == Vector.Max)
            {
                var temp = Mouse.GetPosition(canvas);
                Vector pos = (temp.X, temp.Y);
                point1 = pos.nearestNode(network.keys);
            }
            else if(point2 == Vector.Max)
            {
                var temp = Mouse.GetPosition(canvas);
                Vector pos = (temp.X, temp.Y);
                point2 = pos.nearestNode(network.keys);
            }
            else if(point3 == Vector.Max)
            {
                var temp = Mouse.GetPosition(canvas);
                Vector pos = (temp.X, temp.Y);
                point3 = pos.nearestNode(network.keys);
            }
            else
            {
                var temp = Mouse.GetPosition(canvas);
                Vector pos = (temp.X, temp.Y);
                var point4 = pos.nearestNode(network.keys);
                Console.WriteLine(Vector.doIntersect(point1, point2, point3, point4));
                point1 = Vector.Max;
                point2 = Vector.Max;
                point3 = Vector.Max;
            }
        }
        */
        
        private void Button_Click(object sender, RoutedEventArgs el)
        {
            canvas.Children.Clear();
            RoadNetwork network = new RoadNetwork((int)canvas.Height, (int)canvas.Width, 5, new Random().Next(19919,3872987));

            foreach (Vector Key in network.keys)
            {
                foreach (Vector node in network.Graph[Key])
                {
                    Line line = new Line() { X1 = Key.X, X2 = node.X, Y1 = Key.Y, Y2 = node.Y };
                    line.Stroke = Brushes.White;
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
            }

            foreach (var Key in network.keys)
            {
                Ellipse e = new Ellipse() { Width = 4, Height = 4 };
                Canvas.SetLeft(e, Key.X - 2);
                Canvas.SetTop(e, Key.Y - 2);
                e.Fill = Brushes.Red;
                canvas.Children.Add(e);
            }
        }
        
    }
}