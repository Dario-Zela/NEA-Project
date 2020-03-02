using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Pixel_Engine;
using Models.Sprites;
using Models.Algorithms;

namespace Models.WorldGen
{
    public class Terrain
    {
        public int height = 0, variance = 0, type = 0;
        public int temperature = 0, rainfall = 0, biomeIdx = -1;
    }

    public class World
    {
        public string name = "Test World";

        public int remainingSettlers;
        public int waterDivisor = 4, plainsDivisor = 3, startingSettlers = 10;

        public int waterHeight, plainsHeight, hillsHeight;

        public List<Terrain> topology = new List<Terrain>();
        public List<Biome> biomes = new List<Biome>();
        public List<River> rivers = new List<River>();
        public int idx(int x, int y) { return y * Constants.WORLD_WIDTH + x; }
        public civHolder civs = new civHolder();
        public History history = new History();
    }

    enum blockType
    {
        MAX_BLOCK_TYPE = 9,
        WATER = 1,
        PLAINS = 2,
        HILLS = 3,
        MOUNTAINS = 4,
        MARSH = 5,
        PLATEAU = 6,
        HIGHLANDS = 7,
        COASTAL = 8,
        SALT_MARSH = 9,
    }

    /// <summary>
    /// The base world builder;
    /// It constructs the blank world;
    /// </summary>
    public class MapGen
    {
        const float NOISE_SIZE = 384.0f;

        public MapGen() { }

        public Tuple<float[,], float[,], float[,]> GenerateNoiseMap(int mapDepth, int mapWidth, int seed, int Octaves, float Persistance, float Lacunarity, int REGION_FRACTION_TO_CONSIDER)
        {
            float[,] NoiseMap = new float[mapDepth, mapWidth];
            PerlinNoise perlin = new PerlinNoise(Octaves, Persistance, Lacunarity, seed);
            float[,] maximas = new float[mapDepth, mapWidth];
            float[,] minimas = new float[mapDepth, mapWidth];
            float maxNoise = float.MinValue;
            float minNoise = float.MaxValue;
            for (int zIndex = 0; zIndex < mapDepth; zIndex++)
            {
                for (int xIndex = 0; xIndex < mapWidth; xIndex++)
                {
                    float tempMax = float.MinValue;
                    float tempMin = float.MaxValue;
                    float noise = 0;
                    int nTiles = 0;
                    for (int i = 0; i < Constants.REGION_HEIGHT / REGION_FRACTION_TO_CONSIDER; i++)
                    {
                        for (int j = 0; j < Constants.REGION_WIDTH / REGION_FRACTION_TO_CONSIDER; j++)
                        {
                            float SampleIndexX = noiseX(xIndex, i * REGION_FRACTION_TO_CONSIDER);
                            float SampleIndexZ = noiseY(zIndex, j * REGION_FRACTION_TO_CONSIDER);
                            nTiles++;
                            noise += perlin.OctaveNoiseGen(SampleIndexX, SampleIndexZ);
                            if (noise > maxNoise)
                            {
                                maxNoise = noise;
                            }
                            else if (noise < minNoise)
                            {
                                minNoise = noise;
                            }
                            if (tempMax < noise)
                            {
                                tempMax = noise;
                            }
                            else if (tempMin > noise)
                            {
                                tempMin = noise;
                            }
                        }
                    }
                    NoiseMap[xIndex, zIndex] = noise;
                    maximas[xIndex, zIndex] = tempMax;
                    minimas[xIndex, zIndex] = tempMin;
                }
            }
            for (int i = 0; i < mapDepth; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    NoiseMap[j, i] = InverseLerp(minNoise, maxNoise, NoiseMap[j, i]);
                    maximas[j, i] = InverseLerp(minNoise, maxNoise, maximas[j, i]);
                    minimas[j, i] = InverseLerp(minNoise, maxNoise, minimas[j, i]);
                }
            }

            return new Tuple<float[,], float[,], float[,]>(NoiseMap, maximas, minimas);
        }

        private float InverseLerp(float min, float max, float value)
        {
            return (value - min) / (max - min);
        }

