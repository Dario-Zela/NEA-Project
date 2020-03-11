using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Pixel_Engine;
using System.ComponentModel;

namespace Models.WorldGen
{
    class Region
    {
        public Position Position;
        public int Modifier;
    }

    class UnitType
    {
        public int DefenciveBonus;
        public int OffenciveBonus;
        public int BaseMobility;
        public bool OutDated;
    }

    class Unit
    {
        public Region CurrentPosition;
        int DefenciveBonus;
        int OffenciveBonus;
        int BaseMobility;
        int Supplies;
        int Experience;
        int RemainingUnits;
        int Mobility;

        public Unit(UnitType Division)
        {
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

        static public void operator +(Unit Attacker, Unit Defender)
        {
            if (Attacker.OffenciveBonus > Defender.DefenciveBonus)
            {
                int BaseDamage = (Attacker.OffenciveBonus - Defender.DefenciveBonus) * new Random().Next(8);
                Defender.RemainingUnits = (int)(BaseDamage - (float)(Defender.Mobility + Defender.Supplies + Defender.Experience) / 100);
                Defender.Supplies -= BaseDamage / 10;
            }
            else
            {
                int BaseDamage = (Attacker.OffenciveBonus - Defender.DefenciveBonus) * new Random().Next(8);
                Attacker.RemainingUnits = (int)(BaseDamage - (float)(Attacker.Supplies + Attacker.Experience) / 100);
            }
        }

        public void Moved(Region NewPosition)
        {
            CurrentPosition = NewPosition;
            Mobility = BaseMobility - CurrentPosition.Modifier;
        }
    }

    class Structures
    {

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
        public List<Structures> Buildings;
        public int ResourcesPerTurn;
        public Position Capital;

        public Civilization()
        {
            TechLevel = new int[3];
            CivTag = "";
            Race = new Specie();
            Ai = new int[3];
            Army = new List<Unit>();
            Buildings = new List<Structures>();
            ResourcesPerTurn = 0;
            Capital = new Position();
        }
    }

    class Specie
    {

    }
}
