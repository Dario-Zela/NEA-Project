using System;
using System.Collections.Generic;
using System.Text;

namespace Models.WorldGen
{
    #region OLD
    public class GetBiome
    {
        private Random random = new Random();
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
                    else if (heightMap[i, j] < 0.7f)
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
            Biome[,] biomes = new Biome[mapHeight, mapWidth];

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (idMap[i, j] == 1)
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
                    SpecialAssignment(ref biomes[i, j]);
                }
            }

            return biomes;
        }
        private void SpecialAssignment(ref Biome biome)
        {
            if (random.Next(0,1000) <= biome.probTree)
            {
                biome.red = (byte)0;
                biome.green = (byte)255;
                biome.blue = (byte)0;
                biome.isTree = true;
            } 
            else if (random.Next(0, 1000) <= biome.probBush)
            {
                biome.isBush = true;
                biome.red = (byte)255;
                biome.green = (byte)0;
                biome.blue = (byte)0;
            }
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
        public int probBush;
        public bool doesSnow;
        public byte red;
        public byte green;
        public byte blue;
        public byte alpha;
        public float height;
        public bool isTree;
        public bool isBush;

        public Biome(int Id, float height)
        {
            this.Id = Id.ToString().ToCharArray();
            probBush = 0;
            probTree = 0;
            doesSnow = false;
            red = (byte)255;
            green = (byte)255;
            blue = (byte)255;
            alpha = (byte)255;
            isBush = false;
            isTree = false;
            this.height = height;
            AssignProprieties();
        }

        private void AssignProprieties()
        {
            switch (Id[0])
            {
                case '1':
                    switch (Id[1])
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
                            probBush = 50;
                            probTree = 100;
                            break;
                        case '3':
                            red = (byte)0;
                            green = (byte)140;
                            blue = (byte)0;
                            probBush = 200;
                            probTree = 300;
                            break;
                        default:
                            red = (byte)166;
                            green = (byte)255;
                            blue = (byte)0;
                            probBush = 50;
                            probTree = 5;
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
                            probBush = 5;
                            break;
                        case '2':
                            red = (byte)200;
                            green = (byte)195;
                            blue = (byte)155;
                            probBush = 50;
                            probTree = 100;
                            break;
                        case '3':
                            red = (byte)220;
                            green = (byte)200;
                            blue = (byte)0;
                            probBush = 200;
                            probTree = 300;
                            break;
                        default:
                            red = (byte)140;
                            green = (byte)235;
                            blue = (byte)0;
                            probBush = 50;
                            probTree = 5;
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
                            probBush = 100;
                            probTree = 200;
                            break;
                        default:
                            red = (byte)143;
                            green = (byte)111;
                            blue = (byte)60;
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
                            break;
                        default:
                            red = (byte)143;
                            green = (byte)111;
                            blue = (byte)60;
                            break;
                    }
                    break;
            }
        }
    }
    #endregion

    class BiomeMap
    {
        public void buildBiomes(ref Map World, ref Random rng)
        {
            int nBiomes = Constants.WORLD_TILES_COUNT / (32 + rng.Next(1, 33));

            List<(int, int)> centroids = new List<(int, int)>();
            for (int i = 0; i < nBiomes; i++)
            {
                centroids.Add((rng.Next(1, Constants.WORLD_WIDTH), rng.Next(1, Constants.WORLD_HEIGHT)));
                World.biomes.Add(new Biome());
            }

            for (int y = 0; y < Constants.WORLD_HEIGHT; y++)
            {
                for (int x = 0; x < Constants.WORLD_WIDTH; x++)
                {
                    int distance = int.MaxValue;
                    int closestIndex = -1;

                    for (int i = 0; i < nBiomes; ++i)
                    {
                        int biomeX = centroids[i].Item1;
                        int biomeY = centroids[i].Item2;
                        int dx = (int)(biomeX - x);
                        int dy = (int)(biomeY - y);
                        int biomeDistance = (dx * dx) + (dy * dy);
                        if (biomeDistance < distance)
                        {
                            distance = biomeDistance;
                            closestIndex = i;
                        }
                    }

                    World.landBlocks[World.idx(x, y)].biome_idx = closestIndex;
                }
            }

            int count = 0;
            int noMatch = 0;
            foreach (Biome biome in World.biomes)
            {
                Dictionary<int, doubles> membershipCount = biomeMembership(planet, count);
                if (!(membership_count.Count == 0))
                {
                    List<(double, int)> possibleTypes = findPossibleBiomes(membership_count, biome);
                    if (!(possibleTypes.Count == 0))
                    {
                        double maxRoll = 0.0;
						foreach(var possible in possibleTypes)
						{
							maxRoll += possible.first;
						}
            		    int diceRoll = rng.Next(1, (int)maxRoll);
					
					    foreach(var possible in possibleTypes)
					    {
						    diceRoll -= (int)(possible.Item1);
                		    if (diceRoll < 0)
                		    {
                    		    biome.type = possible.Item2;
                    		    break;
                		    }
					    }
            		    if (biome.type == -1) biome.type = possibleTypes[possible_types.size() - 1].Item2;
						biome.name = nameBiome(planet, rng, biome);
                    }
					else 
					{
						noMatch++;
					}
        		}
			}
        }
    }
}