        public void startMap(World World)
        {
            for (int i = 0; i < Constants.WORLD_TILES_COUNT; i++)
            {
                World.topology.Add(new Terrain());
            }
            World.remainingSettlers = 200;
            for (int i = 0; i < Constants.WORLD_WIDTH; i++)
            {
                for (int j = 0; j < Constants.WORLD_HEIGHT; j++)
                {
                    World.civs.regionInfo.Add(new regionInfo(i, j));
                }
            }
        }

        public void noiseMap(World World, int seed, int octaves, float persistence, float lacunarity)
        {
            const int REGION_FRACTION_TO_CONSIDER = 64;
            Tuple<float[,], float[,], float[,]> temp = GenerateNoiseMap(Constants.WORLD_HEIGHT, Constants.WORLD_WIDTH, seed, octaves, persistence, lacunarity, REGION_FRACTION_TO_CONSIDER);
            float[,] noiseMap = temp.Item1;
            float[,] maximas = temp.Item2;
            float[,] minimas = temp.Item3;
            const float maxTemperature = 56.7f;
            const float minTemperature = -55.2f;
            const float temperatureRange = maxTemperature - minTemperature;
            float halfHeight = Constants.WORLD_HEIGHT / 2.0f;

            for (int y = 0; y < Constants.WORLD_HEIGHT; y++)
            {
                int distanceFromEquator = (int)(halfHeight / 2) - y;
                float tempRangePct = 1.0f - (distanceFromEquator / halfHeight);
                float baseTempByLatitulde = (tempRangePct * temperatureRange) + minTemperature;
                for (int x = 0; x < Constants.WORLD_WIDTH; x++)
                {
                    World.topology[World.idx(x, y)].height = noiseToHeight(noiseMap[x, y]);
                    World.topology[World.idx(x, y)].type = 0;
                    World.topology[World.idx(x, y)].variance = noiseToHeight(maximas[x, y]) - noiseToHeight(minimas[x, y]);
                    float altitudeDeduction = (World.topology[World.idx(x, y)].height - World.waterHeight) / 10f;
                    World.topology[World.idx(x, y)].temperature = (byte)(baseTempByLatitulde - altitudeDeduction);
                    if (World.topology[World.idx(x, y)].temperature < -55) World.topology[World.idx(x, y)].temperature = -55;
                    if (World.topology[World.idx(x, y)].temperature > 55) World.topology[World.idx(x, y)].temperature = 55;
                }
            }
        }

        private float noiseX(int worldX, int regionX)
        {
            float bigX = (worldX * Constants.WORLD_WIDTH) + regionX;
            return bigX / (Constants.WORLD_WIDTH * Constants.REGION_WIDTH) * NOISE_SIZE;
        }

        private float noiseY(int worldY, int regionY)
        {
            float bigY = (worldY * Constants.WORLD_HEIGHT) + regionY;
            return bigY / (Constants.WORLD_HEIGHT * Constants.REGION_HEIGHT) * NOISE_SIZE;
        }

        private int noiseToHeight(float noiseHeight)
        {
            return (int)((noiseHeight + 1) * 150f);
        }

        private int determineProportions(World World, ref int candidate, int target)
        {
            int count = 0;
            while (count < target)
            {
                int copy = candidate;
                count = World.topology.FindAll(block => block.height <= copy).Count;
                if (count >= target)
                {
                    return candidate;
                }
                else
                {
                    candidate++;
                }
            }
            throw new Exception();
        }

