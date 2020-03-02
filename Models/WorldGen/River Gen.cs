using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Models.WorldGen
{
    public struct RiverStep
    {
        public int x;
        public int y;

        public RiverStep(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class River
    {
        public string name = "River";
        public RiverStep Start;
        public RiverStep End;
        public List<RiverStep> route = new List<RiverStep>();
    }
    class RiverGen
    {
        public void buildRivers(World World, ref Random rng)
        {
            int nRivers = Constants.WORLD_WIDTH / 2;
            HashSet<RiverStep> usedStarts = new HashSet<RiverStep>();
            for (int i = 0; i < nRivers; i++)
            {
                River river = new River();

                bool startOK = false;
                do
                {
                    river.Start = new RiverStep(rng.Next(1, Constants.WORLD_WIDTH), rng.Next(1, Constants.WORLD_HEIGHT));
                    Terrain pos = World.topology[World.idx(river.Start.x, river.Start.y)];
                    if ((pos.type == (int)blockType.MOUNTAINS || pos.type == (int)blockType.HIGHLANDS) && !usedStarts.Contains(river.Start))
                        startOK = true;
                }
                while (!startOK);

                RiverStep curPos = river.Start;

                int riverLen = 1;
                while (Check(curPos, World, usedStarts) && riverLen < 30)
                {
                    usedStarts.Add(curPos);
                    curPos = FindNextPos(curPos, World);
                    river.route.Add(curPos);
                    riverLen++;
                }
                river.route.Remove(curPos);
                river.End = curPos;
                usedStarts.Add(curPos);

                World.rivers.Add(river);
            }
        }

        bool Check(RiverStep pos, World World, HashSet<RiverStep> used)
        {
            Terrain posB = World.topology[World.idx(pos.x, pos.y)];
            bool check = posB.type == (int)blockType.WATER || !used.Contains(pos);
            check |= (pos.x == 0 && pos.y == 0) || (pos.x == Constants.WORLD_WIDTH && pos.y == 0) ||
                (pos.x == 0 && pos.y == Constants.WORLD_HEIGHT) || (pos.x == Constants.WORLD_WIDTH && pos.y == Constants.WORLD_HEIGHT);
            return check;
        }

        RiverStep FindNextPos(RiverStep pos, World World)
        {
            RiverStep nextPos = pos;
            int minHeight = World.topology[World.idx(pos.x, pos.y)].height;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (pos.x + i > 0 && pos.x + i < Constants.WORLD_WIDTH && pos.y + j > 0 && pos.y + j < Constants.WORLD_HEIGHT)
                    {
                        if (World.topology[World.idx(pos.x + i, pos.y + j)].height < minHeight)
                        {
                            minHeight = World.topology[World.idx(pos.x + i, pos.y + j)].height;
                            nextPos.x = pos.x + i;
                            nextPos.y = pos.y + j;
                        }
                    }

                }
            }
            return nextPos;
        }
    }
}
