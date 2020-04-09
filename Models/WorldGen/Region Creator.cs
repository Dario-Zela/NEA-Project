using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Pixel_Engine;
using System.ComponentModel;
using Models.NPCs;
using Models.Items;

namespace Models.WorldGen
{
    class bitSet
    {
        TileFlag bits = 0;
        public void Set(TileFlag Flag){ bits |= Flag; }
        public void Reset(TileFlag Flag) { bits &= ~Flag; }
        public bool Test(TileFlag Flag) { return (bits & Flag) != 0; }
    }

    class Region
    {
        #region Init
        List<TileType> TileType;
        List<ushort> TileMaterial;
        List<ushort> HitPoints;
        List<byte> VegHitPoints;
        List<uint> BuildingId;
        List<uint> TreeId;
        List<uint> BridgeId;
        List<ushort> TileVegetationType;
        List<ushort> TileVegetationTicker;
        List<byte> TileVegetationLifecycle;
        List<uint> StockpileId;
        List<bitSet> TileFlags;
        List<uint> WaterLevel;
        int region_x = 0, region_y = 0, biome_idx = 0;

        public Region(int x, int y, int biome)
        {
            TileType = new List<TileType>(Constants.REGION_TILES_COUNT);
            TileMaterial = new List<ushort>(Constants.REGION_TILES_COUNT);
            HitPoints = new List<ushort>(Constants.REGION_TILES_COUNT);
            VegHitPoints = new List<byte>(Constants.REGION_TILES_COUNT);
            BuildingId = new List<uint>(Constants.REGION_TILES_COUNT);
            TreeId = new List<uint>(Constants.REGION_TILES_COUNT);
            BridgeId = new List<uint>(Constants.REGION_TILES_COUNT);
            TileVegetationType = new List<ushort>(Constants.REGION_TILES_COUNT);
            TileVegetationTicker = new List<ushort>(Constants.REGION_TILES_COUNT);
            TileVegetationLifecycle = new List<byte>(Constants.REGION_TILES_COUNT);
            StockpileId = new List<uint>(Constants.REGION_TILES_COUNT);
            TileFlags = new List<bitSet>(Constants.REGION_TILES_COUNT);
            WaterLevel = new List<uint>(Constants.REGION_TILES_COUNT);
        }
        #endregion

        private int MapIdx(int x, int y, int z)
        {
            return (z* Constants.REGION_HEIGHT * Constants.REGION_WIDTH) + (y* Constants.REGION_WIDTH) + x;
        }
        private bool Flag(int idx, TileFlag flag)
        {
            return TileFlags[idx].Test(flag);
        }
        private int GroundZ(int x, int y)
        {
            int z = Constants.REGION_DEPTH - 1;
            bool hitGround = false;
            while (!hitGround)
            {
                int idx = mapidx(x, y, z);
                if (TileType[idx] == WorldGen.TileType.SOLID)
                {
                    hitGround = true;
                    z++;
                }
                else z--;
                if (z == 1) hitGround = true;
            }
            return z;
        }

    }
}