using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Models.WorldGen
{
    /*
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
    */
    class BiomeMap
    {

        List<biomeType> BiomeTypes = new List<biomeType>();

        public BiomeMap()
        {
            generateTypes();
        }

        private enum biomeTypes
        {
            ocean = 1,
            plains = 2,
            hills = 3,
            mountains = 4,
            marsh = 5,
            plateau = 6,
            highlands = 7,
            coast = 8,
            salt_marsh = 9
        }

        private void generateTypes()
        {
            StreamReader reader = new StreamReader(@"C:\Users\arben\Desktop\Dario\Programming\C#.NET\NEA\Models\WorldGen\WorldGenAssets\BiomeTypes.json");
            BiomeTypes = JsonConvert.DeserializeObject<List<biomeType>>(reader.ReadToEnd());
        }

        private Dictionary<int, double> biomeMembership(ref Map World, ref int idx) {
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

                    if (World.landBlocks[blockIdx].biomeIdx == idx) {
                        // Increment total counters
                        nCells++;
                        totalTemperature += World.landBlocks[blockIdx].temperature;
                        totalRainfall += World.landBlocks[blockIdx].rainfall;
                        totalHeight += World.landBlocks[blockIdx].height;
                        totalVariance += World.landBlocks[blockIdx].variance;
                        totalX += x;
                        totalY += y;

                        // Increment count by cell type
                        bool finder = counts.ContainsKey(World.landBlocks[blockIdx].type);
                        if (finder == false)
                        {
                            counts.Add(World.landBlocks[blockIdx].type, 1);
                        }
                        else
                        {
                            counts[World.landBlocks[blockIdx].type]++;
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
                        int dx = (biomeX - x);
                        int dy = (biomeY - y);
                        int biomeDistance = (dx * dx) + (dy * dy);
                        if (biomeDistance < distance)
                        {
                            distance = biomeDistance;
                            closestIndex = i;
                        }
                    }

                    World.landBlocks[World.idx(x, y)].biomeIdx = closestIndex;
                }
            }

            int count = 0;
            int noMatch = 0;
            foreach (Biome biome in World.biomes)
            {
                Dictionary<int, double> membershipCount = biomeMembership(ref World, ref count);
                if (!(membershipCount.Count == 0))
                {
                    List<(double, int)> possibleTypes = findPossibleBiomes(ref membershipCount, ref World.biomes, World.biomes.IndexOf(biome));
                    if (!(possibleTypes.Count == 0))
                    {
                        double maxRoll = 0.0;
                        foreach (var possible in possibleTypes)
                        {
                            maxRoll += possible.Item1;
                        }
                        int diceRoll = rng.Next(1, (int)maxRoll);

                        foreach (var possible in possibleTypes)
                        {
                            diceRoll -= (int)(possible.Item1);
                            if (diceRoll < 0)
                            {
                                biome.type = possible.Item2;
                                break;
                            }
                        }
                        if (biome.type == -1) biome.type = possibleTypes[possibleTypes.Count - 1].Item2;
                        biome.name = nameBiome(ref World, ref rng, biome);
                    }
                    else
                    {
                        noMatch++;
                    }
                }
            }
        }

        public List<(double, int)> findPossibleBiomes(ref Dictionary<int, double> percents, ref List<Biome> biomes, int index)
        {
            List<(double, int)> result = new List<(double, int)>();

            int idx = 0;
            for (int i = 0; i < BiomeTypes.Count; i++)
            {
                possibleBiomesMethod(BiomeTypes[i], ref percents, ref biomes, ref result, ref idx, index);
            }
            return result;
        }

        private void possibleBiomesMethod(biomeType bt, ref Dictionary<int, double> percents, ref List<Biome> biomes, ref List<(double, int)> results, ref int idx, int index)
        {
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
                        results.Add((percents[occour] * 100.0, idx));
                    }
                }
            }
            idx++;
        }

        private string nameBiome(ref Map World, ref Random rng, Biome biome)
        {
	        string name;

            List<string> adjectives = new List<string>();

	        // Location-based adjective
	        if (Math.Abs(biome.centerX - Constants.WORLD_WIDTH/2) < Constants.WORLD_WIDTH /10 && Math.Abs(biome.centerY - Constants.WORLD_HEIGHT /2) < Constants.WORLD_HEIGHT /10) {
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
		        switch (rng.Next(1,4)) {
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
		        switch (rng.Next(1,4)) {
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
            else if (biome.meanRainfall< 50) {
		        switch (rng.Next(1,4)) {
			        case 1 :
				        adjectives.Add("Pleasant");
				        break;
			        case 2 :
				        adjectives.Add("Kind");
				        break;
			        case 3 :
				        adjectives.Add("Gentle");
				        break;
			        case 4 :
				        adjectives.Add("Timid");
				        break;
		        }
	        } 
            else if (biome.meanRainfall< 70) {
		        switch (rng.Next(1,4)) {
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
		        switch (rng.Next(1,4)) {
			        case 1 :
				        adjectives.Add("Wet");
				        break;
			        case 2 :
				        adjectives.Add("Soggy");
				        break;
			        case 3 :
				        adjectives.Add("Soaked");
				        break;
			        case 4 :
				        adjectives.Add("Drenched");
				        break;
		        }
	        }

	        // Temperature based adjectives
	        if (biome.meanTemperature< 10) {
		        switch (rng.Next(1,4)) {
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
		        switch (rng.Next(1,4)) {
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
		        switch (rng.Next(1,4)) {
			        case 1 :
				        adjectives.Add("Pleasant");
				        break;
			        case 2 :
				        adjectives.Add("Nice");
				        break;
			        case 3 :
				        adjectives.Add("Temperate");
				        break;
			        case 4 :
				        adjectives.Add("Comfortable");
				        break;
		        }
	        } 
            else if (biome.meanTemperature< 40) {
		        switch (rng.Next(1,4)) {
			        case 1 :
				        adjectives.Add("Warm");
				        break;
			        case 2 :
				        adjectives.Add("Toasty");
				        break;
			        case 3 :
				        adjectives.Add("Cozy");
				        break;
			        case 4 :
				        adjectives.Add("Snug");
				        break;
		        }
	        } 
            else  {
		        switch (rng.Next(1,4)) {
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
		        int adj1 = rng.Next(1, adjectives.Count) - 1;
                name = adjectives[adj1] + " " + noun;
		        if (adjectives.Count > 1 && rng.Next(1,6)>2) {
			        int adj2 = rng.Next(1, adjectives.Count) - 1;
			        while (adj1 == adj2) {
				        adj2 = rng.Next(1, adjectives.Count)-1;
			        }
			        name = adjectives[adj2] + " " + name;
		        }
	        }
	        return name;
        }
    }

    class RiverBuilder
    {
        public void World_rivers(ref Map World, Random rng) {
	        int nRivers = Constants.WORLD_WIDTH/2;
	        HashSet<int> usedStarts = new HashSet<int>();

	        for (int i=0; i<nRivers; ++i) {
		        River river = new River();

		        bool startOk = false;
		        while (!startOk) 
                {
			        river.startX = rng.Next(1, Constants.WORLD_WIDTH)-1;
			        river.startY = rng.Next(1, Constants.WORLD_HEIGHT)-1;
			        int pidx = World.idx(river.startX, river.startY);
			        if ((World.landBlocks[pidx].type == (int)blockType.MOUNTAINS || World.landBlocks[pidx].type == (int)blockType.HILLS) && !usedStarts.Contains(pidx)) startOk = true;
		        }
		        usedStarts.Add(World.idx(river.startX, river.startY));

		        HashSet<int> usedSteps = new HashSet<int>();
		        bool done = false;
		        int x = river.startX;
		        int y = river.startY;
		        while (!done) {
			        Dictionary<int,(int, int)> candidates = new Dictionary<int,(int, int)>();
			        if (x > 0 && !usedSteps.Contains(World.idx(x-1, y))) candidates.Add(World.landBlocks[World.idx(x-1, y)].height, (x-1, y));
			        if (x < Constants.WORLD_WIDTH-1 && !usedSteps.Contains(World.idx(x+1, y))) candidates.Add(World.landBlocks[World.idx(x+1, y)].height, (x+1, y));
			        if (y > 0 && !usedSteps.Contains(World.idx(x, y-1))) candidates.Add(World.landBlocks[World.idx(x, y-1)].height, (x, y-1));
			        if (y < Constants.WORLD_HEIGHT-1 && !usedSteps.Contains(World.idx(x, y+1))) candidates.Add(World.landBlocks[World.idx(x, y+1)].height, (x, y+1));
			        RiverStep step = new RiverStep();
			        if (candidates.Count == 0) {
				        done = true;
			        } 
                    else
                    {
				        foreach(River test in World.rivers)
                        {
					        if (!done) 
                            {
						        foreach(RiverStep step2 in test.route)
                                {
							        if (x==step2.x && y==step2.y) { done=true; break; }
						        }
					        }
				        }
                        //Left Here
				        if (!done) {
                            int[] temp = new int[candidates.Count];
                            candidates.Keys.CopyTo(temp,0);
                            step.x = candidates[temp[0]].Item1;
					        step.y = candidates[temp[0]].Item1;
					        if (World.landBlocks[World.idx(x,y)].type == (int)blockType.WATER || x == 0 || x == Constants.WORLD_WIDTH || y ==0 || y==Constants.WORLD_HEIGHT) {
						        done = true;
					        } else {
						        river.route.Add(step);
						        usedSteps.Add(World.idx(step.x, step.y));
						        x = step.x;
						        y = step.y;
					        }
				        }
			        }
		        }

		        World.rivers.Add(river);
	        }
        }
    }
    
    class HistoryMaker
    {
        int N_CIVS = Constants.WORLD_WIDTH;
        int FIRST_NAMES_MALE = 1;
        int FIRST_NAMES_FEMALE = 2;
        int LAST_NAMES = 3;

        public HistoryMaker()
        {

        }

        Dictionary<int, stringTable> stringTables = new Dictionary<int, stringTable>();

        public void loadNames(int index, string filename)
        {
            stringTable target = new stringTable();
            StreamReader reader = new StreamReader(filename);
            string line;
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (!(line.Length == 0)) target.strings.Add(line);
            }

            stringTables.Add(index, target);
        }



        /*
        public void buildInitialCivs(ref Map World, Random rng)
        {
            for (int i = 1; i < N_CIVS; ++i)
            {
                Civ civ = new Civ();

                start_over:
                int wx = rng.Next(1, Constants.WORLD_WIDTH - 1);
                int wy = rng.Next(1, Constants.WORLD_HEIGHT - 1);
                int pidx = World.idx(wx, wy);
                if (World.landBlocks[pidx].type == (int)blockType.WATER) goto start_over;
                if (World.civs.regionInfo[pidx].ownerCiv > 0) goto start_over;

                string loc_name = World.biomes[World.landBlocks[pidx].biomeIdx].name;

                civ.speciesTag = get_random_species(rng, 0);
                civ.techLevel = 0;
                civ.extinct = false;
                civ.r = (byte)rng.Next(1, 255);
                civ.g = (byte)rng.Next(1, 255);
                civ.b = (byte)rng.Next(1, 255);
                civ.startX = wx;
                civ.startY = wy;

                // Name generation
                 civ_finder = get_civ_def(civ.species_tag);
                const auto civ_name_func = "civ_name_gen_" + civ_finder->name_generator;
                const auto civ_leader_func = "leader_name_gen_" + civ_finder->name_generator;
                civ.name = lua_str_func(civ_name_func, rng.roll_dice(1,1000)) + std::string (" of the ") + loc_name;
            if (str_contains(civ.name, "{LASTNAME}")) {
                civ.name = str_replace(civ.name, "{LASTNAME}", string_table(LAST_NAMES)->random_entry(rng));
        }
        civ.leader_name = lua_str_func(civ_leader_func, rng.roll_dice(1,1000));
            if (str_contains(civ.leader_name, "{LASTNAME}")) {
                civ.leader_name = str_replace(civ.leader_name, "{LASTNAME}", to_proper_noun_case(string_table(LAST_NAMES)->random_entry(rng)));
            }
            if (str_contains(civ.leader_name, "{FIRSTNAME_M}")) {
                civ.leader_name = str_replace(civ.leader_name, "{FIRSTNAME_M}", to_proper_noun_case(string_table(FIRST_NAMES_MALE)->random_entry(rng)));
            }
            if (str_contains(civ.leader_name, "{FIRSTNAME_F}")) {
                civ.leader_name = str_replace(civ.leader_name, "{FIRSTNAME_F}", to_proper_noun_case(string_table(FIRST_NAMES_FEMALE)->random_entry(rng)));
            }
            civ.origin = loc_name;
		    glog(log_target::GAME, log_severity::info, "Welcome: {0}, lead by {1}", civ.name, civ.leader_name);
    civ.glyph = get_species_def(civ_finder->species_tag)->worldgen_glyph;

            // Appearance
            if (get_species_def(civ_finder->species_tag)->render_composite) {
                civ.skin_color = get_species_def(civ_finder->species_tag)->skin_colors[rng.roll_dice(1, static_cast<int>(get_species_def(civ_finder->species_tag)->skin_colors.size())) - 1];
                civ.hair_color = get_species_def(civ_finder->species_tag)->hair_colors[rng.roll_dice(1, static_cast<int>(get_species_def(civ_finder->species_tag)->hair_colors.size())) - 1];

                civ.hair_style = BALD;
                const int style_roll = rng.roll_dice(1, 4);
                switch (style_roll)
                {
                    case 1:
                        civ.hair_style = SHORT_HAIR;
                        break;
                    case 2:
                        civ.hair_style = LONG_HAIR;
                        break;
                    case 3:
                        civ.hair_style = PIGTAILS;
                        break;
                    case 4:
                        civ.hair_style = TRIANGLE;
                        break;
                }
            }

            planet.civs.civs.push_back(civ);

            // Place the civilization start
            planet.civs.region_info[pidx].owner_civ = i;
            planet.civs.region_info[pidx].blight_level = 0;
            planet.civs.region_info[pidx].settlement_size = 1;

            // Create an initial garrison type unit
            unit_t starter;
    starter.owner_civ = i;
            starter.unit_type = "garrison";
            starter.world_x = wx;
            starter.world_y = wy;
            planet.civs.units.push_back(starter);

            planet_display_update_zoomed(planet, WORLD_WIDTH/2, WORLD_HEIGHT/2);
        }
        }
    */
    }

}
