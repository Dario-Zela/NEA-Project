using System;
using System.Collections.Generic;
using Models.WorldGen;
using Pixel_Engine;
using System.Linq;

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
            world = new WorldCreator(800);
            return true;
        }

        int xoff = 0;
        int yoff = 0;

        public override bool onUserUpdate(float fElapsedTime)
        {
            if (GetKey(Key.DOWN).bPressed)
            {
                yoff++;
            }
            else if (GetKey(Key.UP).bPressed && yoff != 0)
            {
                yoff--;
            }
            else if (GetKey(Key.LEFT).bPressed && xoff !=0)
            {
                xoff--;
            }
            else if (GetKey(Key.RIGHT).bPressed)
            {
                xoff++;
            }

            Console.WriteLine("Pass");

            for (int x = xoff; x < ScreenWidth() / 16 + xoff; x++)
            {
                for (int y = yoff; y < ScreenHeight() / 16 + yoff; y++)
                {
                    DrawPartialSprite(x * 16, y * 16, world.GetBiomeSprite(x, y), 0, 0, 16, 16);
                }
            }
            return true;
        }
    }

    class Start
    {
        static void Main()
        {
            Test demo = new Test();
            if (demo.Construct(800, 800, 1, 1, false, true))
            {
                demo.Start();
            }
        }
    }
}