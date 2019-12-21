﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models.World_Gen
{

    #region PerlinNoise
    /// <summary>
    /// An implementation of the improved perlin noise generator;
    /// Created by Ken Perlin in his second paper;
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
        138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
        151};


        /// <summary>
        /// Class constructor;
        /// </summary>
        public PerlinNoise() { }

        /// <summary>
        /// It create an ease curve that smooths out the results produced by the algorithm;
        /// It is needed as the function works on linear interpolation, which, while computationally simple;
        /// Produces unnatural looking curves;
        /// The function uses the formula 6t^5-15t^4+10t^3, also generated by Ken Perlin in his papers;
        /// </summary>
        /// <param name="t">The variable that it is used upon;
        /// In this case it is the decimal component of the x and y values</param>
        /// <returns>It returns the smoothed out value</returns>
        private double fade(double t)
        {
            double val = Math.Pow(t, 3) * (t * (t * 6 - 15) + 10);
            return val;
        }

        /// <summary>
        /// It takes the pseudorandom value given by the algorithm;
        /// Which is just one of the values in the Hash Table;
        /// And gives the gradient(Math Operation) of the hash vector;
        /// Which is composed of the first and sencond digits of the number;
        /// And the given vector;
        /// which is the combination of the x and y values;
        /// </summary>
        /// <param name="Hash">The given hash value</param>
        /// <param name="x">The given x component</param>
        /// <param name="y">The given y component</param>
        /// <returns>The gradient of the two vectors</returns>
        private double Grad(int Hash, double x, double y)
        {
            var val1 = (Hash & 1) == 0 ? x : -x;        //The x & 1 in the declaration gets the first value of the number
            var val2 = (Hash & 2) == 0 ? y : -y;        //It is identical to x % 2, but it seems to be slightly quicker
            return val1 + val2;
        }

        /// <summary>
        /// It gives the weighted sum of the values;
        /// </summary>
        /// <param name="t">The weight component, given the fade over a value</param>
        /// <param name="a">The first value</param>
        /// <param name="b">The second value</param>
        /// <returns>The weighted sum</returns>
        private double Lerp(double t, double a, double b)
        {
            return a + t * (b - a);
        }

        /// <summary>
        /// This generates the perlin noise for two values
        /// </summary>
        /// <param name="x">The first component</param>
        /// <param name="y">The second component</param>
        /// <returns>A number between 0 and 1</returns>
        public double PerlinNoiseGen(double x, double y)
        {
            var X = (int)x & 0xff;      // x & 0xff is the same as x % 256
            var Y = (int)y & 0xff;

            x -= Math.Floor(x);
            y -= Math.Floor(y);

            var u = fade(x);     
            var v = fade(y);

            var A = (permutations[X    ] + Y) & 0xff;       //This is the hash algorithm implemented by Perlin
            var B = (permutations[X + 1] + Y) & 0xff;

            //The following functions create the smooth terrain.
            //They take hash values generated before and apply the gradient over the x and y components, taking into
            //consideration other positions.
            //A weighted sum is applied twice, one between a change in y coordinates and one between a change in x coordinates
            //This is how the value is formed
            double val1 = Lerp(u, Grad(permutations[A    ], x, y    ), Grad(permutations[B    ], x - 1, y    ));
            double val2 = Lerp(u, Grad(permutations[A + 1], x, y - 1), Grad(permutations[B + 1], x - 1, y - 1));

            return Lerp(v, val1, val2);
        }
    }
    #endregion

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
        public double[,] GenerateNoiseMap(int mapDepth, int mapWidth, double Scale)
        {
            double[,] NoiseMap = new double[mapDepth, mapWidth];
            PerlinNoise perlin = new PerlinNoise();

            for(int zIndex = 0; zIndex< mapDepth; zIndex++)
            {
                for(int xIndex = 0; xIndex<mapWidth; xIndex++)
                {
                    double SampleIndexX = xIndex / Scale;
                    double SampleIndexZ = zIndex / Scale;

                    var noise = perlin.PerlinNoiseGen(SampleIndexX, SampleIndexZ);
                    NoiseMap[xIndex, zIndex] = noise;
                }
            }
            return NoiseMap;
        }
    }

    class WorldCreator
    {
        
    }
}
