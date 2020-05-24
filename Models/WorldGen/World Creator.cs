using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Pixel_Engine;
using Models.Sprites;

namespace Models.WorldGen
{
    public class WorldCreator
    {
        public World World;
        public HistoryGen history;
        public Random rng;
        private Sprite[] BiomeMap = TileSet.Instance.GetSprites("BiomeMap");
        public WorldCreator(int seed, float Persistance = 0.5f, float Lacunarity=2f, int Octaves=5)
        {
            MapGen gen = new MapGen();
            BiomeMap biomes = new BiomeMap();
            RiverGen rivers = new RiverGen();
            World = new World();
            history = new HistoryGen();
            rng = new Random(seed);

            gen.startMap(World);
            gen.noiseMap(World, seed, Octaves, Persistance, Lacunarity);
            gen.baseTypeAllocation(World);
            gen.markCoastlines(World);
            gen.createRainfall(World);

            biomes.buildBiomes(World, ref rng);
            rivers.buildRivers(World, ref rng);
            //history.BuildHistory(World, ref rng);
            //TerrainGen terrain = new TerrainGen(World, seed);
        }

        public Sprite GetBiomeSprite(int x, int y)
        {
            return BiomeMap[World.topology[World.idx(x,y)].type - 1];
        }
    }
}
