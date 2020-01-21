using System;
using System.Runtime.InteropServices;
using System.Numerics;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;

namespace Pixel_Engine
{
    [StructLayout(LayoutKind.Explicit)]
    struct Pixel
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
        #endregion
    }

    public enum rCode
    {
        FAIL = 0,
        OK = 1,
        NO_FILE = -1
    }

    class HWButton
    {
        public bool bPressed = false;
        public bool bReleased = false;
        public bool bHeld = false;
    }

    class ResourcePack
    {
        public ResourcePack() { }
        public class sEntry
        {
            public int nID = 0, nFileOffset = 0, nFileSize = 0;
            public byte[] data = null;
            public BufferedStream buffer;


            public void config()
            {
                buffer = new BufferedStream(new MemoryStream(data, nFileOffset, nFileSize));
            }
        }

        public rCode AddToPack(string sFile)
        {
            try
            {
                BinaryReader reader = new BinaryReader(new FileStream(sFile, FileMode.Open));
                sEntry entry = new sEntry();
                int size = (int)reader.BaseStream.Length;
                entry.nFileSize = size;
                entry.data = new byte[entry.nFileSize];
                reader.Read(entry.data, 0, size);
                mapFiles.Add(sFile, entry);
                reader.Close();
                return rCode.OK;
            }
            catch { return rCode.FAIL; }
        }
        public rCode SavePack(string sFile)
        {
            try
            {
                BinaryWriter writer = new BinaryWriter(new FileStream(sFile, FileMode.OpenOrCreate));
                int MapSize = mapFiles.Count;
                writer.Write(MapSize);
                foreach (var entry in mapFiles)
                {
                    int PathSize = entry.Key.Length;
                    writer.Write(PathSize);
                    writer.Write(entry.Key);
                    writer.Write(entry.Value.nID);
                    writer.Write(entry.Value.nFileSize);
                    writer.Write(entry.Value.nFileOffset);
                }
                int offset = (int)writer.BaseStream.Position;
                foreach (var entry in mapFiles)
                {
                    entry.Value.nFileOffset = offset;
                    writer.Write(entry.Value.data);
                    offset += entry.Value.nFileSize;
                }
                writer.BaseStream.Position = 0;
                foreach (var entry in mapFiles)
                {
                    int PathSize = entry.Key.Length;
                    writer.Write(PathSize);
                    writer.Write(entry.Key);
                    writer.Write(entry.Value.nID);
                    writer.Write(entry.Value.nFileSize);
                    writer.Write(entry.Value.nFileOffset);
                }
                writer.Close();
                return rCode.OK;
            }
            catch { return rCode.FAIL; }
        }
        public rCode LoadPack(string sFile)
        {
            try
            {
                BinaryReader reader = new BinaryReader(new FileStream(sFile, FileMode.Open));
                int MapEntries = reader.ReadInt32();
                for (int i = 0; i < MapEntries; i++)
                {
                    int PathSize = reader.ReadInt32();
                    string FileName = "";
                    for (int j = 0; j < PathSize; j++)
                    {
                        FileName += reader.ReadChar();
                    }
                    sEntry entry = new sEntry();
                    entry.nID = reader.ReadInt32();
                    entry.nFileSize = reader.ReadInt32();
                    entry.nFileOffset = reader.ReadInt32();
                    mapFiles.Add(FileName, entry);
                }

                foreach (var entry in mapFiles)
                {
                    entry.Value.data = new byte[entry.Value.nFileSize];
                    reader.BaseStream.Position = entry.Value.nFileOffset;
                    reader.Read(entry.Value.data, 0, entry.Value.nFileSize);
                }

                reader.Close();
                return rCode.OK;
            }
            catch { return rCode.FAIL; }
        }
        public rCode ClearPack()
        {
            mapFiles.Clear();
            return rCode.OK;
        }
        public sEntry GetStreamBuffer(string sFile)
        {
            return mapFiles[sFile];
        }

        private Dictionary<string, sEntry> mapFiles = new Dictionary<string,sEntry>();
    }

    class Sprite
    {
        public Sprite()
        {
            ColData = null;
            Width = 0;
            Height = 0;
        }
        public Sprite(string sImageFile)
        {
            LoadFromFile(sImageFile);
        }
        public Sprite(string sImageFile, ref ResourcePack pack)
        {
            LoadFromFile(sImageFile);
        }
        public Sprite(int Width, int Height)
        {
            if (ColData != null) ColData = null;
            this.Width = Width;
            this.Height = Height;
            for (int i = 0; i < Width * Height; i++)
                ColData[i] = new Pixel();
        }

        public rCode LoadFromFile(string sImageFile)
        {
            Bitmap bitmap = new Bitmap(sImageFile);
            if (bitmap == null) return rCode.NO_FILE;
            Width = bitmap.Width;
            Height = bitmap.Height;
            ColData = new Pixel[Width * Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Color c = bitmap.GetPixel(x, y);
                    SetPixel(x, y, new Pixel(c.R, c.G, c.B, c.A));
                }
            }
            bitmap.Dispose();
            return rCode.OK;
        }

        /*
        public rCode LoadFromPGESprFile(string sImageFile, ref ResourcePack pack)
        {
            if (ColData != null) ColData = null;
            if (pack == new ResourcePack())
            {
                try
                {
                    BinaryReader reader = new BinaryReader(new FileStream(sImageFile, FileMode.Open));
                    Width = reader.ReadInt32();
                    Height = reader.ReadInt32();
                    ColData = new Pixel[Width * Height];
                    for (int i = 0; i < Width * Height; i++)
                    {
                        ColData[i] = new Pixel(reader.ReadInt32());
                    }
                    reader.Close()
                    return rCode.OK;
                }
                catch { return rCode.FAIL; }
            }
            else
            {
                try
                {
                    var streamBuffer = pack.GetStreamBuffer(sImageFile);
                    StreamReader reader = new StreamReader(streamBuffer.buffer);
                    char[] buffer = new char[sizeof(int)];
                    reader.Read(buffer, 0, sizeof(int));
                    Width = int.Parse(new string(buffer));
                    reader.Read(buffer, 0, sizeof(int));
                    Height = int.Parse(new string(buffer));
                    ColData = new Pixel[Width * Height];
                    buffer = new char[Width * Height * sizeof(int)];
                    reader.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < Width * Height; i++)
                    {
                        string value = "";
                        value += buffer[i * 4];
                        value += buffer[i * 4 + 1];
                        value += buffer[i * 4 + 1];
                        value += buffer[i * 4 + 1];
                        ColData[i] = new Pixel(int.Parse(value));
                    }
                    reader.Close()
                    return rCode.OK;
                }
                catch { return rCode.FAIL; }
            }
        }
        public rCode SaveToPGESprFile(string sImageFile)
        {
            if (ColData == null) return rCode.FAIL;
            try
            {
                BinaryWriter writer = new BinaryWriter(new FileStream(sImageFile, FileMode.OpenOrCreate));
                writer.Write(Width);
                writer.Write(Height);
                for (int i = 0; i < ColData.Length; i++)
                {
                    writer.Write(ColData[i].IntValue);
                }
                reader.Close()
                return rCode.OK;
            }
            catch { return rCode.FAIL; }
        }
        */

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
                if (x >= 0 && x < Width && y <= 0 && y < Height) return ColData[y * Width + x];
                else return Pixel.BLACK;
            }
            else return ColData[Math.Abs(y % Height) * Width + Math.Abs(x % Width)];
        }
        public bool SetPixel(int x, int y, Pixel p)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                ColData[y * Width + x] = p;
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

        private Pixel[] ColData = null;
        private Mode modeSample = Mode.NORMAL;
    }

    enum Key
    {
        NONE,
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        K0, K1, K2, K3, K4, K5, K6, K7, K8, K9,
        F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,
        UP, DOWN, LEFT, RIGHT,
        SPACE, TAB, SHIFT, CTRL, INS, DEL, HOME, END, PGUP, PGDN,
        BACK, ESCAPE, RETURN, ENTER, PAUSE, SCROLL,
        NP0, NP1, NP2, NP3, NP4, NP5, NP6, NP7, NP8, NP9,
        NP_MUL, NP_DIV, NP_ADD, NP_SUB, NP_DECIMAL,
    };

    public virtual class Engine
    {
        public Engine()
        {
            sAppName = "Undefined";
            PGEX.pge = this;
        }

        public rCode Construct(int screenW, int screenH, int pixelW, int pixelH, bool fullScreen = false, bool vSync = false)
        {
            nScreenWidth = screenW;
            nScreenHeight = screenH;
            nPixelWidth = pixelW;
            nPixelHeight = pixelH;
            bFullScreen = fullScreen;
            bEnableVSYNC = vSync;

            fPixelX = 2.0f / (float)nScreenWidth;
            fPixelY = 2.0f / (float)nScreenHeight;
            if (nPixelWidth == 0 || nPixelHeight == 0 || nScreenWidth == 0 || nScreenHeight == 0) return rCode.FAIL;

            AppName = sAppName;
            ConstructFontSheet();
            pDefaultDrawTarget = new Sprite(nScreenWidth, nScreenHeight);
            SetDrawTarget(ref pDefaultDrawTarget);
            return rCode.OK;
        }
        public rCode Start()
        {
            try
            {
                WindowCreate();
                bAtomActivate = true;
                Thread thread = new Thread(EngineThread);
                Message message = new Message();
                while (Message.Create())
                {
                    
                }
            }
            catch { return rCode.FAIL; }
        }

        public virtual bool OnUserCreate();
        public virtual bool OnUserDestroy();
        public virtual bool onUserUpdate(float fElapsedTime);

        public bool IsFocused();
        public HWButton GetKey(Key k);
        public HWButton GetMouse(int b);
        public int GetMouseX();
        public int GetMouseY();
        public int GetMouseWheel();

        public int ScreenWidth();
        public int ScreenHeght();
        public int GetDrawTargetWidth();
        public int GetDrawTargetHeight();
        public Sprite GetDrawTarget();

        public void SetDrawTarget(ref Sprite target);
        public void SetPixelMode(Pixel.Mode m);
        public Pixel.Mode GetPixelMode();
        public void SetPixelMode(Func<int, int, Pixel, Pixel, Pixel> pixelMode);
        public void SetPixelBlend(float fBlend);
        public void SetSubPixelOffset(float ox, float oy);

        public virtual bool Draw(int x, int y, Pixel p);
        public void DrawLine(int x1, int y1, int x2, int y2, Pixel p, uint pattern = 0xFFFFFFFF);
        public void DrawCircle(int x, int y, int radius, Pixel p, byte mask = 0xFF);
        public void FillCircle(int x, int y, int radius, Pixel p);
        public void DrawRect(int x, int y, int w, int h, Pixel p);
        public void FillRect(int x, int y, int w, int h, Pixel p);
        public void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3, Pixel p);
        public void FillTriangle(int x1, int y1, int x2, int y2, int x3, int y3, Pixel p);
        public void DrawSprite(int x, int y, ref Sprite sprite, uint scale = 1);
        public void DrawPartialSprite(int x, int y, ref Sprite sprite, int ox, int oy, int w, int h, uint scale = 1);
        public void DrawString(int x, int y, string sText, Pixel col, uint scale = 1);
        public void Clear(Pixel p);
        public void SetScreenSize(int w, int h)
        {
            pDefaultDrawTarget = null;
            nScreenWidth = w;
            nScreenHeight = h;
            pDefaultDrawTarget = new Sprite(nScreenWidth, nScreenHeight);
            SetDrawTarget(ref pDefaultDrawTarget);
            glClear(GL_COLOR_BUFFER_BIT);
            SwapBuffers(glDeviceContext);
            glClear(GL_COLOR_BUFFER_BIT);
            UpdateViewport();
        }

        public string sAppName;

        Sprite pDefaultDrawTarget = null;
        Sprite pDrawTarget = null;
        Pixel.Mode nPixelMode = Pixel.Mode.NORMAL;
        float fBlendFactor = 1.0f;
        int nScreenWidth = 256;
        int nScreenHeight = 240;
        int nPixelWidth = 4;
        int nPixelHeight = 4;
        int nMousePosX = 0;
        int nMousePosY = 0;
        int nMouseWheelDelta = 0;
        int nMousePosXcache = 0;
        int nMousePosYcache = 0;
        int nMouseWheelDeltaCache = 0;
        int nWindowWidth = 0;
        int nWindowHeight = 0;
        int nViewX = 0;
        int nViewY = 0;
        int nViewW = 0;
        int nViewH = 0;
        bool bFullScreen = false;
        float fPixelX = 1.0f;
        float fPixelY = 1.0f;
        float fSubPixelOffsetX = 0.0f;
        float fSubPixelOffsetY = 0.0f;
        bool bHasInputFocus = false;
        bool bHasMouseFocus = false;
        bool bEnableVSYNC = false;
        float fFrameTimer = 1.0f;
        int nFrameCount = 0;
        Sprite fontSprite = null;
        Func<int, int, Pixel, Pixel, Pixel> funcPixelMode;

        static Dictionary<int, byte> mapKeys;
        bool[] pKeyNewState = new bool[256];
        bool[] pKeyOldState = new bool[256];
        HWButton[] pKeyboardState = new HWButton[256];

        bool[] pMouseNewState = new bool[5];
        bool[] pMouseOldState = new bool[5];
        HWButton[] pMouseState = new HWButton[5];

        bool bAtomActivate;

        void UpdateMouse(int x, int y);
        void UpdateMouseWheel(int delta);
        void UpdateWindowSize(int x, int y);
        void UpdateViewport();
        bool OpenGLCreate();
        void ConstructFontSheet();


        IntPtr glDeviceContext = new IntPtr();
        IntPtr glRenderContext = new IntPtr();

        UInt32 glBuffer;

        private void EngineThread();
        IntPtr HWnd = new IntPtr();
        IntPtr WindowCreate();
        string AppName;

        internal class PGEX
        {
            public static Engine pge;
        }
    }
}


namespace Pixel_Engine
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
