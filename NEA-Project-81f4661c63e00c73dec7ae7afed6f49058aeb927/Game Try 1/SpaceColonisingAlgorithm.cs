﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;

namespace Game_Try_1
{
    #region PerlinNoise
    /// <summary>
    /// An implementation of the improved perlin noise generator;
    /// Created by Ken Perlin in his second paper;
    /// Slightly modified by me to implement a seed mechanic;
    /// </summary>
    class PerlinNoise
    {
        /// <summary>
        /// The Permutations array used in the function;
        /// Created by Ken Perlin it contains all of the values form 0 to 255;
        /// It is used as a hash lookup table when generating the noise
        /// </summary>
        private int[] permutations = {151,160,137,91,90,15,
        131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
        190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
        88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
        77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
        102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
        135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
        223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
        129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
        251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
        49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
        138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180};

        /// <summary>
        /// Actual permutation table; which is simply the one above repeated twice;
        /// </summary>
        private int[] p;

        /// <summary>
        /// The actual seed;
        /// A float multiplyer for the weight on the lerp function;
        /// </summary>
        private float seed = new float();

        /// <summary>
        /// Class constructor;
        /// Assigns the seed value of the world;
        /// </summary>
        public PerlinNoise(float seed = 1)
        {
            this.seed = (seed * 36) / 128.0f;
            p = new int[512];
            permutations.CopyTo(p, 0);
            permutations.CopyTo(p, 256);
        }

        /// <summary>
        /// It create an ease curve that smooths out the results produced by the algorithm;
        /// It is needed as the function works on linear interpolation, which, while computationally simple;
        /// Produces unnatural looking curves;
        /// The function uses the formula 6t^5-15t^4+10t^3, also generated by Ken Perlin in his papers;
        /// </summary>
        /// <param name="t">The variable that it is used upon;
        /// In this case it is the float component of the x and y values</param>
        /// <returns>It returns the smoothed out value</returns>
        private float fade(float t)
        {
            float val = (float)Math.Pow(t, 3) * (t * (t * 6 - 15) + 10);
            return val;
        }

        /// <summary>
        /// It takes the pseudorandom value given by the algorithm;
        /// Which is just one of the values in the Hash Table;
        /// And gives the gradient(Math Operation) of the hash vector;
        /// Which is composed of the digits of the number;
        /// And the given vector;
        /// which is the combination of the x y and z values;
        /// </summary>
        /// <param name="Hash">The given hash value</param>
        /// <param name="x">The given x component</param>
        /// <param name="y">The given y component</param>
        /// <param name="z">The given z component</param>
        /// <returns>The gradient of the two vectors</returns>
        private float Grad(int Hash, float x, float y, float z)
        {
            switch (Hash & 0xF)                 //In this the value 0xF is the value 15
            {
                case 0x0: return x + y;
                case 0x1: return -x + y;
                case 0x2: return x - y;
                case 0x3: return -x - y;
                case 0x4: return x + z;
                case 0x5: return -x + z;
                case 0x6: return x - z;
                case 0x7: return -x - z;
                case 0x8: return y + z;
                case 0x9: return -y + z;
                case 0xA: return y - z;
                case 0xB: return -y - z;
                case 0xC: return y + x;
                case 0xD: return -y + z;
                case 0xE: return y - x;
                case 0xF: return -y - z;
                default: return 0; // never happens
            }
        }

        /// <summary>
        /// It gives the weighted sum of the values;
        /// </summary>
        /// <param name="t">The weight component, given the fade over a value</param>
        /// <param name="a">The first value</param>
        /// <param name="b">The second value</param>
        /// <returns>The weighted sum</returns>
        private float Lerp(float t, float a, float b)
        {
            return a + t * (b - a);
        }

