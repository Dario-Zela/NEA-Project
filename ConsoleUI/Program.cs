using System;
using System.Collections.Generic;
//using Models.WorldGen;
using Pixel_Engine;
using System.Linq;

namespace UI
{
    class Point
    {
        public float x = 0;
        public float y = 0;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Spline
    {
        public List<Point> points = new List<Point>();

        public Point GetSplinePoint(float t, bool Looped = false)
        {
            int p0, p1, p2, p3;
            if (!Looped)
            {
                p1 = (int)t + 1;
                p2 = p1 + 1;
                p3 = p2 + 1;
                p0 = p1 - 1;
            }
            else
            {
                p1 = (int)t;
                p2 = (p1 + 1) % points.Count;
                p3 = (p2 + 1) % points.Count;
                p0 = p1 >= 1 ? p1 - 1 : points.Count - 1;
            }

            t = t - (int)t;

            float tt = t * t;
            float ttt = tt * t;

            float q0 = -ttt + 2 * tt - t;
            float q1 = 3 * ttt - 5 * tt + 2;
            float q2 = -3 * ttt + 4 * tt + t;
            float q3 = ttt - tt;

            float tx = points[p0].x * q0 + points[p1].x * q1 + points[p2].x * q2 + points[p3].x * q3;
            float ty = points[p0].y * q0 + points[p1].y * q1 + points[p2].y * q2 + points[p3].y * q3;

            return new Point(tx * 0.5f, ty * 0.5f);
        }

        public Point GetSplineGrad(float t, bool Looped = false)
        {
            int p0, p1, p2, p3;
            if (!Looped)
            {
                p1 = (int)t + 1;
                p2 = p1 + 1;
                p3 = p2 + 1;
                p0 = p1 - 1;
            }
            else
            {
                p1 = (int)t;
                p2 = (p1 + 1) % points.Count;
                p3 = (p2 + 1) % points.Count;
                p0 = p1 >= 1 ? p1 - 1 : points.Count - 1;
            }

            t = t - (int)t;

            float tt = t * t;
            float ttt = tt * t;

            float q0 = -3 * tt + 4 * t - 1;
            float q1 = 9 * tt - 10 * t;
            float q2 = -9 * tt + 8 * t + 1;
            float q3 = 3 * tt - 2 * t;

            float tx = points[p0].x * q0 + points[p1].x * q1 + points[p2].x * q2 + points[p3].x * q3;
            float ty = points[p0].y * q0 + points[p1].y * q1 + points[p2].y * q2 + points[p3].y * q3;

            return new Point(tx * 0.5f, ty * 0.5f);
        }
    }

    class Splines : Engine
    {

        Spline path = new Spline();

        float agentLoc = 0;

        public Splines()
        {
            sAppName = "Splines";
        }

        int SelectedPoint = 0;

        public override bool onUserUpdate(float fElapsedTime)
        {
            Clear(Pixel.BLACK);

            if (GetKey(Key.X).bPressed)
            {
                SelectedPoint = (SelectedPoint + 1) % path.points.Count;
            }

            if (GetKey(Key.Z).bPressed)
            {
                SelectedPoint--;
                SelectedPoint = SelectedPoint == -1 ? path.points.Count - 1 : SelectedPoint;
            }

            if (GetKey(Key.UP).bHeld)
            {
                path.points[SelectedPoint].y -= 30f * fElapsedTime;
            }

            if (GetKey(Key.DOWN).bHeld)
            {
                path.points[SelectedPoint].y += 30f * fElapsedTime;
            }

            if (GetKey(Key.LEFT).bHeld)
            {
                path.points[SelectedPoint].x -= 30f * fElapsedTime;
            }

            if (GetKey(Key.RIGHT).bHeld)
            {
                path.points[SelectedPoint].x += 30f * fElapsedTime;
            }

            if (GetKey(Key.SPACE).bHeld) agentLoc += 5 * fElapsedTime;

            if (agentLoc >= path.points.Count) agentLoc -= path.points.Count;

            for (float t = 0; t < (float)path.points.Count; t += 0.005f)
            {
                Point pos = path.GetSplinePoint(t, true);
                Draw((int)pos.x, (int)pos.y, Pixel.WHITE);
            }

            for (int i = 0; i < path.points.Count; i++)
            {
                FillRect((int)path.points[i].x - 1, (int)path.points[i].y - 1, 2, 2, Pixel.BLUE);
            }

            FillRect((int)path.points[SelectedPoint].x - 1, (int)path.points[SelectedPoint].y - 1, 2, 2, Pixel.YELLOW);

            Point p1 = path.GetSplinePoint(agentLoc, true);
            Point p2 = path.GetSplineGrad(agentLoc, true);

            float rad = (float)Math.Atan2(-p2.y, p2.x);
            DrawLine((int)(5 * Math.Sin(rad) + p1.x), (int)(5 * Math.Cos(rad) + p1.y), (int)(-5 * Math.Sin(rad) + p1.x), (int)(-5 * Math.Cos(rad) + p1.y), Pixel.RED);

            return true;
        }

        public override bool OnUserCreate()
        {
            for (int i = 1; i < 11; i++)
            {
                path.points.Add(new Point(10 * i, 41));
            }

            return true;
        }
        /*
        static void Main()
        {
            Console.Title = "UI";
            Console.WindowWidth = 128;
            Console.WindowHeight = 40;
            Console.CursorVisible = false;
            Console.BufferHeight = 128;
            WorldCreator creator = new WorldCreator(new Random().Next(1, 10000));
            string output = "";
            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    var item = creator.World.landBlocks[creator.World.idx(i, j)];
                    switch (item.type)
                    {
                        case 1:
                            output += "~";
                            break;
                        case 2:
                            output += "#";
                            break;
                        case 3:
                            output += "\"";
                            break;
                        case 4:
                            output += "-";
                            break;
                        case 5:
                            output += "=";
                            break;
                        case 6:
                            output += "+";
                            break;
                        case 7:
                            output += "|";
                            break;
                        case 8:
                            output += "¬";
                            break;
                        case 9:
                            output += "¦";
                            break;
                        default:
                            break;
                    }
                }
            }
            Console.Write(output);
            Console.Read();
        }
        */
    }

    class Test
    {
        static void Main()
        {
            Splines demo = new Splines();
            if((int)demo.Construct(160, 80, 10, 10, false, true) == 1)
            {
                demo.Start();
            }
        }
    }
}