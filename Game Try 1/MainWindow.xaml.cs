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
        public MainWindow()
        {
            InitializeComponent();
            MaxO.Text = Max.Value.ToString();
            MinO.Text = Min.Value.ToString();
            NumO.Text = Num.Value.ToString();
            CreateMap();
        }

        private void CreateMap(double max = 200, double min = 20, int num = 200, Vector2 root = default)
        {
            Random random = new Random();
            if (root == default)
            {
                root = new Vector2((float)random.Next(10, (int)canvas.Height - 10), (float)random.Next(10, (int)canvas.Width - 10));
            }
            Tree tree = new Tree(max, min, root, (int)canvas.Height, (int)canvas.Width, num);
            foreach (var branch in tree.Branches)
            {
                if (branch.parent != null)
                {
                    Line line = new Line() { X1 = branch.parent.position.X, X2 = branch.position.X, Y1 = branch.parent.position.Y, Y2 = branch.position.Y };
                    line.Stroke = Brushes.White;
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
            }
        }

        private void Max_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MaxO.Text = Max.Value.ToString();
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
            CreateMap(Max.Value, Min.Value, (int)Num.Value, root);
        }
    }
}