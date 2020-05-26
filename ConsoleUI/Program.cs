using System;
using System.Collections.Generic;
using Models.WorldGen;
using Models.Sprites;
using Pixel_Engine;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

/*
 * Make Civs create large cities
 * Fix Region Creator
 * Start NPC creation
*/

namespace UI
{
    interface IGameElement
    {
        void Draw();
    }

    partial class Game : Engine
    {
        public Game()
        {
            sAppName = "demo";
        }

        IGameElement CurrentElement;
        public override bool OnUserCreate()
        {
            CurrentElement = new MainMenu();
            return true;
        }

        public override bool onUserUpdate(float fElapsedTime)
        {
            CurrentElement.Draw();
            return true;
        }
    }


    class Test : Engine
    {
        public Test()
        {
            sAppName = "World";
        }

        static WorldCreator world;
        IDraw draw = new Regular();
        public override bool OnUserCreate()
        {
            world = new WorldCreator(78217192, 1.3f, 0.4f, 6);
            return true;
        }
        public override bool onUserUpdate(float fElapsedTime)
        {   
            if (GetKey(Key.ENTER).bPressed)
            {
                world = new WorldCreator(new Random().Next(100000), 1.3f, 0.4f, 6);
            }
            if (GetKey(Key.TAB).bPressed)
            {
                if (draw.GetName() == "reg")
                {
                    draw = new Thermal();
                }
                else
                {
                    draw = new Regular();
                }
            }
            draw.Drawing();
            return true;
        }

        interface IDraw
        {
            string GetName();
            void Drawing();
        }

        internal class Regular : IDraw
        {
            string name = "reg";
            public void Drawing()
            {
                Parallel.For(0, Constants.WORLD_WIDTH, (x) =>
                {
                    Parallel.For(0, Constants.WORLD_HEIGHT, (y) =>
                    {
                        DrawPartialSprite(x, y, world.GetBiomeSprite(x, y), 0, 0, 1, 1);
                    });
                });
                foreach (var item in world.World.rivers)
                {
                    Draw(item.Start.x, item.Start.y, Pixel.RED);
                    Draw(item.End.x, item.End.y, Pixel.MAGENTA);
                    foreach (var it2 in item.route)
                    {
                        Draw(it2.x, it2.y, Pixel.BLUE);
                    }
                }

                foreach (var civ in world.World.civs)
                {
                    foreach (var land in civ.Land)
                    {
                        Draw(land.pos.x, land.pos.y, civ.Flag);
                    }
                    foreach (var Unit in civ.Army)
                    {
                        Draw(Unit.Position.x, Unit.Position.y, civ.Flag+new Pixel(30,30,30));
                    }
                }
            }

            public string GetName()
            {
                return name;
            }
        }

        internal class Thermal : IDraw
        {
            string name = "ther";
            public void Drawing()
            {
                foreach (var civ in world.World.civs)
                {
                    foreach (var land in civ.Land)
                    {
                        Draw(land.pos.x, land.pos.y, new Pixel((byte)(land.Buildings.Count*4),0,0));
                    }
                }
            }

            public string GetName()
            {
                return name;
            }
        }

        internal class Terra : IDraw
        {
            RegionInfo regionInfo;
            Region region;
            public Terra(RegionInfo r)
            {
                regionInfo = r;
                region = new Region(40, ScreenWidth() - 1, ScreenHeight() - 1, 92012);
                region.RunRooms(r.Buildings.Count * 3);
            }

            string name = "Terra";

            public void Drawing()
            {
                foreach (var leaf in region.leafs)
                {
                    Clear(Pixel.BLACK);
                    if (leaf.leftChild == null && leaf.rightChild == null)
                    {
                        //DrawRect(leaf.x, leaf.y, leaf.width, leaf.height, Pixel.GREY);
                        FillRect(leaf.room.x, leaf.room.y, leaf.room.width, leaf.room.height, Pixel.WHITE);
                    }
                    if (leaf.halls != null)
                        foreach (var hall in leaf.halls)
                        {
                            FillRect(hall.x, hall.y, hall.width, hall.height, Pixel.BLUE);
                        }
                }
            }

            public string GetName()
            {
                return name;
            }
        }

        /*
        bool pass = false;
        Pixel[] biomeCol;
        public override bool onUserUpdate(float fElapsedTime)
        {
            if (GetMouse(0).bPressed)
            {
                int x = GetMouseX();
                int y = GetMouseY();
                try
                {
                    var val = world.World.biomes[world.World.landBlocks[world.World.idx(x, y)].biomeIdx];
                    Biome vel = new Biome();
                    int count = -1;
                    foreach (var item in world.World.biomes)
                    {
                        count++;
                        if (item.centerX == x && item.centerY == y)
                        {
                            vel = item;
                            break;
                        }
                    }
                    Console.WriteLine(val.name + "  (" + val.centerX + ", " + val.centerY + ", " + world.World.landBlocks[world.World.idx(x, y)].biomeIdx + ")" + "{0} ({1}, {2}, {3}) ", vel.name, vel.centerX, vel.centerY, count);
                }
                catch
                {
                    Console.WriteLine("({0}, {1})", x, y);
                }
            }
            if (GetKey(Key.ENTER).bPressed)
            {
                Random rng = new Random();
                world = new WorldCreator(rng.Next(1000000), 1.3f, 0.4f, 6);
                biomeCol = new Pixel[world.World.biomes.Count];
                for (int i = 0; i < biomeCol.Length; i++)
                {
                    biomeCol[i] = new Pixel((byte)rng.Next(255), (byte)rng.Next(255), (byte)rng.Next(255));
                }
                pass = false;
            }
            List<Tuple<int,int>> centres = new List<Tuple<int,int>>();
            if (!pass)
            {
                Parallel.For(0, ScreenWidth(), (x) =>
                {
                    Parallel.For(0, ScreenHeight(), (y) =>
                    {
                        var z=world.World.landBlocks[world.World.idx(x,y)];
                        Draw(x, y, biomeCol[z.biomeIdx]);
                    });
                });
                pass= true;
                foreach (var item in world.World.biomes)
                {
                    centres.Add(new Tuple<int,int>(item.centerX, item.centerY));
                    Draw(item.centerX, item.centerY, Pixel.DARK_GREY);
                }
                for (int i = 0; i < centres.Count; i++)
                {
                    int x = centres[i].Item1;
                    int y = centres[i].Item2;
                    try
                    {
                        var val = world.World.biomes[world.World.landBlocks[world.World.idx(x, y)].biomeIdx];
                        if (val.centerX != x && val.centerY != y)
                        {
                            Console.WriteLine("(" + x + ", " + y);
                        }
                    }
                    catch { }
                }
            }

            return true;
        }
        */
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

