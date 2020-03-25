using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Algorithms
{
    class AStar<T>
    {
        public AStar(T[] Map, int _MapWidth, int _MapHeight, Func<T[],Node[]> Converter, int StartLoc, int EndLoc)
        {
            this.Converter = Converter;
            nodes = Converter(Map);
            MapHeight = _MapHeight;
            MapWidth = _MapWidth;
            Start = nodes[StartLoc];
            End = nodes[EndLoc];
            SolveAStar();
        }

        Func<T[], Node[]> Converter;
        Node[] nodes;
        Node Start;
        Node End;
        int MapWidth;
        int MapHeight;
        public float BestDistance
        {
            get
            {
                if (Start == End) return 1;
                else if (End.Parent == null) return float.PositiveInfinity;
                float Distance = 0;
                Node Current = End;
                Func<Node, Node, float> distance = new Func<Node, Node, float>((a, b) =>
                {
                    return (float)Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
                });
                do
                {
                    Distance += distance(Current.Parent, Current);
                    Current = Current.Parent;
                }
                while (Current.Parent != null);
                return Distance;
            }
        }
        public List<int> BestPath
        {
            get
            {
                if (Start == End) return null;
                List<int> Path = new List<int>();
                Node Current = End;
                do
                {
                    Path.Add(Current.y * MapWidth + Current.x);
                    Current = Current.Parent;
                }
                while (Current.Parent != null);
                return Path;
            }
        }

        void SolveAStar()
        {
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    nodes[y * MapWidth + x].Visited = false;
                    nodes[y * MapWidth + x].GlobalGoal = float.PositiveInfinity;
                    nodes[y * MapWidth + x].LocalGoal = float.PositiveInfinity;
                    nodes[y * MapWidth + x].Parent = null;
                }
            }

            Func<Node, Node, float> distance = new Func<Node, Node, float>((a, b) =>
            {
                return (float)Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
            });

            Func<Node, Node, float> heuristic = new Func<Node, Node, float>((a, b) =>
            {
                return distance(a, b);
            });

            Node Current = Start;
            Start.LocalGoal = 0f;
            Start.GlobalGoal = heuristic(Start, End);

            List<Node> ToBeTested = new List<Node>();
            ToBeTested.Add(Start);

            while (ToBeTested.Count != 0 && Current != End)
            {
                ToBeTested.Sort(new Comparison<Node>((lhs, rhs) =>
                {
                    return lhs.GlobalGoal < rhs.GlobalGoal ? -1 : 1;
                }));

                ToBeTested.RemoveAll(new Predicate<Node>((node) =>
                {
                    return node.Visited;
                }));

                if (ToBeTested.Count == 0)
                    break;

                Current = ToBeTested.First();
                Current.Visited = true;

                foreach (Node Neighbour in Current.Neighbours)
                {
                    if (!Neighbour.Visited && !Neighbour.Obstacle)
                    {
                        ToBeTested.Add(Neighbour);
                    }
                    float PossiblyLowerGoal = Current.LocalGoal + distance(Current, Neighbour);

                    if (PossiblyLowerGoal < Neighbour.LocalGoal)
                    {
                        Neighbour.Parent = Current;
                        Neighbour.LocalGoal = PossiblyLowerGoal;
                        Neighbour.GlobalGoal = Neighbour.LocalGoal + heuristic(Neighbour, End);
                    }
                }


            }
        }

        public void ReEval(T[] Map, int StartLoc, int EndLoc)
        {
            nodes = Converter(Map);
            Start = nodes[StartLoc];
            End = nodes[EndLoc];
            SolveAStar();
        }

        public void EditStart(int StartLoc)
        {
            Start = nodes[StartLoc];
            SolveAStar();
        }
        public void EditEnd(int EndLoc)
        {
            End = nodes[EndLoc];
            SolveAStar();
        }
    }
    class Node
    {
        public bool Obstacle = false;
        public bool Visited = false;
        public float GlobalGoal;
        public float LocalGoal;
        public int x;
        public int y;
        public List<Node> Neighbours = new List<Node>();
        public Node Parent;
    }
}
