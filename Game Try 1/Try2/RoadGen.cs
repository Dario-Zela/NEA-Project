﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Try_1
{
    struct Vector : IEquatable<Vector>
    {
        public int X;
        public int Y;

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        static public Vector Zero = (0, 0);

        static public Vector Max = (int.MaxValue, int.MaxValue);

        public double Distance(Vector other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }

        public double Length => Distance(new Vector(0, 0));

        public Vector Normalise => NormaliseSelf();

        private Vector NormaliseSelf()
        {
            X = X == 0 ? 0 : X * -1 == Math.Abs(X) ? -1 : 1;
            Y = Y == 0 ? 0 : Y * -1 == Math.Abs(Y) ? -1 : 1;
            return new Vector(X, Y);
        }

        public bool Equals(Vector other)
        {
            return X == other.X && Y == other.Y;
        }

        static public Vector operator +(Vector Vector1, Vector Vector2)
        {
            int x = Vector1.X + Vector2.X;
            int y = Vector1.Y + Vector2.Y;
            return new Vector(x, y);
        }

        static public Vector operator -(Vector Vector1, Vector Vector2)
        {
            int x = Vector1.X - Vector2.X;
            int y = Vector1.Y - Vector2.Y;
            return new Vector(x, y);
        }

        static public Vector operator *(Vector Vector1, double Vector2)
        {
            int x = (int)(Vector1.X * Vector2);
            int y = (int)(Vector1.Y * Vector2);
            return new Vector(x, y);
        }

        static public Vector operator /(Vector Vector1, double Vector2)
        {
            int x = (int)(Vector1.X / Vector2);
            int y = (int)(Vector1.Y / Vector2);
            return new Vector(x, y);
        }

        static public bool operator ==(Vector Vector1, Vector Vector2)
        {
            return Vector1.X == Vector2.X && Vector1.Y == Vector2.Y;
        }

        static public bool operator !=(Vector Vector1, Vector Vector2)
        {
            return !(Vector1.X == Vector2.X && Vector1.Y == Vector2.Y);
        }

        static public implicit operator Vector((int, int) vector)
        {
            int X = vector.Item1;
            int Y = vector.Item2;
            return new Vector(X, Y);
        }
        static public implicit operator Vector((double, double) vector)
        {
            return ((int)vector.Item1, (int)vector.Item2);
        }
        static public implicit operator Vector((float, float) vector)
        {
            return ((int)vector.Item1, (int)vector.Item2);
        }

        static public bool operator <(Vector vector1, Vector vector2)
        {
            return vector1.Length < vector2.Length;
        }

        static public bool operator <=(Vector vector1, Vector vector2)
        {
            return vector1.Length <= vector2.Length;
        }

        static public bool operator >(Vector vector1, Vector vector2)
        {
            return vector1.Length > vector2.Length;
        }

        static public bool operator >=(Vector vector1, Vector vector2)
        {
            return vector1.Length >= vector2.Length;
        }

        public bool inRange(Vector vector, int range)
        {
            bool xCond = vector.X < X + range && vector.X > X - range;
            bool yCond = vector.Y < Y + range && vector.Y > Y - range;
            return xCond && yCond;
        }

        public Vector nearestNode(List<Vector> OArray, params Vector[] toOmmit)
        {
            List<Vector> array = new List<Vector>(OArray);
            if (array.Contains(this)) array.Remove(this);
            foreach (Vector vector in toOmmit)
            {
                array.Remove(vector);
            }
            double distance = double.MaxValue;
            Vector closestNode = Vector.Max;
            foreach (Vector node in array)
            {
                if (Distance(node) < distance)
                {
                    distance = Distance(node);
                    closestNode = node;
                }
            }
            return closestNode;
        }

        static public Vector FindIntersection(Vector start1, Vector end1, Vector start2, Vector end2)
        {
            if (end1 == start1 && end2 == start2) return Vector.Max;
            else
            {
                double grad1, yInt1, grad2, yInt2, intX, intY;

                grad1 = (double)(end1.Y - start1.Y) / (double)(end1.X - start1.X);
                yInt1 = start1.Y - grad1 * start1.X;
                grad2 = (double)(end2.Y - start2.Y) / (double)(end2.X - start2.X);
                yInt2 = start2.Y - grad2 * start2.X;

                if (grad1 == grad2) return Vector.Max;

                if (yInt1 == yInt2) return (yInt2, 0);

                intX = (yInt2 - yInt1) / (grad1 - grad2);
                intY = grad2 * intX + yInt1;
                return (intX, intY);
            }
        }

        static public bool doIntersect(Vector start1, Vector end1, Vector start2, Vector end2)
        {
            int intersect = FindIntersection(start1, end1, start2, end2).X;
            if (intersect == int.MaxValue) return false;

            int boundX1 = start1.X < end1.X ? start1.X : end1.X, boundX2 = start1.X > end1.X ? start1.X : end1.X;
            int boundX3 = start2.X < end2.X ? start2.X : end2.X, boundX4 = start2.X > end2.X ? start2.X : end2.X;

            bool check1 = boundX1 < intersect && boundX2 > intersect;
            bool check2 = boundX3 < intersect && boundX4 > intersect;

            return check1 && check2;
        }

        static public double angle(Vector node1, Vector node2, Vector node3)
        {
            double X = Math.Sqrt(Math.Pow(node2.Y - node1.Y, 2) - Math.Pow(node3.X - node2.X, 2));
            double Y = Math.Sqrt(Math.Pow(node2.Y - node3.Y, 2) - Math.Pow(node2.X - node3.X, 2));
            return Math.Atan2(Y, X);
        }
    }

    class Graph
    {
        private Dictionary<Vector, List<Vector>> graph;
        public List<Vector> keys => graph.Keys.ToList();

        public Graph()
        {
            graph = new Dictionary<Vector, List<Vector>>();
        }

        public void addNode(Vector position)
        {
            if (!graph.ContainsKey(position)) graph.Add(position, new List<Vector>());
        }

        public void addConnection(Vector node1, Vector node2)
        {
            if (graph[node1].Count == 0 || !graph[node1].Contains(node2))
            {
                graph[node1].Add(node2);
                graph[node2].Add(node1);
            }
        }

        public int Count => graph.Count;

        public void removeNode(Vector node)
        {
            if (graph.ContainsKey(node))
            {
                List<Vector> toDel = graph[node];
                graph.Remove(node);
                foreach (Vector key in toDel)
                {
                    graph[key].Remove(node);
                }
            }
        }

        public void removeEdge(Vector node1, Vector node2)
        {
            if (graph.ContainsKey(node1) && graph.ContainsKey(node2))
            {
                graph[node1].Remove(node2);
                graph[node2].Remove(node1);
                if (graph[node1].Count == 0) removeNode(node1);
                if (graph[node2].Count == 0) removeNode(node2);
            }
        }

        public List<Vector> this[Vector key]
        {
            get
            {
                if (graph.ContainsKey(key))
                {
                    return graph[key];
                }
                else
                {
                    return null;
                }
            }
        }
    }

    class RoadNetwork
    {
        public Graph Graph = new Graph();
        public List<Vector> keys => Graph.keys;
        public LinkedList<(Vector, Vector)> toDel;

        public RoadNetwork(int mapHeight, int mapWidth, double[,] heightMap, int numRows, int seed)
        {
            SetUp(mapHeight, mapWidth, numRows, seed);
        }

        private void SetUp(int mapHeight, int mapWidth, int numRows, int seed)
        {
            Random rand = new Random(seed);
            Vector[,] grid = new Vector[numRows, numRows];
            CreateNodes(mapHeight, mapWidth, numRows, rand, ref grid);
            addAllEdges(numRows, grid);
        }

        private void CreateNodes(int mapHeight, int mapWidth, int numRows, Random rand, ref Vector[,] grid)
        {
            for (int i = 1; i < numRows + 1; i++)
            {
                for (int j = 1; j < numRows + 1; j++)
                {
                    int X = rand.Next((mapWidth * (i - 1) / numRows) + (mapWidth / (numRows * 4)), (mapWidth * i / numRows) - (mapWidth / (numRows * 4)));
                    int Y = rand.Next((mapHeight * (j - 1) / numRows) + (mapHeight / (numRows * 4)), (mapHeight * j / numRows) - (mapHeight / (numRows * 4)));
                    Graph.addNode((X, Y));
                    grid[i - 1, j - 1] = (X, Y);
                }
            }
        }

        private void addAllEdges(int numRows, Vector[,] grid)
        {
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numRows; j++)
                {
                    try
                    {
                        Graph.addConnection(grid[i, j], grid[i + 1, j]);
                    }
                    catch { };
                    try
                    {
                        Graph.addConnection(grid[i, j], grid[i, j - 1]);
                    }
                    catch { };
                }
            }
        }
    }
}