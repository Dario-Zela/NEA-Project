/*
using Models.Sprites;
using Models.WorldGen;
using System.ComponentModel;

namespace UI
{
    partial class Game
    {
        internal class GameUI : IGameElement
        {
            ChunkLoaded Chunks;
            public GameUI(World World, int StartX, int StartY, int Seed)
            {
                Chunks = new ChunkLoaded(World, StartX, StartY, Seed);
                Assets = TileSet.Instance.GetSprites("TerrainMap");
                WorldX = StartX;
                WorldY = StartY;
            }

            internal class ChunkLoaded
            {
                Chunk[] Chunks = new Chunk[9];
                World World;
                readonly int Seed;
                public byte[] Assets
                {
                    get
                    {
                        byte[] Assets = new byte[9 * Constants.REGION_WIDTH * Constants.REGION_HEIGHT];
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                for (int x = 0; x < Constants.REGION_WIDTH; x++)
                                {
                                    for (int y = 0; y < Constants.REGION_HEIGHT; y++)
                                    {
                                        Assets[Constants.REGION_WIDTH * (3 * (y + j * Constants.REGION_HEIGHT) + i) + x] = Chunks[3 * j + i].Assets[Constants.REGION_WIDTH * y + x];
                                    }
                                }
                            }
                        }

                        return Assets;
                    }
                }

                public ChunkLoaded(World world, int StartX, int StartY, int Seed)
                {
                    World = world;
                    this.Seed = Seed;
                    FirstLoad(StartX, StartY);
                }

                private void FirstLoad(int x, int y)
                {
                    int counter = 0;
                    for (int i = x - 1; i < x + 2; i++)
                    {

                        for (int j = y - 1; j < y + 2; j++)
                        {
                            if (i < 0 || i > Constants.WORLD_WIDTH || j < 0 || j > Constants.WORLD_HEIGHT) break;
                            else
                            {
                                Chunks[counter] = new Chunk(Constants.REGION_HEIGHT, Constants.REGION_WIDTH,
                                    World.BiomeTypes[World.topology[i + j * Constants.WORLD_WIDTH].type],
                                    //World.RegionInfos[i + j * Constants.WORLD_WIDTH].isCity,
                                    true, Seed * (y + j) + i + x);
                                counter++;
                            }
                        }
                    }
                }

                public void Load(int x, int y)
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler((sender, e) =>
                    {
                        Chunk[] Temp = new Chunk[9];
                        int counter = 0;
                        for (int i = x - 1; i < x + 2; i++)
                        {

                            for (int j = y - 1; j < y + 2; j++)
                            {
                                if (i < 0 || i > Constants.WORLD_WIDTH || j < 0 || j > Constants.WORLD_HEIGHT) break;
                                else
                                {
                                    Temp[counter] = new Chunk(Constants.REGION_HEIGHT, Constants.REGION_WIDTH,
                                        World.BiomeTypes[World.topology[i + j * Constants.WORLD_WIDTH].type],
                                        //World.RegionInfos[i + j * Constants.WORLD_WIDTH].isCity,
                                        true,Seed * (y + j) + i + x);
                                    counter++;
                                }
                            }
                        }
                        Chunks = Temp;
                    });
                    worker.RunWorkerAsync();
                }
            }

            int WorldX = 0;
            int WorldY = 0;
            int RelativeX = 128;
            int RelativeY = 128;
            readonly Pixel_Engine.Sprite[] Assets;
            bool draw = true;

            public void Draw()
            {
                if (GetKey(Pixel_Engine.Key.LEFT).bPressed && !(WorldX == 0 && RelativeX == 0))
                {
                    RelativeX--;
                    if (RelativeX < 0)
                    {
                        RelativeX = Constants.REGION_WIDTH - 1;
                        WorldX--;
                        Chunks.Load(WorldX, WorldY);
                    }
                    draw = true;
                }
                if (GetKey(Pixel_Engine.Key.RIGHT).bPressed && !(WorldX == Constants.WORLD_WIDTH - 1 && RelativeX == Constants.REGION_WIDTH - 1))
                {
                    RelativeX++;
                    if(RelativeX > Constants.REGION_WIDTH - 1)
                    {
                        RelativeX = 0;
                        WorldX++;
                        Chunks.Load(WorldX, WorldY);
                    }
                    draw = true;
                }
                if (GetKey(Pixel_Engine.Key.UP).bPressed && !(WorldY == 0 && RelativeY == 0))
                {
                    RelativeY--;
                    if (RelativeY < 0)
                    {
                        RelativeY = Constants.REGION_HEIGHT - 1;
                        WorldY--;
                        Chunks.Load(WorldX, WorldY);
                    }
                    draw = true;
                }
                if (GetKey(Pixel_Engine.Key.DOWN).bPressed && !(WorldY == Constants.WORLD_HEIGHT - 1 && RelativeY == Constants.REGION_HEIGHT - 1))
                {
                    RelativeY++;
                    if (RelativeY > Constants.REGION_HEIGHT - 1)
                    {
                        RelativeY = 0;
                        WorldY++;
                        Chunks.Load(WorldX, WorldY);
                    }
                    draw = true;
                }
                if (draw)
                {
                    for (int i = -9; i < 10; i++)
                    {
                        for (int j = -10; j < 12; j++)
                        {
                            int x = RelativeX + i, y = RelativeY + j, cx = 1, cy = 1;
                            if (x < 0)
                            {
                                x = Constants.REGION_WIDTH - 1 + x;
                                cx--;
                            }
                            if (y < 0)
                            {
                                y = Constants.REGION_HEIGHT - 1 + y;
                                cy--;
                            }
                            if (y > Constants.REGION_HEIGHT - 1)
                            {
                                y -= Constants.REGION_HEIGHT - 1;
                                cy++;
                            }
                            if (x > Constants.REGION_WIDTH - 1)
                            {
                                x -= Constants.REGION_WIDTH - 1;
                                cx++;
                            }
                            DrawSprite((i + 9) * 32, (j + 10) * 32, Assets[Chunks.Assets[((3 * cy + y) * 3 + cx) * Constants.REGION_WIDTH + x]]);
                        }
                    }
                    draw = false;
                }
            }

            public IGameElement NewElement()
            {
                return this;
            }
        }
    }
}
*/