        public void baseTypeAllocation(World World)
        {
            int candidate = 0;
            int remainingDivisor = 10 - (World.waterDivisor + World.plainsDivisor);
            int nCellWater = Constants.WORLD_TILES_COUNT / World.waterDivisor;
            int nCellPlains = (Constants.WORLD_TILES_COUNT / World.plainsDivisor) + nCellWater;
            int nCellHills = (Constants.WORLD_TILES_COUNT / remainingDivisor) + nCellPlains;

            World.waterHeight = determineProportions(World, ref candidate, nCellWater);
            World.plainsHeight = determineProportions(World, ref candidate, nCellPlains);
            World.hillsHeight = determineProportions(World, ref candidate, nCellHills);

            for (int i = 0; i < World.topology.Count; i++)
            {
                if (World.topology[i].height <= World.waterHeight)
                {
                    World.topology[i].type = (int)blockType.WATER;
                    World.topology[i].rainfall = 10;
                    if (World.topology[i].height + World.topology[i].variance / 2 > World.waterHeight)
                        World.topology[i].type = (int)blockType.SALT_MARSH;
                }
                else if (World.topology[i].height <= World.plainsHeight)
                {
                    World.topology[i].type = (int)blockType.PLAINS;
                    World.topology[i].rainfall = 10;
                    if (World.topology[i].height - World.topology[i].variance / 2 > World.waterHeight)
                        World.topology[i].type = (int)blockType.MARSH;
                }
                else if (World.topology[i].height <= World.hillsHeight)
                {
                    World.topology[i].type = (int)blockType.HILLS;
                    World.topology[i].rainfall = 20;
                    if (World.topology[i].variance < 2)
                    {
                        World.topology[i].type = (int)blockType.HIGHLANDS;
                        World.topology[i].rainfall = 10;
                    }
                }
                else
                {
                    World.topology[i].type = (int)blockType.MOUNTAINS;
                    World.topology[i].rainfall = 30;
                    if (World.topology[i].variance < 3)
                    {
                        World.topology[i].type = (int)blockType.PLATEAU;
                        World.topology[i].rainfall = 10;
                    }
                }
            }
        }

        public void markCoastlines(World World)
        {
            for (int y = 1; y < Constants.WORLD_HEIGHT - 1; y++)
            {
                for (int x = 1; x < Constants.WORLD_WIDTH - 1; x++)
                {
                    if (World.topology[World.idx(x, y)].type > (int)blockType.WATER)
                    {
                        if (
                            World.topology[World.idx(x - 1, y - 1)].type == (int)blockType.WATER ||
                            World.topology[World.idx(x, y - 1)].type == (int)blockType.WATER ||
                            World.topology[World.idx(x + 1, y - 1)].type == (int)blockType.WATER ||
                            World.topology[World.idx(x - 1, y)].type == (int)blockType.WATER ||
                            World.topology[World.idx(x + 1, y)].type == (int)blockType.WATER ||
                            World.topology[World.idx(x - 1, y + 1)].type == (int)blockType.WATER ||
                            World.topology[World.idx(x, y + 1)].type == (int)blockType.WATER ||
                            World.topology[World.idx(x + 1, y + 1)].type == (int)blockType.WATER
                        )
                        {
                            World.topology[World.idx(x, y)].type = (int)blockType.COASTAL;
                            World.topology[World.idx(x, y)].rainfall = 20;
                        }
                    }
                }
            }
        }

        public void createRainfall(World World)
        {
            for (int y = 0; y < Constants.WORLD_HEIGHT; ++y)
            {
                int rainAmount = 10;
                for (int x = 0; x < Constants.WORLD_WIDTH; ++x)
                {
                    if (World.topology[World.idx(x, y)].type == (int)blockType.MOUNTAINS ||
                        World.topology[World.idx(x, y)].type == (int)blockType.HIGHLANDS)
                    {
                        rainAmount -= 20;
                    }
                    else if (World.topology[World.idx(x, y)].type == (int)blockType.HILLS ||
                        World.topology[World.idx(x, y)].type == (int)blockType.SALT_MARSH)
                    {
                        rainAmount -= 10;
                    }
                    else if (World.topology[World.idx(x, y)].type == (int)blockType.COASTAL)
                    {
                        rainAmount -= 5;
                    }
                    else
                    {
                        rainAmount += 1;
                    }
                    if (rainAmount < 0) rainAmount = 0;
                    if (rainAmount > 20) rainAmount = 20;

                    World.topology[World.idx(x, y)].rainfall += rainAmount;
                    if (World.topology[World.idx(x, y)].rainfall < 0) World.topology[World.idx(x, y)].rainfall = 0;
                    if (World.topology[World.idx(x, y)].rainfall > 100) World.topology[World.idx(x, y)].rainfall = 100;
                }
            }
        }
    }
}
