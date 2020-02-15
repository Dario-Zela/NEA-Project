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

        MapGen world;
        public override bool OnUserCreate()
        {
            world = new MapGen();
            return true;
        }

        bool pass = false;
        float Scale = 40;
        uint Seed = 800;
        int Octaves = 1;
        float Pers = (float)0.2;
        float Luca = 2;
        public override bool onUserUpdate(float fElapsedTime)
        {
            if (GetMouseWheel() > 0)
            {
                Scale++;
                pass = false;
                Console.WriteLine(Scale);
            }
            if (GetMouseWheel() < 0)
            {
                Scale--;
                pass = false;
                Console.WriteLine(Scale);
            }
            if (GetKey(Key.UP).bPressed)
            {
                Seed++;
                pass = false;
            }
            if (GetKey(Key.DOWN).bPressed)
            {
                Seed--;
                pass = false;
            }
            if (GetKey(Key.LEFT).bPressed)
            {
                Octaves++;
                pass = false;
            }
            if (GetKey(Key.RIGHT).bPressed)
            {
                Octaves--;
                pass = false;
            }
            if (GetKey(Key.TAB).bPressed)
            {
                if (GetKey(Key.SHIFT).bHeld)
                {
                    Pers -= (float)0.1;
                    pass = false;
                }
                else
                {
                    Pers += (float)0.1;
                    pass = false;
                }
            }
            if (GetKey(Key.ENTER).bPressed)
            {
                if (GetKey(Key.SHIFT).bHeld)
                {
                    Luca -= (float)0.1;
                    pass = false;
                }
                else
                {
                    Luca += (float)0.1;
                    pass = false;
                }
            }
            if (GetKey(Key.SPACE).bPressed)
            {
                Console.WriteLine("Scale: " + Scale + "\nSeed: " + Seed + "\nOctaves: " + Octaves + "\nPersistance: " + Pers + "\nLucanarity: " + Luca);
            }

            if (!pass)
            {
                float[,] vs = world.GenerateNoiseMap(128, 128, Scale, (int)Seed, Octaves, Pers, Luca);
                Parallel.For(0, ScreenWidth(), (x) =>
                {
                    Parallel.For(0, ScreenHeight(), (y) =>
                    {
                        float i = vs[x, y];
                        Pixel p = new Pixel((byte)(255 * i), (byte)(255 * i), (byte)(255 * i));
                        Draw(x, y, p);
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
            if (demo.Construct(128, 128, 4, 4, false, true))
            {
                demo.Start();
            }
        }

    }
}
