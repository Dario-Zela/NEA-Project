using System;
using System.Collections.Generic;
using Models.WorldGen;
using Pixel_Engine;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

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
            world = new WorldCreator(978120, 1.3f, 0.4f, 6);
            /*
            Random rng = new Random();
            biomeCol = new Pixel[world.World.biomes.Count];
            for (int i = 0; i < biomeCol.Length; i++)
            {
                biomeCol[i] = new Pixel((byte)rng.Next(255), (byte)rng.Next(255), (byte)rng.Next(255));
            }
            */
            BackgroundWorker b = new BackgroundWorker();
            b.DoWork += new DoWorkEventHandler((sender, e) =>
            {
                while (true)
                {
                    //world.history.RunYear(world.World, ref world.rng);
                    pass = false;
                }
            });
            b.RunWorkerAsync();
            return true;
        }

        bool pass = false;
        public override bool onUserUpdate(float fElapsedTime)
        {
            if (GetKey(Key.ENTER).bPressed)
            {
                world = new WorldCreator(new Random().Next(100000), 1.3f, 0.4f, 6);
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
                foreach (var item in world.World.rivers)
                {
                    Draw(item.Start.x, item.Start.y, Pixel.RED);
                    Draw(item.End.x, item.End.y, Pixel.MAGENTA);
                    foreach (var it2 in item.route)
                    {
                        Draw(it2.x, it2.y, Pixel.BLUE);
                    }
                }
                /*
                foreach (var item in world.World.civs.regionInfo)
                {
                    try
                    {
                        if (item.ownerCiv != 0)
                            Draw(item.x, item.y, world.World.civs.civs[item.ownerCiv].flag);
                    }
                    catch { }
                }
                */
                pass = true;
            }
            return true;
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
    

    //1.3 0.3999996 6
    //1.7 0.4 6

    class AStarTest : Engine
    {
        Node[] nodes;
        int MapWidth = 10;
        int MapHeight = 10;
        Node Start;
        Node End;
        public override bool OnUserCreate()
        {
            nodes = new Node[MapHeight * MapWidth];
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    nodes[y * MapWidth + x] = new Node();
                    nodes[y * MapWidth + x].x = x;
                    nodes[y * MapWidth + x].y = y;
                    nodes[y * MapWidth + x].Obstacle = false;
                    nodes[y * MapWidth + x].Parent = null;
                    nodes[y * MapWidth + x].Visited = false;
                }
            }
            for (int x = 0; x < MapWidth; x++)
                for (int y = 0; y < MapHeight; y++)
                {
                    if (y > 0)
                    {
                        if (x > 0)
                        {
                            nodes[y * MapWidth + x].Neighbours.Add(nodes[(y-1) * MapWidth + (x - 1)]);
                        }
                        if (x < MapWidth - 1)
                        {
                            nodes[y * MapWidth + x].Neighbours.Add(nodes[(y - 1) * MapWidth + (x + 1)]);
                        }
                        nodes[y * MapWidth + x].Neighbours.Add(nodes[(y - 1) * MapWidth + x]);
                    }
                    if (y < MapHeight - 1)
                    {
                        if (x > 0)
                        {
                            nodes[y * MapWidth + x].Neighbours.Add(nodes[(y + 1) * MapWidth + (x - 1)]);
                        }
                        if (x < MapWidth - 1)
                        {
                            nodes[y * MapWidth + x].Neighbours.Add(nodes[(y + 1) * MapWidth + (x + 1)]);
                        }
                        nodes[y * MapWidth + x].Neighbours.Add(nodes[(y + 1) * MapWidth + x]);
                    }
                    if (x > 0)
                    {
                        nodes[y * MapWidth + x].Neighbours.Add(nodes[y * MapWidth + (x - 1)]);
                    }
                    if (x < MapWidth - 1)
                    {
                        nodes[y * MapWidth + x].Neighbours.Add(nodes[y * MapWidth + (x + 1)]);
                    }
                }

            Start = nodes[(MapHeight / 2) * MapWidth + 1];
            End = nodes[(MapHeight / 2) * MapWidth + MapWidth - 2];
            return true;
        }

        public override bool onUserUpdate(float fElapsedTime)
        {
            int NodeSize = 9;
            int NodeBorder = 2;
            Clear(Pixel.BLACK);

            if (GetMouse(0).bReleased)
            {
                int SelectedX = GetMouseX() / (NodeSize + NodeBorder);
                int SelectedY = GetMouseY() / (NodeSize + NodeBorder);
                if(SelectedX >=0 && SelectedX<MapWidth && SelectedY>=0 && SelectedY < MapHeight)
                {
                    if (GetKey(Key.SHIFT).bHeld)
                        Start = nodes[SelectedY * MapWidth + SelectedX];
                    else if(GetKey(Key.CTRL).bHeld)
                        End = nodes[SelectedY * MapWidth + SelectedX];
                    else
                        nodes[SelectedY * MapWidth + SelectedX].Obstacle ^= true;
                    SolveAStar();
                }
            }
            
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    FillRect(x * (NodeSize + NodeBorder), y * (NodeSize + NodeBorder), NodeSize, NodeSize, nodes[y*MapWidth+x].Obstacle? Pixel.WHITE : Pixel.BLUE);
                    if (nodes[y * MapWidth + x] == Start)
                    {
                        FillRect(x * (NodeSize + NodeBorder), y * (NodeSize + NodeBorder), NodeSize, NodeSize, Pixel.GREEN);
                    }
                    else if (nodes[y * MapWidth + x] == End)
                    {
                        FillRect(x * (NodeSize + NodeBorder), y * (NodeSize + NodeBorder), NodeSize, NodeSize, Pixel.RED);
                    }
                    else if (nodes[y * MapWidth + x].Visited)
                    {
                        FillRect(x * (NodeSize + NodeBorder), y * (NodeSize + NodeBorder), NodeSize, NodeSize, Pixel.DARK_BLUE);
                    }
                }
            }

            if(End != null)
            {
                Node target = End;
                while(target.Parent != null)
                {
                    DrawLine(target.x * (NodeSize + NodeBorder), target.y * (NodeSize + NodeBorder),
                            target.Parent.x * (NodeSize + NodeBorder), target.Parent.y * (NodeSize + NodeBorder), Pixel.YELLOW);
                    target = target.Parent;
                }
            }
            
            return true;
        }

        void SolveAStar()
        {
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    nodes[y * MapWidth + x].Visited = false;
                    nodes[y * MapWidth + x].GlobalGoal = float.PositiveInfinity;
                    nodes[y * MapWidth + x].LocalGoal = float.PositiveInfinity;
                    nodes[y * MapWidth + x].Parent = null;
                }
            }

            Func<Node, Node, float> distance = new Func<Node, Node, float>((a, b) =>
            {
                return (float)Math.Sqrt((a.x + b.x) * (a.x + b.x) + (a.y + b.y) * (a.y + b.y));
            });

            Func<Node, Node, float> heuristic = new Func<Node, Node, float>((a, b) =>
            {
                return distance(a, b);
            });

            Node Current = Start;
            Start.LocalGoal = 0f;
            Start.GlobalGoal = heuristic(Start, End);

            List<Node> ToBeTested = new List<Node>();
            ToBeTested.Add(Start);

            while(ToBeTested.Count != 0 && Current != End)
            {
                ToBeTested.Sort(new Comparison<Node>((lhs, rhs) =>
                {
                    return lhs.GlobalGoal > rhs.GlobalGoal?1:-1;
                }));

                ToBeTested.RemoveAll(new Predicate<Node>((node) =>
                {
                    return node.Visited;
                }));

                if(ToBeTested.Count == 0)
                    break;

                Current = ToBeTested.First();
                Current.Visited = true;;

                foreach(Node Neighbour in Current.Neighbours)
                {
                    if(!Neighbour.Visited && !Neighbour.Obstacle)
                    {
                        ToBeTested.Add(Neighbour);
                    }
                    float PossiblyLowerGoal = Current.LocalGoal + distance(Current, Neighbour);

                    if (PossiblyLowerGoal < Neighbour.LocalGoal)
                    {
                        Neighbour.Parent = Current;
                        Neighbour.LocalGoal = PossiblyLowerGoal;
                        Neighbour.GlobalGoal = Neighbour.LocalGoal + heuristic(Neighbour, End);
                    }
                }

                
            }
        }
    }

    class Node
    {
        public bool Obstacle = false;
        public bool Visited = false;
        public float GlobalGoal;
        public float LocalGoal;
        public int x;
        public int y;
        public List<Node> Neighbours = new List<Node>();
        public Node Parent;
    }

    class Start
    {
        static void Main()
        {
            /*
            Test demo = new Test();
            if (demo.Construct(128, 128, 4, 4, false, true))
            {
                demo.Start();
            }
            */
            AStarTest test = new AStarTest();
            if (test.Construct(160, 160, 6, 6))
            {
                test.Start();
            }
        }

    }
}
