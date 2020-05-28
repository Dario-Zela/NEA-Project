using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Pixel_Engine;

namespace UI
{
    partial class Game
    {
        class WorldMenu : IGameElement
        {
            public WorldMenu()
            {
                Saves = new StreamReader(new FileStream(@"..\..\..\Models\Saves\Worlds.txt", FileMode.Open));
                WorldNames = Saves.ReadToEnd().Split(new char[] { ',' });
            }

            static Action CurrentDrawingFunc = new Action(OldWorlds);
            static string[] WorldNames;
            static readonly string[] Settings = new[] { "Copy", "Reset", "Delete" };
            static StreamReader Saves;
            static bool draw = true;
            static bool left = true;
            static int itemLeft = 0;
            static int itemRight = 0;

            public void Draw()
            {
                CurrentDrawingFunc.Invoke();
            }

            static private void OldWorlds()
            {
                if (GetKey(Key.RIGHT).bPressed && left)
                {
                    left = false;
                    draw = true;
                    itemRight = 0;
                }
                else if (GetKey(Key.LEFT).bPressed && !left)
                {
                    left = true;
                    draw = true;
                }
                if (GetKey(Key.DOWN).bPressed)
                {
                    if (left) itemLeft = ++itemLeft > WorldNames.Length - 1 ? 0 : itemLeft;
                    else itemRight = ++itemRight > 2 ? 0 : itemRight;
                    draw = true;
                }
                else if (GetKey(Key.UP).bPressed)
                {
                    if (left) itemLeft = --itemLeft < 0 ? WorldNames.Length - 1 : itemLeft;
                    else itemRight = --itemRight < 0 ? 2 : itemRight;
                    draw = true;
                }
                if (draw)
                {
                    Clear(Pixel.BLACK);
                    DrawRect(10, 10, 680, 680, Pixel.WHITE);
                    DrawText(65, 100, "Current Worlds", Pixel.WHITE, 20, 0);
                    DrawRect(60, 90, 155, 40, Pixel.WHITE);
                    DrawRect(220, 90, 175, 40, Pixel.WHITE);
                    DrawText(225, 100, "Create New World", Pixel.WHITE, 20, 0);
                    DrawRect(50, 130, 600, 500, Pixel.WHITE);
                    DrawLine(61, 130, 215, 130, Pixel.BLACK);
                    DrawLine(275, 130, 275, 630, Pixel.WHITE);
                    for (int i = 0; i < WorldNames.Length; i++)
                    {
                        if (i == itemLeft) DrawText(65, 140 + 30 * i, WorldNames[i], Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                        else DrawText(65, 140 + 30 * i, WorldNames[i], Pixel.WHITE, 20, 0);
                    }
                    for (int i = 0; i < Settings.Length; i++)
                    {
                        if (i == itemRight && !left) DrawText(290, 140 + 30 * i, Settings[i], Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                        else DrawText(290, 140 + 30 * i, Settings[i], Pixel.WHITE, 20, 0);
                    }
                    draw = false;
                }
                if (GetKey(Key.TAB).bPressed)
                {
                    CurrentDrawingFunc = new Action(NewWorld);
                    draw = true;
                }
            }
            static private void NewWorld()
            {
                if (draw)
                {
                    Clear(Pixel.BLACK);
                    DrawRect(10, 10, 680, 680, Pixel.WHITE);
                    DrawText(65, 100, "Current Worlds", Pixel.WHITE, 20, 0);
                    DrawRect(60, 90, 155, 40, Pixel.WHITE);
                    DrawRect(220, 90, 175, 40, Pixel.WHITE);
                    DrawText(225, 100, "Create New World", Pixel.WHITE, 20, 0);
                    DrawRect(50, 130, 600, 500, Pixel.WHITE);
                    DrawLine(220, 130, 395, 130, Pixel.BLACK);
                    DrawLine(275, 130, 275, 630, Pixel.WHITE);
                    for (int i = 0; i < WorldNames.Length; i++)
                    {
                        if (i == itemLeft) DrawText(65, 140 + 30 * i, WorldNames[i], Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                        else DrawText(65, 140 + 30 * i, WorldNames[i], Pixel.WHITE, 20, 0);
                    }
                    for (int i = 0; i < Settings.Length; i++)
                    {
                        if (i == itemRight && !left) DrawText(290, 140 + 30 * i, Settings[i], Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                        else DrawText(290, 140 + 30 * i, Settings[i], Pixel.WHITE, 20, 0);
                    }
                    draw = false;
                }
                if (GetKey(Key.TAB).bPressed && isShifted)
                {
                    CurrentDrawingFunc = new Action(OldWorlds);
                    draw = true;
                }
            }

            public IGameElement NewElement()
            {
                if (GetKey(Key.ESCAPE).bPressed)
                {
                    return new MainMenu(1);
                }
                return this;
            }
        }
    }
}
