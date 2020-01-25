using System;
using System.Collections.Generic;
//using Models.WorldGen;
using Pixel_Engine;

namespace UI
{
    class Edge
    {
        public float sx = 0, sy = 0;
        public float ex = 0, ey = 0;
    }

    class Cell
    {
        public int[] edgeId = new int[4];
        public bool[] edgeExist = new bool[4];
        public bool exist = false;
    }

    enum Direction
    {
        North, South, East, West
    }

    class ShadowCasting : Engine
    {
        Cell[] world;
        int worldWidth = 40;
        int worldHeight = 30;
        List<Edge> lisEdges;
        List<(float, float, float)> lisVisibilityPolygonPoints;
        void CalculateVisibilityPolygon(float ox, float oy, float radius)
        {
            lisVisibilityPolygonPoints.Clear();

            foreach (Edge edge1 in lisEdges)
            {
                for (int i = 0; i < 2; i++)
                {
                    float rdx, rdy;
                    rdx = (i == 0 ? edge1.sx : edge1.ex) - ox;
                    rdy = (i == 0 ? edge1.sy : edge1.ey) - oy;

                    float baseAngle = (float)Math.Atan2(rdy, rdx);

                    float angle = 0;

                    for (int j = 0; j < 3; j++)
                    {
                        angle = j == 0 ? baseAngle - 0.0001f : j == 1 ? baseAngle : baseAngle + 0.0001f;

                        rdx = radius * (float)Math.Cos(angle);
                        rdy = radius * (float)Math.Sin(angle);

                        float minT1 = float.PositiveInfinity, minPx = 0, minPy = 0, minAngle = 0;

                        bool Valid = false;

                        foreach (Edge edge2 in lisEdges)
                        {
                            float sdx, sdy;
                            sdx = edge2.ex - edge2.sx;
                            sdy = edge2.ey - edge2.sy;

                            if(Math.Abs(sdx - rdx) > 0 && Math.Abs(sdy - rdy) > 0)
                            {
                                float t2 = (rdx * (edge2.sy - oy) + (rdy * (ox - edge2.sx))) / (sdx * rdy - sdy * rdx);
                                float t1 = (edge2.sx + sdx * t2 - ox) / rdx;
                                if(t1 > 0 && t2 >= 0 && t2 <= 1)
                                {
                                    if(t1 < minT1)
                                    {
                                        minT1 = t1;
                                        minPx = ox + rdx * t1;
                                        minPy = oy + rdy * t1;
                                        minAngle = (float)Math.Atan2(minPy - oy, minPx - ox);
                                        Valid = true;
                                    }
                                }
                            }
                        }
                        if(Valid) lisVisibilityPolygonPoints.Add((minAngle, minPx, minPy));
                    }
                }
            }
            lisVisibilityPolygonPoints.Sort(new Comparison<(float, float, float)>((x, y) =>
            {
                if (x.Item1 < y.Item1)
                {
                    return -1;
                }
                else if (x.Item1 == y.Item1)
                {
                    return 0;
                }
                else return 1;
            }));
        }

        void ConvertTileMapToPolyMap(int sx, int sy, int wi, int h, float BlockWidth, int pitch)
        {
            lisEdges.Clear();

            for (int x = 0; x < wi; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        world[(y + sy) * pitch + (x + sx)].edgeId[j] = 0;
                        world[(y + sy) * pitch + (x + sx)].edgeExist[j] = false;
                    }
                }
            }

