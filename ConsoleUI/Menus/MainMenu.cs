using Pixel_Engine;
using System;

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
            readonly string[] Settings = new[] { "[Character Menu]", "[World Menu]", "[Options]", "[Quit]" };
            readonly int[] SettingX = new[] { 120, 290, 430, 525 };

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
                    for (int i = 0; i < Settings.Length; i++)
                    {
                        if (i == CurrentIdx) DrawText(SettingX[i], 550, Settings[i], Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                        else DrawText(SettingX[i], 550, Settings[i], Pixel.WHITE, 20, 0);
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
