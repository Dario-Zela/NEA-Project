using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pixel_Engine;

namespace UI
{
    partial class Game
    {
        class MainMenu : IGameElement
        {
            public MainMenu()
            {

            }
            public MainMenu(int CurrentIdx)
            {
                this.CurrentIdx = CurrentIdx;
            }

            bool draw = true;
            int CurrentIdx = 0;

            public void Draw()
            {
                if (GetKey(Key.LEFT).bPressed)
                {
                    CurrentIdx = --CurrentIdx < 0 ? 3 : CurrentIdx;
                    draw = true;
                }
                if (GetKey(Key.RIGHT).bPressed)
                {
                    CurrentIdx = ++CurrentIdx > 3 ? 0 : CurrentIdx;
                    draw = true;
                }
                if (draw)
                {
                    Clear(Pixel.BLACK);
                    DrawRect(10, 10, 680, 680, Pixel.WHITE);
                    DrawText(160, 130, "Welcome to", Pixel.GREEN, 60, 0);
                    DrawText(230, 210, "Game", Pixel.DARK_GREEN, 60, 0);
                    switch (CurrentIdx)
                    {
                        case 0:
                            DrawText(120, 550, "[Character Menu]", Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                            DrawText(290, 550, "[World Menu]", Pixel.WHITE, 20, 0);
                            DrawText(430, 550, "[Options]", Pixel.WHITE, 20, 0);
                            DrawText(525, 550, "[Quit]", Pixel.WHITE, 20, 0);
                            break;
                        case 1:
                            DrawText(120, 550, "[Character Menu]", Pixel.WHITE, 20, 0);
                            DrawText(290, 550, "[World Menu]", Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                            DrawText(430, 550, "[Options]", Pixel.WHITE, 20, 0);
                            DrawText(525, 550, "[Quit]", Pixel.WHITE, 20, 0);
                            break;
                        case 2:
                            DrawText(120, 550, "[Character Menu]", Pixel.WHITE, 20, 0);
                            DrawText(290, 550, "[World Menu]", Pixel.WHITE, 20, 0);
                            DrawText(430, 550, "[Options]", Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                            DrawText(525, 550, "[Quit]", Pixel.WHITE, 20, 0);
                            break;
                        case 3:
                            DrawText(120, 550, "[Character Menu]", Pixel.WHITE, 20, 0);
                            DrawText(290, 550, "[World Menu]", Pixel.WHITE, 20, 0);
                            DrawText(430, 550, "[Options]", Pixel.WHITE, 20, 0);
                            DrawText(525, 550, "[Quit]", Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                            break;
                    }
                    draw = false;
                }
            }

            public IGameElement NewElement()
            {
                if (GetKey(Key.ENTER).bPressed || GetKey(Key.UP).bPressed)
                {
                    switch (CurrentIdx)
                    {
                        case 0:
                            return new CharactersMenu();
                        case 1:
                            return new WorldMenu();
                        case 2:
                            return new OptionsMenu();
                        case 3:
                            Quit();
                            return null;
                        default:
                            throw new Exception();
                    }
                }
                else return this;
            }
        }
    }
}