            for (int x = 1; x < wi - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    int i = (y + sy) * pitch + (x + sx);
                    int n = (y + sy - 1) * pitch + (x + sx);
                    int s = (y + sy + 1) * pitch + (x + sx);
                    int e = (y + sy) * pitch + (x + sx + 1);
                    int w = (y + sy) * pitch + (x + sx - 1);

                    if (world[i].exist)
                    {
                        if (!world[w].exist)
                        {
                            if (world[n].edgeExist[(int)Direction.West])
                            {
                                lisEdges[world[n].edgeId[(int)Direction.West]].ey += BlockWidth;
                                world[i].edgeId[(int)Direction.West] = world[n].edgeId[(int)Direction.West];
                                world[i].edgeExist[(int)Direction.West] = true;
                            }
                            else
                            {
                                Edge edge = new Edge();
                                edge.sx = (sx + x) * BlockWidth; edge.sy = (sy + y) * BlockWidth;
                                edge.ex = edge.sx; edge.ey = edge.sy + BlockWidth;

                                world[i].edgeId[(int)Direction.West] = lisEdges.Count;
                                world[i].edgeExist[(int)Direction.West] = true;
                                lisEdges.Add(edge);
                            }
                        }
                        if (!world[e].exist)
                        {
                            if (world[n].edgeExist[(int)Direction.East])
                            {
                                lisEdges[world[n].edgeId[(int)Direction.East]].ey += BlockWidth;
                                world[i].edgeId[(int)Direction.East] = world[n].edgeId[(int)Direction.East];
                                world[i].edgeExist[(int)Direction.East] = true;
                            }
                            else
                            {
                                Edge edge = new Edge();
                                edge.sx = (sx + x + 1) * BlockWidth; edge.sy = (sy + y) * BlockWidth;
                                edge.ex = edge.sx; edge.ey = edge.sy + BlockWidth;

                                world[i].edgeId[(int)Direction.East] = lisEdges.Count;
                                world[i].edgeExist[(int)Direction.East] = true;
                                lisEdges.Add(edge);
                            }
                        }
                        if (!world[n].exist)
                        {
                            if (world[w].edgeExist[(int)Direction.North])
                            {
                                lisEdges[world[w].edgeId[(int)Direction.North]].ex += BlockWidth;
                                world[i].edgeId[(int)Direction.North] = world[w].edgeId[(int)Direction.North];
                                world[i].edgeExist[(int)Direction.North] = true;
                            }
                            else
                            {
                                Edge edge = new Edge();
                                edge.sx = (sx + x) * BlockWidth; edge.sy = (sy + y) * BlockWidth;
                                edge.ex = edge.sx + BlockWidth; edge.ey = edge.sy;

                                world[i].edgeId[(int)Direction.North] = lisEdges.Count;
                                world[i].edgeExist[(int)Direction.North] = true;
                                lisEdges.Add(edge);
                            }
                        }
                        if (!world[s].exist)
                        {
                            if (world[w].edgeExist[(int)Direction.South])
                            {
                                lisEdges[world[w].edgeId[(int)Direction.South]].ex += BlockWidth;
                                world[i].edgeId[(int)Direction.South] = world[w].edgeId[(int)Direction.South];
                                world[i].edgeExist[(int)Direction.South] = true;
                            }
                            else
                            {
                                Edge edge = new Edge();
                                edge.sx = (sx + x) * BlockWidth; edge.sy = (sy + y + 1) * BlockWidth;
                                edge.ex = edge.sx + BlockWidth; edge.ey = edge.sy;

                                world[i].edgeId[(int)Direction.South] = lisEdges.Count;
                                world[i].edgeExist[(int)Direction.South] = true;
                                lisEdges.Add(edge);
                            }
                        }
                    }
                }
            }

        }

        public ShadowCasting()
        {
            sAppName = "ShadowCasting";
        }

        public override bool onUserUpdate(float fElapsedTime)
        {
            float BlockWidth = 16;
            float SourceX = GetMouseX();
            float SourceY = GetMouseY();

            if (GetMouse(0).bPressed)
            {
                int i = ((int)SourceY / (int)BlockWidth) * worldWidth + (int)SourceX / (int)BlockWidth;
                world[i].exist = !world[i].exist;
            }

            if (GetMouse(1).bHeld)
            {
                CalculateVisibilityPolygon(SourceX, SourceY, 1000);
            }

            Clear(Pixel.BLACK);

            if(GetMouse(1).bHeld && lisVisibilityPolygonPoints.Count > 1)
            {
                for (int i = 0; i < lisVisibilityPolygonPoints.Count - 1; i++)
                {
                    FillTriangle(
                        (int)SourceX,
                        (int)SourceY,

                        (int)lisVisibilityPolygonPoints[i].Item2,
                        (int)lisVisibilityPolygonPoints[i].Item3,

                        (int)lisVisibilityPolygonPoints[i + 1].Item2,
                        (int)lisVisibilityPolygonPoints[i + 1].Item3,

                        Pixel.YELLOW);
                }
                FillTriangle(
                        (int)SourceX,
                        (int)SourceY,

                        (int)lisVisibilityPolygonPoints[0].Item2,
                        (int)lisVisibilityPolygonPoints[0].Item3,

                        (int)lisVisibilityPolygonPoints[lisVisibilityPolygonPoints.Count - 1].Item2,
                        (int)lisVisibilityPolygonPoints[lisVisibilityPolygonPoints.Count - 1].Item3,

                        Pixel.YELLOW
                    );
            }

            for (int x = 0; x < worldWidth; x++)
            {
                for (int y = 0; y < worldHeight; y++)
                {
                    if (world[y * worldWidth + x].exist)
                    {
                        FillRect(x * (int)BlockWidth, y * (int)BlockWidth, (int)BlockWidth, (int)BlockWidth, Pixel.BLUE);
                    }
                }
            }

            if(GetMouse(0).bPressed) ConvertTileMapToPolyMap(0, 0, worldWidth, worldHeight, BlockWidth, worldWidth);

            foreach (Edge edge in lisEdges)
            {
                DrawLine((int)edge.sx, (int)edge.sy, (int)edge.ex, (int)edge.ey, Pixel.RED);
                FillCircle((int)edge.sx, (int)edge.sy, 3, Pixel.RED);
                FillCircle((int)edge.ex, (int)edge.ey, 3, Pixel.RED);
            }

            return true;
        }

        public override bool OnUserCreate()
        {
            world = new Cell[worldWidth * worldHeight];
            for (int i = 0; i < worldWidth * worldHeight; i++)
            {
                world[i] = new Cell();
            }
            lisEdges = new List<Edge>();
            lisVisibilityPolygonPoints = new List<(float, float, float)>();

            for (int i = 1; i < (worldWidth - 1); i++)
            {
                world[worldWidth + i].exist = true;
                world[(worldHeight - 2) * worldWidth + i].exist = true;
            }

            for (int i = 1; i < (worldHeight - 1); i++)
            {
                world[i * worldWidth + 1].exist = true;
                world[i * worldWidth + worldWidth - 2].exist = true;
            }

            ConvertTileMapToPolyMap(0, 0, worldWidth, worldHeight, 16, worldWidth);

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
            ShadowCasting demo = new ShadowCasting();
            if((int)demo.Construct(640, 480, 2, 2, false, true) == 1)
            {
                demo.Start();
            }
        }
    }
}