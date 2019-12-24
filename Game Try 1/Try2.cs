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
        private readonly Vector2 orDir;
        public Vector2 newBranchPos;
        public int count;

        public Branch(Vector2 pos, Branch parent, Vector2 dir)
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

        public Tree(double maxDist, double minDist, Vector2 rootPos, int mapHeight, int mapWidth, int numLeaves)
        {
            this.maxDist = maxDist;
            this.minDist = minDist;
            Random random = new Random();
            for (int i = 0; i < numLeaves; i++)
            {
                Vector2 pos = new Vector2(random.Next(mapHeight), random.Next(mapWidth));
                Leaves.AddLast(new Leaves(pos));
            }
            Branches.AddLast(new Branch(rootPos, null, new Vector2(10, 10)));
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
                    else if(dir.Length() < distance)
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
                if(branch.count != 0)
                {
                    branch.dir = new Vector2(branch.dir.X / (float)branch.count, branch.dir.Y / (float)branch.count);
                    branch.dir = new Vector2(branch.dir.X / branch.dir.Length(), branch.dir.Y / branch.dir.Length());
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
}