    class Test2 : Engine
    {
        public Test2()
        {
            sAppName = "World";
        }

        Region region;
        public override bool OnUserCreate() {

            region = new Region(50, ScreenWidth() - 1, ScreenHeight() - 1, 29120);
            region.RunRooms(10);
            return true;
        }

        bool pass = false;
        public override bool onUserUpdate(float fElapsedTime)
        {
            if (GetMouse(0).bPressed)
            {
                Console.WriteLine(GetMouseX() + " " + GetMouseY());
            }
            if (GetKey(Key.ENTER).bPressed)
            {
                region.RunRooms(10);
                pass = false;
            }
            if (!pass)
            {
                Clear(Pixel.BLACK);
                foreach (var leaf in region.leafs)
                {
                    if (leaf.leftChild == null && leaf.rightChild == null)
                    {
                        //DrawRect(leaf.x, leaf.y, leaf.width, leaf.height, Pixel.GREY);
                        FillRect(leaf.room.x, leaf.room.y, leaf.room.width, leaf.room.height, Pixel.WHITE);
                    }
                    if (leaf.halls != null)
                        foreach (var hall in leaf.halls)
                        {
                            FillRect(hall.x, hall.y, hall.width, hall.height, Pixel.BLUE);
                        }
                }
                pass = true;
            }
            return true;
        }
    }
    
    class Test3 : Engine
    {
        public Test3()
        {
            sAppName = "World";
        }
        Sprite[] Sprites;
        WorldCreator world;
        Chunk c;
        public override bool OnUserCreate()
        {
            world = new WorldCreator(78217192, 1.3f, 0.4f, 6);
            Sprites = TileSet.Instance.GetSprites("TerrainMap");
            return true;
        }

        int x = 0;
        int y = 0;
        bool pass = true;
        bool pass1 = true;
        public override bool onUserUpdate(float fElapsedTime)
        {
            if (pass1)
            {
                c = new Chunk(Constants.REGION_HEIGHT, Constants.REGION_WIDTH, world.World.BiomeTypes
                            [world.World.topology[0 + 0 * Constants.WORLD_WIDTH].type],
                            //world.World.RegionInfos[0 + 0 * Constants.WORLD_WIDTH].isCity,
                            true, 78217192 * y + x);
                pass1 = false;
            }
            if (GetKey(Key.LEFT).bPressed)
            {
                x = x-- < 0 ? 0 : x;
                pass = true;
            }
            else if (GetKey(Key.RIGHT).bPressed)
            {
                x = x++ > Constants.WORLD_WIDTH - 1 ? Constants.WORLD_WIDTH - 1 : x;
                pass = true;
            }
            else if (GetKey(Key.UP).bPressed)
            {
                y = y-- < 0 ? 0 : y;
                pass = true;
            }
            else if (GetKey(Key.DOWN).bPressed)
            {
                y = y++ > Constants.WORLD_HEIGHT - 1 ? Constants.WORLD_HEIGHT - 1 : y;
                pass = true;
            }

            if (pass)
            {
                for (int i = 0; i < ScreenWidth() / 32; i++)
                {
                    for (int j = 0; j < ScreenHeight() / 32; j++)
                    {
                        DrawSprite(i * 32, j * 32, Sprites[c.Assets[i + x + (j + y) * Constants.REGION_WIDTH]]);
                    }
                }
                pass = false;
            }
            return true;
        }
    }
    

    class Start
    {
        static void Main()
        {
            Engine demo = new Game();
            if (demo.Construct(700, 700, 1, 1, false, true))
            {
                demo.Start();
            }
            
            /*
            int x = 0;
            int y = 0;
            while (true)
            {
                Console.Clear();
                if (Console.KeyAvailable)
                {
                    if(Console.ReadKey().Key == ConsoleKey.LeftArrow)
                    {
                        x--;
                    }
                    else if(Console.ReadKey().Key == ConsoleKey.RightArrow)
                    {
                        x++;
                    }
                    else if (Console.ReadKey().Key == ConsoleKey.UpArrow)
                    {
                        y--;
                    }
                    else if (Console.ReadKey().Key == ConsoleKey.DownArrow)
                    {
                        y++;
                    }

                }
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Console.CursorLeft = i*3;
                        Console.CursorTop = j;
                        Console.Write(i + x + (j + y) * 10);
                    }
                }
                Console.ReadKey();
            }
            */
        }

    }
}
