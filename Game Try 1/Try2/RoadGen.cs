using System;
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

        static public Vector Zero = (0,0);

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

        static public implicit operator Vector((int,int) vector)
        {
            int X = vector.Item1;
            int Y = vector.Item2;
            return new Vector(X, Y);
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
    }

    class AttractionPoint
    {
        public Vector Position;
        public Road ClosestRoad;

        public AttractionPoint(Vector pos)
        {
            Position = pos;
        }

        public void Reset()
        {
            ClosestRoad = null;
        }
    }

    class Road
    {
        public Vector Position;
        public Road Parent;
        public Vector dir;
        public List<Road> Children;
        public Vector PrePosition = Vector.Max;
        private readonly double minLength;
        public int counter;
        public Vector dirSplit;
        public int counterSplit;

        public Road(Vector Position, Road Parent, Vector dir, double minLength)
        {
            this.Position = Position;
            this.Parent = Parent;
            this.dir = dir;
            Children = new List<Road>();
            this.minLength = minLength;
            dirSplit = (0, 0);
            counter = 0;
        }

        public bool isValid()
        {
            if (Parent != null)
            {
                return Position.Distance(Parent.Position) > minLength;
            }
            else
            {
                return true;
            }
        }

        public List<Road> createIntersection()
        {
            Road childRoad1 = new Road(Position + (dir / (counter + 1)).Normalise, this, (dir / (counter + 1)).Normalise, minLength);
            Road childRoad2 = new Road(PrePosition + (dirSplit / (counterSplit + 1)).Normalise, this, (dirSplit / (counterSplit + 1)).Normalise, minLength);
            Children.Add(childRoad1);
            Children.Add(childRoad2);
            return Children;
        }

        public void Move()
        {
            PrePosition = Position;
            Position += (dir  / (counter + 1)).Normalise;
        }

        public void Reset()
        {
            counter = 0;
            counterSplit = 0;
        }
    }

    class RoadNetwork
    {
        private double maxDistance;
        private double minDistance;
        public List<AttractionPoint> AttractionPoints;
        public List<Road> Roads;

        public RoadNetwork(double minDistance, double maxDistance, Vector startPosition, int mapHeight, int mapWidth, int numAttPoints, int seed, double minLength)
        {
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
            AttractionPoints = new List<AttractionPoint>();
            Roads = new List<Road>();
            SetUp(startPosition, mapHeight, mapWidth, numAttPoints, seed, minLength);
            /*
            do
            {
                Grow();
            }
            while (AttractionPoints.Count != 0);

            List<Road> toDel = new List<Road>();
            foreach (Road Road in Roads)
            {
                if (!Road.isValid() && !toDel.Contains(Road))
                {
                    Road.Parent.Children.Remove(Road);
                    foreach (var child in Road.Children)
                    {
                        toDel.Add(child);
                    }
                    toDel.Add(Road);
                }
            }

            foreach (Road road in toDel)
            {
                Roads.Remove(road);
            }
            */
        }

        public void Grow()
        {
            List<AttractionPoint> toDel = new List<AttractionPoint>();
            foreach (AttractionPoint AttractionPoint in AttractionPoints)
            {
                Vector direction = Vector.Zero;
                double distance = double.PositiveInfinity;
                foreach (Road Road in Roads)
                {
                    direction = AttractionPoint.Position - Road.Position;
                    if(direction.Length < minDistance)
                    {
                        toDel.Add(AttractionPoint);
                        break;
                    }
                    else if(direction.Length > maxDistance) { }
                    else if(direction.Length < distance)
                    {
                        AttractionPoint.ClosestRoad = Road;
                        distance = direction.Length;
                    }
                }
                if(AttractionPoint.ClosestRoad != null)
                {
                    direction = AttractionPoint.Position - AttractionPoint.ClosestRoad.Position;
                    if (AttractionPoint.ClosestRoad.PrePosition != Vector.Max)
                    {
                        Vector direction2 = AttractionPoint.Position - AttractionPoint.ClosestRoad.PrePosition;
                        if (direction2 < direction)
                        {
                            AttractionPoint.ClosestRoad.dirSplit += direction2.Normalise;
                            AttractionPoint.ClosestRoad.counterSplit++;
                        }
                        else
                        {
                            AttractionPoint.ClosestRoad.dir += direction.Normalise;
                            AttractionPoint.ClosestRoad.counter++;
                        }
                    }
                    else
                    {
                        AttractionPoint.ClosestRoad.dir += direction.Normalise;
                        AttractionPoint.ClosestRoad.counter++;
                    }
                }
                AttractionPoint.Reset();
            }

            foreach (AttractionPoint AttractionPoint in toDel)
            {
                AttractionPoints.Remove(AttractionPoint);
            }

            List<Road> toAdd = new List<Road>();

            foreach (Road Road in Roads)
            {
                if(Road.counter != 0 || Road.counterSplit != 0)
                {
                    if (Road.counterSplit != 0)
                    {
                        toAdd.AddRange(Road.createIntersection());
                    }
                    else
                    {
                        Road.Move();
                    }
                }
                Road.Reset();
            }

            Roads.AddRange(toAdd);
        }

        private void SetUp(Vector startPosition, int mapHeight, int mapWidth, int numAttPoints, int seed, double minLength)
        {
            Random random = new Random(seed);
            for (int i = 0; i < numAttPoints; i++)
            {
                AttractionPoints.Add(new AttractionPoint((random.Next(0, mapWidth), random.Next(0, mapHeight))));
            }
            Roads.Add(new Road(startPosition, null, (0,0), minLength));
        }
    }
}
