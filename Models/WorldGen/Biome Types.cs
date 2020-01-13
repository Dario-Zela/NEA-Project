﻿using System;
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
        private Dictionary<int, double> biome_membership(ref Map World, ref int idx){
	        Dictionary<int, double> percents = new Dictionary<int,double>();
	        Dictionary<int, long> counts = new Dictionary<int,long>();
	        int nCells = 0;
	        int totalTemperature = 0;
	        int totalRainfall = 0;
	        int totalHeight = 0;
	        int totalVariance = 0;
	        int totalX = 0;
	        int totalY = 0;

	        for (int y=0; y<Constants.WORLD_HEIGHT; y++) {
		        for (int x=0; x<Constants.WORLD_WIDTH; x++) {
			        int blockIdx = planet.idx(x,y);

			        if (World.landBlocks[blockIdx].biomeIdx == idx) {
				        // Increment total counters
				        nCells++;
				        totalTemperature += planet.landblocks[block_idx].temperature;
				        totalRainfall += planet.landblocks[block_idx].rainfall;
				        totalHeight += planet.landblocks[block_idx].height;
				        totalVariance += planet.landblocks[block_idx].variance;
				        totalX += x;
				        totalY += y;

				        // Increment count by cell type
				        bool finder = counts.ContainsKey(planet.landblocks[block_idx].type);
				        if (finder == false) 
                        {
					        counts.Add(planet.landblocks[block_idx].type, 1);
				        } 
                        else 
                        {
					        counts[planet.landblocks[block_idx].type]++;
				        }
			        }
		        }
	        }

	        // Calculate the averages
	        if (nCells == 0) {
		        //std::unordered_map<uint8_t, double>();
		        nCells = 1;
	        }

	        double counter =(double)(nCells);
	        World.biomes[idx].meanAltitude = (int)(total_height / counter);
	        World.biomes[idx].meanRainfall = (int)(total_rainfall / counter);
	        World.biomes[idx].meanTemperature = (int)(total_temperature / counter);
	        World.biomes[idx].meanVariance = (int)(total_variance / counter);
	        World.biomes[idx].centerX = totalX / nCells;
	        World.biomes[idx].centerY = totalY / nCells;

	        int distancePole = (int)(Math.Min( distance2d(planet.biomes[idx].center_x,planet.biomes[idx].center_y, WORLD_WIDTH/2, 0), 
                distance2d(planet.biomes[idx].center_x, planet.biomes[idx].center_y, WORLD_WIDTH/2, WORLD_HEIGHT) ));
	        int distanceCenter = static_cast<int>(distance2d(planet.biomes[idx].center_x,planet.biomes[idx].center_y, WORLD_WIDTH/2, WORLD_HEIGHT/2));

	        if (distancePole > 200) 
            {
		        planet.biomes[idx].warp_mutation = 0;
	        } 
            else 
            {
		        planet.biomes[idx].warp_mutation = (200 - distance_from_pole)/2;
	        }
	        planet.biomes[idx].savagery = Math.Min(100, distance_from_center);

	        for (int i=0; i< (int)blockType.MAX_BLOCK_TYPE + 1; i++) {
		        bool finder = counts.ContainsKey(i);
		        if (finder == false) {
			        percents.Add(i, 0.0);
		        } 
                else 
                {
			        double pct = (double)(counts[i]) / counter;
			        percents.Add(i, pct);
		        }

	        }

	        return percents;
        }

        private float distance2d(int x1, int y1, int x2, int y2)
        {
            float dx = (float)x1 - (float)x2;
            float dy = (float)y1 - (float)y2;
            return Math.Sqrt((dx*dx) + (dy*dy));
        }

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

    class RiverBuilder
    {
        public void planet_rivers(ref Map World, Random rng) {
	        int nRivers = Constants.WORLD_WIDTH/2;
	        HashSet<int> usedStarts = new HashSet<int>();

	        for (int i=0; i<n_rivers; ++i) {
		        River river = new River();

		        bool startOk = false;
		        while (!start_ok) 
                {
			        river.startX = rng.Next(1, Constants.WORLD_WIDTH)-1;
			        river.startY = rng.Next(1, Constants.WORLD_HEIGHT)-1;
			        int pidx = World.idx(river.startX, river.startY);
			        if ((World.landBlocks[pidx].type == (int)blockType.MOUNTAINS || World.landBlocks[pidx].type == (int)blockType.HILLS) && !usedStarts.Contains(pidx)) start_ok = true;
		        }
		        usedStarts.Add(planet.idx(river.startX, river.startY));

		        HashSet<int> usedSteps = new HashSet<int>();
		        bool done = false;
		        int x = river.startX;
		        int y = river.startY;
		        while (!done) {
			        Dictionary<int,(int, int)> candidates = new Dictionary<int,(int, int)>();
			        if (x > 0 && !usedSteps.Contains(World.idx(x-1, y))) candidates.Add(World.landBlocks[planet.idx(x-1, y)].height, (x-1, y));
			        if (x < Constants.WORLD_WIDTH-1 && !usedSteps.Contains(World.idx(x+1, y))) candidates.Add(World.landBlocks[World.idx(x+1, y)].height, (x+1, y));
			        if (y > 0 && !usedSteps.Contains(World.idx(x, y-1))) candidates.Add(World.landBlocks[World.idx(x, y-1)].height, (x, y-1));
			        if (y < Constants.WORLD_HEIGHT-1 && !usedSteps.Contains(World.idx(x, y+1))) candidates.Add(World.landBlocks[World.idx(x, y+1)].height, (x, y+1));
			        riverStep step = new riverStep();
			        if (candidates.empty()) {
				        done = true;
			        } 
                    else
                    {
				        foreach(River test in World.rivers)
                        {
					        if (!done) 
                            {
						        foreach(riverStep step in test.steps)
                                {
							        if (x==step.x && y==step.y) { done=true; break; }
						        }
					        }
				        }
                        //Left Here
				        if (!done) {
					        step.x = candidates.begin()->second.first;
					        step.y = candidates.begin()->second.second;

					        if (planet.landblocks[planet.idx(x,y)].type == block_type::WATER || x == 0 || x == WORLD_WIDTH || y ==0 || y==WORLD_HEIGHT) {
						        done = true;
					        } else {
						        river.steps.push_back(step);
						        used_steps.insert(planet.idx(step.x, step.y));
						        x = step.x;
						        y = step.y;
					        }
				        }
			        }
		        }

		        planet_display_update_zoomed(planet, WORLD_WIDTH/2, WORLD_HEIGHT/2);
		        planet.rivers.push_back(river);
	        }
        }
    }
}
