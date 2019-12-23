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
        private Biome[,] IdMap;
        public bool valid = false;

        public GetBiome(float[,] heightMap, float[,] humidityMap, float[,] temperatureMap, int mapWidth, int mapHeight)
        {
            this.heightMap = heightMap;
            this.humidityMap = humidityMap;
            this.temperatureMap = temperatureMap;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            IdMap = ReturnBioms();
            CreateRivers();
        }

        private void CreateRivers()
        {
            LinkedList<int[]> sources = new LinkedList<int[]>();
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    bool isClose = false;
                    foreach (var source in sources)
                    {
                        if(source[0] <= i + 5 && source[0] >= i - 5)
                        {
                            isClose = true;
                        }
                        else if(source[1] <= j + 5 && source[1] >= j - 5)
                        {
                            isClose = true;
                        }
                    }
                    if (IdMap[i, j].isSource && !isClose)
                    {
                        AStarRivers aStar = new AStarRivers(heightMap, (i, j), IdMap);
                        IdMap = aStar.biomes;
                        sources.AddLast(new int[] { i, j });
                    }
                }
            }
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
                    else if(heightMap[i, j] < 0.7f)
                    {
                        IdMap[i, j] = 4;
                    }
                    else if (heightMap[i, j] < 0.8f)
                    {
                        IdMap[i, j] = 5;
                    }
                    else if (heightMap[i, j] < 1.1f)
                    {
                        IdMap[i, j] = 6;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
        }

        private Biome[,] AssignBioms(int[,] idMap)
        {
            Biome[,] biomes = new Biome[mapHeight,mapWidth];

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (idMap[i,j] == 1)
                    {
                        if (temperatureMap[i, j] < 0.4f)
                        {
                            biomes[i, j] = new Biome(11, heightMap[i, j]);
                        }
                        else
                        {
                            biomes[i, j] = new Biome(10, heightMap[i, j]);
                        }
                    }
                    if (idMap[i, j] == 2)
                    {
                        if (temperatureMap[i, j] < 0.4f)
                        {
                            biomes[i, j] = new Biome(21, heightMap[i, j]);
                        }
                        else
                        {
                            biomes[i, j] = new Biome(20, heightMap[i, j]);
                        }
                    }
                    else if (idMap[i, j] == 3)
                    {
                        if (temperatureMap[i, j] < 0.4f)
                        {
                            if (humidityMap[i, j] < 0.4f)
                            {
                                biomes[i, j] = new Biome(31, heightMap[i, j]);
                            }
                            else
                            {
                                biomes[i, j] = new Biome(32, heightMap[i, j]);
                            }
                        }
                        else
                        {
                            if (humidityMap[i, j] < 0.4f)
                            {
                                biomes[i, j] = new Biome(30, heightMap[i, j]);
                            }
                            else
                            {
                                biomes[i, j] = new Biome(33, heightMap[i, j]);
                            }
                        }
                    }
                    else if (idMap[i, j] == 4)
                    {
                        if (temperatureMap[i, j] < 0.4f)
                        {
                            if (humidityMap[i, j] < 0.4f)
                            {
                                biomes[i, j] = new Biome(41, heightMap[i, j]);
                            }
                            else
                            {
                                biomes[i, j] = new Biome(42, heightMap[i, j]);
                            }
                        }
                        else
                        {
                            if (humidityMap[i, j] < 0.4f)
                            {
                                biomes[i, j] = new Biome(43, heightMap[i, j]);
                            }
                            else
                            {
                                biomes[i, j] = new Biome(40, heightMap[i, j]);
                            }
                        }
                    }
                    else if (idMap[i, j] == 5)
                    {
                        if (temperatureMap[i, j] < 0.4f)
                        {
                            if (humidityMap[i, j] < 0.4f)
                            {
                                biomes[i, j] = new Biome(50, heightMap[i, j]);
                            }
                            else
                            {
                                biomes[i, j] = new Biome(51, heightMap[i, j]);
                            }
                        }
                        else
                        {
                            biomes[i, j] = new Biome(50, heightMap[i, j]);
                        }
                    }
                    else if (idMap[i, j] == 6)
                    {
                        if (temperatureMap[i, j] < 0.4f)
                        {
                            if (humidityMap[i, j] < 0.4f)
                            {
                                biomes[i, j] = new Biome(60, heightMap[i, j]);
                            }
                            else
                            {
                                biomes[i, j] = new Biome(61, heightMap[i, j]);
                            }
                        }
                        else
                        {
                            biomes[i, j] = new Biome(60, heightMap[i, j]);
                        }
                    }
                }
            }

            return biomes;
        }

        private Biome[,] ReturnBioms()
        {
            int[,] IdMapTemp = new int[mapHeight, mapWidth];
            Biome[,] biomes;
            CheckHeight(ref IdMapTemp);
            biomes = AssignBioms(IdMapTemp);
            return biomes;
        }

        public Biome this[int index, int index2]
        {
            get 
            {
                return IdMap[index, index2];
            }
        }
    }

    public struct Biome
    {
        public char[] Id;
        public int probTree;
        public int probWaterSource;
        public int probBush;
        public bool doesSnow;
        public byte red;
        public byte green;
        public byte blue;
        public byte alpha;
        public float height;
        public bool isTree;
        public bool isBush;
        public bool isSource;
        public bool isRiverLevee;

        public Biome(int Id, float height)
        {
            this.Id = Id.ToString().ToCharArray();
            probBush = 0;
            probTree = 0;
            probWaterSource = 0;
            doesSnow = false;
            red = (byte)255;
            green = (byte)255;
            blue = (byte)255;
            alpha = (byte)255;
            isBush = false;
            isTree = false;
            isSource = false;
            this.height = height;
            isRiverLevee = false;
            AssignProprieties();
            SpecialAssignment();
        }

        private void SpecialAssignment()
        {
            /*
            if (new Random().Next(0,100) <= probTree)
            {
                red = (byte)0;
                green = (byte)255;
                blue = (byte)0;
                isTree = true;
            } 
            else if (new Random().Next(0, 100) <= probBush)
            {
                isBush = true;
                red = (byte)255;
                green = (byte)0;
                blue = (byte)0;
            }
            else*/ if (new Random().Next(0, 1000) < probWaterSource)
            {
                isSource = true;
                red = (byte)255;
                green = (byte)0;
                blue = (byte)0;
            }
        }

        private void AssignProprieties()
        {
            switch (Id[0])
            {
                case '1':
                    switch(Id[1])
                    {
                        case '1':
                            red = (byte)158;
                            green = (byte)216;
                            blue = (byte)240;
                            break;
                        default:
                            red = (byte)0;
                            green = (byte)0;
                            blue = (byte)255;
                            break;
                    }
                    break;
                case '2':
                    switch (Id[1])
                    {
                        case '1':
                            red = (byte)233;
                            green = (byte)225;
                            blue = (byte)210;
                            break;
                        default:
                            red = (byte)239;
                            green = (byte)221;
                            blue = (byte)111;
                            break;
                    }
                    break;
                case '3':
                    switch (Id[1])
                    {
                        case '1':
                            red = (byte)208;
                            green = (byte)236;
                            blue = (byte)152;
                            probBush = 1;
                            break;
                        case '2':
                            red = (byte)224;
                            green = (byte)216;
                            blue = (byte)176;
                            probBush = 5;
                            probTree = 10;
                            break;
                        case '3':
                            red = (byte)0;
                            green = (byte)140;
                            blue = (byte)0;
                            probBush = 20;
                            probTree = 30;
                            break;
                        default:
                            red = (byte)166;
                            green = (byte)255;
                            blue = (byte)0;
                            probBush = 5;
                            probTree = 1;
                            break;
                    }
                    break;
                case '4':
                    switch (Id[1])
                    {
                        case '1':
                            red = (byte)180;
                            green = (byte)200;
                            blue = (byte)120;
                            probBush = 1;
                            break;
                        case '2':
                            red = (byte)200;
                            green = (byte)195;
                            blue = (byte)155;
                            probBush = 5;
                            probTree = 10;
                            break;
                        case '3':
                            red = (byte)220;
                            green = (byte)200;
                            blue = (byte)0;
                            probBush = 20;
                            probTree = 30;
                            break;
                        default:
                            red = (byte)140;
                            green = (byte)235;
                            blue = (byte)0;
                            probBush = 5;
                            probTree = 1;
                            break;
                    }
                    break;
                case '5':
                    switch (Id[1])
                    {
                        case '1':
                            red = (byte)50;
                            green = (byte)126;
                            blue = (byte)26;
                            probBush = 10;
                            probTree = 20;
                            break;
                        default:
                            red = (byte)143;
                            green = (byte)111;
                            blue = (byte)60;
                            probWaterSource = 1;
                            break;
                    }
                    break;
                case '6':
                    switch (Id[1])
                    {
                        case '1':
                            red = (byte)255;
                            green = (byte)255;
                            blue = (byte)255;
                            doesSnow = true;
                            probWaterSource = 50;
                            break;
                        default:
                            red = (byte)143;
                            green = (byte)111;
                            blue = (byte)60;
                            break;
                    }
                    break;
                case '7':
                    red = (byte)0;
                    green = (byte)0;
                    blue = (byte)255;
                    break;
            }
        }
    }
}
