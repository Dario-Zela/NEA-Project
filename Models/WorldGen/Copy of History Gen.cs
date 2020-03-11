using System;
using System.Collections.Generic;
using Pixel_Engine;
using System.IO;
using Newtonsoft.Json;

namespace Models.WorldGen
{
    public class RegionInfo
    {
        public int OwnerCiv;
        public List<Structure> Buildings;
        public int UseableSlots;
    }

    class StringTable
    {
        public List<string> strings = new List<string>();

        public string randomEntry(ref Random rng)
        {
            int Position = rng.Next(0, strings.Count);
            return strings[Position];
        }
    }

    class History
    {
        public List<Tuple<Position, int, string>> HistoryRecord = new List<Tuple<Position, int, string>>();
    }

    class RegionModifier
    {
        public int Modifier;

        static public explicit operator RegionModifier(Terrain t)
        {
            return new RegionModifier() { Modifier = (int)((t.height * 10 + t.rainfall * 20 + t.temperature * 10) / 100f) };
        }

        public void addStructurModifier(Structure structure)
        {
            Modifier += structure.DefenceMod;
        }
    }

    class UnitType
    {
        public int DefenciveBonus;
        public int OffenciveBonus;
        public int BaseMobility;
        public bool OutDated;
        public int Cost;
        public bool CanSwim;

        public UnitType(int DefenciveBonus, int OffenciveBonus, int BaseMobility, int Cost, bool CanSwim)
        {
            this.BaseMobility = BaseMobility;
            this.DefenciveBonus = DefenciveBonus;
            this.OffenciveBonus = OffenciveBonus;
            this.Cost = Cost;
            OutDated = false;
            this.CanSwim = true;
        }
    }

    class Unit
    {
        public Position Position;
        public RegionModifier CurrentPosition;
        private int DefenciveBonus;
        private int OffenciveBonus;
        private int BaseMobility;
        private int Supplies;
        private int Experience;
        private int RemainingUnits;
        private int Mobility;
        public bool CanSwim;

        public Unit(UnitType Division, Position Pos)
        {
            Position = Pos;
            DefenciveBonus = Division.DefenciveBonus;
            OffenciveBonus = Division.OffenciveBonus;
            BaseMobility = Division.BaseMobility;
            CanSwim = Division.CanSwim;
            RemainingUnits = 100;
            Supplies = 100;
            Experience = 0;
        }

        public void Upgrade(UnitType Division)
        {
            DefenciveBonus = Division.DefenciveBonus;
            OffenciveBonus = Division.OffenciveBonus;
            BaseMobility = Division.BaseMobility;
            CanSwim = Division.CanSwim;
            Experience = Experience < 20 ? 0 : Experience - 20;
        }

        public void Train()
        {
            Supplies -= 50;
            Experience += 25;
        }

        public void Resupply(int Value)
        {
            Supplies += Value;
        }

        static public bool operator +(Unit Attacker, Unit Defender)
        {
            if (Attacker.OffenciveBonus > Defender.DefenciveBonus)
            {
                int BaseDamage = (Attacker.OffenciveBonus - Defender.DefenciveBonus) * new Random().Next(8);
                Defender.RemainingUnits = (int)(BaseDamage - (float)(Defender.Mobility + Defender.Supplies + Defender.Experience) / 100);
                Defender.Supplies -= BaseDamage / 10;
                return true;
            }
            else
            {
                int BaseDamage = (Attacker.OffenciveBonus - Defender.DefenciveBonus) * new Random().Next(8);
                Attacker.RemainingUnits = (int)(BaseDamage - (float)(Attacker.Supplies + Attacker.Experience) / 100);
                Attacker.Supplies -= BaseDamage / 10;
                return false;
            }
        }

        public void Moved(RegionModifier NewPosition)
        {
            CurrentPosition = NewPosition;
            Mobility = BaseMobility - CurrentPosition.Modifier;
        }
    }

    public class Structure
    {
        public int DefenceMod;
        public int SupplyGen;
        public int ResourceGrowth;
        public int Cost;
    }

    struct Position
    {
        Tuple<int,int> Pos;

        public Position(int x, int y)
        {
            Pos = new Tuple<int,int>(x,y);
        }

