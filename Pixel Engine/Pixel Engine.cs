using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using SharpGL;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Pixel_Engine
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Pixel
    {
        [FieldOffset(0)]
        public byte R;
        [FieldOffset(1)]
        public byte G;
        [FieldOffset(2)]
        public byte B;
        [FieldOffset(3)]
        public byte A;
        [FieldOffset(0)]
        public int IntValue;

        public Pixel(byte Red, byte Green, byte Blue, byte Alpha = 255)
        {
            IntValue = 0xFF00000;
            R = Red;
            G = Green;
            B = Blue;
            A = Alpha;
        }
        public Pixel(int IntValue)
        {
            R = 0x00;
            G = 0x00;
            B = 0x00;
            A = 0x00;
            this.IntValue = IntValue;
        }

        public enum Mode { NORMAL, MASK, ALPHA, CUSTOM }

        public static bool operator ==(Pixel pixel1, Pixel pixel2)
        {
            return pixel1.IntValue == pixel2.IntValue;
        }
        public static bool operator !=(Pixel pixel1, Pixel pixel2)
        {
            return pixel1.IntValue != pixel2.IntValue;
        }

        #region DefaultPixels
        public static Pixel WHITE = new Pixel(255, 255, 255),
        GREY = new Pixel(192, 192, 192),
        DARK_GREY = new Pixel(128, 128, 128),
        VERY_DARK_GREY = new Pixel(64, 64, 64),
        RED = new Pixel(255, 0, 0),
        DARK_RED = new Pixel(128, 0, 0),
        VERY_DARK_RED = new Pixel(64, 0, 0),
        YELLOW = new Pixel(255, 255, 0),
        DARK_YELLOW = new Pixel(128, 128, 0),
        VERY_DARK_YELLOW = new Pixel(64, 64, 0),
        GREEN = new Pixel(0, 255, 0),
        DARK_GREEN = new Pixel(0, 128, 0),
        VERY_DARK_GREEN = new Pixel(0, 64, 0),
        CYAN = new Pixel(0, 255, 255),
        DARK_CYAN = new Pixel(0, 128, 128),
        VERY_DARK_CYAN = new Pixel(0, 64, 64),
        BLUE = new Pixel(0, 0, 255),
        DARK_BLUE = new Pixel(0, 0, 128),
        VERY_DARK_BLUE = new Pixel(0, 0, 64),
        MAGENTA = new Pixel(255, 0, 255),
        DARK_MAGENTA = new Pixel(128, 0, 128),
        VERY_DARK_MAGENTA = new Pixel(64, 0, 64),
        BLACK = new Pixel(0, 0, 0),
        BLANK = new Pixel(0, 0, 0, 0);


        public override int GetHashCode()
        {
            var hashCode = 166155883;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            hashCode = hashCode * -1521134295 + IntValue.GetHashCode();
            return hashCode;
        }
        #endregion

        public static implicit operator Pixel(Color c)
        {
            return new Pixel(c.R, c.G, c.B, c.A);
        }
        public static implicit operator Color(Pixel p)
        {
            return Color.FromArgb(p.A, p.R, p.G, p.B);
        }

        public override bool Equals(object o)
        {
            if(o.GetType() == typeof(Pixel))
            {
                return (Pixel)o == this;
            }
            return false;
        }
    }

    public class HWButton
    {
        public bool bPressed = false;
        public bool bReleased = false;
        public bool bHeld = false;
    }

    public class Sprite
    {
        public Sprite()
        {
            ColData = null;
            ColDataInt = null;
            Width = 0;
            Height = 0;
        }
        public Sprite(string sImageFile)
        {
            LoadFromFile(sImageFile);
        }
        public Sprite(int Width, int Height) : this(Width, Height, Pixel.WHITE) { }
        public Sprite(Bitmap bitmap)
        {
            LoadFromBitmap(bitmap);
        }
        public Sprite(int Width, int Height, Pixel p)
        {
            if (ColData != null) ColData = null;
            this.Width = Width;
            this.Height = Height;
            ColData = new Pixel[Width * Height];
            ColDataInt = new int[Width * Height];
            for (int i = 0; i < Width * Height; i++)
            {
                ColData[i] = p;
                ColDataInt[i] = p.IntValue;
            }
        }

        public bool LoadFromBitmap(Bitmap bitmap)
        {
            if (bitmap == null) return false;
            Width = bitmap.Width;
            Height = bitmap.Height;
            ColData = new Pixel[Width * Height];
            ColDataInt = new int[Width * Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    SetPixel(x, y, bitmap.GetPixel(x, y));
                }
            }
            return true;
        }
        public bool LoadFromFile(string sImageFile)
        {
            Bitmap bitmap = (Bitmap)Image.FromFile(sImageFile);
            if (bitmap == null) return false;
            Width = bitmap.Width;
            Height = bitmap.Height;
            ColData = new Pixel[Width * Height];
            ColDataInt = new int[Width * Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    SetPixel(x, y, bitmap.GetPixel(x, y));
                }
            }
            bitmap.Dispose();
            return true;
        }

        public int Width;
        public int Height;
        public enum Mode { NORMAL, PERIODIC }

        public void SetSampleMode(Mode mode = Mode.NORMAL)
        {
            modeSample = mode;
        }
        public Pixel GetPixel(int x, int y)
        {
            if (modeSample == Mode.NORMAL)
            {
                if (x >= 0 && x < Width && y >= 0 && y < Height) return ColData[y * Width + x];
                else return Pixel.BLACK;
            }
            else return ColData[Math.Abs(y % Height) * Width + Math.Abs(x % Width)];
        }
        public bool SetPixel(int x, int y, Pixel p)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                ColData[y * Width + x] = p;
                ColDataInt[y * Width + x] = p.IntValue;
                return true;
            }
            else
                return false;
        }

        public Pixel Sample(float x, float y)
        {
            int sx = (int)Math.Min(x * Width, Width - 1);
            int sy = (int)Math.Min(y * (float)Height, Height - 1);
            return GetPixel(sx, sy);
        }
        public Pixel SampleBL(float u, float v)
        {
            u = u * Width - 0.5f;
            v = v * Height - 0.5f;
            int x = (int)Math.Floor(u);
            int y = (int)Math.Floor(v);
            float uRatio = u - x;
            float vRatio = v - y;
            float uOpposite = 1 - uRatio;
            float vOpposite = 1 - vRatio;

            Pixel p1 = GetPixel(Math.Max(x, 0), Math.Max(y, 0));
            Pixel p2 = GetPixel(Math.Min(x + 1, Width - 1), Math.Max(y, 0));
            Pixel p3 = GetPixel(Math.Max(x, 0), Math.Min(y + 1, Height - 1));
            Pixel p4 = GetPixel(Math.Min(x + 1, Width - 1), Math.Min(y + 1, Height - 1));

            return new Pixel(
                (byte)((p1.R * uOpposite + p2.R * uRatio) * vOpposite + (p3.R * uOpposite + p4.R * uRatio) * vRatio),
                (byte)((p1.G * uOpposite + p2.G * uRatio) * vOpposite + (p3.G * uOpposite + p4.G * uRatio) * vRatio),
                (byte)((p1.B * uOpposite + p2.B * uRatio) * vOpposite + (p3.B * uOpposite + p4.B * uRatio) * vRatio));
        }
        public Pixel[] GetData()
        {
            return ColData;
        }
        public byte[] GetByteData()
        {
            byte[] ret = new byte[ColData.Length * 4];
            Parallel.For(0, ColData.Length, (i) =>
            {
                ret[i * 4] = ColData[i].A;
                ret[i * 4 + 1] = ColData[i].R;
                ret[i * 4 + 2] = ColData[i].G;
                ret[i * 4 + 3] = ColData[i].B;
            });
            return ret;
        }
        public int[] GetIntData()
        {
            return ColDataInt;
        }
        public void SetData(Pixel[] data)
        {
            ColData = data;
            Parallel.For(0, ColData.Length, (i) =>
            {
                ColDataInt[i] = ColData[i].IntValue;
            });
        }

        private Pixel[] ColData;
        private int[] ColDataInt;
        private Mode modeSample = Mode.NORMAL;
    }

    public enum Key
    {
        NONE,
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        K0, K1, K2, K3, K4, K5, K6, K7, K8, K9,
        F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,
        UP, DOWN, LEFT, RIGHT,
        SPACE, TAB, SHIFT, CTRL, INS, DEL, HOME, END, PGUP, PGDN,
        BACKSPACE, ESCAPE, RETURN, ENTER, PAUSE, SCROLL,
        NP0, NP1, NP2, NP3, NP4, NP5, NP6, NP7, NP8, NP9,
        NP_MUL, NP_DIV, NP_ADD, NP_SUB, NP_DECIMAL,
    };

    public class Engine
    {

        public Engine()
        {
            sAppName = "Undefined";
            PGEX.pge = this;
        }

        public bool Construct(int screenW, int screenH, int pixelW, int pixelH, bool fullScreen = false, bool showFPS = false)
        {
            nScreenWidth = screenW;
            nScreenHeight = screenH;
            nPixelWidth = pixelW;
            nPixelHeight = pixelH;
            bFullScreen = fullScreen;
            bShowFPS = showFPS;

            fPixelX = 2.0f / (float)nScreenWidth;
            fPixelY = 2.0f / (float)nScreenHeight;
            if (nPixelWidth == 0 || nPixelHeight == 0 || nScreenWidth == 0 || nScreenHeight == 0) return false;

            Action<Array> Init = new Action<Array>((arr) =>
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    arr.SetValue(new HWButton(), i);
                }
            });

            Init(pKeyboardState);
            Init(pMouseState);

            AppName = sAppName;
            pDefaultDrawTarget = new Sprite(nScreenWidth, nScreenHeight);
            SetDrawTarget(ref pDefaultDrawTarget);
            Clear(Pixel.BLACK);
            return true;
        }
        public bool Start()
        {
            try
            {
                Action<BackgroundWorker> action = new Action<BackgroundWorker>(EngineThread);
                bAtomActive = true;
                WindowCreate(action);
                ((Form1)Control.FromHandle(HWnd)).Start();
                return true;
            }
            catch { return false; }
        }

        public virtual bool OnUserCreate()
        {
            return false;
        }
        public virtual bool OnUserDestroy()
        {
            return true;
        }
        public virtual bool onUserUpdate(float fElapsedTime)
        {
            return false;
        }

        public static bool IsFocused()
        {
            return bHasInputFocus;
        }
        public static bool HasMouseFocus()
        {
            return bHasMouseFocus;
        }
        public static HWButton GetKey(Key k)
        {
            return pKeyboardState[(int)k];
        }
        public static HWButton GetMouse(int b)
        {
            return pMouseState[b];
        }
        public static int GetMouseX()
        {
            return nMousePosX;
        }
        public static int GetMouseY()
        {
            return nMousePosY;
        }
        public static int GetMouseWheel()
        {
            return nMouseWheelDelta;
        }

        public static int ScreenWidth()
        {
            return nScreenWidth;
        }
        public static int ScreenHeight()
        {
            return nScreenHeight;
        }
        public static int GetDrawTargetWidth()
        {
            if (pDrawTarget != null)
                return pDrawTarget.Width;
            else
                return 0;
        }
        public static int GetDrawTargetHeight()
        {
            if (pDrawTarget != null)
                return pDrawTarget.Height;
            else
                return 0;
        }
        public static Sprite GetDrawTarget()
        {
            return pDrawTarget;
        }

        public static void SetDrawTarget(ref Sprite target)
        {
            if (target != null)
            {
                pDrawTarget = target;
            }
            else
            {
                pDrawTarget = pDefaultDrawTarget;
            }
        }
        public static void SetPixelMode(Pixel.Mode m)
        {
            nPixelMode = m;
        }
        public static Pixel.Mode GetPixelMode()
        {
            return nPixelMode;
        }
        public void SetPixelMode(Func<int, int, Pixel, Pixel, Pixel> pixelMode)
        {
            funcPixelMode = pixelMode;
            nPixelMode = Pixel.Mode.CUSTOM;
        }
        public void SetPixelBlend(float fBlend)
        {
            fBlendFactor = fBlend;
            if (fBlendFactor < 0.0f) fBlendFactor = 0.0f;
            if (fBlendFactor > 1.0f) fBlendFactor = 1.0f;
        }
        public void SetSubPixelOffset(float ox, float oy)
        {
            fSubPixelOffsetX = ox * fPixelX;
            fSubPixelOffsetY = oy * fPixelY;
        }

        public static bool Draw(int x, int y, Pixel p)
        {
            if (pDrawTarget == null) return false;
            if (nPixelMode == Pixel.Mode.NORMAL)
            {
                return pDrawTarget.SetPixel(x, y, p);
            }
            if (nPixelMode == Pixel.Mode.MASK)
            {
                if (p.A == 255) return pDrawTarget.SetPixel(x, y, p);
            }
            if (nPixelMode == Pixel.Mode.ALPHA)
            {
                Pixel d = pDrawTarget.GetPixel(x, y);
                float a = (p.A / 255.0f) * fBlendFactor;
                float c = 1.0f - a;
                float r = a * p.R + c * d.R;
                float g = a * p.G + c * d.G;
                float b = a * p.B + c * d.B;
                return pDrawTarget.SetPixel(x, y, new Pixel((byte)r, (byte)g, (byte)b));
            }
            if (nPixelMode == Pixel.Mode.CUSTOM)
            {
                return pDrawTarget.SetPixel(x, y, funcPixelMode(x, y, p, pDrawTarget.GetPixel(x, y)));
            }
            return false;
        }
        public static void DrawLine(int x1, int y1, int x2, int y2, Pixel p, uint pattern = 0xFFFFFFFF)
        {
            int x, y, dx, dy, dx1, dy1, px, py, xe, ye, i;
            dx = x2 - x1; dy = y2 - y1;

            Func<bool> rol = new Func<bool>(() =>
            {
                pattern = (pattern << 1) | (pattern >> 31);
                return (pattern & 1) == 1;
            });

            if (dx == 0)
            {
                if (y2 < y1) swap(ref y2, ref y1);
                for (int j = y1; j < y2; j++)
                {
                    if (rol()) Draw(x1, j, p);
                }
                return;
            }
            if (dy == 0)
            {
                if (x2 < x1) swap(ref x2, ref x1);
                for (int j = x1; j < x2; j++)
                {
                    if (rol()) Draw(j, y1, p);
                }
                return;
            }
            dx1 = Math.Abs(dx); dy1 = Math.Abs(dy);
            px = 2 * dy1 - dx1; py = 2 * dx1 - dy1;
            if (dy1 <= dx1)
            {
                if (dx >= 0)
                {
                    x = x1; y = y1; xe = x2;
                }
                else
                {
                    x = x2; y = y2; xe = x1;
                }

                if (rol()) Draw(x, y, p);

                for (i = 0; x < xe; i++)
                {
                    x++;
                    if (px < 0)
                        px += 2 * dy1;
                    else
                    {
                        if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0)) y++; else y--;
                        px += 2 * (dy1 - dx1);
                    }
                    if (rol()) Draw(x, y, p);
                }
            }
            else
            {
                if (dy >= 0)
                {
                    x = x1; y = y1; ye = y2;
                }
                else
                {
                    x = x2; y = y2; ye = y1;
                }

                if (rol()) Draw(x, y, p);

                for (i = 0; y < ye; i++)
                {
                    y++;
                    if (py <= 0)
                        py += 2 * dx1;
                    else
                    {
                        if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0)) x++; else x--;
                        py += 2 * (dx1 - dy1);
                    }
                    if (rol()) Draw(x, y, p);
                }
            }
        }
        public static void DrawCircle(int x, int y, int radius, Pixel p)
        {
            int x0 = 0;
            int y0 = radius;
            int d = 3 - 2 * radius;
            if (radius == 0) return;

            while (y0 >= x0) // only formulate 1/8 of circle
            {
                Draw(x + x0, y - y0, p);
                Draw(x + y0, y - x0, p);
                Draw(x + y0, y + x0, p);
                Draw(x + x0, y + y0, p);
                Draw(x - x0, y + y0, p);
                Draw(x - y0, y + x0, p);
                Draw(x - y0, y - x0, p);
                Draw(x - x0, y - y0, p);
                if (d < 0) d += 4 * x0++ + 6;
                else d += 4 * (x0++ - y0--) + 10;
            }
        }
        public void FillCircle(int x, int y, int radius, Pixel p)
        {
            int x0 = 0;
            int y0 = radius;
            int d = 3 - 2 * radius;
            if (radius == 0) return;

            Action<int, int, int> drawline = new Action<int, int, int>((sx, ex, ny) =>
            {
                for (int i = sx; i <= ex; i++)
                {
                    Draw(i, ny, p);
                }
            });

            while (y0 >= x0)
            {
                drawline(x - x0, x + x0, y - y0);
                drawline(x - y0, x + y0, y - x0);
                drawline(x - x0, x + x0, y + y0);
                drawline(x - y0, x + y0, y + x0);
                if (d < 0) d += 4 * x0++ + 6;
                else d += 4 * (x0++ - y0--) + 10;
            }
        }
        public static void DrawRect(int x, int y, int w, int h, Pixel p)
        {
            DrawLine(x, y, x + w, y, p);
            DrawLine(x + w, y, x + w, y + h, p);
            DrawLine(x, y + h, x + w, y + h, p);
            DrawLine(x, y, x, y + h, p);
        }
        public void FillRect(int x, int y, int w, int h, Pixel p)
        {
            int x2 = x + w;
            int y2 = y + h;

            if (x < 0) x = 0;
            if (x >= nScreenWidth) x = nScreenWidth;
            if (y < 0) y = 0;
            if (y >= nScreenHeight) y = nScreenHeight;

            if (x2 < 0) x2 = 0;
            if (x2 >= nScreenWidth) x2 = nScreenWidth;
            if (y2 < 0) y2 = 0;
            if (y2 >= nScreenHeight) y2 = nScreenHeight;

            for (int i = x; i < x2; i++)
                for (int j = y; j < y2; j++)
                    Draw(i, j, p);
        }
        public static void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3, Pixel p)
        {
            DrawLine(x1, y1, x2, y2, p);
            DrawLine(x2, y2, x3, y3, p);
            DrawLine(x3, y3, x1, y1, p);
        }
        static void swap(ref int x, ref int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }
        public void FillTriangle(int x1, int y1, int x2, int y2, int x3, int y3, Pixel p)
        {

            Action<int, int, int> drawline = new Action<int, int, int>((sx, ex, ny) =>
            {
                for (int i = sx; i <= ex; i++)
                {
                    Draw(i, ny, p);
                }
            });

            int t1x, t2x, y, minx, maxx, t1xp, t2xp;
            bool changed1 = false;
            bool changed2 = false;
            int signx1, signx2, dx1, dy1, dx2, dy2;
            int e1, e2;
            // Sort vertices
            if (y1 > y2) { swap(ref y1, ref y2); swap(ref x1, ref x2); }
            if (y1 > y3) { swap(ref y1, ref y3); swap(ref x1, ref x3); }
            if (y2 > y3) { swap(ref y2, ref y3); swap(ref x2, ref x3); }

            t1x = t2x = x1; y = y1;   // Starting points
            dx1 = x2 - x1;
            if (dx1 < 0) { dx1 = -dx1; signx1 = -1; }
            else signx1 = 1;
            dy1 = y2 - y1;

            dx2 = x3 - x1;
            if (dx2 < 0) { dx2 = -dx2; signx2 = -1; }
            else signx2 = 1;
            dy2 = y3 - y1;

            if (dy1 > dx1)
            {   // swap values
                swap(ref dx1, ref dy1);
                changed1 = true;
            }
            if (dy2 > dx2)
            {   // swap values
                swap(ref dy2, ref dx2);
                changed2 = true;
            }

            e2 = dx2 >> 1;
            // Flat top, just process the second half
            if (y1 == y2) goto next;
            e1 = dx1 >> 1;

            for (int i = 0; i < dx1;)
            {
                t1xp = 0; t2xp = 0;
                if (t1x < t2x) { minx = t1x; maxx = t2x; }
                else { minx = t2x; maxx = t1x; }
                // process first line until y value is about to change
                while (i < dx1)
                {
                    i++;
                    e1 += dy1;
                    while (e1 >= dx1)
                    {
                        e1 -= dx1;
                        if (changed1) t1xp = signx1;//t1x += signx1;
                        else goto next1;
                    }
                    if (changed1) break;
                    else t1x += signx1;
                }
            // Move line
            next1:
                // process second line until y value is about to change
                while (true)
                {
                    e2 += dy2;
                    while (e2 >= dx2)
                    {
                        e2 -= dx2;
                        if (changed2) t2xp = signx2;//t2x += signx2;
                        else goto next2;
                    }
                    if (changed2) break;
                    else t2x += signx2;
                }
            next2:
                if (minx > t1x) minx = t1x;
                if (minx > t2x) minx = t2x;
                if (maxx < t1x) maxx = t1x;
                if (maxx < t2x) maxx = t2x;
                drawline(minx, maxx, y);    // Draw line from min to max points found on the y
                                            // Now increase y
                if (!changed1) t1x += signx1;
                t1x += t1xp;
                if (!changed2) t2x += signx2;
                t2x += t2xp;
                y += 1;
                if (y == y2) break;

            }
        next:
            // Second half
            dx1 = x3 - x2;
            if (dx1 < 0) { dx1 = -dx1; signx1 = -1; }
            else signx1 = 1;
            dy1 = y3 - y2;
            t1x = x2;

            if (dy1 > dx1)
            {   // swap values
                swap(ref dy1, ref dx1);
                changed1 = true;
            }
            else changed1 = false;

            e1 = dx1 >> 1;

            for (int i = 0; i <= dx1; i++)
            {
                t1xp = 0; t2xp = 0;
                if (t1x < t2x) { minx = t1x; maxx = t2x; }
                else { minx = t2x; maxx = t1x; }
                // process first line until y value is about to change
                while (i < dx1)
                {
                    e1 += dy1;
                    while (e1 >= dx1)
                    {
                        e1 -= dx1;
                        if (changed1) { t1xp = signx1; break; }//t1x += signx1;
                        else goto next3;
                    }
                    if (changed1) break;
                    else t1x += signx1;
                    if (i < dx1) i++;
                }
            next3:
                // process second line until y value is about to change
                while (t2x != x3)
                {
                    e2 += dy2;
                    while (e2 >= dx2)
                    {
                        e2 -= dx2;
                        if (changed2) t2xp = signx2;
                        else goto next4;
                    }
                    if (changed2) break;
                    else t2x += signx2;
                }
            next4:

                if (minx > t1x) minx = t1x;
                if (minx > t2x) minx = t2x;
                if (maxx < t1x) maxx = t1x;
                if (maxx < t2x) maxx = t2x;
                drawline(minx, maxx, y);
                if (!changed1) t1x += signx1;
                t1x += t1xp;
                if (!changed2) t2x += signx2;
                t2x += t2xp;
                y += 1;
                if (y > y3) return;
            }
        }
        public static void DrawSprite(int x, int y, Sprite sprite, int scale = 1)
        {
            if (sprite == null)
                return;

            if (scale > 1)
            {
                Parallel.For(0, sprite.Width, (i) =>
                {
                    Parallel.For(0, sprite.Height, (j) =>
                    {
                        Parallel.For(0, scale, (isc) =>
                        {
                            Parallel.For(0, scale, (js) =>
                            {
                                Draw(x + (i * scale) + isc, y + (j * scale) + js, sprite.GetPixel(i, j));
                            });
                        });
                    });
                });
            }
            else
            {
                Parallel.For(0, sprite.Width, (i) =>
                {
                    Parallel.For(0, sprite.Height, (j) =>
                    {
                        Draw(x + i, y + j, sprite.GetPixel(i, j));
                    });
                });
            }
        }
        public static void DrawPartialSprite(int x, int y, Sprite sprite, int ox, int oy, int w, int h, int scale = 1)
        {
            if (sprite == null)
                return;

            if (scale > 1)
            {
                Parallel.For(0, w, (i) =>
                {
                    Parallel.For(0, h, (j) =>
                    {
                        Parallel.For(0, scale, (isc) =>
                        {
                            Parallel.For(0, scale, (js) =>
                            {
                                Draw(x + (i * scale) + isc, y + (j * scale) + js, sprite.GetPixel(i + ox, j + oy));
                            });
                        });
                    });
                });
            }
            else
            {
                Parallel.For(0, w, (i) =>
                {
                    Parallel.For(0, h, (j) =>
                    {
                        Draw(x + i, y + j, sprite.GetPixel(i + ox, j + oy));
                    });
                });
            }
        }
        public static void DrawString(int x, int y, string sText, Pixel col, int Size, int font)
        {
            if (sText.Length == 0) return;
            string[] strings = sText.Split(new[] { '\n' });
            if (strings[strings.Length - 1].Length == 0)
            {
                strings = strings.Take(strings.Length - 1).ToArray();
            }
            Parallel.For(0, strings.Length, (i) =>
            {
                Bitmap bitmap = new Bitmap(9 * strings[i].Length, 11);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(Pixel.BLACK);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                TextRenderer.DrawText(g, strings[i], new Font(FontFamily.Families[font], 9, GraphicsUnit.Pixel), Point.Empty, Pixel.WHITE);
                Pixel.Mode a = GetPixelMode();
                SetPixelMode(Pixel.Mode.MASK);
                DrawSprite(x, y + i * Size * 11, new Sprite(bitmap), Size);
                SetPixelMode(a);
                bitmap.Dispose();
            });
        }
        public static void Clear(Pixel p)
        {
            int pixels = GetDrawTargetWidth() * GetDrawTargetHeight();
            Pixel[] m = GetDrawTarget().GetData();
            for (int i = 0; i < pixels; i++)
                m[i] = p;
            GetDrawTarget().SetData(m);
        }
        public static void SetScreenSize(int w, int h)
        {
            pDefaultDrawTarget = null;
            nScreenWidth = w;
            nScreenHeight = h;
            pDefaultDrawTarget = new Sprite(nScreenWidth, nScreenHeight);
            SetDrawTarget(ref pDefaultDrawTarget);
            OpenGL GL = ((Form1)Control.FromHandle(HWnd)).GetGLControl().OpenGL;
            GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
            Win32.SwapBuffers(glDeviceContext);
            GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
            UpdateViewport();
        }
        public static char ReadKey()
        {
            for (int i = 0; i < CharacterKeys.Length; i++)
            {
                if ((GetKey(Key.SHIFT).bPressed || GetKey(Key.SHIFT).bHeld) && GetKey(CharacterKeys[i]).bPressed)
                {
                    return CharactersUpper[i];
                }
                else if (GetKey(CharacterKeys[i]).bPressed)
                {
                    return CharactersLower[i];
                }
            }
            for (int i = 0; i < NumberKeys.Length; i++)
            {
                if (GetKey(NumberKeys[i]).bPressed)
                {
                    return Numbers[i];
                }
            }
            return '\0';
        }

        #region Declerations
        public static string sAppName;

        static Key[] CharacterKeys = new[] {Key.A, Key.B, Key.C, Key.D, Key.E,
            Key.F, Key.G, Key.H, Key.I, Key.J, Key.K, Key.L, Key.M, Key.N,
            Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U, Key.V, Key.W,
            Key.X, Key.Y, Key.Z};
        static Key[] NumberKeys = new[] {Key.NP1, Key.NP2, Key.NP3, Key.NP4, Key.NP5,
            Key.NP6, Key.NP7, Key.NP8, Key.NP9, Key.NP0, Key.NP_ADD, Key.NP_DIV,
            Key.NP_MUL, Key.NP_SUB, Key.NP_DECIMAL, Key.SPACE, Key.K1, Key.K2, Key.K3, Key.K4, Key.K5,
            Key.K6, Key.K7, Key.K8, Key.K9, Key.K0};
        static char[] CharactersUpper = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G',
            'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z'};
        static char[] Numbers = new[]  {'1', '2', '3', '4', '5', '6', '7',
            '8', '9', '0', '+', '/', '*', '-', '.', ' ', '1', '2', '3', '4',
            '5', '6', '7', '8', '9', '0',};
        static char[] CharactersLower = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g',
            'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
            'u', 'v', 'w', 'x', 'y', 'z' };
        static Sprite pDefaultDrawTarget = null;
        static Sprite pDrawTarget = null;
        static Pixel.Mode nPixelMode = Pixel.Mode.NORMAL;
        static float fBlendFactor = 1.0f;
        static int nScreenWidth = 256;
        static int nScreenHeight = 240;
        static int nPixelWidth = 4;
        static int nPixelHeight = 4;
        static int nMousePosX = 0;
        static int nMousePosY = 0;
        static int nMouseWheelDelta = 0;
        static int nMousePosXcache = 0;
        static int nMousePosYcache = 0;
        static int nMouseWheelDeltaCache = 0;
        static int nWindowWidth = 0;
        static int nWindowHeight = 0;
        static int nViewX = 0;
        static int nViewY = 0;
        static int nViewW = 0;
        static int nViewH = 0;
        static bool bFullScreen = false;
        static float fPixelX = 1.0f;
        static float fPixelY = 1.0f;
        static float fSubPixelOffsetX = 0.0f;
        static float fSubPixelOffsetY = 0.0f;
        static bool bHasInputFocus = false;
        static bool bHasMouseFocus = false;
        static bool bShowFPS = false;
        static float fFrameTimer = 1.0f;
        static int nFrameCount = 0;
        static Func<int, int, Pixel, Pixel, Pixel> funcPixelMode;

        static Dictionary<int, byte> mapKeys = new Dictionary<int, byte>();
        static bool[] pKeyNewState = new bool[256];
        static bool[] pKeyOldState = new bool[256];
        static HWButton[] pKeyboardState = new HWButton[256];

        static bool[] pMouseNewState = new bool[5];
        static bool[] pMouseOldState = new bool[5];
        static HWButton[] pMouseState = new HWButton[5];

        static bool bAtomActive;
        #endregion
        static void UpdateMouse(int x, int y)
        {
            x -= nViewX;
            y -= nViewY;

            nMousePosXcache = (int)(((float)x / (float)(nWindowWidth - (nViewX * 2)) * (float)nScreenWidth));
            nMousePosYcache = (int)(((float)y / (float)(nWindowHeight - (nViewY * 2)) * (float)nScreenHeight));

            if (nMousePosXcache >= nScreenWidth)
                nMousePosXcache = nScreenWidth - 1;
            if (nMousePosYcache >= nScreenHeight)
                nMousePosYcache = nScreenHeight - 1;

            if (nMousePosXcache < 0)
                nMousePosXcache = 0;
            if (nMousePosYcache < 0)
                nMousePosYcache = 0;
        }
        static void UpdateMouseWheel(int delta)
        {
            nMouseWheelDeltaCache += delta;
        }
        static void UpdateWindowSize(int x, int y)
        {
            nWindowWidth = x;
            nWindowHeight = y;
            UpdateViewport();
        }
        static void UpdateViewport()
        {
            int ww = nScreenWidth * nPixelWidth;
            int wh = nScreenHeight * nPixelHeight;
            float wasp = ww / (float)wh;

            nViewW = nWindowWidth;
            nViewH = (int)(nViewW / wasp);

            if (nViewH > nWindowHeight)
            {
                nViewH = nWindowHeight;
                nViewW = (int)(nViewH * wasp);
            }

            nViewX = (nWindowWidth - nViewW) / 2;
            nViewY = (nWindowHeight - nViewH) / 2;
        }

        static bool OpenGLCreate()
        {
            OpenGLControl GLControl = ((Form1)Control.FromHandle(HWnd)).GetGLControl();
            OpenGL GL = GLControl.OpenGL;

            glDeviceContext = Graphics.FromHwnd(HWnd).GetHdc();

            #region PIXELFORMATDESCRIPTOR Construct
            Win32.PIXELFORMATDESCRIPTOR pfd = new Win32.PIXELFORMATDESCRIPTOR();
            pfd.nSize = 40;
            pfd.nVersion = 1;
            pfd.dwFlags = Win32.PFD_DRAW_TO_WINDOW | Win32.PFD_SUPPORT_OPENGL | Win32.PFD_DOUBLEBUFFER;
            pfd.iPixelType = Win32.PFD_TYPE_RGBA;
            pfd.cColorBits = 32;
            pfd.cRedBits = 0;
            pfd.cRedShift = 0;
            pfd.cGreenBits = 0;
            pfd.cGreenShift = 0;
            pfd.cBlueBits = 0;
            pfd.cBlueShift = 0;
            pfd.cAlphaBits = 0;
            pfd.cAlphaShift = 0;
            pfd.cAccumBits = 0;
            pfd.cAccumRedBits = 0;
            pfd.cAccumGreenBits = 0;
            pfd.cAccumBlueBits = 0;
            pfd.cAccumAlphaBits = 0;
            pfd.cDepthBits = 0;
            pfd.cStencilBits = 0;
            pfd.cAuxBuffers = 0;
            pfd.iLayerType = Win32.PFD_MAIN_PLANE;
            pfd.bReserved = 0;
            pfd.dwLayerMask = 0;
            pfd.dwVisibleMask = 0;
            pfd.dwDamageMask = 0;
            #endregion

            int pf = Win32.ChoosePixelFormat(glDeviceContext, pfd);
            if (pf == 0) return false;
            Win32.SetPixelFormat(glDeviceContext, pf, pfd);

            glRenderContext = Win32.wglCreateContext(glDeviceContext);
            if (glRenderContext == IntPtr.Zero) return false;
            Win32.wglMakeCurrent(glDeviceContext, glRenderContext);

            GL.Viewport(nViewX, nViewY, nViewW, nViewH);

            return true;
        }

        static IntPtr glDeviceContext;
        static IntPtr glRenderContext;

        static uint glBuffer = 0;

        private void EngineThread(BackgroundWorker worker)
        {
            try
            {
                OpenGLCreate();

                Form1 window = (Form1)Control.FromHandle(HWnd);
                OpenGL GL = window.GetGLControl().OpenGL;

                GL.Enable(OpenGL.GL_TEXTURE_2D);
                GL.GenTextures(1, new uint[] { glBuffer });
                GL.BindTexture(OpenGL.GL_TEXTURE_2D, glBuffer);
                GL.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, new uint[] { OpenGL.GL_NEAREST });
                GL.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, new uint[] { OpenGL.GL_NEAREST });
                GL.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL);

                GL.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGBA, nScreenWidth, nScreenHeight, 0, OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, pDefaultDrawTarget.GetByteData());

                if (!OnUserCreate()) bAtomActive = false;

                Stopwatch tp1 = Stopwatch.StartNew();

                while (bAtomActive)
                {
                    while (bAtomActive)
                    {
                        float elapsedTime = (float)tp1.Elapsed.TotalSeconds;
                        tp1.Reset();
                        tp1.Start();
                        Parallel.For(0, 256, (i) =>
                        {
                            pKeyboardState[i].bPressed = false;
                            pKeyboardState[i].bReleased = false;

                            if (pKeyNewState[i] != pKeyOldState[i])
                            {
                                if (pKeyNewState[i])
                                {
                                    pKeyboardState[i].bPressed = !pKeyboardState[i].bHeld;
                                    pKeyboardState[i].bHeld = true;
                                }
                                else
                                {
                                    pKeyboardState[i].bReleased = true;
                                    pKeyboardState[i].bHeld = false;
                                }
                            }

                            pKeyOldState[i] = pKeyNewState[i];
                        });

                        Parallel.For(0, 5, (i) =>
                        {
                            pMouseState[i].bPressed = false;
                            pMouseState[i].bReleased = false;

                            if (pMouseNewState[i] != pMouseOldState[i])
                            {
                                if (pMouseNewState[i])
                                {
                                    pMouseState[i].bPressed = !pMouseState[i].bHeld;
                                    pMouseState[i].bHeld = true;
                                }
                                else
                                {
                                    pMouseState[i].bReleased = true;
                                    pMouseState[i].bHeld = false;
                                }
                            }

                            pMouseOldState[i] = pMouseNewState[i];
                        });

                        nMousePosX = nMousePosXcache;
                        nMousePosY = nMousePosYcache;

                        nMouseWheelDelta = nMouseWheelDeltaCache;
                        nMouseWheelDeltaCache = 0;

                        if (!onUserUpdate(elapsedTime))
                            bAtomActive = false;

                        GL.Viewport(nViewX, nViewY, nViewW, nViewH);

                        GL.TexSubImage2D(OpenGL.GL_TEXTURE_2D, 0, 0, 0, nScreenWidth, nScreenHeight, OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, pDefaultDrawTarget.GetIntData());

                        GL.Begin(OpenGL.GL_QUADS);
                        GL.TexCoord(0.0, 1.0); GL.Vertex(-1.0f + (fSubPixelOffsetX), -1.0f + (fSubPixelOffsetY), 0.0f);
                        GL.TexCoord(0.0, 0.0); GL.Vertex(-1.0f + (fSubPixelOffsetX), 1.0f + (fSubPixelOffsetY), 0.0f);
                        GL.TexCoord(1.0, 0.0); GL.Vertex(1.0f + (fSubPixelOffsetX), 1.0f + (fSubPixelOffsetY), 0.0f);
                        GL.TexCoord(1.0, 1.0); GL.Vertex(1.0f + (fSubPixelOffsetX), -1.0f + (fSubPixelOffsetY), 0.0f);
                        GL.End();

                        Win32.SwapBuffers(glDeviceContext);

                        glDeviceContext = Graphics.FromHwnd(HWnd).GetHdc();

                        fFrameTimer += elapsedTime;
                        nFrameCount++;
                        if (fFrameTimer > 1.0f && bShowFPS)
                        {
                            fFrameTimer -= 1.0f;
                            string sTitle = AppName + " - FPS: " + nFrameCount.ToString();
                            if (window.InvokeRequired)
                            {
                                window.Invoke(new Action(() => window.Text = sTitle));
                            }
                            else
                            {
                                window.Text = sTitle;
                            }
                            nFrameCount = 0;
                        }
                        worker.ReportProgress(0);
                    }
                    if (OnUserDestroy())
                    {

                    }
                    else
                    {
                        bAtomActive = true;
                    }
                }
                Win32.wglDeleteContext(glRenderContext);
            }
            catch 
            {
            };
        }

        static IntPtr HWnd = new IntPtr();
        static IntPtr WindowCreate(Action<BackgroundWorker> action)
        {
            Form1 Window = new Form1(action);

            if (bFullScreen)
            {
                Window.WindowState = FormWindowState.Maximized;
            }

            Window.Height = nScreenHeight * nPixelWidth;
            Window.Width = nScreenWidth * nPixelHeight;

            nWindowHeight = Window.Height;
            nWindowWidth = Window.Width;

            nViewW = nWindowWidth;
            nViewH = nWindowHeight;

            UpdateViewport();

            HWnd = Window.Handle;

            Window.MouseMove += new MouseEventHandler((sender, e) =>
            {
                UpdateMouse(e.X, e.Y);
            });
            Window.SizeChanged += new EventHandler((sender, e) =>
            {
                UpdateWindowSize(Window.Width, Window.Height - 39);
            });
            Window.MouseWheel += new MouseEventHandler((sender, e) =>
            {
                UpdateMouseWheel(e.Delta);
            });
            Window.MouseLeave += new EventHandler((sender, e) => bHasMouseFocus = false);
            Window.MouseEnter += new EventHandler((sender, e) => bHasMouseFocus = true);
            Window.GotFocus += new EventHandler((sender, e) => bHasInputFocus = true);
            Window.LostFocus += new EventHandler((sender, e) => bHasInputFocus = false);
            Window.KeyDown += new KeyEventHandler((sender, e) => { try { pKeyNewState[mapKeys[(int)e.KeyCode]] = true; } catch { } });
            Window.KeyUp += new KeyEventHandler((sender, e) => { try { pKeyNewState[mapKeys[(int)e.KeyCode]] = false; } catch { } });
            Window.MouseDown += new MouseEventHandler((sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    pMouseNewState[0] = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    pMouseNewState[1] = true;
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    pMouseNewState[2] = true;
                }
            });
            Window.MouseUp += new MouseEventHandler((sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    pMouseNewState[0] = false;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    pMouseNewState[1] = false;
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    pMouseNewState[2] = false;
                }
            });
            Window.FormClosing += new FormClosingEventHandler((sender, e) =>
            {
                bAtomActive = false;
            });

            UpdateWindowSize(Window.Width, Window.Height - 39);

            for (int i = 0; i < 255; i++)
            {
                mapKeys.Add(i, 255);
            }

            mapKeys[0x00] = (byte)Key.NONE;
            mapKeys[0x41] = (byte)Key.A; mapKeys[0x42] = (byte)Key.B; mapKeys[0x43] = (byte)Key.C; mapKeys[0x44] = (byte)Key.D; mapKeys[0x45] = (byte)Key.E;
            mapKeys[0x46] = (byte)Key.F; mapKeys[0x47] = (byte)Key.G; mapKeys[0x48] = (byte)Key.H; mapKeys[0x49] = (byte)Key.I; mapKeys[0x4A] = (byte)Key.J;
            mapKeys[0x4B] = (byte)Key.K; mapKeys[0x4C] = (byte)Key.L; mapKeys[0x4D] = (byte)Key.M; mapKeys[0x4E] = (byte)Key.N; mapKeys[0x4F] = (byte)Key.O;
            mapKeys[0x50] = (byte)Key.P; mapKeys[0x51] = (byte)Key.Q; mapKeys[0x52] = (byte)Key.R; mapKeys[0x53] = (byte)Key.S; mapKeys[0x54] = (byte)Key.T;
            mapKeys[0x55] = (byte)Key.U; mapKeys[0x56] = (byte)Key.V; mapKeys[0x57] = (byte)Key.W; mapKeys[0x58] = (byte)Key.X; mapKeys[0x59] = (byte)Key.Y;
            mapKeys[0x5A] = (byte)Key.Z;

            mapKeys[0x70] = (byte)Key.F1; mapKeys[0x71] = (byte)Key.F2; mapKeys[0x73] = (byte)Key.F3; mapKeys[0x74] = (byte)Key.F4;
            mapKeys[0x75] = (byte)Key.F5; mapKeys[0x76] = (byte)Key.F6; mapKeys[0x77] = (byte)Key.F7; mapKeys[0x78] = (byte)Key.F8;
            mapKeys[0x79] = (byte)Key.F9; mapKeys[0x80] = (byte)Key.F10; mapKeys[0x81] = (byte)Key.F11; mapKeys[0x82] = (byte)Key.F12;

            mapKeys[0x28] = (byte)Key.DOWN; mapKeys[0x25] = (byte)Key.LEFT; mapKeys[0x27] = (byte)Key.RIGHT; mapKeys[0x26] = (byte)Key.UP;
            mapKeys[0x0D] = (byte)Key.ENTER;

            mapKeys[0x08] = (byte)Key.BACKSPACE; mapKeys[0x1B] = (byte)Key.ESCAPE; mapKeys[0x13] = (byte)Key.PAUSE;
            mapKeys[0x91] = (byte)Key.SCROLL; mapKeys[0x09] = (byte)Key.TAB; mapKeys[0x2E] = (byte)Key.DEL; mapKeys[0x24] = (byte)Key.HOME;
            mapKeys[0x23] = (byte)Key.END; mapKeys[0x21] = (byte)Key.PGUP; mapKeys[0x22] = (byte)Key.PGDN; mapKeys[0x2D] = (byte)Key.INS;
            mapKeys[0x10] = (byte)Key.SHIFT; mapKeys[0x11] = (byte)Key.CTRL;
            mapKeys[0x20] = (byte)Key.SPACE;

            mapKeys[0x30] = (byte)Key.K0; mapKeys[0x31] = (byte)Key.K1; mapKeys[0x32] = (byte)Key.K2; mapKeys[0x33] = (byte)Key.K3; mapKeys[0x34] = (byte)Key.K4;
            mapKeys[0x35] = (byte)Key.K5; mapKeys[0x36] = (byte)Key.K6; mapKeys[0x37] = (byte)Key.K7; mapKeys[0x38] = (byte)Key.K8; mapKeys[0x39] = (byte)Key.K9;

            mapKeys[0x60] = (byte)Key.NP0; mapKeys[0x61] = (byte)Key.NP1; mapKeys[0x62] = (byte)Key.NP2; mapKeys[0x63] = (byte)Key.NP3; mapKeys[0x64] = (byte)Key.NP4;
            mapKeys[0x65] = (byte)Key.NP5; mapKeys[0x66] = (byte)Key.NP6; mapKeys[0x67] = (byte)Key.NP7; mapKeys[0x68] = (byte)Key.NP8; mapKeys[0x69] = (byte)Key.NP9;
            mapKeys[0x6A] = (byte)Key.NP_MUL; mapKeys[0x6B] = (byte)Key.NP_ADD; mapKeys[0x6F] = (byte)Key.NP_DIV; mapKeys[0x6D] = (byte)Key.NP_SUB; mapKeys[0x6E] = (byte)Key.NP_DECIMAL;

            for (int i = 0; i < 255; i++)
            {
                if (mapKeys[i] == 255)
                {
                    mapKeys.Remove(i);
                }
            }

            return HWnd;
        }
        static string AppName;

        internal class PGEX
        {
            public static Engine pge;
        }
    }
}

namespace Test
{
    class Ret
    {
        static void Main()
        {

        }
    }
}
