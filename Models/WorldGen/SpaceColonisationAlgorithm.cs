using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

//taken from https://www.researchgate.net/publication/221314843_Modeling_Trees_with_a_Space_Colonization_Algorithm
//and Code Train;
namespace Models.WorldGen
{
    class Node
    {
        public (int, int) pos;
        public float height;
        public bool reached;

        public Node(float height, int mapHeight, int mapWidth)
        {
            Random random = new Random();
            pos = (random.Next(0, mapHeight + 1), random.Next(0, mapWidth + 1));
            this.height = height;
        }
    }

    class Road
    {
        public (int, int) pos;
        public Road parent;
        public Vector direction;
        public readonly Vector originalDirection;
        public int count = 0;

        public void Reset()
        {
            direction = originalDirection;
            count = 0;
        }

        public Road((int, int) pos, Vector direction ,Road parent)
        {
            this.pos = pos;
            this.parent = parent;
            direction.Normalize();
            this.direction = direction;
            originalDirection = direction;
        }

        public Road NextRoad()
        {
            Road newRoad = new Road((pos.Item1 + (int)direction.X, pos.Item2 + (int)direction.Y), direction, this);
            return newRoad;
        }
    }

    class City
    {
        public LinkedList<Node> nodes;
        public Road current;
        public LinkedList<Road> roads = new LinkedList<Road>();
        public float minDistance;
        public float maxDistance;

        public float Distance((int, int) pos1, (int, int) pos2)
        {
            double distance = new Vector(pos1.Item1 - pos2.Item1, pos1.Item2 - pos2.Item2).Length;
            return (float)distance;
        }

        public City(int numNodes, float height, int mapHeght, int mapWidth, float minDistance, float maxDistance)
        {
            this.maxDistance = maxDistance;
            this.minDistance = minDistance;
            for (int i = 0; i < numNodes; i++)
            {
                nodes.AddLast(new Node(height, mapHeght, mapWidth));
            }
            Random random = new Random();
            current = new Road((mapHeght / 2, mapWidth / 2), new Vector(random.Next(0,2), random.Next(0,2)) ,null);
            roads.AddLast(current);
            Start();
            Grow();
        }

        public void Start()
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
                    if(distance < minDistance)
                    {
                        node.reached = true;
                        closestRoad = null;
                        break;
                    }
                    else if(distance > maxDistance) { }
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
                }
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                var currentNode = nodes.First;
                if (currentNode.Value.reached)
                {
                    currentNode = currentNode.Next;
                    nodes.Remove(currentNode.Previous);
                }
            }

            LinkedList<Road> temp = new LinkedList<Road>();

            foreach (var road in roads)
            {
                if(road.count > 0)
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
        public SpaceColonisationAlgorithm()
        {

        }
    }
}
