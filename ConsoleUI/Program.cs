using System;
using System.Linq;
using Models.WorldGen;
using Pixel_Engine;

namespace UI
{
    class Program : Engine
    {
        public Program()
        {
            sAppName = "Example";
        }

        public override bool onUserUpdate(float fElapsedTime)
        {
            Random random = new Random();
            for (int i = 0; i < ScreenWidth(); i++)
            {
                for (int j = 0; j < ScreenHeght(); j++)
                {
                    Draw(i, j, new Pixel((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)));
                }
            }
            return true;
        }

        public override bool OnUserCreate()
        {
            return true;
        }
        /*
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
        */
    }

    class Test
    {
        static void Main()
        {
            Program demo = new Program();
            if((int)demo.Construct(256, 240, 4,4) == 1)
            {
                demo.Start();
            }
        }
    }
}
