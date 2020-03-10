using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Pixel_Engine;
using System.ComponentModel;

namespace Models.WorldGen
{
    class stringTable
    {
        public List<string> strings = new List<string>();

        public string randomEntry(ref Random rng)
        {
            int position = rng.Next(0, strings.Count);
            return strings[position];
        }
    }

    public struct lifeEvent
    {
        public readonly int year;
        public readonly string type;
        public lifeEvent(int year, string type)
        {
            this.year = year;
            this.type = type;
        }
    }

    public class History
    {
        public Dictionary<int, List<lifeEvent>> settlerLifeEvent = new Dictionary<int, List<lifeEvent>>();
    }

    public class Civ
    {
        public int techLevel = 0, glyph = 0;
        public bool extinct = false;
        public string speciesTag = "", name = "", leaderName = "", origin = "";
        public Pixel flag = Pixel.BLANK;
        public int startX = 0, startY = 0, cordexFeelings;
        public bool metCordex = false;
        public Dictionary<int, int> relations = new Dictionary<int, int>();
    }

    public class regionInfo
    {
        public int ownerCiv = 0, blightLevel = 0, settlementSize = 0, x, y;
        public List<string> improvements = new List<string>();

        public regionInfo(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Unit
    {
        public int ownerCiv = 0, worldX = 0, worldY = 0;
        public string unitType = "";
        public bool dead = false;
    }

    public class civHolder
    {
        public List<Civ> civs = new List<Civ>();
        public List<Unit> units = new List<Unit>();
        public List<regionInfo> regionInfo = new List<regionInfo>();
    }

    public class rawSpecies
    {
        public string tag = "", name = "", maleName = "", femaleName = "", collectiveName = "", description = "";
        public Dictionary<string, int> statMods = new Dictionary<string, int>();
        public List<Tuple<string, int, int>> bodyParts = new List<Tuple<string, int, int>>();
        public Diet diet = Diet.Omnivore;
        public Alignment alignment = Alignment.Neutral;
        public bool spreadsBlight = false;
        public int maxAge = 90, infantAge = 5, childAge = 12;
        public char glyph = '@', glyphAscii = '@', worldgenGlyph = '@';
        public List<Tuple<string, Pixel>> skinColors = new List<Tuple<string, Pixel>>(), hairColors = new List<Tuple<string, Pixel>>();
    };

    public class civUnitNaturalAttack
    {
        public string type = "";
        public int hitBonus = 0;
        public int Dice = 0;
        public int dieType = 6;
        public int dieMod = 0;
        public int range = 0;
    };

    public class civEquipment
    {
        public List<Tuple<int, string, string>> startingClothes = new List<Tuple<int, string, string>>();
        public string melee = "";
        public string ranged = "";
        public string ammo = "";
        public string mount = "";
    };

    public class civUnitSentient
    {
        public int count = 0;
        public int baseLevel = 0;
        public string tag = "";
        public string name = "";
        public int baseArmorClass = 10;
        public int hp = 1;
        public int hpDice = 10;
        public int hpMod = 0;
        public string gender = "male";
        public List<civUnitNaturalAttack> naturalAttacks = new List<civUnitNaturalAttack>();
        public civEquipment equipment = new civEquipment();
    };

    public class civUnit
    {
        public string tag = "";
        public int bpPerTurn = 0;
        public int speed = 0;
        public string name = "";
        public List<civUnitSentient> sentients = new List<civUnitSentient>();
        public int worldgenStrength = 1;
    };

    public class Civilization
    {
        public int techLevel = 0;
        public string tag = "";
        public string speciesTag = "";
        public string ai = "";
        public string nameGenerator = "normal";
        public Dictionary<string, civUnit> units = new Dictionary<string, civUnit>();
        public List<string> evolvesInto = new List<string>();
        public List<string> canBuild = new List<string>();
    };

    public enum Gender
    {
        Male,
        Female,
        Unknown
    }

    public enum Sexuality
    {
        Heterosexual,
        Homosexual,
        Bisexual,
        Asexual
    }

    public enum HairStyle
    {
        Bald,
        Short,
        Long,
        Pigtails,
        Mohawk,
        Balding
    }

    public class Specie
    {
        public string tag = "";
        public int index = 0;
        public Gender gender = new Gender();
        public Sexuality sexuality = new Sexuality();
        public HairStyle hairStyle = new HairStyle();
        public Tuple<string, Pixel> skinPixel = new Tuple<string, Pixel>("", new Pixel(0, 0, 0));
        public Tuple<string, Pixel> hairColor = new Tuple<string, Pixel>("", new Pixel(0, 0, 0));
        public float height = 0;
        public float weight = 0;
        public bool bearded = false;
        public int baseMaleGlyph = 0;
        public int baseFemaleGlyph = 0;

        public string proNoun
        {
            get
            {
                switch (gender)
                {
                    case Gender.Male: return "He";
                    case Gender.Female: return "She";
                    case Gender.Unknown: return "Xi";
                    default: throw new Exception();
                }
            }
        }
        public string Ethnicity
        {
            get
            {
                return skinPixel.Item1;
            }
        }
    }

    public enum Diet
    {
        Omnivore,
        Herbivore,
        Carnivore
    };

    public enum Alignment
    {
        Good,
        Neutral,
        Evil,
        Devour
    };

    public class HistoryMaker
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
            StreamReader reader = new StreamReader(@"..\..\..\Models\WorldGen\WorldGenAssets\Scentients.json");
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

        public void buildInitialCivs(World World, ref Random rng)
        {
            for (int i = 1; i < N_CIVS; ++i)
            {
                Civ civ = new Civ();

            start_over:
                int wx = rng.Next(1, Constants.WORLD_WIDTH);
                int wy = rng.Next(1, Constants.WORLD_HEIGHT);
                int pidx = World.idx(wx, wy);
                if (World.topology[pidx].type == (int)blockType.WATER) goto start_over;
                if (World.civs.regionInfo[pidx].ownerCiv > 0) goto start_over;

                string locName = World.biomes[World.topology[pidx].biomeIdx].name;

                civ.speciesTag = getRandomSpecies(ref rng, 0);
                civ.techLevel = 0;
                civ.extinct = false;
                civ.flag = new Pixel((byte)rng.Next(0, 256), (byte)rng.Next(0, 256), (byte)rng.Next(0, 256));
                civ.startX = wx;
                civ.startY = wy;

                // Name generation
                Civilization civFinder = getCivilizationDef(civ.speciesTag);
                civ.name = "Test of the " + locName;
                civ.leaderName = "Test Leader" + rng.Next(0, 1000); ;
                civ.origin = locName;
                civ.glyph = getSpeciesDef(civFinder.speciesTag).worldgenGlyph;

                World.civs.civs.Add(civ);

                // Place the civilization start
                World.civs.regionInfo[pidx].ownerCiv = i;
                World.civs.regionInfo[pidx].blightLevel = 0;
                World.civs.regionInfo[pidx].settlementSize = 1;

                // Create an initial garrison type unit
                Unit starter = new Unit();
                starter.ownerCiv = i;
                starter.unitType = "garrison";
                starter.worldX = wx;
                starter.worldY = wy;
                World.civs.units.Add(starter);
            }
        }

        private string randomUnitType(ref Civilization civ, ref Random rng)
        {
            List<string> available = new List<string>();
            foreach (var it in civ.units)
            {
                if (it.Value.tag != "garrison") available.Add(it.Key);
            }

            int roll = rng.Next(0, available.Count);
            return available[roll];
        }

        private Civ BuildCivYear(World World, ref Random rng, Civ civ, int id)
        {

            var civF = getCivilizationDef(civ.speciesTag);
            var speciesF = getSpeciesDef(civF.speciesTag);

            // Total build points, find settlements
            int bp = 0;
            HashSet<int> towns = new HashSet<int>();
            int i = 0;
            int unitCount = 0;
            foreach (var settlement in World.civs.regionInfo)
            {
                if (settlement.ownerCiv == id)
                {
                    bp += settlement.settlementSize * 10;
                    towns.Add(i);
                }
                ++i;
            }
            foreach (Unit unit in World.civs.units)
            {
                if (unit.ownerCiv == id)
                {
                    if (civF.units.ContainsKey(unit.unitType))
                    {
                        if (civF.units[unit.unitType].bpPerTurn > 0) bp *= 2;
                        unitCount++;
                    }
                }
            }

            // If blight-spreader, then spread some blight
            if (speciesF.spreadsBlight)
            {
                foreach (int pidx in towns)
                {
                    if (World.civs.regionInfo[pidx].blightLevel < 100 && bp > 10)
                    {
                        World.civs.regionInfo[pidx].blightLevel = 100;
                        bp -= 10;
                    }
                }
            }
            else
            {
                foreach (int pidx in towns)
                {
                    if (World.civs.regionInfo[pidx].blightLevel > 0 && bp > 10)
                    {
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
                            if (!hasOne)
                            {
                                bp -= 10;
                                World.civs.regionInfo[pidx].improvements.Add(build);
                            }
                        }
                    }

                    // Consider bigger towns
                    if (bp > civ.techLevel * 20 && World.civs.regionInfo[pidx].settlementSize < civ.techLevel)
                    {
                        bp -= civ.techLevel * 20;
                        World.civs.regionInfo[pidx].settlementSize++;
                    }
                }
            }

            // Tech-level improvement
            if (civF.evolvesInto.Count > 0 && bp > civ.techLevel * 15 && rng.Next(1, 26) == 1)
            {
                // Evolve!
                bp = 0;
                civ.techLevel++;
                int roll = rng.Next(0, civF.evolvesInto.Count);
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
                if (unit.ownerCiv == id)
                {
                    int moves = civF.units[unit.unitType].speed;
                    while (moves > 0)
                    {
                        HashSet<int> candidates = new HashSet<int>();
                        if (unit.worldX > 1 && World.topology[World.idx(unit.worldX - 1, unit.worldY)].type != (int)blockType.WATER) candidates.Add(World.idx(unit.worldX - 1, unit.worldY));
                        if (unit.worldX < Constants.WORLD_WIDTH - 2 && World.topology[World.idx(unit.worldX + 1, unit.worldY)].type != (int)blockType.WATER) candidates.Add(World.idx(unit.worldX + 1, unit.worldY));
                        if (unit.worldY > 1 && World.topology[World.idx(unit.worldX, unit.worldY - 1)].type != (int)blockType.WATER) candidates.Add(World.idx(unit.worldX, unit.worldY - 1));
                        if (unit.worldY < Constants.WORLD_HEIGHT - 2 && World.topology[World.idx(unit.worldX, unit.worldY + 1)].type != (int)blockType.WATER) candidates.Add(World.idx(unit.worldX, unit.worldY + 1));

                        if (candidates.Count > 0)
                        {
                            int roll = rng.Next(0, candidates.Count);
                            i = 0;
                            foreach (int it in candidates)
                            {
                                if (i == roll)
                                {
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

        public void RunYear(World World, ref Random rng)
        {
            // All civs get a turn
            int i = 0;
            for (int j = 0; j < World.civs.civs.Count; j++)
            {
                var civ = World.civs.civs[i];
                if (!civ.extinct)
                {
                    World.civs.civs[i] = BuildCivYear(World, ref rng, civ, i);
                }
                ++i;
            }

            // Unit combat - units in the same region but of different civs kill one another
            int killed = 0;
            Random rngCopy = rng;
            Parallel.For(0, Constants.WORLD_HEIGHT, (y) =>
            {
                Parallel.For(0, Constants.WORLD_WIDTH, (x) =>
                {
                    int pidx = World.idx(x, y);
                    Dictionary<int, List<int>> occupants = new Dictionary<int, List<int>>();
                    i = 0;
                    foreach (Unit unit in World.civs.units)
                    {
                        if (unit.worldX == x && unit.worldY == y)
                        {
                            if (occupants.ContainsKey(unit.ownerCiv))
                            {
                                occupants[unit.ownerCiv].Add(i);
                            }
                            else
                            {
                                occupants.Add(unit.ownerCiv, new List<int>());
                                occupants[unit.ownerCiv].Add(i);
                            }
                        }
                    }

                    if (occupants.Count > 1)
                    {
                        // Fight!
                        Dictionary<int, int> strengths = new Dictionary<int, int>();
                        foreach (var cit in occupants)
                        {
                            int str = 0;
                            // Defense bonus
                            if (World.civs.regionInfo[pidx].ownerCiv == cit.Key)
                            {
                                str += 1; // Minimal bonus for home ground
                                foreach (string imp in World.civs.regionInfo[pidx].improvements)
                                {
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
                                        if (civF.units.ContainsKey(ut))
                                        {
                                            str += civF.units[ut].worldgenStrength;
                                        }
                                    }
                                }
                            }
                            strengths[cit.Key] = str + rngCopy.Next(2, 7);
                        }

                        int max = 0, winner = 0;
                        foreach (var it in strengths)
                        {
                            if (it.Value > max)
                            {
                                max = it.Value;
                                winner = it.Key;
                            }
                        }

                        foreach (var it in occupants)
                        {
                            if (it.Key != winner)
                            {
                                foreach (var uid in it.Value)
                                {
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
                });
            });
            rng = rngCopy;
            //std::cout << "War has killed " << killed << " units this turn.\n";
            World.civs.units.RemoveAll(u => u.dead);

            // Units in an unclaimed region build there and claim it
            foreach (Unit unit in World.civs.units)
            {
                int pidx = World.idx(unit.worldX, unit.worldY);
                if (World.civs.regionInfo[pidx].ownerCiv == 0)
                {
                    World.civs.regionInfo[pidx].ownerCiv = unit.ownerCiv;
                    World.civs.regionInfo[pidx].settlementSize = 1;
                }
            }

            // Remove all extinct civilizations
            int I = 0;
            foreach (Civ civ in World.civs.civs)
            {
                if (!civ.extinct)
                {
                    bool found = false;
                    foreach (Unit u in World.civs.units)
                    {
                        if (u.ownerCiv == I) found = true;
                    }
                    if (!found)
                    {
                        civ.extinct = true;
                        foreach (var t in World.civs.regionInfo)
                        {
                            if (t.ownerCiv == I) t.ownerCiv = 0;
                        }
                    }
                }
                ++I;
            }
        }

        public void buildInitialHistory(World World, ref Random rng)
        {
            int STARTING_YEAR = 2425;
            for (int year = STARTING_YEAR; year < 2430; ++year)
            {
                RunYear(World, ref rng);
            }
        }
    }
}
