using System;
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

        public Vector Normal(double angle, double size)
        {
            double x = Math.Cos(angle) * size;
            double y = Math.Sin(angle) * size;
            return (x + X, y + Y);
        }

        public LinkedList<Vector> Subdivide(int sections, Vector target)
        {
            LinkedList<Vector> ret = new LinkedList<Vector>();
            double grad = (double)(target.Y - Y) / (target.X - X);
            double yInt = Y - grad * X;
            int xDis = (target.X - X)/sections;
            for (int i = 0; i < sections + 1; i++)
            {
                ret.AddLast((X + xDis * i, yInt + grad * (X + xDis * i)));
            }
            return ret;
        }
    }

    class LSytem
    {
        struct Rule
        {
            public readonly char input;
            public readonly string output;
            public Rule(char input, string output)
            {
                this.input = input;
                this.output = output;
            }
        }

        private List<Rule> Rules;
        public string currentSentence; 

        public LSytem(string axiom)
        {
            Rules = new List<Rule>();
            currentSentence = axiom;
        }

        public void AddRule(char input, string output)
        {
            Rules.Add(new Rule(input, output));
        }
        
        public void Generate()
        {
            string newSentence = "";
            foreach (char input in currentSentence)
            {
                foreach (Rule rule in Rules)
                {
                    if (input == rule.input)
                    {
                        newSentence += rule.output;
                    }
                }
            }
            currentSentence = newSentence;
        }
    }
}