using System;
using System.Collections.Generic;
using System.Text;
using Models.WorldGen;

namespace Models.WorldGen
{
    class AStarRivers
    {
        private float[,] map;
        private (int, int) Start;
        public Biome[,] biomes;
        private bool tributary = false;

        public AStarRivers(float[,] map, (int, int) Start, Biome[,] biomes)
        {
            this.map = map;
            this.Start = Start;
            this.biomes = biomes;
            Path();
        }

        private (int, int) Move((int, int) Pos)
        {
            float min = map[Pos.Item1,Pos.Item2];
            (int, int) minPos = Pos;
            for (int i = Pos.Item1 - 1; i < Pos.Item1 + 1; i++)
            {
                for (int j = Pos.Item2 - 1; j < Pos.Item2 + 1; j++)
                {
                    if(map[i,j] < min)
                    {
                        min = map[i, j];
                        minPos = (i,j);
                    }
                }
            }
            return minPos;
        }

        private void Flood((int, int) pos)
        {
            for (int i = pos.Item1; i < (int)(1 / (map[pos.Item1, pos.Item2] * 3) + 3); i++)
            {
                for (int j = pos.Item2; j < (int)(1 / (map[pos.Item1, pos.Item2] * 2) + 3) ; j++)
                {
                    if(biomes[i, j].isRiverLevee == true)
                    {
                        biomes[i, j].isRiverLevee = false;
                        tributary = true;
                    }
                    biomes[i, j] = new Biome(70,map[i,j]) ;
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 1; l++)
                        {
                            try
                            {
                                biomes[i + k, j + l].isRiverLevee = true;
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }
        }

        private void Path()
        {
            (int, int) prevPos;
            (int, int) currPos = Start;
            bool cond1 = false;
            bool cond2 = false;
            bool cond3 = false;
            if (currPos.Item1 != 0 && currPos.Item2 != 0)
            {
                do
                {
                    prevPos = currPos;
                    currPos = Move(currPos);
                    biomes[currPos.Item1, currPos.Item2] = new Biome(70, map[currPos.Item1, currPos.Item2]);
                    Flood(currPos);
                    cond1 = map[currPos.Item1, currPos.Item2] < 0.1 || currPos == prevPos;
                    cond2 = currPos.Item1 == 0 || currPos.Item2 == 0;
                    cond3 = biomes[currPos.Item1, currPos.Item2].isRiverLevee;
                }
                while (!(cond1 || cond2 || cond3 || tributary));
                if (cond3)
                {
                    for (int i = -3; i < 3; i++)
                    {
                        for (int j = -3; j < 3; j++)
                        {
                            biomes[i, j] = new Biome(70, map[i, j]);
                        }
                    }
                }
            }
        }
    }
}
