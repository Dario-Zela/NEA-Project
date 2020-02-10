using System;
using System.Collections.Generic;
using Models.WorldGen;
using Pixel_Engine;
using System.Linq;
using System.IO;

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

        public override bool onUserUpdate(float fElapsedTime)
        {
            Console.WriteLine("Pass");

            for (int x = 0; x < ScreenWidth() / 8 ; x++)
            {
                for (int y = 0; y < ScreenHeight() / 8; y++)
                {
                    DrawPartialSprite(x * 8, y * 8, world.GetBiomeSprite(x, y), 0, 0, 8, 8);
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