        public int x {get{return Pos.Item1;}}

        public int y {get{return Pos.Item2;}}
    }

    class Civilization
    {
        public int[] TechLevel;
        public int[] TechGrowth;
        public string CivName;
        public Specie Race;
        public int[] Ai;
        public List<Unit> Army;
        public List<UnitType> PossibleDivisions;
        public List<Structure> PossibleBuildings;
        public int ResourcesPerTurn;
        public int SupplyGen;
        public List<Civilization> Allies;
        public Position Capital;
        public Pixel flag;
        public History History;

        public Civilization()
        {
            PossibleDivisions = new List<UnitType>();
            TechLevel = new int[3];
            TechGrowth = new int[3];
            CivName = "";
            Race = new Specie();
            Ai = new int[3];
            Army = new List<Unit>();
            PossibleBuildings = new List<Structure>();
            ResourcesPerTurn = 0;
            SupplyGen = 0;
            Capital = new Position();
            flag = new Pixel(new Random().Next(int.MaxValue));
            flag.A = 255;
            History = new History();
        }

        public void AddStructure(Structure s)
        {
            SupplyGen += s.SupplyGen;
            ResourcesPerTurn += s.ResourceGrowth;
        }
    }

    class Specie
    {
        public int[] Sprites;
        public string Name;

        public int BaseMilitaryTechGrowth;
        public int BaseAgricolturalTechGrowth;
        public int BaseCivilianTechGrowth;
        public int BaseAggressiveness;
        public int BaseResearchFocus;
        public int BaseResourceFoucus;
    }

    public class HistoryGen
    {
        #region Loader
        List<Specie> Species;
        Dictionary<int, StringTable> StringTables = new Dictionary<int, StringTable>();
        Dictionary<string, Civilization> CivDef = new Dictionary<string, Civilization>();
        public HistoryGen()
        {
            loader();
        }
        private void loader()
        {
            LoadSpecies();
            loadNames(1, @"WorldGenAssets\firstNamesMale.txt");
            loadNames(2, @"WorldGenAssets\firstNamesFemale.txt");
            loadNames(3, @"WorldGenAssets\lastNames.txt");
        }
        private void LoadSpecies()
        {
            StreamReader reader = new StreamReader(@"..\..\..\Models\WorldGen\WorldGenAssets\Species.json");
            List<Specie> temp = JsonConvert.DeserializeObject<List<Specie>>(reader.ReadToEnd());
            foreach (Specie species in temp)
            {
                Species.Add(species);
            }
        }
        private void loadNames(int index, string filename)
        {
            StringTable target = new StringTable();
            StreamReader reader = new StreamReader(@"..\..\..\Models\WorldGen\" + filename);
            string line;
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (!(line.Length == 0)) target.strings.Add(line);
            }

            StringTables.Add(index, target);
        }
        #endregion

        public void BuildInitialCivs(World World, ref Random rng)
        {
            Structure TownHall = new Structure();
            TownHall.DefenceMod = 20;
            TownHall.ResourceGrowth = 100;
            TownHall.SupplyGen = 50;

            for (int i = 0; i < Constants.WORLD_WIDTH; i++)
            {
                Civilization civ = new Civilization();

                retry:
                    int WorldX = rng.Next(Constants.WORLD_WIDTH);
                    int WorldY = rng.Next(Constants.WORLD_HEIGHT);
                    int Idx = World.idx(WorldX, WorldY);
                    if (World.topology[Idx].type == (int)blockType.WATER || World.civs.regionInfo[Idx].ownerCiv != 0) goto retry;

                civ.Capital = new Position(WorldX, WorldY);
                World.RegionInfos[Idx].OwnerCiv = i;
                World.RegionInfos[Idx].Buildings.Add(TownHall);
                civ.AddStructure(TownHall);
                civ.Ai = new[] { civ.Race.BaseAggressiveness, civ.Race.BaseResearchFocus, civ.Race.BaseResourceFoucus };
                civ.TechLevel = new[] { civ.Race.BaseMilitaryTechGrowth, civ.Race.BaseCivilianTechGrowth, civ.Race.BaseAgricolturalTechGrowth };

            }
        }
    }
}
