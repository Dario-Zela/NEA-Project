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
        Tree tree;

        public MainWindow()
        {
            InitializeComponent();
            MinO.Text = Min.Value.ToString();
            CreateMap();
        }

        private void CreateMap()
        {
            var root = new Vector2((float)canvas.Height, (float)canvas.Width);
            tree = new Tree(1000, 70, root, (int)canvas.Height, (int)canvas.Width, 10);
            foreach (var branch in tree.Branches)
            {
                if (branch.parent != null)
                {
                    Line line = new Line() { X1 = branch.parent.position.X, X2 = branch.position.X, Y1 = branch.parent.position.Y, Y2 = branch.position.Y };
                    line.Stroke = Brushes.White;
                    if (branch.isNode)
                    {
                        Ellipse ellipse = new Ellipse();
                        ellipse.Height = 3;
                        ellipse.Width = 3;
                        ellipse.Fill = Brushes.Red;
                        Canvas.SetLeft(ellipse, branch.position.X - 1.5);
                        Canvas.SetTop(ellipse, branch.position.Y - 1.5);
                        canvas.Children.Add(ellipse);
                    }
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
            }
            Console.WriteLine(tree.Branches.Count);
        }

        private void Min_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MinO.Text = Min.Value.ToString();
        }

        private void Num_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            NumO.Text = Num.Value.ToString();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            var mousePos = Mouse.GetPosition(canvas);
            var root = new Vector2((float)mousePos.X, (float)mousePos.Y);
            CreateMap();
        }
    }
}