        /// <summary>
        /// This generates the perlin noise for two values
        /// </summary>
        /// <param name="x">The first component</param>
        /// <param name="y">The second component</param>
        /// <returns>A number between 0 and 1</returns>
        public float PerlinNoiseGen(float x, float y)
        {
            var X = (int)x & 0xff;      // x & 0xff is the same as x % 256
            var Y = (int)y & 0xff;
            var Z = (int)seed & 0xff;    // I use the z vaue as the seed

            x -= (int)x;
            y -= (int)y;
            var z = seed - (int)seed;

            var u = fade(x);        //These are the normalised weights
            var v = fade(y);
            var w = fade(z);

            int aaa, aba, aab, abb, baa, bba, bab, bbb;             //This is the hash algorithm implemented by Ken Perlin
            aaa = p[p[p[X] + Y] + Z];
            aba = p[p[p[X] + inc(Y)] + Z];
            aab = p[p[p[X] + Y] + inc(Z)];
            abb = p[p[p[X] + inc(Y)] + inc(Z)];
            baa = p[p[p[inc(X)] + Y] + Z];
            bba = p[p[p[inc(X)] + inc(Y)] + Z];
            bab = p[p[p[inc(X)] + Y] + inc(Z)];
            bbb = p[p[p[inc(X)] + inc(Y)] + inc(Z)];

            //The following functions create the smooth terrain.
            //They take hash values generated before and apply the gradient over the x and y components, taking into
            //consideration other positions.
            //A weighted sum over all the possible gratients is taken to create the value
            //This is how the value is formed
            float x1, x2, y1, y2;

            x1 = Lerp(u, Grad(aaa, x, y, z), Grad(baa, x - 1, y, z));
            x2 = Lerp(u, Grad(aba, x, y - 1, z), Grad(bba, x - 1, y - 1, z));
            y1 = Lerp(v, x1, x2);

            x1 = Lerp(u, Grad(aab, x, y, z - 1), Grad(bab, x - 1, y, z - 1));
            x2 = Lerp(u, Grad(abb, x, y - 1, z - 1), Grad(bbb, x - 1, y - 1, z - 1));
            y2 = Lerp(v, x1, x2);

            return (Lerp(w, y1, y2) + 1) / 2;
        }

        /// <summary>
        /// Does an increament to a value and then returns it
        /// </summary>
        /// <param name="y">The value to be incremented</param>
        /// <returns>y+1</returns>
        private int inc(int y)
        {
            y++;
            return y;
        }

        /// <summary>
        /// Creates an octave noise, which is a combination of Perlin genarated noise
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="octaves">How many frequencies should be combined</param>
        /// <param name="persistence">How the amplitude changes between Perlin noise executions</param>
        /// <param name="lacunarity">How the frequency should change between executions;
        /// Usually 2, it can be changed to edit how the frequency changes between octaves</param>
        /// <returns></returns>
        public float OctaveNoiseGen(float x, float y, int octaves, float persistence, float lacunarity = 2)
        {
            float total = 0;
            float frequency = 1;
            float amplitude = 1;
            float MaxValue = 0;
            for (int i = 0; i < octaves; i++)
            {
                total += (PerlinNoiseGen(x * frequency, y * frequency) * 2 - 1) * amplitude;

                MaxValue += amplitude;
                amplitude *= persistence;

                frequency *= lacunarity;
            }
            return total / MaxValue;
        }
    }
    #endregion

    #region MapGen
    /// <summary>
    /// The base world builder;
    /// It constructs the blank world;
    /// </summary>
    public class MapGen
    {
        /// <summary>
        /// The constructor of the class;
        /// </summary>
        public MapGen() { }

        /// <summary>
        /// This function generates a 2D array where the index indicates the coordinates;
        /// While the value is the noise at the position.
        /// </summary>
        /// <param name="mapDepth">The size of the map (x)</param>
        /// <param name="mapWidth">The size of the map (y)</param>
        /// <param name="Scale">The zoom of the function</param>
        /// <returns>The noise map</returns>
        public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float Scale, int seed, int Octaves, float Persistance, float Lacunarity)
        {
            float[,] NoiseMap = new float[mapDepth, mapWidth];
            PerlinNoise perlin = new PerlinNoise(seed);
            float maxNoise = float.MinValue;
            float minNoise = float.MaxValue;
            for (int zIndex = 0; zIndex < mapDepth; zIndex++)
            {
                for (int xIndex = 0; xIndex < mapWidth; xIndex++)
                {
                    float SampleIndexX = xIndex / Scale;
                    float SampleIndexZ = zIndex / Scale;
                    float noise = perlin.OctaveNoiseGen(SampleIndexX, SampleIndexZ, Octaves, Persistance, Lacunarity);
                    if (noise > maxNoise)
                    {
                        maxNoise = noise;
                    }
                    else if (noise < minNoise)
                    {
                        minNoise = noise;
                    }
                    NoiseMap[xIndex, zIndex] = noise;
                }
            }
            for (int i = 0; i < mapDepth; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    NoiseMap[j, i] = InverseLerp(minNoise, maxNoise, NoiseMap[j, i]);
                }
            }

