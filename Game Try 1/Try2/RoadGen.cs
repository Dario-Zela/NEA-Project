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

        public double Lenght => Distance(new Vector(0, 0));

        public Vector Normalise => NormaliseSelf();

        public Vector NormaliseSelf()
        {
            X = X == 0 ? 0 : X / Math.Abs(X);
            Y = Y == 0 ? 0 : Y / Math.Abs(Y);
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
            return vector1.Lenght < vector2.Lenght;
        }

        static public bool operator <=(Vector vector1, Vector vector2)
        {
            return vector1.Lenght <= vector2.Lenght;
        }

        static public bool operator >(Vector vector1, Vector vector2)
        {
            return vector1.Lenght > vector2.Lenght;
        }

        static public bool operator >=(Vector vector1, Vector vector2)
        {
            return vector1.Lenght >= vector2.Lenght;
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
        private List<Road> Children;
        public Vector PrePosition = Vector.Max;
        private readonly double minLenght;
        public bool isGoingToSplit = false;
        public int counter;
        public Vector dirSplit;
        public int counterSplit;

        public Road(Vector Position, Road Parent, Vector dir, double minLenght)
        {
            this.Position = Position;
            this.Parent = Parent;
            this.dir = dir;
            Children = new List<Road>();
            this.minLenght = minLenght;
        }

        public bool isValid()
        {
            return Position.Distance(Parent.Position) > minLenght;
        }

        public List<Road> createIntersection()
        {
            Road childRoad1 = new Road(Position + (dir / counter).Normalise, this, (dir / counter).Normalise, minLenght);
            Road childRoad2 = new Road(PrePosition + (dirSplit / counterSplit).Normalise, this, (dirSplit / counterSplit).Normalise, minLenght);
            Children.Add(childRoad1);
            Children.Add(childRoad2);
            return Children;
        }

        public void Move()
        {
            Position = Position + (dir / counter).Normalise;
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

        public RoadNetwork(double minDistance, double maxDistance, Vector startPosition, int mapHeight, int mapWidth, int numAttPoints, int seed, double minLenght)
        {
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
            AttractionPoints = new List<AttractionPoint>();
            Roads = new List<Road>();
            SetUp(startPosition, mapHeight, mapWidth, numAttPoints, seed, minLenght);
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
                    if(direction.Lenght < minDistance)
                    {
                        toDel.Add(AttractionPoint);
                        break;
                    }
                    else if(direction.Lenght > maxDistance) { }
                    else if(direction.Lenght < distance)
                    {
                        AttractionPoint.ClosestRoad = Road;
                        distance = direction.Lenght;
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
                            direction = direction2;
                            AttractionPoint.ClosestRoad.isGoingToSplit = true;
                            direction.NormaliseSelf();
                            AttractionPoint.ClosestRoad.dirSplit += direction;
                            AttractionPoint.ClosestRoad.counter++;
                        }
                        else
                        {
                            direction.NormaliseSelf();
                            AttractionPoint.ClosestRoad.dir += direction;
                            AttractionPoint.ClosestRoad.counterSplit++;
                        }
                    }
                    else
                    {
                        direction.NormaliseSelf();
                        AttractionPoint.ClosestRoad.dir += direction;
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
                    if (Road.isGoingToSplit)
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

        private void SetUp(Vector startPosition, int mapHeight, int mapWidth, int numAttPoints, int seed, double minLenght)
        {
            Random random = new Random(seed);
            for (int i = 0; i < numAttPoints; i++)
            {
                AttractionPoints.Add(new AttractionPoint((random.Next(0, mapWidth), random.Next(0, mapHeight))));
            }
            Roads.Add(new Road(startPosition, null, (10,10), minLenght));
        }
    }
}
