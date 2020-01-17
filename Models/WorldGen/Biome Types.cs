using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Models.WorldGen
{
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
            StreamReader reader = new StreamReader(@"..\..\..\Models\WorldGen\WorldGenAssets\BiomeTypes.json");
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

        private List<(double, int)> findPossibleBiomes(ref Dictionary<int, double> percents, ref List<Biome> biomes, int index)
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
        public void buildRivers(ref Map World, ref Random rng) {
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
        #region startup
        int N_CIVS = Constants.WORLD_WIDTH;
        int FIRST_NAMES_MALE = 1;
        int FIRST_NAMES_FEMALE = 2;
        int LAST_NAMES = 3;

        public HistoryMaker()
        {
            loader();
        }

        private void loader()
        {
            loadCivs();
            loadSpecies();
            loadNames(FIRST_NAMES_MALE, @"WorldGenAssets\firstNamesMale.txt");
            loadNames(FIRST_NAMES_FEMALE, @"WorldGenAssets\firstNamesFemale.txt");
            loadNames(LAST_NAMES, @"WorldGenAssets\lastNames.txt");
        }

        Dictionary<string, Civilization> civDef = new Dictionary<string, Civilization>();

        private void loadCivs()
        {
            StreamReader reader = new StreamReader(@"..\..\..\Models\WorldGen\WorldGenAssets\CivTypes.json");
            List<Civilization> temp = JsonConvert.DeserializeObject<List<Civilization>>(reader.ReadToEnd());
            foreach (Civilization species in temp)
            {
                civDef.Add(species.tag, species);
            }
        }

        Dictionary<string, rawSpecies> speciesDef = new Dictionary<string, rawSpecies>();

        private void loadSpecies()
        {
            StreamReader reader = new StreamReader(@"..\..\..\Models\WorldGen\WorldGenAssets\CivTypes.json");
            List<rawSpecies> temp = JsonConvert.DeserializeObject<List<rawSpecies>>(reader.ReadToEnd());
            foreach (rawSpecies species in temp)
            {
                speciesDef.Add(species.tag, species);
            }
        }

        Dictionary<int, stringTable> stringTables = new Dictionary<int, stringTable>();

        private void loadNames(int index, string filename)
        {
            stringTable target = new stringTable();
            StreamReader reader = new StreamReader(@"..\..\..\Models\WorldGen\" + filename);
            string line;
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (!(line.Length == 0)) target.strings.Add(line);
            }

            stringTables.Add(index, target);
        }

        #endregion

        private rawSpecies getSpeciesDef(string tag)
        {
            rawSpecies ret;
            speciesDef.TryGetValue(tag, out ret);
            return ret;
        }

        private Civilization getCivilizationDef(string tag)
        {
            Civilization ret;
            civDef.TryGetValue(tag, out ret);
            return ret;
        }

        private void findCiv(Action<Civilization> func)
        {
            foreach (string Key in civDef.Keys)
            {
                func.Invoke(civDef[Key]);
            }
        }

        private string getRandomSpecies(ref Random rng, int techLevel = 0)
        {
            List<Civilization> elegible = new List<Civilization>();
            findCiv(civ =>
            {
                if (civ.techLevel == techLevel) elegible.Add(civ);
            });
            return elegible[rng.Next(0, elegible.Count)].tag;
        }

        public void buildInitialCivs(ref Map World, ref Random rng)
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

                string locName = World.biomes[World.landBlocks[pidx].biomeIdx].name;

                civ.speciesTag = getRandomSpecies(ref rng, 0);
                civ.techLevel = 0;
                civ.extinct = false;
                civ.r = (byte)rng.Next(1, 255);
                civ.g = (byte)rng.Next(1, 255);
                civ.b = (byte)rng.Next(1, 255);
                civ.startX = wx;
                civ.startY = wy;

                // Name generation
                Civilization civFinder = getCivilizationDef(civ.speciesTag);
                civ.name = "Test of the " + locName;
                civ.leaderName = "Test Leader" + rng.Next(0,1000);;
                civ.origin = locName;
                civ.glyph = getSpeciesDef(civFinder.speciesTag).worldgenGlyph;

                World.civs.civs.Add(civ);

                // Place the civilization start
                World.civs.regionInfo[pidx].ownerCiv = i;
                World.civs.regionInfo[pidx].blightLevel = 0;
                World.civs.regionInfo[pidx].settlementSize = 1;

                // Create an initial garrison type unit
                Unit starter= new Unit();
                starter.ownerCiv = i;
                starter.unitType = "garrison";
                starter.worldX = wx;
                starter.worldY = wy;
                World.civs.units.Add(starter);
            }
        }

        private string randomUnitType(ref Civilization civ, ref Random rng) {
            List<string> available = new List<string>();
	        foreach (var it in civ.units) 
            {
                if (it.Value.tag != "garrison") available.Add(it.Key);
            }

            int roll = rng.Next(1, available.Count) - 1;
            return available[roll];
        }

        private Civ BuildCivYear(ref Map World, ref Random rng, Civ civ, int id) {

            var civF = getCivilizationDef(civ.speciesTag);
            var speciesF = getSpeciesDef(civF.speciesTag);

            // Total build points, find settlements
	        int bp = 0;
            HashSet<int> towns = new HashSet<int>();
            int i=0;
	        int unitCount = 0;
            foreach (var settlement in World.civs.regionInfo) {
                if (settlement.ownerCiv == id) {
                    bp += settlement.settlementSize*10;
                    towns.Add(i);
                }
                ++i;
            }
            foreach (Unit unit in World.civs.units) {
                if (unit.ownerCiv == id) {
                    if (civF.units.ContainsKey(unit.unitType))
                    {
                        if (civF.units[unit.unitType].bpPerTurn > 0) bp *= 2;
                        unitCount++;
                    }
                }
            }

            // If blight-spreader, then spread some blight
            if (speciesF.spreadsBlight) {
                foreach (int pidx in towns) 
                {
                    if (World.civs.regionInfo[pidx].blightLevel < 100 && bp > 10) {
                        World.civs.regionInfo[pidx].blightLevel = 100;
                        bp -= 10;
                    }
                }
            } 
            else
            {
                foreach (int pidx in towns) 
                {
                    if (World.civs.regionInfo[pidx].blightLevel > 0 && bp > 10) {
                        World.civs.regionInfo[pidx].blightLevel = 0;
                        bp -= 10;
                    }
                }
            }

            // Build improvements
            if (bp > 9 && civF.canBuild.Count > 0)
            {
                foreach (int pidx in towns) 
                {
                    if (bp > 9)
                    {
                        foreach (string build in civF.canBuild)
                        {
					        bool hasOne = false;
                            foreach (string imp in World.civs.regionInfo[pidx].improvements)
                            {
                                if (imp == build) hasOne = true;
                            }
                            if (!hasOne) {
                                bp -= 10;
                                World.civs.regionInfo[pidx].improvements.Add(build);
                            }
                        }
                    }

                    // Consider bigger towns
                    if (bp > civ.techLevel*20 && World.civs.regionInfo[pidx].settlementSize < civ.techLevel) {
                        bp -= civ.techLevel * 20;
                        World.civs.regionInfo[pidx].settlementSize++;
                    }
                }
            }

            // Tech-level improvement
            if (civF.evolvesInto.Count > 0 && bp > civ.techLevel*15 && rng.Next(1,25)==1) {
                // Evolve!
                bp = 0;
                civ.techLevel++;
                int roll = rng.Next(1, civF.evolvesInto.Count)-1;
                civ.speciesTag = civF.evolvesInto[roll];
                Civilization civ2F = getCivilizationDef(civ.speciesTag);
                civF = civ2F;
                speciesF = getSpeciesDef(civF.speciesTag);
                civ.glyph = speciesF.worldgenGlyph;
            }

            // Consider new units
            int unitCap = towns.Count + civ.techLevel + 1;
            //std::cout << "Unit count: " << unit_count << ", cap " << unit_cap << "\n";
            while (bp > 5 && unitCount < unitCap) 
            {
                Unit unit = new Unit();
                unit.ownerCiv = id;
                //std::cout << "(Civ " << id << "), " << World.civs.civs[id].species_tag << "\n";
                unit.unitType = randomUnitType(ref civF, ref rng);
                unit.worldX = civ.startX;
                unit.worldY = civ.startY;
                World.civs.units.Add(unit);
                //std::cout << "Built a new " << unit.unit_type << ", for 5 bp\n";
                bp -= 5;
                unitCount++;
            }

            // Movement goes here
            foreach (Unit unit in World.civs.units) 
            {
                if (unit.ownerCiv == id) {
                    int moves = civF.units[unit.unitType].speed;
                    while (moves > 0) {
                        HashSet<int> candidates = new HashSet<int>();
                        if (unit.worldX > 1 && World.landBlocks[World.idx(unit.worldX - 1, unit.worldY)].type != (int)blockType.WATER) candidates.Add(World.idx(unit.worldX - 1, unit.worldY));
                        if (unit.worldX < Constants.WORLD_WIDTH -2 && World.landBlocks[World.idx(unit.worldX + 1, unit.worldY)].type != (int)blockType.WATER) candidates.Add(World.idx(unit.worldX + 1, unit.worldY));
                        if (unit.worldY > 1 && World.landBlocks[World.idx(unit.worldX, unit.worldY-1)].type != (int)blockType.WATER) candidates.Add(World.idx(unit.worldX, unit.worldY - 1));
                        if (unit.worldY < Constants.WORLD_HEIGHT -2 && World.landBlocks[World.idx(unit.worldX, unit.worldY+1)].type != (int)blockType.WATER) candidates.Add(World.idx(unit.worldX, unit.worldY + 1));

                        if (candidates.Count > 0) {
                            int roll = rng.Next(1, candidates.Count)-1;
                            i=0;
                            foreach (int it in candidates) 
                            {
                                if (i == roll) {
                                    int pidx = it;
                                    unit.worldX = pidx % Constants.WORLD_WIDTH;
                                    unit.worldY = pidx / Constants.WORLD_WIDTH;
                                }
                                ++i;
                            }
                        }

                        --moves;
                    }
                }
            }
            return civ;
        }

        private void RunYear(ref Map World, ref Random rng) {
            // All civs get a turn
            int i=0;
            foreach (var civ in World.civs.civs) {
                if (!civ.extinct) {
                    World.civs.civs[i] = BuildCivYear(ref World, ref rng, civ, i);
                }
                ++i;
            }

            // Unit combat - units in the same region but of different civs kill one another

	        int killed = 0;
            for (int y=0; y< Constants.WORLD_HEIGHT; y++) 
            {
                for (int x=0; x< Constants.WORLD_WIDTH; x++) 
                {
                    int pidx = World.idx(x,y);
                    Dictionary<int, List<int>> occupants = new Dictionary<int, List<int>>();
                    i=0;
                    foreach (Unit unit in World.civs.units) {
                        if (unit.worldX == x && unit.worldY == y) {
                            occupants[unit.ownerCiv].Add(i);
                        }
                    }

                    if (occupants.Count>1) 
                    {
                        // Fight!
                        Dictionary<int, int> strengths = new Dictionary<int, int>();
				        foreach (var cit in occupants) 
                        {
					        int str = 0;
                            // Defense bonus
                            if (World.civs.regionInfo[pidx].ownerCiv == cit.Key) {
                                str += 1; // Minimal bonus for home ground
                                foreach (string imp in World.civs.regionInfo[pidx].improvements) {
                                    if (imp == "antMound") str += 2;
                                    if (imp == "antTunnel") str += 1;
                                    if (imp == "earthworks") str += 3;
                                    if (imp == "wood-pallisade") str += 5;
                                }
                            }

                            foreach (int uid in cit.Value) 
                            {
						        if (World.civs.civs.Count < cit.Key) 
                                {
							        string ut = World.civs.units[uid].unitType;
							        string st = World.civs.civs[cit.Key].speciesTag;
							        Civilization civF = getCivilizationDef(st);
							        if (civF != null) 
                                    {
								        if (civF.units.ContainsKey(ut)) {
									        str += civF.units[ut].worldgenStrength;
								        }
							        }
						        }
                            }
                            strengths[cit.Key] = str + rng.Next(2, 6);
                        }

				        int max = 0, winner = 0;
				        foreach (var it in strengths) {
                            if (it.Value > max) {
                                max = it.Value;
                                winner = it.Key;
                            }
                        }

				        foreach (var it in occupants) {
                            if (it.Key != winner) {
                                foreach (var uid in it.Value) {
                                    World.civs.units[uid].dead = true;
                                    ++killed;
                                }
                            }
                        }

                        World.civs.regionInfo[pidx].ownerCiv = winner;
                    } 
                    else if (occupants.Count > 0) 
                    {
                        int[] temp = new int[occupants.Count];
                        occupants.Keys.CopyTo(temp, 0);
                        World.civs.regionInfo[pidx].ownerCiv = temp[0];
                        if (World.civs.regionInfo[pidx].settlementSize == 0) World.civs.regionInfo[pidx].settlementSize = 1;
                    }
                }
            }
            //std::cout << "War has killed " << killed << " units this turn.\n";
            World.civs.units.RemoveAll( u => u.dead);

            // Units in an unclaimed region build there and claim it
            foreach (Unit unit in World.civs.units)
            {
                int pidx = World.idx(unit.worldX, unit.worldY);
                if (World.civs.regionInfo[pidx].ownerCiv == 0) {
                    World.civs.regionInfo[pidx].ownerCiv = unit.ownerCiv;
                    World.civs.regionInfo[pidx].settlementSize = 1;
                }
            }

            // Remove all extinct civilizations
            int I=0;
            foreach (Civ civ in World.civs.civs) {
                if (!civ.extinct) {
                    bool found = false;
                    foreach (Unit u in World.civs.units) {
                        if (u.ownerCiv == I) found = true;
                    }
                    if (!found) 
                    {
                        civ.extinct = true;
                        foreach (var t in World.civs.regionInfo) {
                            if (t.ownerCiv == I) t.ownerCiv = 0;
                        }
                    }
                }
                ++I;
            }
        }

        public void buildInitialHistory(ref Map World, ref Random rng)
        {
            int STARTING_YEAR = 2425;
            for (int year = STARTING_YEAR; year < 2525; ++year)
            {
                RunYear(ref World, ref rng);
            }
        }
    }

}
