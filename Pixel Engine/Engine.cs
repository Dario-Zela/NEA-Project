using System;
using System.Runtime.InteropServices;
using System.Numerics;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Win32;

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
            public int nID = 0, nFileOffset = 0;
            public IntPtr data = new IntPtr();
            public BufferedStream buffer;


            public void config()
            {
                buffer = new BufferedStream(new FileStream(new SafeFileHandle(data, true), FileAccess.Read));
            }
        }

        public extern rCode AddToPack(string sFile);
        public extern rCode SavePack(string sFile);
        public extern rCode LoadPack(string sFile);
        public extern rCode ClearPack(string sFile);
        public extern sEntry GetStreamBuffer(string sFile);

        private Dictionary<string, sEntry> mapFiles;
    }

    class Sprite
    {
        public extern Sprite();
        public extern Sprite(string sImageFile);
        public extern Sprite(string sImageFile, ref ResourcePack pack);
        public extern Sprite(int Width, int Height);

        public extern rCode LoadFromFile(string sImageFile, ref ResourcePack pack);
        public extern rCode LoadFromPGESprFile(string sImageFile, ref ResourcePack pack);
        public extern rCode SaveToPGESprFile(string sImageFile);

        public int Width;
        public int Height;
        public enum Mode { NORMAL, PERIODIC}

        public extern void SetSampleMode(Sprite.Mode mode = Sprite.Mode.NORMAL);
        public extern Pixel GetPixel(int x, int y);
        public extern bool SetPixel(int x, int y, Pixel p);

        public extern Pixel Sample(float x, float y);
        public extern Pixel SampleBL(float u, float v);
        public extern Pixel GetData();

        private Pixel ColData = Pixel.BLANK;
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
        public extern rCode Construct(int screen_w, int screen_h, int pixel_w, int pixel_h, bool full_screen = false, bool vsync = false);
        public extern rCode Start();

        public virtual bool OnUserCreate();
        public virtual bool OnUserDestroy();
        public virtual bool onUserUpdate(float fElapsedTime);

        public bool IsFocused();
        public HWButton GetKey(Key k);
        public HWButton GetMouse(int b);
        public public int GetMouseX();
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
        public void SetScreenSize(int w, int h);

        public string sAppName;

        Sprite pDefaultDrawTarget = null;
        Sprite pDrawTarget = null;
        Pixel.Mode nPixelMode = Pixel.Mode.NORMAL;
        float fBlendFactor = 1.0f;
        uint nScreenWidth = 256;
        uint nScreenHeight = 240;
        uint nPixelWidth = 4;
        uint nPixelHeight = 4;
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


        private void EngineThread();
        IntPtr hWnd = IntPtr.Zero;
        IntPtr WindowCreate();
        string AppName;

        protected override void WndProc(ref Mess);

        
    }
}