            return NoiseMap;
        }

        private float InverseLerp(float min, float max, float value)
        {
            return (value - min) / (max - min);
        }
    }
    #endregion

    class Leaves
    {
        public Vector position;
        public Branch closestBranch;

        public Leaves(Vector pos)
        {
            position = pos;
        }
    }
    class Branch
    {
        public Vector position;
        public Branch parent;
        public Vector dir;
        private readonly Vector orDir;
        public Vector newBranchPos;
        public int count;

        public Branch(Vector pos, Branch parent, Vector dir)
        {
            position = pos;
            this.parent = parent;
            this.dir = dir;
            orDir = dir;
            count = 0;
        }

        public void Reset()
        {
            dir = orDir;
            count = 0;
        }

        public Branch NewBranch()
        {
            newBranchPos = dir + position;
            Branch branch = new Branch(newBranchPos, this, dir);
            return branch;
        }
    }
    class Tree
    {
        public double maxDist;
        public double minDist;
        public LinkedList<Leaves> Leaves = new LinkedList<Leaves>();
        public LinkedList<Branch> Branches = new LinkedList<Branch>();

        public Tree(double maxDist, double minDist, Vector rootPos, int mapHeight, int mapWidth, int numLeaves)
        {
            this.maxDist = maxDist;
            this.minDist = minDist;
            Random random = new Random();
            for (int i = 0; i < numLeaves; i++)
            {
                Vector pos = new Vector(random.Next(mapHeight), random.Next(mapWidth));
                Leaves.AddLast(new Leaves(pos));
            }
            Branches.AddLast(new Branch(rootPos, null, new Vector(10, 10)));
            bool cont = true;
            int counter = 0;
            while (cont && counter < 1000)
            {
                counter++;
                cont = Grow();
            }
        }

        private bool Grow()
        {
            LinkedList<Leaves> temp = new LinkedList<Leaves>();

            foreach (var leaf in Leaves)
            {
                Vector dir;
                double distance = double.MaxValue;
                foreach (var branch in Branches)
                {
                    dir = leaf.position - branch.position;
                    if(dir.Length < minDist)
                    {
                        temp.AddLast(leaf);
                    }
                    else if(dir.Length > maxDist){ }
                    else if(dir.Length < distance)
                    {
                        leaf.closestBranch = branch;
                        distance = dir.Length;
                    }
                }
                if(leaf.closestBranch != null)
                {
                    dir = leaf.position - leaf.closestBranch.position;
                    dir = new Vector(dir.X / (float)distance, dir.Y / (float)distance);
                    leaf.closestBranch.dir += dir;
                    leaf.closestBranch.count++;
                }
            }

            foreach (var item in temp)
            {
                Leaves.Remove(item);
            }

            bool isEvolving = false;

            LinkedList<Branch> temp2 = new LinkedList<Branch>();

            foreach (var branch in Branches)
            {
                if(branch.count != 0)
                {
                    branch.dir = new Vector(branch.dir.X / (float)branch.count, branch.dir.Y / (float)branch.count);
                    temp2.AddLast(branch.NewBranch());
                    branch.Reset();
                    isEvolving = true;
                }
            }
            foreach (var item in temp2)
            {
                Branches.AddLast(item);
            }
            return isEvolving;
        }
    }

    class City
    {
        public Tree t;
        public float[,] x;

        public City(double Height, double Width)
        {
            t = new Tree(400, 29, new Vector(Height / 2, Width / 2), (int)Height, (int)Width, 50);
            float[,] v = new MapGen().GenerateNoiseMap((int)Height, (int)Width, 10f, 21987, 6, 0.5f, 2f);
            x = new float[(int)Height, (int)Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    foreach (var item in t.Branches)
                    {
                        if ((new Vector(i, j) - item.position).Length < 5)
                        {
                            x[i,j] = 1;
                        }
                    }
                }
            }
        }
    }
}
