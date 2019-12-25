﻿using System;
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
                    if (dir.Length() < minDist)
                    {
                        temp.AddLast(leaf);
                    }
                    else if (dir.Length() > maxDist) { }
                    else if (dir.Length() < distance)
                    {
                        leaf.closestBranch = branch;
                        distance = dir.Length();
                    }
                }
                if (leaf.closestBranch != null)
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
                if (branch.count != 0)
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

    class RoadNode
    {
        public Vector2 position;
        public List<RoadEdge> targets;
        public RoadNode previous;

        public RoadNode(Vector2 position, RoadNode previous)
        {
            this.position = position;
            this.previous = previous;
            targets = new List<RoadEdge>();
        }

        public void addEdge(RoadNode Target)
        {
            targets.Add(new RoadEdge(this, Target));
        }
    }
    class RoadEdge
    {
        public RoadNode Source;
        public RoadNode Target;

        public RoadEdge(RoadNode Source, RoadNode Target)
        {
            this.Source = Source;
            this.Target = Target;
        }
    }
    class RoadGraph
    {
        public List<RoadNode> graph;

        public RoadGraph()
        {
            graph = new List<RoadNode>();
        }

        public RoadNode addNode(Vector2 Position, RoadNode Source)
        {
            RoadNode RoadNode;
            if (Source != null)
            {
                int index = graph.IndexOf(Source);
                RoadNode = new RoadNode(Position, graph[index]);
            }
            else
            {
                RoadNode = new RoadNode(Position, null);
            }
            graph.Add(RoadNode);
            return RoadNode;
        }

        public void addEdge(RoadNode Source, RoadNode Target)
        {
            int index = graph.IndexOf(Target);
            int index2 = graph.IndexOf(Source);
            graph[index2].addEdge(graph[index]);
        }

        public bool hasEdge(RoadNode Source, RoadNode Target)
        {
            foreach (var RoadEdge in Source.targets)
            {
                if (RoadEdge.Target == Target)
                {
                    return true;
                }
            }
            return false;
        }
    }

    class BlockGraph
    {
        List<Block> blocks;

        public BlockGraph(RoadGraph road, int Size)
        {
            blocks = new List<Block>();
            List<RoadNode> EndPoints = new List<RoadNode>();
            foreach (var item in road.graph)
            {
                if(item.targets.Count == 0)
                {
                    EndPoints.Add(item);
                }
            }
            foreach (var EndPoint in EndPoints)
            {
                List<RoadNode> roads = new List<RoadNode>();
                roads.Add(EndPoint);
                RoadNode node = null;
                var Source = EndPoint.previous;
                do
                {
                    roads.Add(Source);
                    double angle = 90;
                    foreach (var Target in Source.targets)
                    {
                        var X = Math.Sqrt(Math.Pow(EndPoint.position.X, 2) + Math.Pow(EndPoint.position.Y, 2));
                        var Y = Math.Sqrt(Math.Pow(EndPoint.position.X - Target.Target.position.X, 2) + Math.Pow(EndPoint.position.Y - Target.Target.position.Y, 2));
                        try
                        {
                            if (Math.Atan2(Y, X) < angle)
                            {
                                node = Target.Target;
                                angle = Math.Atan2(Y, X);
                            }
                        }
                        catch { }
                    }
                    Source = Source.previous;
                }
                while (node == null);
                roads.Add(node);
                blocks.Add(new Block(Size, roads));
            }

        }

        internal class Block
        {
            int Size;
            List<RoadNode> Face;

            public Block(int Size, List<RoadNode> Face)
            {
                this.Size = Size;
                this.Face = Face;
            }
        }
    }

    class City
    {
        Vector2 root;
        Tree tree;
        public Branch[,] map;
        public RoadGraph roadMap;
        BlockGraph blocks;
        public List<Vector2> Nodes = new List<Vector2>();

        private void createMap(int Width, int Height)
        {
            root = new Vector2((float)Width / 2, (float)Height / 2);
            tree = new Tree(200, 20, root, Height, Width, 200);
            map = new Branch[Width, Height];
            foreach (var road in tree.Branches)
            {
                map[(int)road.position.X, (int)road.position.Y] = road;
            }
        }

        public City(int Width, int Height)
        {
            createMap(Width, Height);

            createRoadsAndBlocks(Width, Height);
        }

        private void createRoadsAndBlocks(int Width, int Height)
        {
            roadMap = new RoadGraph();

            for (int i = 0; i<Width; i++)
            {
                for (int j = 0; j<Height; j++)
                {
                    if (map[i, j] != null)
                    {
                        int adjecentRoads = 0;
                        for (int k = -1; k< 2; k++)
                        {
                            for (int l = -1; l < 2; l++)
                            {
                                if (map[i + k, j + l] != null)
                                {
                                    adjecentRoads++;
                                }
                            }
                        }
                        if(adjecentRoads == 2)
                        {
                            Nodes.Add(new Vector2(i, j));
                        }
                        else if(adjecentRoads > 4)
                        {
                            adjecentRoads = checkAround(i, j, map, 2);
                            if(adjecentRoads > 5)
                            {
                                Nodes.Add(new Vector2(i, j));
                            }
                        }
                    }
                }
            }

            List<Vector2> toDel = new List<Vector2>();

            foreach (var node1 in Nodes)
            {
                List<Vector2> closeNodes = new List<Vector2>();
                foreach (var node2 in Nodes)
                {
                    bool cond1 = node2.X < node1.X + 3 && node2.X > node1.X - 3;
                    bool cond2 = node2.Y < node1.Y + 3 && node2.Y > node1.Y - 3;
                    if (cond1 && cond2)
                    {
                        closeNodes.Add(node2);
                    }
                }
                if(closeNodes.Count != 1)
                {
                    int xSum = 0;
                    int ySum = 0;
                    foreach (var node in closeNodes)
                    {
                        xSum += (int)node.X;
                        ySum += (int)node.Y;
                    }
                    xSum /= closeNodes.Count;
                    ySum /= closeNodes.Count;
                    closeNodes.Remove(new Vector2(xSum, ySum));
                    toDel.AddRange(closeNodes);
                }
            }

            foreach (var node in toDel)
            {
                Nodes.Remove(node);
            }

            Nodes.Remove(root);
            roadMap.addNode(root, null);
            addNodes(Nodes, roadMap.graph[0]);
            Nodes.Add(root);
            //blocks = new BlockGraph(roadMap, 10);
        }

        public int checkAround(int x, int y, Branch[,] map, int Size)
        {
            int adjecentRoads = 0;
            for (int i = -Size; i < Size + 1; i++)
            {
                if (map[x -1, y + i] != null)
                {
                    adjecentRoads++;
                }
                if (map[x + 1, y + i] != null)
                {
                    adjecentRoads++;
                }
            }
            for (int i = -Size; i < Size + 1; i++)
            {
                if (map[x + i, y - 1] != null)
                {
                    adjecentRoads++;
                }
                if (map[x + i, y - 1] != null)
                {
                    adjecentRoads++;
                }
            }
            return adjecentRoads;
        }

        public List<(Vector2, Vector2)> FindPairs(List<Vector2> Nodes)
        {
            List<(Vector2, Vector2)> pairs = new List<(Vector2, Vector2)>();
            foreach (var node in Nodes)
            {
                var temp = FindSource(Nodes, node);
                pairs.Add((node, temp));
            }
            return pairs;
        }

        public void addNodes(List<Vector2> Nodes, RoadNode Source)
        {
            var pairs = FindPairs(Nodes);
            List<RoadNode> nodes = new List<RoadNode>();
            nodes.Add(Source);
            do
            {
                List<RoadNode> temp2 = new List<RoadNode>();
                foreach (var pair in pairs)
                {
                    foreach (var source in nodes)
                    {
                        if (pair.Item2 == source.position)
                        {
                            var temp = roadMap.addNode(pair.Item1, source);
                            roadMap.addEdge(source, temp);
                            temp2.Add(temp);
                        }
                    }
                }
                nodes = temp2;
            } while (nodes.Capacity != 0);
        }

        public Vector2 FindSource(List<Vector2> Nodes, Vector2 pos)
        {
            var currentPos = map[(int)pos.X, (int)pos.Y];
            Branch branch = currentPos;

            foreach (var Node in Nodes)
            {
                var possibleSource = map[(int)Node.X, (int)Node.Y];
                do
                {
                    currentPos = currentPos.parent;
                }
                while (currentPos != possibleSource & currentPos.parent != null);
                if(currentPos == possibleSource)
                {
                    return new Vector2(currentPos.position.X, currentPos.position.Y);
                }
                else
                {
                    currentPos = branch;
                }
            }
            return root;
        }
    }
}
