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
            world = new WorldCreator(800);
            return true;
        }

        bool pass = false;
        public override bool onUserUpdate(float fElapsedTime)
        {
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
