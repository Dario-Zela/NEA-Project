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
            Console.WindowWidth = 128;
            Console.WindowHeight = 40;
            Console.CursorVisible = false;
            Console.BufferHeight = 128;
            WorldCreator creator = new WorldCreator(new Random().Next(1, 10000));
            string output = "";
            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    var item = creator.World.landBlocks[creator.World.idx(i, j)];
                    switch (item.type)
                    {
                        case 1:
                            output += "~";
                            break;
                        case 2:
                            output += "#";
                            break;
                        case 3:
                            output += "\"";
                            break;
                        case 4:
                            output += "-";
                            break;
                        case 5:
                            output += "=";
                            break;
                        case 6:
                            output += "+";
                            break;
                        case 7:
                            output += "|";
                            break;
                        case 8:
                            output += "¬";
                            break;
                        case 9:
                            output += "¦";
                            break;
                        default:
                            break;
                    }
                }
            }
            Console.Write(output);
            Console.Read();
        }
    }
}
