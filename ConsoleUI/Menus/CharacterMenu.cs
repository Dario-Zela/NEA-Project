﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pixel_Engine;

namespace UI
{
    partial class Game
    {
        class CharactersMenu : IGameElement
        {
            bool draw = true;

            public void Draw()
            {
                if (draw)
                {
                    Clear(Pixel.BLACK);
                    DrawRect(10, 10, 680, 680, Pixel.WHITE);
                    DrawText(100, 100, "Characters Menu", Pixel.WHITE, 20, 0);
                    DrawRect(90, 90, 165, 40, Pixel.WHITE);
                    DrawRect(50, 130, 600, 500, Pixel.WHITE);
                    DrawLine(90, 130, 255, 130, Pixel.BLACK);
                    draw = false;
                }
            }

            public IGameElement NewElement()
            {
                if (GetKey(Key.ESCAPE).bPressed)
                {
                    return new MainMenu(0);
                }
                return this;
            }
        }
    }
}