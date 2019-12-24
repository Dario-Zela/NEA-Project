using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Numerics;

//taken from https://www.researchgate.net/publication/221314843_Modeling_Trees_with_a_Space_Colonization_Algorithm
//and Code Train;
namespace Game_Try_1
{
    class Node
    {
        public (double, double) pos;
        public bool reached;

        public Node((double, double) pos)
        {
            this.pos = pos;
        }
    }

    class Road
    {
        public (double, double) pos;
        public Road parent;
        public Vector direction;
        public readonly Vector originalDirection;
        public int count = 0;

        public void Reset()
        {
            direction = originalDirection;
            count = 0;
        }

        public Road((double, double) pos, Vector direction, Road parent)
        {
            this.pos = pos;
            this.parent = parent;
            direction.Normalize();
            this.direction = direction;
            originalDirection = direction;
        }

        public Road NextRoad()
        {
            Road newRoad = new Road((pos.Item1 + direction.X, pos.Item2 + direction.Y), direction, this);
            return newRoad;
        }
    }

    class City 
    {
        public LinkedList<Node> nodes = new LinkedList<Node>();
        public Road current;
        public LinkedList<Road> roads = new LinkedList<Road>();
        public float minDistance;
        public float maxDistance;

        public float Distance((double, double) pos1, (double, double) pos2)
        {
            double distance = new Vector(pos1.Item1 - pos2.Item1, pos1.Item2 - pos2.Item2).Length;
            return (float)distance;
        }

        public City(int numNodes, int mapHeight, int mapWidth, float minDistance, float maxDistance)
        {
            this.maxDistance = maxDistance;
            this.minDistance = minDistance;
            Random random = new Random();
            for (int i = 0; i < numNodes; i++)
            {
                nodes.AddLast(new Node((random.Next(0, mapHeight - 100), random.Next(0, mapWidth + 1))));
            }
            double xDir;
            double yDir;
            do
            {
                xDir = random.NextDouble() * 100;
                yDir = random.NextDouble() * 100;
            } 
            while (xDir == 0 && yDir == 0);
            current = new Road((0, mapWidth / 2), new Vector(xDir, yDir), null);
            roads.AddLast(current);
            Start();
        }

        private void Start()
        {
            bool found = false;
            while (!found)
            {
                foreach (Node node in nodes)
                {
                    float distance = Distance(current.pos, node.pos);
                    if (distance < maxDistance)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    current = current.NextRoad();
                    roads.AddLast(current);
                }
            }
        }

        public void Grow()
        {
            Road closestRoad = null;
            float recordDistance = float.MaxValue;
            foreach (var node in nodes)
            {
                foreach (var road in roads)
                {
                    float distance = Distance(node.pos, road.pos);
                    if (distance < minDistance)
                    {
                        node.reached = true;
                        closestRoad = null;
                        break;
                    }
                    else if (distance > maxDistance) { }
                    else if (closestRoad == null || recordDistance < distance)
                    {
                        closestRoad = road;
                        recordDistance = distance;
                    }
                }
                if (closestRoad != null)
                {
                    Vector direction = new Vector(node.pos.Item1 - closestRoad.pos.Item1, node.pos.Item2 - closestRoad.pos.Item2);
                    direction.Normalize();
                    closestRoad.direction += direction;
                    closestRoad.count++;
                    closestRoad = null;
                }
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                var currentNode = nodes.First;
                if (currentNode.Value.reached)
                {
                    if (currentNode.Next != null)
                    {
                        currentNode = currentNode.Next;
                        nodes.Remove(currentNode.Previous);
                    }
                    else
                    {
                        nodes.Remove(currentNode);
                    }
                }
            }

            LinkedList<Road> temp = new LinkedList<Road>();

            foreach (var road in roads)
            {
                if (road.count > 0)
                {
                    road.direction /= road.count + 1;
                    Road newRoad = road.NextRoad();
                    temp.AddLast(newRoad);
                }
                road.Reset();
            }

            foreach (var road in temp)
            {
                roads.AddLast(road);
            }
        }
    }

    class SpaceColonisationAlgorithm
    {
        public class Leaf
        {
            public Vector Position { get; set; }
            public Branch ClosestBranch { get; set; }

            public Leaf(Vector position)
            {
                Position = position;
            }
        }
        public class Branch
        {
            public Branch Parent { get; private set; }
            public Vector GrowDirection { get; set; }
            public Vector OriginalGrowDirection { get; set; }
            public int GrowCount { get; set; }
            public Vector Position { get; private set; }

            public Branch(Branch parent, Vector position, Vector growDirection)
            {
                Parent = parent;
                Position = position;
                GrowDirection = growDirection;
                OriginalGrowDirection = growDirection;
            }

            public void Reset()
            {
                GrowCount = 0;
                GrowDirection = OriginalGrowDirection;
            }
        }
        public class Tree
        {
            bool DoneGrowing = false;

