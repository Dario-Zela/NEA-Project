using System;
using System.Runtime.InteropServices;

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
        enum Mode { NORMAL, MASK, ALPHA, CUSTOM }

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

    public class Engine
    {

    }
}
