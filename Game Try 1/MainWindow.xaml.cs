using System;
using System.Collections.Generic;
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

namespace Game_Try_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        City City;
        Timer timer = new Timer();
        Canvas canvas = new Canvas();

        public MainWindow()
        {
            InitializeComponent();
            Content = canvas;
            timer.Interval = 100;
            timer.Elapsed += TimerTick;
            timer.Start();
        }

        private void Set()
        {
            City = new City(150, (int)Pong.ActualHeight, (int)Pong.ActualHeight, 10, 300);
            foreach (var item in City.nodes)
            {
                Ellipse ellipse = new Ellipse { Width = 2, Height = 2 };
                ellipse.Fill = new SolidColorBrush(new Color() { R = 255, A = 255, B = 255, G = 255 } );
                Canvas.SetLeft(ellipse, item.pos.Item1);
                Canvas.SetTop(ellipse, item.pos.Item2);
                canvas.Children.Add(ellipse);
            }
            foreach (var item in City.roads)
            {
                if (item.parent != null)
                {
                    Line line = new Line() { X1 = item.parent.pos.Item1, X2 = item.pos.Item1, Y1 = item.parent.pos.Item2, Y2 = item.pos.Item2 };
                    line.Stroke = new SolidColorBrush(new Color() { R = 255, A = 255, B = 0, G = 0 });
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (City == null)
            {
                Dispatcher.Invoke(new Action(Set));
            }
            else
            {
                Dispatcher.Invoke(new Action(Grow));
            }
        }

        private void Grow()
        {
            if(City.nodes.Count == 0)
            {
                timer.Stop();
            }
            else
            {
                canvas.Children.Clear();
                City.Grow();
                foreach (var item in City.nodes)
                {
                    Ellipse ellipse = new Ellipse { Width = 2, Height = 2 };
                    ellipse.Fill = Brushes.White;
                    Canvas.SetLeft(ellipse, item.pos.Item1);
                    Canvas.SetTop(ellipse, item.pos.Item2);
                    canvas.Children.Add(ellipse);
                }
                foreach (var item in City.roads)
                {
                    if (item.parent != null)
                    {
                        Line line = new Line() { X1 = item.parent.pos.Item1, X2 = item.pos.Item1, Y1 = item.parent.pos.Item2, Y2 = item.pos.Item2 };
                        line.Stroke = Brushes.Red;
                        line.StrokeThickness = 0.5;
                        canvas.Children.Add(line);
                    }
                }
            }
        }
    }
}