            Vector Position = new Vector(0,0);

            int LeafCount = 400;
            int TreeWidth = 80;
            int TreeHeight = 150;
            int TrunkHeight = 40;
            int MinDistance = 2;
            int MaxDistance = 15;
            int BranchLength = 2;

            Branch Root;
            List<Leaf> Leaves;
            Dictionary<Vector, Branch> Branches;

            Rectangle Crown;

            public Tree(Vector position)
            {
                Position = position;
                GenerateCrown();
                GenerateTrunk();
            }

            private void GenerateCrown()
            {
                Crown = new Rectangle() { Margin = new Thickness((int)Position.X - TreeWidth / 2, (int)Position.Y - TreeHeight - TrunkHeight, TreeWidth, TreeHeight) };
                Leaves = new List<Leaf>();
                Random random = new Random();

                //randomly place leaves within our rectangle
                for (int i = 0; i < LeafCount; i++)
                {
                    Vector location = new Vector(random.Next((int)Crown.Margin.Left, (int)Crown.Margin.Right + 1), random.Next((int)Crown.Margin.Top, (int)Crown.Margin.Bottom + 1));
                    Leaf leaf = new Leaf(location);
                    Leaves.Add(leaf);
                }
            }

            private void GenerateTrunk()
            {
                Branches = new Dictionary<Vector, Branch>();

                Root = new Branch(null, Position, new Vector(0, -1));
                Branches.Add(Root.Position, Root);

                Branch current = new Branch(Root, new Vector(Position.X, Position.Y - BranchLength), new Vector(0, -1));
                Branches.Add(current.Position, current);

                //Keep growing trunk upwards until we reach a leaf       
                while ((Root.Position - current.Position).Length < TrunkHeight)
                {
                    Branch trunk = new Branch(current, new Vector(current.Position.X, current.Position.Y - BranchLength), new Vector(0, -1));
                    Branches.Add(trunk.Position, trunk);
                    current = trunk;
                }
            }

            public void Grow()
            {
                if (DoneGrowing) return;

                //If no leaves left, we are done
                if (Leaves.Count == 0)
                {
                    DoneGrowing = true;
                    return;
                }

                //process the leaves
                for (int i = 0; i < Leaves.Count; i++)
                {
                    bool leafRemoved = false;

                    Leaves[i].ClosestBranch = null;
                    Vector direction = new Vector(0,0);

                    //Find the nearest branch for this leaf
                    foreach (Branch b in Branches.Values)
                    {
                        direction = Leaves[i].Position - b.Position;                       //direction to branch from leaf
                        float distance = (float)Math.Round(direction.Length);            //distance to branch from leaf
                        direction.Normalize();

                        if (distance <= MinDistance)            //Min leaf distance reached, we remove it
                        {
                            Leaves.Remove(Leaves[i]);
                            i--;
                            leafRemoved = true;
                            break;
                        }
                        else if (distance <= MaxDistance)       //branch in range, determine if it is the nearest
                        {
                            if (Leaves[i].ClosestBranch == null)
                                Leaves[i].ClosestBranch = b;
                            else if ((Leaves[i].Position - Leaves[i].ClosestBranch.Position).Length > distance)
                                Leaves[i].ClosestBranch = b;
                        }
                    }

                    //if the leaf was removed, skip
                    if (!leafRemoved)
                    {
                        //Set the grow parameters on all the closest branches that are in range
                        if (Leaves[i].ClosestBranch != null)
                        {
                            Vector dir = Leaves[i].Position - Leaves[i].ClosestBranch.Position;
                            dir.Normalize();
                            Leaves[i].ClosestBranch.GrowDirection += dir;       //add to grow direction of branch
                            Leaves[i].ClosestBranch.GrowCount++;
                        }
                    }
                }

                //Generate the new branches
                HashSet<Branch> newBranches = new HashSet<Branch>();
                foreach (Branch b in Branches.Values)
                {
                    if (b.GrowCount > 0)    //if at least one leaf is affecting the branch
                    {
                        Vector avgDirection = b.GrowDirection / b.GrowCount;
                        avgDirection.Normalize();

                        Branch newBranch = new Branch(b, b.Position + avgDirection * BranchLength, avgDirection);

                        newBranches.Add(newBranch);
                        b.Reset();
                    }
                }

                //Add the new branches to the tree
                bool BranchAdded = false;
                foreach (Branch b in newBranches)
                {
                    //Check if branch already exists.  These cases seem to happen when leaf is in specific areas
                    Branch existing;
                    if (!Branches.TryGetValue(b.Position, out existing))
                    {
                        Branches.Add(b.Position, b);
                        BranchAdded = true;
                    }
                }

                //if no branches were added - we are done
                //this handles issues where leaves equal out each other, making branches grow without ever reaching the leaf
                if (!BranchAdded)
                    DoneGrowing = true;
            }
        }
    }
}

