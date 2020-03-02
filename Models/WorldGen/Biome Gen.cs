using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Models.WorldGen
{
    public class Biome
    {
        public string name = "";
        public int type = 0, meanTemperature = 0, meanRainfall = 0, meanAltitude = 0;
        public int meanVariance = 0, warpMutation = 0, evil = 0, savagery = 0;
        public int centerX = 0, centerY = 0;
    }

    class biomeType
    {
        public string name = "";
        public int minRain = 0, maxRain = 100, minTemp = -100, maxTemp = 100;
        public int minMutation = 0, maxMutation = 100, soilPct = 50, sandPct = 50;
        public List<int> occurs = new List<int>();
        public int TextureIndex = 0;
        public List<Tuple<string, int>> plants = new List<Tuple<string, int>>();
        public List<string> wildlife = new List<string>();
        public int deciduousTreeChance = 0;
        public int evergreenTreeChance = 0;
        public List<string> nouns = new List<string>();
    };

    class BiomeMap
    {
        List<biomeType> BiomeTypes = new List<biomeType>();

        public BiomeMap()
        {
            generateTypes();
        }

        private void generateTypes()
        {
            StreamReader reader = new StreamReader(@"..\..\..\Models\WorldGen\WorldGenAssets\BiomeTypes.json");
            BiomeTypes = JsonConvert.DeserializeObject<List<biomeType>>(reader.ReadToEnd());
            reader.Close();
        }

        private Dictionary<int, double> biomeMembership(World World, int idx) {
            Dictionary<int, double> percents = new Dictionary<int, double>();
            Dictionary<int, long> counts = new Dictionary<int, long>();
            int nCells = 0;
            int totalTemperature = 0;
            int totalRainfall = 0;
            int totalHeight = 0;
            int totalVariance = 0;
            int totalX = 0;
            int totalY = 0;

            for (int y = 0; y < Constants.WORLD_HEIGHT; y++) {
                for (int x = 0; x < Constants.WORLD_WIDTH; x++) {
                    int blockIdx = World.idx(x, y);

                    if (World.topology[blockIdx].biomeIdx == idx) {
                        // Increment total counters
                        nCells++;
                        totalTemperature += World.topology[blockIdx].temperature;
                        totalRainfall += World.topology[blockIdx].rainfall;
                        totalHeight += World.topology[blockIdx].height;
                        totalVariance += World.topology[blockIdx].variance;
                        totalX += x;
                        totalY += y;

                        // Increment count by cell type
                        bool finder = counts.ContainsKey(World.topology[blockIdx].type);
                        if (finder == false)
                        {
                            counts.Add(World.topology[blockIdx].type, 1);
                        }
                        else
                        {
                            counts[World.topology[blockIdx].type]++;
                        }
                    }
                }
            }

            // Calculate the averages
            if (nCells == 0) {
                //std::unordered_map<uint8_t, double>();
                nCells = 1;
            }

            double counter = (double)(nCells);
            World.biomes[idx].meanAltitude = (int)(totalHeight / counter);
            World.biomes[idx].meanRainfall = (int)(totalRainfall / counter);
            World.biomes[idx].meanTemperature = (int)(totalTemperature / counter);
            World.biomes[idx].meanVariance = (int)(totalVariance / counter);
            World.biomes[idx].centerX = totalX / nCells;
            World.biomes[idx].centerY = totalY / nCells;

            int distancePole = (int)(Math.Min(distance2d(World.biomes[idx].centerX, World.biomes[idx].centerY, Constants.WORLD_WIDTH / 2, 0),
                distance2d(World.biomes[idx].centerX, World.biomes[idx].centerY, Constants.WORLD_WIDTH / 2, Constants.WORLD_HEIGHT)));
            int distanceCenter = (int)(distance2d(World.biomes[idx].centerY, World.biomes[idx].centerY, Constants.WORLD_WIDTH / 2, Constants.WORLD_HEIGHT / 2));

            if (distancePole > 200)
            {
                World.biomes[idx].warpMutation = 0;
            }
            else
            {
                World.biomes[idx].warpMutation = (200 - distancePole) / 2;
            }
            World.biomes[idx].savagery = Math.Min(100, distanceCenter);

            for (int i = 0; i < (int)blockType.MAX_BLOCK_TYPE + 1; i++) {
                bool finder = counts.ContainsKey(i);
                if (finder == false) {
                    percents.Add(i, 0.0);
                }
                else
                {
                    double pct = counts[i] / counter;
                    percents.Add(i, pct);
                }

            }

            return percents;
        }

        private float distance2d(int x1, int y1, int x2, int y2)
        {
            float dx = x1 - x2;
            float dy = y1 - y2;
            return (float)Math.Sqrt((dx * dx) + (dy * dy));
        }

        public void buildBiomes(World World, ref Random rng)
        {
            int nBiomes = Constants.WORLD_TILES_COUNT / (32 + rng.Next(1, 34));
            List<Tuple<int,int>> centroids = new List<Tuple<int,int>>();
            for (int i = 0; i < nBiomes; i++)
            {
                centroids.Add(new Tuple<int,int>(rng.Next(1, Constants.WORLD_WIDTH), rng.Next(1, Constants.WORLD_HEIGHT)));
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
                        int dx = (biomeX - x);
                        int dy = (biomeY - y);
                        int biomeDistance = (dx * dx) + (dy * dy);
                        if (biomeDistance < distance)
                        {
                            distance = biomeDistance;
                            closestIndex = i;
                        }
                    }
                    World.topology[World.idx(x, y)].biomeIdx = closestIndex;
                }
            }

            int count = 0;
            int noMatch = 0;
            foreach (Biome biome in World.biomes)
            {
                Dictionary<int, double> membershipCount = biomeMembership(World, count);
                if(biome.centerX == 0 && biome.centerY == 0)
                {
                    biome.centerX = -1;
                    biome.centerY = -1;
                }
                if (!(membershipCount.Count == 0))
                {
                    List<Tuple<double, int>> possibleTypes = findPossibleBiomes(ref membershipCount, World.biomes, World.biomes.IndexOf(biome));
                    if (!(possibleTypes.Count == 0))
                    {
                        double maxRoll = 0.0;
                        foreach (var possible in possibleTypes)
                        {
                            maxRoll += possible.Item1;
                        }
                        int diceRoll = rng.Next(1, (int)maxRoll + 1);

                        foreach (var possible in possibleTypes)
                        {
                            diceRoll -= (int)possible.Item1;
                            if (diceRoll < 0)
                            {
                                biome.type = possible.Item2;
                                break;
                            }
                        }
                        if (biome.type == -1) biome.type = possibleTypes[possibleTypes.Count - 1].Item2;
                        biome.name = nameBiome(ref rng, biome);
                    }
                    else
                    {
                        noMatch++;
                    }
                }
                count++;
            }

        }

        private List<Tuple<double,int>> findPossibleBiomes(ref Dictionary<int, double> percents, List<Biome> biomes, int index)
        {
            List<Tuple<double,int>> result = new List<Tuple<double,int>>();

            int idx = 0;
            for (int i = 0; i < BiomeTypes.Count; i++)
            {
                var bt = BiomeTypes[i];
                if (biomes[index].meanTemperature >= bt.minTemp && biomes[index].meanTemperature <= bt.maxTemp
                && biomes[index].meanRainfall >= bt.minRain && biomes[index].meanRainfall <= bt.maxRain
                && biomes[index].warpMutation >= bt.minMutation && biomes[index].warpMutation <= bt.maxMutation)
                {

                    // It's possible, so check to see if tile types are available
                    foreach (int occour in bt.occurs)
                    {
                        bool finder = percents.ContainsKey(occour);
                        if (finder && percents[occour] > 0)
                        {
                            result.Add(new Tuple<double, int>(percents[occour] * 100.0, idx));
                        }
                    }
                }
                idx++;
            }
            return result;
        }

        private string nameBiome(ref Random rng, Biome biome)
        {
	        string name;

            List<string> adjectives = new List<string>();

	        // Location-based adjective
	        if (Math.Abs(biome.centerX - Constants.WORLD_WIDTH/2) < Constants.WORLD_WIDTH /5 && Math.Abs(biome.centerY - Constants.WORLD_HEIGHT /2) < Constants.WORLD_HEIGHT /5) {
		        adjectives.Add("Central");
	        } 
            else
            {
		        if (biome.centerX < Constants.WORLD_WIDTH / 2) adjectives.Add("Western");
		        if (biome.centerX > Constants.WORLD_WIDTH / 2) adjectives.Add("Eastern");
		        if (biome.centerY < Constants.WORLD_HEIGHT / 2) adjectives.Add("Northern");
		        if (biome.centerY > Constants.WORLD_WIDTH / 2) adjectives.Add("Southern");
	        }

	        // Water-based adjectives
	        if (biome.meanRainfall< 10) {
		        switch (rng.Next(1,5)) {
			        case 1 :
				        adjectives.Add("Dry");
				        break;
			        case 2 :
				        adjectives.Add("Arid");
				        break;
			        case 3 :
				        adjectives.Add("Parched");
				        break;
			        case 4 :
				        adjectives.Add("Cracked");
				        break;
		        }
	        } 
            else if (biome.meanRainfall< 30) {
		        switch (rng.Next(1,5)) {
			        case 1 :
				        adjectives.Add("Dusty");
				        break;
			        case 2 :
				        adjectives.Add("Withered");
				        break;
			        case 3 :
				        adjectives.Add("Droughty");
				        break;
			        case 4 :
				        adjectives.Add("Dehydrated");
				        break;
		        }
	        }
            else if(biome.meanRainfall < 50) { }
            else if (biome.meanRainfall< 70) {
		        switch (rng.Next(1,5)) {
			        case 1 :
				        adjectives.Add("Damp");
				        break;
			        case 2 :
				        adjectives.Add("Dank");
				        break;
			        case 3 :
				        adjectives.Add("Moist");
				        break;
			        case 4 :
				        adjectives.Add("Fresh");
				        break;
		        }
	        } 
            else {
		        switch (rng.Next(1,3)) {
			        case 1 :
				        adjectives.Add("Wet");
				        break;
			        case 2 :
				        adjectives.Add("Soaked");
				        break;
			        case 3 :
				        adjectives.Add("Drenched");
				        break;
		        }
	        }

	        // Temperature based adjectives
	        if (biome.meanTemperature< 10) {
		        switch (rng.Next(1,5)) {
			        case 1 :
				        adjectives.Add("Frozen");
				        break;
			        case 2 :
				        adjectives.Add("Cold");
				        break;
			        case 3 :
				        adjectives.Add("Icy");
				        break;
			        case 4 :
				        adjectives.Add("Biting");
				        break;
		        }
	        } 
            else if (biome.meanTemperature< 20) {
		        switch (rng.Next(1,5)) {
			        case 1 :
				        adjectives.Add("Chilly");
				        break;
			        case 2 :
				        adjectives.Add("Frigid");
				        break;
			        case 3 :
				        adjectives.Add("Chilling");
				        break;
			        case 4 :
				        adjectives.Add("Shivering");
				        break;
		        }
	        } 
            else if (biome.meanTemperature< 30) {
		        switch (rng.Next(1,3)) {
			        case 1 :
				        adjectives.Add("Temperate");
				        break;
			        case 2 :
				        adjectives.Add("Comfortable");
				        break;
		        }
	        } 
            else if (biome.meanTemperature< 40) { } 
            else  {
		        switch (rng.Next(1,5)) {
			        case 1 :
				        adjectives.Add("Hot");
				        break;
			        case 2 :
				        adjectives.Add("Scorching");
				        break;
			        case 3 :
				        adjectives.Add("Burning");
				        break;
			        case 4 :
				        adjectives.Add("Fuming");
				        break;
		        }
	        }

            //Savegry based adjectives
            if(biome.savagery < 20)
            {
                if (rng.Next(1, 3) == 1)
                {
                    adjectives.Add("Peaceful");
                }
                else
                {
                    adjectives.Add("Timid");
                }
            }
            else if(biome.savagery < 70) { }
            else
            {
                if (rng.Next(1, 3) == 1)
                {
                    adjectives.Add("Savage");
                }
                else
                {
                    adjectives.Add("Barbaric");
                }
            }

	        string noun = "";
            biomeType bt = BiomeTypes[biome.type];
	        if (bt.nouns.Count == 0) {
		        Console.WriteLine("No nouns defined for {0}", bt.name);
	        }
            else
            {
		        noun = bt.nouns[rng.Next(1, bt.nouns.Count) - 1];
	        }

	        name = noun;
	        if (!(adjectives.Count == 0))
            {
		        int adj1 = rng.Next(0, adjectives.Count);
                name = adjectives[adj1] + " " + noun;
		        if (adjectives.Count > 1 && rng.Next(1,7)>2) {
			        int adj2 = rng.Next(0, adjectives.Count);
			        while (adj1 == adj2) {
				        adj2 = rng.Next(0, adjectives.Count);
			        }
			        name = adjectives[adj2] + " " + name;
		        }
	        }
	        return name;
        }
    }

}
