using System;
using System.Linq;
using Models.WorldGen;
using Console = Colorful.Console;
using Color = System.Drawing.Color;
using Colorful;

namespace UI
{
    class Program
    {
        static void Main()
        {
            Console.Title = "UI";
            WorldCreator creator = new WorldCreator(new Random().Next(1, 10000));
            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    var item = creator.World.landBlocks[creator.World.idx(i, j)];
                    switch (item.type)
                    {
                        case 1:
                            Console.BackgroundColor = Color.DarkBlue;
                            break;
                        case 2:
                            Console.BackgroundColor = Color.Green;
                            break;
                        case 3:
                            Console.BackgroundColor = Color.DarkGreen;
                            break;
                        case 4:
                            Console.BackgroundColor = Color.Brown;
                            break;
                        case 5:
                            Console.BackgroundColor = Color.SaddleBrown;
                            break;
                        case 6:
                            Console.BackgroundColor = Color.Olive;
                            break;
                        case 7:
                            Console.BackgroundColor = Color.DarkOliveGreen;
                            break;
                        case 8:
                            Console.BackgroundColor = Color.SandyBrown;
                            break;
                        case 9:
                            Console.BackgroundColor = Color.GhostWhite;
                            break;
                        default:
                            break;
                    }
                    Console.Write(" ");
                    Console.ResetColor();
                }
                Console.Write("\n");
            }
        }
    }
}
