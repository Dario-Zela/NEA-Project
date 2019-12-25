using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Game_Try_1
{
    class Leaves
    {
        public Vector2 position;
        public Branch closestBranch;

        public Leaves(Vector2 pos)
        {
            position = pos;
        }
    }
    class Branch
    {
        public Vector2 position;
        public Branch parent;
        public Vector2 dir;
        public readonly Vector2 orDir;
        public Vector2 newBranchPos;
        public int count;
        public bool isNode = false;
        public bool isRoot = false;
        public bool isUsed = false;

        public Branch(Vector2 pos, Branch parent, Vector2 dir)
        {
            position = pos;
            this.parent = parent;
            orDir = dir;
            if (parent != null && parent.parent != null)
            {
                orDir = this.parent.orDir;
            }
            this.dir = orDir;
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
        public Random random = new Random();
        private LinkedList<Branch> Nodes = new LinkedList<Branch>();

        public Tree(double maxDist, double minDist, Vector2 rootPos, int mapHeight, int mapWidth, int numLeaves)
        {
            this.maxDist = maxDist;
            this.minDist = minDist;
            for (int i = 0; i < numLeaves; i++)
            {
                int x;
                int y;
                do
                {
                    x = random.Next(mapHeight);
                    y = random.Next(mapWidth);
                }
                while ((x < mapHeight * 9 / 10 && x > mapHeight / 10) || (y < mapWidth * 9 / 10 && y > mapWidth /10));
                Vector2 pos = new Vector2(x, y);
                Leaves.AddLast(new Leaves(pos));
            }
            Branches.AddLast(new Branch(rootPos, null, new Vector2(10, 10)));
            Branches.First.Value.isRoot = true;
            bool cont = true;
            while (cont && Branches.Count < 1000)
            {
                cont = Grow();
            }
            //FindNodes();
            //SecondaryRoads();
        }

        private void SecondaryRoads()
        {
            
            foreach (var item in Nodes)
            {
                Branch NewBranch1 = null;
                Branch NewBranch2 = null;
                double lenght1 = double.MaxValue;
                double lenght2 = double.MaxValue;
                foreach (var branch in Nodes)
                {
                    item.dir = branch.position - item.position;
                    if(item.dir.Length() < lenght1)
                    {
                        NewBranch1 = branch;
                        lenght1 = item.dir.Length();
                    }
                    else if (item.dir.Length() < lenght2)
                    {
                        NewBranch2 = branch;
                        lenght2 = item.dir.Length();
                    }
                    item.Reset();
                }
                item.dir = NewBranch1.position - item.position;
                Branches.AddLast(item.NewBranch());
                item.Reset();
                item.dir = NewBranch2.position - item.position;
                Branches.AddLast(item.NewBranch());
                item.Reset();
            }
        }

        private void FindNodes()
        {
            foreach (var item in Branches)
            {
                if (random.Next(0,2000) < 20)
                {
                    item.isNode = true;
                    Nodes.AddLast(item);
                }
            }
        }

        private double FindAngle(Vector2 vector)
        {
            double angle = Math.Atan2(vector.Y, vector.X);
            return angle;
        }

        private bool Grow()
        {
            LinkedList<Leaves> temp = new LinkedList<Leaves>();

            foreach (var leaf in Leaves)
            {
                Vector2 dir;
                double distance = double.MaxValue;
                foreach (var branch in Branches)
                {
                    dir = leaf.position - branch.position;
                    if(dir.Length() < minDist)
                    {
                        temp.AddLast(leaf);
                    }
                    else if(dir.Length() > maxDist){ }
                    else if(dir.Length() < distance && FindAngle(dir) < FindAngle(branch.orDir) + 5 && FindAngle(dir) > FindAngle(branch.orDir) - 5)
                    {
                        leaf.closestBranch = branch;
                        distance = dir.Length();
                    }
                }
                if(leaf.closestBranch != null)
                {
                    dir = leaf.position - leaf.closestBranch.position;
                    dir = new Vector2(dir.X / (float)distance, dir.Y / (float)distance);
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
                if(branch.count != 0 && (!branch.isUsed || branch.isRoot))
                {
                    branch.dir = new Vector2(branch.dir.X / (float)branch.count, branch.dir.Y / (float)branch.count);
                    branch.dir = new Vector2(branch.dir.X / branch.dir.Length(), branch.dir.Y / branch.dir.Length());
                    temp2.AddLast(branch.NewBranch());
                    isEvolving = true;
                    branch.Reset();
                    branch.isUsed = true;
                }
            }
            foreach (var item in temp2)
            {
                Branches.AddLast(item);
            }
            return isEvolving;
        }
    }

    class CreateRoadMap
    {
        public CreateRoadMap()
        {
            //Tree tree = new Tree();
        }
    }
}
