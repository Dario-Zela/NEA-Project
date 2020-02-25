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

        static WorldCreator world;
        public override bool OnUserCreate()
        {
            world = new WorldCreator(800, 1.3f, 0.4f, 6);
            return true;
        }

        bool pass = false;
        public override bool onUserUpdate(float fElapsedTime)
        {
            if (GetKey(Key.ENTER).bPressed)
            {
                world = new WorldCreator(new Random().Next(1000000), 1.3f, 0.4f, 6);
                pass = false;
            }
            if (!pass)
            {
                Parallel.For(0, ScreenWidth(), (x) =>
                {
                    Parallel.For(0, ScreenHeight(), (y) =>
                    {
                        DrawPartialSprite(x, y, world.GetBiomeSprite(x, y), 0, 0, 1, 1);
                    });
                });
                pass= true;
            }
            return true;
        }

        /*
        bool pass = false;
        int Seed = 800;
        int Octaves = 6;
        float Pers = 1.3f;
        float Luca = 0.4f;
        public override bool onUserUpdate(float fElapsedTime)
        {
            if (GetMouse(0).bPressed)
            {
                int x = GetMouseX();
                int y = GetMouseY();
                switch (world.World.landBlocks[world.World.idx(x, y)].type)
                {
                    case 1:
                        Console.WriteLine("WATER");
                        break;
                    case 2:
                        Console.WriteLine("PLAINS");
                        break;
                    case 3:
                        Console.WriteLine("HILLS");
                        break;
                    case 4:
                        Console.WriteLine("MOUNTAINS");
                        break;
                    case 5:
                        Console.WriteLine("MARSH");
                        break;
                    case 6:
                        Console.WriteLine("PLATEAU");
                        break;
                    case 7:
                        Console.WriteLine("HIGHLANDS");
                        break;
                    case 8:
                        Console.WriteLine("COASTAL");
                        break;
                    case 9:
                        Console.WriteLine("SALT MARSH");
                        break;
                    default:
                        break;
                }
            }

            if (GetMouseWheel() > 0)
            {
                Luca -= (float)0.1;
                pass = false;
            }
            if (GetMouseWheel() < 0)
            {
                Luca += (float)0.1;
                pass = false;
            }
            if (GetKey(Key.UP).bPressed)
            {
                Pers += (float)0.1;
                pass = false;
            }
            if (GetKey(Key.DOWN).bPressed)
            {
                Pers -= (float)0.1;
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
            if (GetKey(Key.ENTER).bPressed)
            {
                Seed = new Random().Next(1000000);
                pass = false;
            }
            if (!pass)
            {
                Console.WriteLine(Seed + " " + Pers + " " + Luca + " " + Octaves);
                world = new WorldCreator(Seed, Pers, Luca, Octaves);
                Parallel.For(0, ScreenWidth(), (x) =>
                {
                    Parallel.For(0, ScreenHeight(), (y) =>
                    {
                        DrawPartialSprite(x, y, world.GetBiomeSprite(x, y), 0, 0, 1, 1);
                    });
                });
                pass = true;
            }
            return true;
        }
        */
        /*
        bool pass = false;
        public override bool onUserUpdate(float fElapsedTime)
        {
            if (!pass)
            {
                ISelect s = new Load();
                s.Draw();
                pass = true;
            }
            if (GetKey(Key.ENTER).bPressed)
            {
                world.Load();
            }

            if (pass)
            {
                Parallel.For(0, ScreenWidth(), (x) =>
                {
                    Parallel.For(0, ScreenHeight(), (y) =>
                    {
                        DrawPartialSprite(x, y, world.GetBiomeSprite(x, y), 0, 0, 1, 1);
                    });
                });
                pass = true;
            }
            return true;
        }

        interface ISelect
        {
            void Draw();
        }

        class Load : ISelect
        {
            public void Draw()
            {
                world = new WorldCreator(800, 1.3f, 0.4f, 6);
                world.Save();
            }
        }
        */
    }

    //1.3 0.3999996 6
    //1.7 0.4 6

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
