using System;

namespace Models.WorldGen
{
    public class Chunk
    {
        int ChunkHeight;
        int ChunkWidth;
        biomeType Biome;
        bool isCity;
        int Seed;
        public byte[] Assets;

        public Chunk(int Height, int Width, biomeType Biome, bool isCity, int Seed)
        {
            ChunkHeight = Height;
            ChunkWidth = Width;
            this.Biome = Biome;
            Assets = new byte[ChunkHeight * ChunkWidth];
            this.isCity = isCity;
            this.Seed = Seed;
            GenerateAssets();
        }

        private void swap(ref int x, ref int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }
        private void Draw(int x, int y, byte Asset)
        {
            Assets[x + y * ChunkWidth] = Asset;
        }
        private void DrawLine(int x1, int y1, int x2, int y2, byte Asset)
        {
            uint pattern = 0xFFFFFFFF;
            int x, y, dx, dy, dx1, dy1, px, py, xe, ye, i;
            dx = x2 - x1; dy = y2 - y1;

            Func<bool> rol = new Func<bool>(() =>
            {
                pattern = (pattern << 1) | (pattern >> 31);
                return (pattern & 1) == 1;
            });

            if (dx == 0)
            {
                if (y2 < y1) swap(ref y2, ref y1);
                for (int j = y1; j < y2; j++)
                {
                    if (rol()) Draw(x1, j, Asset);
                }
                return;
            }
            if (dy == 0)
            {
                if (x2 < x1) swap(ref x2, ref x1);
                for (int j = x1; j < x2; j++)
                {
                    if (rol()) Draw(j, y1, Asset);
                }
                return;
            }
            dx1 = Math.Abs(dx); dy1 = Math.Abs(dy);
            px = 2 * dy1 - dx1; py = 2 * dx1 - dy1;
            if (dy1 <= dx1)
            {
                if (dx >= 0)
                {
                    x = x1; y = y1; xe = x2;
                }
                else
                {
                    x = x2; y = y2; xe = x1;
                }

                if (rol()) Draw(x, y, Asset);

                for (i = 0; x < xe; i++)
                {
                    x++;
                    if (px < 0)
                        px += 2 * dy1;
                    else
                    {
                        if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0)) y++; else y--;
                        px += 2 * (dy1 - dx1);
                    }
                    if (rol()) Draw(x, y, Asset);
                }
            }
            else
            {
                if (dy >= 0)
                {
                    x = x1; y = y1; ye = y2;
                }
                else
                {
                    x = x2; y = y2; ye = y1;
                }

                if (rol()) Draw(x, y, Asset);

                for (i = 0; y < ye; i++)
                {
                    y++;
                    if (py <= 0)
                        py += 2 * dx1;
                    else
                    {
                        if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0)) x++; else x--;
                        py += 2 * (dx1 - dy1);
                    }
                    if (rol()) Draw(x, y, Asset);
                }
            }
        }
        private void DrawRect(int x, int y, int w, int h, byte Asset)
        {
            DrawLine(x, y, x + w, y, Asset);
            DrawLine(x + w, y, x + w, y + h, Asset);
            DrawLine(x, y + h, x + w, y + h, Asset);
            DrawLine(x, y, x, y + h, Asset);
            Draw(x + w, y + h, Asset);
        }
        private void FillRect(int x, int y, int w, int h, byte Asset)
        {
            int x2 = x + w;
            int y2 = y + h;

            if (x < 0) x = 0;
            if (x >= ChunkWidth) x = ChunkWidth;
            if (y < 0) y = 0;
            if (y >= ChunkHeight) y = ChunkHeight;

            if (x2 < 0) x2 = 0;
            if (x2 >= ChunkWidth) x2 = ChunkWidth;
            if (y2 < 0) y2 = 0;
            if (y2 >= ChunkHeight) y2 = ChunkHeight;

            for (int i = x; i < x2; i++)
                for (int j = y; j < y2; j++)
                    Draw(i, j, Asset);
        }
        private void GenerateAssets()
        {
            if (isCity)
            {
                Region region = new Region(10, ChunkWidth / 2, ChunkHeight / 2, Seed);
                region.RunRooms();
                foreach (Leaf leaf in region.leafs)
                {
                    if (leaf.leftChild == null && leaf.rightChild == null)
                    {
                        DrawRect(leaf.room.x + ChunkWidth / 4, leaf.room.y + ChunkHeight / 4, leaf.room.width, leaf.room.height, 5);
                        FillRect(leaf.room.x + 1 + ChunkWidth / 4, leaf.room.y + 1 + ChunkHeight / 4, leaf.room.width - 1, leaf.room.height - 1, 6);
                    }
                    if (leaf.halls != null)
                    {
                        foreach (var hall in leaf.halls)
                        {
                            FillRect(hall.x + ChunkWidth / 4, hall.y + ChunkHeight / 4, hall.width, hall.height, 7);
                        }
                    }
                }
            }
            Random rng = new Random(Seed);
            for (int i = 0; i < Assets.Length; i++)
            {
                if (Assets[i] == 0)
                {
                    if (rng.Next(0, 100) < Biome.deciduousTreeChance)
                    {
                        Assets[i] = 3;
                    }
                    else if (rng.Next(0, 100) < Biome.evergreenTreeChance)
                    {
                        Assets[i] = 4;
                    }
                    else
                    {
                        if (Biome.plants[0].Item1 == "grass")
                        {
                            if (rng.Next(0, 100) < Biome.plants[0].Item2)
                            {
                                Assets[i] = 1;
                            }
                            else
                            {
                                for (int j = 1; j < Biome.plants.Count; j++)
                                {
                                    if (rng.Next(0, 100) < Biome.plants[j].Item2)
                                    {
                                        Assets[i] = 2;
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var plant in Biome.plants)
                            {
                                if (rng.Next(0, 100) < plant.Item2)
                                {
                                    Assets[i] = 2;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
