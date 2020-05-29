using System;
using System.Collections.Generic;

namespace Models.WorldGen
{
    public class River
    {
        public string name = "River";
        public Position Start;
        public Position End;
        public List<Position> route = new List<Position>();
    }
    class RiverGen
    {
        public void buildRivers(World World, ref Random rng)
        {
            int nRivers = Constants.WORLD_WIDTH / 2;
            HashSet<Position> usedStarts = new HashSet<Position>();
            for (int i = 0; i < nRivers; i++)
            {
                River river = new River();

                bool startOK = false;
                do
                {
                    river.Start = new Position(rng.Next(1, Constants.WORLD_WIDTH), rng.Next(1, Constants.WORLD_HEIGHT));
                    Terrain pos = World.topology[World.idx(river.Start)];
                    if ((pos.type == (int)blockType.MOUNTAINS || pos.type == (int)blockType.HIGHLANDS) && !usedStarts.Contains(river.Start))
                        startOK = true;
                }
                while (!startOK);

                Position curPos = river.Start;

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

        bool Check(Position pos, World World, HashSet<Position> used)
        {
            Terrain posB = World.topology[World.idx(pos)];
            bool check = posB.type == (int)blockType.WATER || !used.Contains(pos);
            check |= (pos.x == 0 && pos.y == 0) || (pos.x == Constants.WORLD_WIDTH && pos.y == 0) ||
                (pos.x == 0 && pos.y == Constants.WORLD_HEIGHT) || (pos.x == Constants.WORLD_WIDTH && pos.y == Constants.WORLD_HEIGHT);
            return check;
        }

        Position FindNextPos(Position pos, World World)
        {
            Position nextPos = pos;
            int minHeight = World.topology[World.idx(pos)].height;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (pos.x + i > 0 && pos.x + i < Constants.WORLD_WIDTH && pos.y + j > 0 && pos.y + j < Constants.WORLD_HEIGHT)
                    {
                        if (World.topology[World.idx(pos.x + i, pos.y + j)].height < minHeight)
                        {
                            minHeight = World.topology[World.idx(pos.x + i, pos.y + j)].height;
                            nextPos = new Position(pos.x + i, pos.y + j);
                        }
                    }

                }
            }
            return nextPos;
        }
    }
}
