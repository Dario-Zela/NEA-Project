using System;
using System.Collections.Generic;
//using Models.WorldGen;
using Pixel_Engine;
using System.Linq;
using Audio_Engine;

namespace UI
{
    class Audio : Engine
    {
        Audio_Engine.Audio_Engine audio;
        public override bool OnUserCreate()
        {
            audio = new Audio_Engine.Audio_Engine(44100, 1, 16);
            return true;
        }

        public override bool onUserUpdate(float fElapsedTime)
        {
            foreach (string key in Enum.GetNames(typeof(Key)))
            {
                if (GetKey((Key)Enum.Parse(typeof(Key),key)).bHeld)
                {
                    audio.PlaySine(440f, short.MaxValue);
                }
            }
            return true;
        }
    }

    class Test
    {
        static void Main()
        {
            Audio audio = new Audio();
            if ((int)audio.Construct(10, 10, 100, 100) == 1)
            {
                audio.Start();
            }
        }
    }
}