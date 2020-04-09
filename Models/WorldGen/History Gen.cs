using System;
using System.Collections.Generic;
using Pixel_Engine;
using System.IO;
using Newtonsoft.Json;
using Models.Algorithms;

namespace Models.WorldGen
{
    public class RegionInfo
    {
        public int OwnerCiv;
        public List<Structure> MilitaryBuildings = new List<Structure>();
        public List<Structure> ResearchBuildings = new List<Structure>();
        public List<Structure> EconomicBuildings = new List<Structure>();
        public List<Structure> DefenceBuildings = new List<Structure>();
        public List<Structure> Buildings
        {
            get
            {
                List<Structure> ToReturn = new List<Structure>(MilitaryBuildings);
                ToReturn.AddRange(ResearchBuildings);
                ToReturn.AddRange(EconomicBuildings);
                ToReturn.AddRange(DefenceBuildings);
                return ToReturn;
            }
        }
        public int DefenceMod
        {
            get
            {
                int DefenceMod = 0;
                foreach (Structure structure in Buildings)
                {
                    DefenceMod += structure.DefenceMod;
                }
                return DefenceMod;
            }
        }
        public int CityLevel = 0;
        public int UseableSlots;
        public List<Unit> UnitsInTheCity = new List<Unit>();
        public bool FullControl
        {
            get
            {
                foreach (Unit unit in UnitsInTheCity)
                {
                    if(unit.CivTag != CityLevel)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public Tuple<int,Structure> Upgrade(Structure[] Upgrades, int[] CurrentTechLevel)
        {
            foreach (Structure structure in MilitaryBuildings)
            {
                if (structure.TechLevel < CurrentTechLevel[0] && structure.TechLevel != -1)
                {
                    Tuple<int,Structure> value = new Tuple<int, Structure>(0, structure);
                    structure.Upgrade(Upgrades[0]);
                    return value;
                }
            }
            foreach (Structure structure in ResearchBuildings)
            {
                if (structure.TechLevel < CurrentTechLevel[1] && structure.TechLevel != -1)
                {
                    Tuple<int, Structure> value = new Tuple<int, Structure>(1, structure);
                    structure.Upgrade(Upgrades[1]);
                    return value;
                }
            }
            foreach (Structure structure in EconomicBuildings)
            {
                if (structure.TechLevel < CurrentTechLevel[2] && structure.TechLevel != -1)
                {
                    Tuple<int, Structure> value = new Tuple<int, Structure>(2, structure);
                    structure.Upgrade(Upgrades[2]);
                    return value;
                }
            }
            foreach (Structure structure in DefenceBuildings)
            {
                if (structure.TechLevel < CurrentTechLevel[3] && structure.TechLevel != -1)
                {
                    Tuple<int, Structure> value = new Tuple<int, Structure>(3, structure);
                    structure.Upgrade(Upgrades[3]);
                    return value;
                }
            }
            return new Tuple<int, Structure>(0, null);
        }
        public List<Tuple<int, string>> History = new List<Tuple<int, string>>();
        public Position pos;
        public string Name = "";
        public void DestroyBuilding(ref Random rng)
        {
            if (Buildings.Count == 0) return;
            Structure ToRemove = Buildings[rng.Next(0, Buildings.Count - 1)];
            if (MilitaryBuildings.Remove(ToRemove)) return;
            if (EconomicBuildings.Remove(ToRemove)) return;
            if (ResearchBuildings.Remove(ToRemove)) return;
            if (DefenceBuildings.Remove(ToRemove)) return;
        }
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

    public class TerrainModifier
    {
        public int Modifier;

        static public explicit operator TerrainModifier(Terrain t)
        {
            return new TerrainModifier() { Modifier = (int)((t.height * 10 + t.rainfall * 20 + t.temperature * 10) / 100f) };
        }
    }

    public class UnitType
    {
        public int DefenciveBonus;
        public int OffenciveBonus;
        public int BaseMobility;
        public int Cost;
        public bool CanSwim;
        public int SupplyCost;
        public int UnitClass;

        public UnitType(int DefenciveBonus, int OffenciveBonus, int BaseMobility, int Cost, bool CanSwim, int SupplyCost, int UnitClass)
        {
            this.BaseMobility = BaseMobility;
            this.DefenciveBonus = DefenciveBonus;
            this.OffenciveBonus = OffenciveBonus;
            this.Cost = Cost;
            this.CanSwim = CanSwim;
            this.SupplyCost = SupplyCost;
            this.UnitClass = UnitClass;
        }

        static public UnitType CreateNewUnit(UnitType OldModel, int Class, int TechLevel, ref Random rng)
        {
            switch (Class)
            {
                case 0:
                    return new UnitType(OldModel.DefenciveBonus + rng.Next(2, 5), OldModel.OffenciveBonus + rng.Next(4, 10), OldModel.BaseMobility + rng.Next(-1, 3),
                        OldModel.Cost + rng.Next(5, 20), TechLevel > 9 ? rng.Next(0, 6) < 1 : false, OldModel.SupplyCost + rng.Next(4, 7), Class);
                case 1:
                    return new UnitType(OldModel.DefenciveBonus + rng.Next(4, 10), OldModel.OffenciveBonus + rng.Next(2, 5), OldModel.BaseMobility + rng.Next(-1, 3),
                        OldModel.Cost + rng.Next(5, 20), TechLevel > 9 ? rng.Next(0, 6) < 1 : false, OldModel.SupplyCost + rng.Next(4, 7), Class);
                case 2:
                    return new UnitType(OldModel.DefenciveBonus + rng.Next(-1, 3), OldModel.OffenciveBonus + rng.Next(2, 5), OldModel.BaseMobility + rng.Next(4, 10),
                        OldModel.Cost + rng.Next(5, 20), TechLevel > 9 ? rng.Next(0, 6) < 1 : false, OldModel.SupplyCost + rng.Next(4, 7), Class);
            }
            return null;
        }
    }

    public class Unit
    {
        public Position Position;
        public TerrainModifier CurrentPosition;
        private int BaseDefenciveBonus;
        public int OffenciveBonus;
        public int BaseMobility;
        private int Supplies;
        private int Experience;
        private int RemainingUnits;
        private int Mobility;
        public bool CanSwim;
        private int SupplyCost;
        public int UnitClass;
        public int LastResupply;
        public int DefenciveBonus;
        public int CivTag;

        public Unit(UnitType Division, Position Pos, Terrain CurPos, RegionInfo region, int CivTag)
        {
            BaseDefenciveBonus = Division.DefenciveBonus;
            OffenciveBonus = Division.OffenciveBonus;
            BaseMobility = Division.BaseMobility;
            CanSwim = Division.CanSwim;
            Moved(CurPos, Pos, region);
            RemainingUnits = 100;
            Supplies = 100;
            Experience = 0;
            SupplyCost = Division.SupplyCost;
            UnitClass = Division.UnitClass;
            LastResupply = 100;
            this.CivTag = CivTag;
        }

        public bool NeedUnits()
        {
            return RemainingUnits < 80;
        }

        public void Upgrade(UnitType Division)
        {
            BaseDefenciveBonus = Division.DefenciveBonus;
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

        public void Resupply(int Supplies)
        {
            this.Supplies += Supplies;
            LastResupply = Supplies;
        }

        public void AddUnits(int Manpower)
        {
            RemainingUnits += Manpower;
            Experience = Experience < 20 ? 0 : Experience - 20;
        }

        static public bool operator +(Unit Attacker, Unit Defender)
        {
            int BaseDamage = (Attacker.OffenciveBonus - Defender.DefenciveBonus) * new Random().Next(8);
            Defender.RemainingUnits -= (int)(BaseDamage - (float)(Defender.Mobility + Defender.Supplies + Defender.Experience + Defender.CurrentPosition.Modifier) / 100);
            Defender.Supplies -= BaseDamage / 10 + Defender.SupplyCost;
            Attacker.RemainingUnits -= (int)(BaseDamage - (float)(Attacker.Supplies + Attacker.Experience - Defender.CurrentPosition.Modifier) / 100);
            Attacker.Supplies -= BaseDamage / 10 + Attacker.SupplyCost;
            return true;
        }

        public void Moved(Terrain NewPosition, Position NewPos, RegionInfo region)
        {
            Position = NewPos;
            CurrentPosition = (TerrainModifier)NewPosition;
            Mobility = BaseMobility - CurrentPosition.Modifier;
            DefenciveBonus = BaseDefenciveBonus + region.DefenceMod;
        }

        static public Predicate<Unit> isDefeated = new Predicate<Unit>((unit) =>
        {
            if (unit.Supplies < 0) unit.RemainingUnits -= 30;
            return unit.RemainingUnits <= 0;
        });
    }

    public class Structure
    {
        public int DefenceMod;
        public int SupplyGen;
        public int ResourceGrowth;
        public int[] ResearchGrowth;
        public int ManpowerGrowth;
        public int Cost;
        public int TechLevel;

        public void Upgrade(Structure structure)
        {
            DefenceMod = structure.DefenceMod;
            SupplyGen = structure.SupplyGen;
            ResourceGrowth = structure.ResourceGrowth;
            ResearchGrowth = structure.ResearchGrowth;
            ManpowerGrowth = structure.ManpowerGrowth;
            Cost = structure.Cost;
            TechLevel = structure.TechLevel;
        }

        static public Structure CreateNewStructure(Structure OldModel, int Class, ref Random rng)
        {
            switch (Class)
            {
                case 0:
                    OldModel.Cost += rng.Next(15, 30);
                    OldModel.DefenceMod += rng.Next(2, 5);
                    OldModel.ManpowerGrowth += rng.Next(50, 100);
                    OldModel.ResourceGrowth += rng.Next(-30, -10);
                    OldModel.SupplyGen += rng.Next(-20, -10);
                    OldModel.ResearchGrowth[0] += rng.Next(5, 15);
                    OldModel.ResearchGrowth[1] += rng.Next(0, 5);
                    OldModel.ResearchGrowth[2] += 0;
                    OldModel.TechLevel++;
                    return OldModel;
                case 1:
                    OldModel.Cost += rng.Next(15, 30);
                    OldModel.DefenceMod += rng.Next(-1, 3);
                    OldModel.ManpowerGrowth += rng.Next(-20, -10);
                    OldModel.ResourceGrowth += rng.Next(-30, -10);
                    OldModel.SupplyGen += rng.Next(-20, -10);
                    OldModel.ResearchGrowth[0] += rng.Next(15, 25);
                    OldModel.ResearchGrowth[1] += rng.Next(30, 40);
                    OldModel.ResearchGrowth[2] += rng.Next(15, 25);
                    OldModel.TechLevel++;
                    return OldModel;
                case 2:
                    OldModel.Cost += rng.Next(15, 30);
                    OldModel.DefenceMod += rng.Next(-1, 3);
                    OldModel.ManpowerGrowth += rng.Next(-10, 10);
                    OldModel.ResourceGrowth += rng.Next(30, 100);
                    OldModel.SupplyGen += rng.Next(30, 50);
                    OldModel.ResearchGrowth[0] += 0;
                    OldModel.ResearchGrowth[1] += rng.Next(0, 5);
                    OldModel.ResearchGrowth[2] += rng.Next(5, 15);
                    OldModel.TechLevel++;
                    return OldModel;
                case 3:
                    OldModel.Cost += rng.Next(15, 30);
                    OldModel.DefenceMod += rng.Next(10, 20);
                    OldModel.ManpowerGrowth += rng.Next(-20, 10);
                    OldModel.ResourceGrowth += rng.Next(-30, -10);
                    OldModel.SupplyGen += rng.Next(-20, -10);
                    OldModel.ResearchGrowth[0] += rng.Next(5, 15);
                    OldModel.ResearchGrowth[1] += rng.Next(0, 5);
                    OldModel.ResearchGrowth[2] += 0;
                    OldModel.TechLevel++;
                    return OldModel;
            }
            return null;
        }
    }

    public struct Position
    {
        Tuple<int,int> Pos;

        public Position(int x, int y)
        {
            Pos = new Tuple<int,int>(x,y);
        }

        public int x {get{return Pos.Item1;}}

        public int y {get{return Pos.Item2;}}
    }

    public class Civilization
    {
        public string Name;
        public int[] TechLevel;
        public int[] TechGrowth;
        public string CivName;
        public Specie Race;
        public int[] Ai;
        public List<Unit> Army;
        public UnitType[] PossibleDivisions;
        public Structure[] PossibleBuildings;
        public int ResourceGen;
        public int SupplyGen;
        public Position Capital;
        public Pixel Flag;
        public List<Tuple<Position, int, string>> History;
        public int Tag;
        public List<RegionInfo> Land;
        public List<RegionInfo> BuildableLand
        {
            get
            {
                List<RegionInfo> toRet = new List<RegionInfo>();
                foreach (RegionInfo region in Land)
                {
                    if (region.UseableSlots != 0)
                    {
                        toRet.Add(region);
                    }
                }
                return toRet;
            }
        }
        public int ManpowerGrowth;
        public Tuple<int,int> LeastCost
        {
            get
            {
                int pos = -1;
                int Cost = int.MaxValue;
                int Count = 0;
                foreach (Structure structure in PossibleBuildings)
                {
                    if (structure.Cost < Cost)
                    {
                        Cost = structure.Cost;
                        pos = Count;
                    }
                    Count++;
                }
                return new Tuple<int, int>(Cost, pos);
            }
        }
        public HashSet<RegionInfo> RequestedDefence;
        public HashSet<RegionInfo> RequestedUpgreades;
        public int MaxDevelopment
        {
            get
            {
                int Development = -1;
                foreach (RegionInfo region in Land)
                {
                    if(region.CityLevel > Development)
                    {
                        Development = region.CityLevel;
                    }
                }
                return Development;
            }
        }
        public int[] TechProgress;
        public List<Unit> UnitsToUpgrade;

        public Civilization()
        {
            PossibleDivisions = new UnitType[3];
            TechLevel = new int[3];
            TechGrowth = new int[3];
            TechProgress = new int[3];
            CivName = "";
            Race = new Specie();
            Ai = new int[3];
            Army = new List<Unit>();
            PossibleBuildings = new Structure[4];
            ResourceGen = 0;
            SupplyGen = 0;
            Capital = new Position();
            History = new List<Tuple<Position, int, string>>();
            Land = new List<RegionInfo>();
            ManpowerGrowth = 0;
            RequestedDefence = new HashSet<RegionInfo>();
            UnitsToUpgrade = new List<Unit>();
            RequestedUpgreades = new HashSet<RegionInfo>();
        }

        public void AddStructure(Structure NewStructure)
        {
            SupplyGen += NewStructure.SupplyGen;
            ResourceGen += NewStructure.ResourceGrowth;
            ManpowerGrowth += NewStructure.ManpowerGrowth;
            for (int i = 0; i < 3; i++)
            {
                TechGrowth[i] += NewStructure.ResearchGrowth[i];
            }
        }

        public void UpgradeStructure(Tuple<int, Structure> TypeOldStructPair)
        {
            Structure NewStructure = PossibleBuildings[TypeOldStructPair.Item1];
            Structure OldStructure = TypeOldStructPair.Item2;
            SupplyGen += NewStructure.SupplyGen - OldStructure.SupplyGen;
            ResourceGen += NewStructure.ResourceGrowth - OldStructure.ResourceGrowth;
            ManpowerGrowth += NewStructure.ManpowerGrowth - OldStructure.ManpowerGrowth;
            for (int i = 0; i < 3; i++)
            {
                TechGrowth[i] += NewStructure.ResearchGrowth[i] - OldStructure.ResearchGrowth[i];
            }
        }

        public void AddLand(ref RegionInfo region)
        {
            Land.Add(region);
            foreach(Structure stucture in region.Buildings)
            {
                AddStructure(stucture);
            }
            ManpowerGrowth += 100;
        }

        static public Predicate<Civilization> isExtinct = new Predicate<Civilization>((civ) =>
        {
            return civ.Land.Count == 0;
        });
    }

    public class Specie
    {
        public int[] Sprites;
        public string Name;

        public int BaseMilitaryTechGrowth;
        public int BaseEconomicalTechGrowth;
        public int BaseResearchTechGrowth;
        public int BaseAggressiveness;
        public int BaseResearchFocus;
        public int BaseEconomicFoucus;
    }

    public class HistoryGen
    {
        #region Loader
        List<Specie> Species = new List<Specie>();
        Dictionary<int, StringTable> StringTables = new Dictionary<int, StringTable>();
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
            loadNames(4, @"WorldGenAssets\EmpireNames.txt");
            loadNames(5, @"WorldGenAssets\CityNames.txt");
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
            Structure TownHall = new Structure() { DefenceMod = 20, ResourceGrowth = 100, SupplyGen = 50, ResearchGrowth = new[] { 0, 0, 0 }, ManpowerGrowth = 100, TechLevel = -1};
            Structure StarterEconomicBuilding = new Structure() { Cost = 20, DefenceMod = -3, ManpowerGrowth = 0, ResearchGrowth = new[] { 0, 1, 3 }, ResourceGrowth = 30, SupplyGen = 20, TechLevel = 0 };
            Structure StarterMilitaryBuilding = new Structure() { Cost = 30, DefenceMod = 4, ManpowerGrowth = 100, ResearchGrowth = new[] { 3, 1, 0 }, ResourceGrowth = -10, SupplyGen = -5, TechLevel = 0 };
            Structure StarterResearchBuilding = new Structure() { Cost = 20, DefenceMod = -3, ManpowerGrowth = 0, ResearchGrowth = new[] { 15, 20, 15 }, ResourceGrowth = -10, SupplyGen = 0, TechLevel = 0 };
            Structure StarterDefenceBuilding = new Structure() { Cost = 20, DefenceMod = 10, ManpowerGrowth = 3, ResearchGrowth = new[] { 3, 1, 0 }, ResourceGrowth = -5, SupplyGen = 0, TechLevel = 0 };

            UnitType StarterAttackUnit = new UnitType(5, 15, 5, 30, false, 10, 0);
            UnitType StarterDefenceUnit = new UnitType(15, 5, 5, 30, false, 10, 1);
            UnitType StarterMobileUnit = new UnitType(5, 5, 15, 30, false, 10, 2);

            for (int i = 0; i < Constants.WORLD_WIDTH / 8; i++)
            {
                Civilization civ = new Civilization();

                retry:
                    int WorldX = rng.Next(Constants.WORLD_WIDTH);
                    int WorldY = rng.Next(Constants.WORLD_HEIGHT);
                    int Idx = World.idx(WorldX, WorldY);
                    if (World.topology[Idx].type == (int)blockType.WATER || World.RegionInfos[Idx].OwnerCiv != 0) goto retry;

                civ.Race = Species[rng.Next(0, Species.Count - 1)];
                civ.Capital = new Position(WorldX, WorldY);
                World.RegionInfos[Idx].OwnerCiv = i;
                World.RegionInfos[Idx].EconomicBuildings.Add(TownHall);
                World.RegionInfos[Idx].UseableSlots--;
                civ.AddStructure(TownHall);
                civ.Flag = new Pixel(rng.Next(int.MaxValue));
                civ.Flag.A = 255;
                civ.Ai = new[] { civ.Race.BaseAggressiveness + World.topology[Idx].savagery, civ.Race.BaseResearchFocus - World.topology[Idx].savagery, civ.Race.BaseEconomicFoucus - World.topology[Idx].temperature };
                civ.TechGrowth = new[] { civ.Race.BaseMilitaryTechGrowth + World.topology[Idx].savagery, civ.Race.BaseResearchTechGrowth - World.topology[Idx].savagery, civ.Race.BaseEconomicalTechGrowth - World.topology[Idx].temperature };
                civ.Tag = i;
                civ.Name = CreateEmpireName(ref rng, civ, World.topology[Idx]);
                civ.PossibleBuildings = new[] { StarterMilitaryBuilding, StarterResearchBuilding, StarterEconomicBuilding, StarterDefenceBuilding };
                civ.PossibleDivisions = new[] { StarterAttackUnit, StarterDefenceUnit, StarterMobileUnit };
                civ.AddLand(ref World.RegionInfos[Idx]);
                World.civs.Add(civ);
            }
        }
        public void RunYear(World World, Random rng)
        {
            List<RegionInfo> RegionsOfConflict = new List<RegionInfo>();
            foreach(Civilization civ in World.civs)
            {
                int Supplies = civ.SupplyGen;
                int Resources = civ.ResourceGen;
                Func<RegionInfo[], Node[]> Conv = new Func<RegionInfo[], Node[]>((regions) =>
                {
                    int MapWidth = Constants.WORLD_WIDTH;
                    int MapHeight = Constants.WORLD_HEIGHT;
                    Node[] nodes = new Node[regions.Length];
                    for (int i = 0; i < regions.Length; i++)
                    {
                        RegionInfo region = regions[i];
                        nodes[i] = new Node()
                        {
                            x = i / MapWidth,
                            y = i % MapWidth,
                            Obstacle = region.OwnerCiv != civ.Tag,
                        };
                    }

                    for (int x = 0; x < MapWidth; x++)
                        for (int y = 0; y < MapHeight; y++)
                        {
                            if (y > 0)
                            {
                                if (x > 0)
                                {
                                    nodes[y * MapWidth + x].Neighbours.Add(nodes[(y - 1) * MapWidth + (x - 1)]);
                                }
                                if (x < MapWidth - 1)
                                {
                                    nodes[y * MapWidth + x].Neighbours.Add(nodes[(y - 1) * MapWidth + (x + 1)]);
                                }
                                nodes[y * MapWidth + x].Neighbours.Add(nodes[(y - 1) * MapWidth + x]);
                            }
                            if (y < MapHeight - 1)
                            {
                                if (x > 0)
                                {
                                    nodes[y * MapWidth + x].Neighbours.Add(nodes[(y + 1) * MapWidth + (x - 1)]);
                                }
                                if (x < MapWidth - 1)
                                {
                                    nodes[y * MapWidth + x].Neighbours.Add(nodes[(y + 1) * MapWidth + (x + 1)]);
                                }
                                nodes[y * MapWidth + x].Neighbours.Add(nodes[(y + 1) * MapWidth + x]);
                            }
                            if (x > 0)
                            {
                                nodes[y * MapWidth + x].Neighbours.Add(nodes[y * MapWidth + (x - 1)]);
                            }
                            if (x < MapWidth - 1)
                            {
                                nodes[y * MapWidth + x].Neighbours.Add(nodes[y * MapWidth + (x + 1)]);
                            }
                        }
                    return nodes;
                });
                AStar<RegionInfo> PathFinder = new AStar<RegionInfo>(World.RegionInfos, Constants.WORLD_WIDTH, Constants.WORLD_HEIGHT, Conv,
                    World.idx(civ.Capital.x, civ.Capital.y), 0);
                int SupplyDefecit = 0;
                for (int i = 0; i < 3; i++)
                {
                    int TechGrowth = civ.TechGrowth[i];
                    civ.TechProgress[i] += TechGrowth;
                    if (civ.TechProgress[i] > (civ.TechLevel[i] + 1) * 1000)
                    {
                        civ.TechProgress[i] = 0;
                        civ.TechLevel[i]++;
                        switch (i)
                        {
                            case 0:
                                switch (rng.Next(0, 2))
                                {
                                    case 0:
                                        int Class = rng.Next(0, 3);
                                        civ.PossibleDivisions[Class] = UnitType.CreateNewUnit(civ.PossibleDivisions[Class], Class, civ.TechLevel[i], ref rng);
                                        foreach (Unit unit in civ.Army)
                                        {
                                            if (unit.UnitClass == Class)
                                            {
                                                civ.UnitsToUpgrade.Add(unit);
                                            }
                                        }
                                        break;
                                    case 1:
                                        if (rng.Next(0, 2) == 0)
                                        {
                                            civ.PossibleBuildings[0] = Structure.CreateNewStructure(civ.PossibleBuildings[0], 0, ref rng);
                                            foreach (RegionInfo region in civ.Land)
                                            {
                                                civ.RequestedUpgreades.Add(region);
                                            }
                                        }
                                        else
                                        {
                                            civ.PossibleBuildings[3] = Structure.CreateNewStructure(civ.PossibleBuildings[3], 3, ref rng);
                                            foreach (RegionInfo region in civ.Land)
                                            {
                                                civ.RequestedUpgreades.Add(region);
                                            }
                                        }
                                        break;
                                }
                                break;
                            default:
                                civ.PossibleBuildings[i] = Structure.CreateNewStructure(civ.PossibleBuildings[i], i, ref rng);
                                foreach (RegionInfo region in civ.Land)
                                {
                                    civ.RequestedUpgreades.Add(region);
                                }
                                break;
                        }
                    }
                }
                foreach (Unit unit in civ.Army)
                {
                    int EndLoc = World.idx(unit.Position.x, unit.Position.y);
                    PathFinder.EditEnd(EndLoc);
                    float Distance = 1;
                    Distance = PathFinder.BestDistance;
                    unit.Resupply((int)(Supplies / civ.Army.Count / Distance));
                    if (unit.NeedUnits()) unit.AddUnits((int)(civ.ManpowerGrowth / civ.Army.Count / Distance));
                    SupplyDefecit = (int)(Supplies / civ.Army.Count / Distance);
                }
                Supplies -= SupplyDefecit;
                if (rng.Next(0, 100) < (SupplyDefecit / (float)civ.SupplyGen + 1f / civ.Army.Count) * 5 * civ.Ai[0] * 0.05f)
                {
                    UnitType chosen = civ.PossibleDivisions[rng.Next(0, civ.PossibleDivisions.Length - 1)];
                    civ.Army.Add(new Unit(chosen, civ.Capital, World.topology[World.idx(civ.Capital.x, civ.Capital.y)],
                        World.RegionInfos[World.idx(civ.Capital.x, civ.Capital.y)], civ.Tag));
                    Resources -= chosen.Cost;
                }
                foreach (RegionInfo region in civ.RequestedDefence)
                {
                    if (Resources > civ.ResourceGen * 3 / 4)
                    {
                        region.DefenceBuildings.Add(civ.PossibleBuildings[3]);
                        region.UseableSlots--;
                        Resources -= civ.PossibleBuildings[3].Cost;
                        civ.AddStructure(civ.PossibleBuildings[3]);
                    }
                }
                List<RegionInfo> Upgraded = new List<RegionInfo>();
                foreach (RegionInfo region in civ.RequestedUpgreades)
                {
                    if (Resources > civ.ResourceGen / 2)
                    {
                        var temp = region.Upgrade(civ.PossibleBuildings, civ.TechLevel);
                        if (temp.Item1 == 0)
                        {
                            Upgraded.Add(region);
                            break;
                        }
                        Resources -= civ.PossibleBuildings[temp.Item1].Cost;
                        civ.UpgradeStructure(temp);
                    }
                }
                foreach (RegionInfo region in Upgraded)
                {
                    civ.RequestedUpgreades.Remove(region);
                }
                List<Unit> UgradedUnits = new List<Unit>();
                foreach (Unit unit in civ.UnitsToUpgrade)
                {
                    if (Resources > civ.ResourceGen / 4)
                    {
                        unit.Upgrade(civ.PossibleDivisions[unit.UnitClass]);
                        Resources -= civ.PossibleDivisions[unit.UnitClass].Cost;
                    }
                }
                while (Resources > civ.LeastCost.Item1)
                {
                    if (civ.BuildableLand.Count == 0) break;
                    retry:
                    RegionInfo r = civ.BuildableLand[rng.Next(0, civ.BuildableLand.Count - 1)];
                    if (rng.Next(0, civ.MaxDevelopment) < r.CityLevel) goto retry;
                    if (rng.Next(0, 100) < civ.Ai[0] && civ.PossibleBuildings[0].Cost < Resources)
                    {
                        Structure NewBuilding = civ.PossibleBuildings[0];
                        civ.AddStructure(NewBuilding);
                        Resources -= NewBuilding.Cost;
                        r.MilitaryBuildings.Add(NewBuilding);
                        r.UseableSlots--;

                    }
                    else if (rng.Next(0, 100) < civ.Ai[1] && civ.PossibleBuildings[1].Cost < Resources)
                    {
                        Structure NewBuilding = civ.PossibleBuildings[1];
                        civ.AddStructure(NewBuilding);
                        Resources -= NewBuilding.Cost;
                        r.ResearchBuildings.Add(NewBuilding);
                        r.UseableSlots--;
                    }
                    else if (civ.PossibleBuildings[2].Cost < Resources)
                    {
                        Structure NewBuilding = civ.PossibleBuildings[2];
                        civ.AddStructure(NewBuilding);
                        Resources -= NewBuilding.Cost;
                        r.EconomicBuildings.Add(NewBuilding);
                        r.UseableSlots--;
                    }
                    else
                    {
                        Structure NewBuilding = civ.PossibleBuildings[civ.LeastCost.Item2];
                        civ.AddStructure(NewBuilding);
                        Resources -= NewBuilding.Cost;
                        switch (civ.LeastCost.Item2)
                        {
                            case 0:
                                r.MilitaryBuildings.Add(NewBuilding);
                                break;
                            case 1:
                                r.ResearchBuildings.Add(NewBuilding);
                                break;
                            case 2:
                                r.EconomicBuildings.Add(NewBuilding);
                                break;
                        }
                        r.UseableSlots--;
                    }
                }
                foreach (RegionInfo region in civ.Land)
                {
                    if(region.CityLevel == 2 && region.Name == "")
                    {
                        region.Name = StringTables[5].randomEntry(ref rng);
                    }
                    if (region.UseableSlots == 0 && region.CityLevel < 4)
                    {
                        region.UseableSlots += region.Buildings.Count / 2;
                    }
                }
                foreach (Unit unit in civ.Army)
                    {
                        if (World.RegionInfos[World.idx(unit.Position.x, unit.Position.y)].UnitsInTheCity.Count != 1 &&
                            !World.RegionInfos[World.idx(unit.Position.x, unit.Position.y)].FullControl)
                        {
                            foreach (Unit unit1 in World.RegionInfos[World.idx(unit.Position.x, unit.Position.y)].UnitsInTheCity)
                            {
                                if (unit1.CivTag != civ.Tag)
                                {
                                    _ = unit + unit1;
                                }
                            }
                        }
                        else if (unit.LastResupply / 100f * civ.Ai[0] / 2 > 2)
                        {
                            int Counter = 0;
                        retry:
                            if (World.RegionInfos[World.idx(unit.Position.x, unit.Position.y)].UnitsInTheCity.Count == 1 &&
                                World.RegionInfos[World.idx(unit.Position.x, unit.Position.y)].OwnerCiv == civ.Tag)
                            {
                                World.RegionInfos[World.idx(unit.Position.x, unit.Position.y)].OwnerCiv = civ.Tag;
                            }
                            int yDis = 0;
                            int xDis = 0;
                            Position NewPos = new Position(-1, -1);
                            do
                            {
                                yDis = rng.Next(-1, 2);
                                xDis = rng.Next(-1, 2);
                                NewPos = new Position(unit.Position.x + xDis, unit.Position.y + yDis);
                            }
                            while (!(World.idx(NewPos.x, NewPos.y) > 0 &&
                                    World.idx(NewPos.x, NewPos.y) < World.topology.Count &&
                                    NewPos.x > 0 && (xDis != 0 || yDis != 0)));
                            Terrain NewPosTer = NewPosTer = World.topology[World.idx(NewPos.x, NewPos.y)];
                            RegionInfo region = World.RegionInfos[World.idx(NewPos.x, NewPos.y)];

                            if (NewPosTer.type == (int)blockType.WATER && unit.CanSwim) goto retry;
                            unit.Moved(NewPosTer, NewPos, region);
                            if (region.UnitsInTheCity.Count == 0)
                            {
                                region.UnitsInTheCity.Add(unit);
                                if (region.OwnerCiv != civ.Tag)
                                {
                                    region.OwnerCiv = civ.Tag;
                                    civ.Land.Add(region);
                                }
                                Counter++;
                                if (Counter < unit.BaseMobility / 10) goto retry;
                            }
                            else
                            {
                                foreach (Unit unit1 in region.UnitsInTheCity)
                                {
                                    if (unit1.CivTag != civ.Tag)
                                    {
                                        _ = unit + unit1;
                                        if (unit.OffenciveBonus + unit1.DefenciveBonus > 90) region.DestroyBuilding(ref rng);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (World.RegionInfos[World.idx(unit.Position.x, unit.Position.y)].UnitsInTheCity.Count == 1)
                            {
                                World.RegionInfos[World.idx(unit.Position.x, unit.Position.y)].OwnerCiv = civ.Tag;
                            }
                            unit.Train();
                        }
                    }
                civ.Army.RemoveAll(Unit.isDefeated);
            }
            World.civs.RemoveAll(Civilization.isExtinct);
        }
        private string CreateEmpireName(ref Random rng, Civilization civ, Terrain terrain)
        {
            string Class = "";
        retry:
            switch (rng.Next(0, 4))
            {
                case 0:
                    if (terrain.savagery > 30)
                    {
                        Class = "Tribal Collective";
                        civ.Ai[0] += 20;
                        civ.Ai[1] -= 10;
                        civ.Ai[2] -= 10;
                    }
                    else goto retry;
                    break;
                case 1:
                    if (terrain.savagery < 30)
                    {
                        Class = "Republic";
                        civ.Ai[0] -= 20;
                        civ.Ai[1] += 10;
                        civ.Ai[2] += 10;
                    }
                    else goto retry;
                    break;
                case 2:
                    Class = "Monarchy";
                    civ.Ai[0] += 10;
                    civ.Ai[1] -= 5;
                    civ.Ai[2] += 5;
                    break;
                case 3:
                    Class = "Empire";
                    civ.Ai[0] += 15;
                    civ.Ai[1] += 5;
                    civ.Ai[2] -= 5;
                    break;
            }
            return "The " + StringTables[4].randomEntry(ref rng) + " " + Class;
        }
        public void BuildHistory(World World, ref Random rng)
        {
            BuildInitialCivs(World, ref rng);
            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine(i);
                RunYear(World, rng);
            }
        }
    }
}