using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Pixel_Engine;
using Models.Sprites;

namespace Models.WorldGen
{
    public class Colour
    {
        public Colour(byte R, byte G, byte B)
        {
            r = R;
            g = G;
            b = B;
        }

        public byte r;
        public byte g;
        public byte b;

        public static bool operator ==(Colour A, Colour B)
        {
            return A.r == B.r && A.g == B.g && A.b == B.b;
        }

        public static bool operator !=(Colour A, Colour B)
        {
            return !(A == B);
        }

        public override bool Equals(object obj)
        {
            return obj is Colour;
        }

        public override int GetHashCode()
        {
            var hashCode = -839137856;
            hashCode = hashCode * -1521134295 + r.GetHashCode();
            hashCode = hashCode * -1521134295 + g.GetHashCode();
            hashCode = hashCode * -1521134295 + b.GetHashCode();
            return hashCode;
        }

        static public implicit operator Pixel(Colour c)
        {
            return new Pixel(c.r, c.g, c.b);
        }

        static public implicit operator Colour(Pixel c)
        {
            return new Colour(c.R, c.G, c.B);
        }
    };

    public class WorldCreator
    {
        //public GetBiome biomeMap;
        public World World;
        public HistoryMaker history;
        public Random rng;
        private Sprite[] BiomeMap = TileSet.Instance.GetSprites("BiomeMap");
        public WorldCreator(int seed, float Persistance = 0.5f, float Lacunarity=2f, int Octaves=5)
        {
            MapGen gen = new MapGen();
            BiomeMap biomes = new BiomeMap();
            RiverGen rivers = new RiverGen();
            World = new World();
            history = new HistoryMaker();
            rng = new Random(seed);

            gen.startMap(World);
            gen.noiseMap(World, seed, Octaves, Persistance, Lacunarity);
            gen.baseTypeAllocation(World);
            gen.markCoastlines(World);
            gen.createRainfall(World);

            biomes.buildBiomes(World, ref rng);
            rivers.buildRivers(World, ref rng);
            history.buildInitialCivs(World, ref rng);
            //history.buildInitialHistory(World, ref rng);
        }

        public Sprite GetBiomeSprite(int x, int y)
        {
            return BiomeMap[World.topology[World.idx(x,y)].type - 1];
        }
    }
}
