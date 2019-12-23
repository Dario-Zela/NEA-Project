using System;
using System.Collections.Generic;
using System.Text;

namespace Models.WorldGen
{
    public class GetBiome
    {
        private int mapWidth;
        private int mapHeight;
        private float[,] heightMap;
        private float[,] humidityMap;
        private float[,] temperatureMap;
        private readonly int[,] IdMap;

        public GetBiome(float[,] heightMap, float[,] humidityMap, float[,] temperatureMap, int mapWidth, int mapHeight)
        {
            this.heightMap = heightMap;
            this.humidityMap = humidityMap;
            this.temperatureMap = temperatureMap;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            IdMap = ReturnBioms();
        }

        private void CheckHeight(ref int[,] IdMap)
        {
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (heightMap[i, j] < 0.1f)
                    {
                        IdMap[i, j] = 1;
                    }
                    else if (heightMap[i, j] < 0.15f)
                    {
                        IdMap[i, j] = 2;
                    }
                    else if (heightMap[i, j] < 0.6f)
                    {
                        IdMap[i, j] = 3;
                    }
                    else if(heightMap[i, j] < 0.7)
                    {
                        IdMap[i, j] = 4;
                    }
                    else if (heightMap[i, j] < 0.9)
                    {
                        IdMap[i, j] = 5;
                    }
                    else if (heightMap[i, j] < 1)
                    {
                        IdMap[i, j] = 6;
                    }
                }
            }
        }

        private void AssignBioms(ref int[,] idMap)
        {
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (idMap[i,j] == 1)
                    {
                        if (temperatureMap[i, j] < 0.4f)
                        {
                            IdMap[i, j] = 11;
                        }
                        else
                        {
                            IdMap[i, j] = 10;
                        }
                    }
                    if (idMap[i, j] == 2)
                    {
                        if (temperatureMap[i, j] < 0.4f)
                        {
                            IdMap[i, j] = 21;
                        }
                        else
                        {
                            IdMap[i, j] = 20;
                        }
                    }
                    else if (idMap[i, j] == 3)
                    {
                        if (temperatureMap[i, j] < 0.4f)
                        {
                            if (humidityMap[i, j] < 0.4f)
                            {
                                IdMap[i, j] = 31;
                            }
                            else
                            {
                                IdMap[i, j] = 32;
                            }
                        }
                        else
                        {
                            if (humidityMap[i, j] < 0.4f)
                            {
                                IdMap[i, j] = 30;
                            }
                            else
                            {
                                IdMap[i, j] = 33;
                            }
                        }
                    }
                    else if (idMap[i, j] == 4)
                    {
                        if (temperatureMap[i, j] < 0.4f)
                        {
                            if (humidityMap[i, j] < 0.4f)
                            {
                                IdMap[i, j] = 41;
                            }
                            else
                            {
                                IdMap[i, j] = 42;
                            }
                        }
                        else
                        {
                            if (humidityMap[i, j] < 0.4f)
                            {
                                IdMap[i, j] = 43;
                            }
                            else
                            {
                                IdMap[i, j] = 40;
                            }
                        }
                    }
                    else if (idMap[i, j] == 5)
                    {
                        if (temperatureMap[i, j] < 0.4f)
                        {
                            if (humidityMap[i, j] < 0.4f)
                            {
                                IdMap[i, j] = 50;
                            }
                            else
                            {
                                IdMap[i, j] = 51;
                            }
                        }
                        else
                        {
                            IdMap[i, j] = 50;
                        }
                    }
                    else if (idMap[i, j] == 6)
                    {
                        if (temperatureMap[i, j] < 0.4f)
                        {
                            if (humidityMap[i, j] < 0.4f)
                            {
                                IdMap[i, j] = 60;
                            }
                            else
                            {
                                IdMap[i, j] = 61;
                            }
                        }
                        else
                        {
                            IdMap[i, j] = 60;
                        }
                    }
                }
            }
        }

        private int[,] ReturnBioms()
        {
            int[,] IdMapTemp = new int[heightMap.Rank, heightMap.Length];
            CheckHeight(ref IdMapTemp);
            AssignBioms(ref IdMapTemp);
            return IdMapTemp;
        }
    }

    public struct Biome
    {
        private char[] Id;
        public float probTree;
        public float probWaterSource;
        public float probBush;
        public bool doesSnow;

        public Biome(int Id)
        {
            this.Id = Id.ToString().ToCharArray();
            probBush = 0f;
            probTree = 0f;
            probWaterSource = 0f;
            doesSnow = false;
            AssignProprieties();
        }

        private void AssignProprieties()
        {
            switch (Id[0])
            {
                case '3':
                    switch (Id[1])
                    {
                        case '1':
                            probBush = 0.005f;
                            break;
                        case '2':
                            probBush = 0.05f;
                            probTree = 0.1f;
                            break;
                        case '3':
                            probBush = 0.2f;
                            probTree = 0.3f;
                            break;
                        default:
                            probBush = 0.05f;
                            probTree = 0.005f;
                            break;
                    }
                    break;
                case '4':
                    switch (Id[1])
                    {
                        case '1':
                            probBush = 0.005f;
                            break;
                        case '2':
                            probBush = 0.05f;
                            probTree = 0.1f;
                            break;
                        case '3':
                            probBush = 0.2f;
                            probTree = 0.3f;
                            break;
                        default:
                            probBush = 0.05f;
                            probTree = 0.005f;
                            break;
                    }
                    break;
                case '5':
                    switch (Id[1])
                    {
                        case '1':
                            probBush = 0.005f;
                            probTree = 0.0005f;
                            probWaterSource = 0.00003f;
                            break;
                        default:
                            probWaterSource = 0.00003f;
                            break;
                    }
                    break;
                case '6':
                    switch (Id[1])
                    {
                        case '1':
                            doesSnow = true;
                            probWaterSource = 0.03f;
                            break;
                        default:
                            probWaterSource = 0.03f;
                            break;
                    }
                    break;

            }
        }
    }
}
