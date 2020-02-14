using System;
using System.Collections.Generic;
using Models.WorldGen;
using Pixel_Engine;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UI
{
    class Test : Engine
    {
        public Test()
        {
            sAppName = "World";
        }

        WorldCreator world;
        public override bool OnUserCreate()
        {
            //world = new WorldCreator(800);
            return true;
        }

        string output = "abcdefghijklmno\npqrstuvwxyz\nABCDEFGHIJKLMNO\nPQRSTUVWXYZ";
        int Size = 1;
        int font = 22;
        public override bool onUserUpdate(float fElapsedTime)
        {
            if (GetMouseWheel() > 0)
            {
                Size++;
            }
            else if (GetMouseWheel() < 0 && Size != 1)
            {
                Size--;
            }
            Clear(Pixel.BLACK);
            var x = ReadKey();
            if (x != '\0' && !GetKey(Key.ENTER).bPressed)
            {
                output += x;
            }
            else if(GetKey(Key.ENTER).bPressed)
            {
                output+='\n';
            }
            else if (GetKey(Key.BACKSPACE).bPressed && output.Length != 0)
            {
                output = output.Remove(output.Length-1);
            }
            if (GetKey(Key.DOWN).bPressed && font != 0)
            {
                font--;
            }
            else if (GetKey(Key.UP).bPressed)
            {
               font++;
               Console.WriteLine(font);
            }

            DrawString(0, 0, output, Pixel.WHITE, Size, font);
            
            /*
            if (!pass)
            {
                Parallel.For(0, ScreenWidth() / 8, (x) =>
                {
                    Parallel.For(0, ScreenWidth() / 8, (y) =>
                    {
                        DrawPartialSprite(x * 8, y * 8, world.GetBiomeSprite(x, y), 0, 0, 8, 8);
                    });
                });
                pass = true;
            }
            */
            return true;
        }
    }

    class Start
    {
        
        static void Main()
        {
            Test demo = new Test();
            if (demo.Construct(1040, 540, 1, 1, true, true))
            {
                demo.Start();
            }
        }

    }
}
