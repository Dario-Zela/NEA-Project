using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.ComponentModel;
using Models.WorldGen;

namespace Models.WorldGens
{
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

        public UnitType(int DefenciveBonus, int OffenciveBonus, int BaseMobility, int Cost)
        {
            this.BaseMobility = BaseMobility;
            this.DefenciveBonus = DefenciveBonus;
            this.OffenciveBonus = OffenciveBonus;
            this.Cost = Cost;
            OutDated = false;
        }
    }

    class Unit
    {
        public Position Position;
        public RegionModifier CurrentPosition;
        int DefenciveBonus;
        int OffenciveBonus;
        int BaseMobility;
        int Supplies;
        int Experience;
        int RemainingUnits;
        int Mobility;

        public Unit(UnitType Division, Position pos)
        {
            Position = pos;
            DefenciveBonus = Division.DefenciveBonus;
            OffenciveBonus = Division.OffenciveBonus;
            BaseMobility = Division.BaseMobility;
            RemainingUnits = 100;
            Supplies = 100;
            Experience = 0;
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

    class Structure
    {
        public int DefenceMod;
    }

    struct Position
    {
        Tuple<int,int> pos;

        public Position(int x, int y)
        {
            pos = new Tuple<int,int>(x,y);
        }

        public int x {get{return pos.Item1;}}

        public int y {get{return pos.Item2;}}
    }

    class Civilization
    {
        public int[] TechLevel;
        public string CivTag;
        public Specie Race;
        public int[] Ai;
        public List<Unit> Army;
        public List<UnitType> PossibleDivisions;
        public List<Structure> Buildings;
        public int ResourcesPerTurn;
        public int SupplyGen;
        public List<Civilization> Allies;
        public Position Capital;

        public Civilization()
        {
            PossibleDivisions = new List<UnitType>();
            TechLevel = new int[3];
            CivTag = "";
            Race = new Specie();
            Ai = new int[3];
            Army = new List<Unit>();
            Buildings = new List<Structure>();
            ResourcesPerTurn = 0;
            Capital = new Position();
        }

        public A
    }

    class Specie
    {

    }
}
