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
        LSytem sytem;
        double lenght;

        public MainWindow()
        {
            InitializeComponent();
            sytem = new LSytem("F");
            sytem.AddRule('F', "F+F+[F+F]F+[F-F]");
            lenght = 500;
            Draw();
        }
        public void Draw()
        {
            canvas.Children.Clear();
            Vector pos = (canvas.ActualWidth/2, canvas.ActualHeight/2);
            lenght *= 0.5;
            double angle = Math.PI / 2;
            List<(Vector, double)> prePos = new List<(Vector, double)>();
            foreach (char input in sytem.currentSentence)
            {
                if(input == 'F')
                {
                    Line line = new Line();
                    line.X1 = pos.X;
                    line.Y1 = pos.Y;
                    line.X2 = pos.X + Math.Cos(angle) * lenght;
                    line.Y2 = pos.Y + Math.Sin(angle) * lenght;
                    pos += (Math.Cos(angle) * lenght, Math.Sin(angle) * lenght);
                    line.Stroke = Brushes.White;
                    line.StrokeThickness = 0.5;
                    canvas.Children.Add(line);
                }
                else if(input == '+')
                {
                    angle += Math.PI / 6;
                }
                else if (input == '-')
                {
                    angle -= Math.PI / 6;
                }
                else if (input == '[')
                {
                    prePos.Add((pos, angle));
                }
                else if(input == ']')
                {
                    pos = prePos.Last().Item1;
                    angle = prePos.Last().Item2;
                    prePos.RemoveAt(prePos.Count - 1);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sytem.Generate();
            Draw();
        }
    }
}