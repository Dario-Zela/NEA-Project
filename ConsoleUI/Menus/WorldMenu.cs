using Pixel_Engine;
using System;
using System.IO;

namespace UI
{
    partial class Game
    {
        class WorldMenu : IGameElement
        {
            public WorldMenu()
            {
                StreamReader Saves = new StreamReader(new FileStream(@"..\..\..\Models\Saves\Worlds.txt", FileMode.Open));
                WorldNames = Saves.ReadToEnd().Split(new char[] { ',' });
                Saves.Close();
                CurrentDrawingFunc = new Action(OldWorlds);
                draw = true;
                left = true;
                itemLeft = 0;
                itemRight = 0;
                NewWorldName = "";
                CursorPos = 0;
                Seed = "";
                ReturnToMainMenu = false;
            }

            static Action CurrentDrawingFunc;
            static string[] WorldNames;
            static readonly string[] Settings = new[] { "Copy", "Reset", "Delete" };
            static bool draw;
            static bool left;
            static int itemLeft;
            static int itemRight;

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
                if (GetKey(Key.ENTER).bPressed && !left)
                {
                    switch (itemRight)
                    {
                        case 0:
                            StreamWriter writer = new StreamWriter(new FileStream(@"..\..\..\Models\Saves\Worlds.txt", FileMode.Append));
                            writer.Write("," + WorldNames[itemLeft] + " Copy");
                            writer.Close();
                            break;
                        case 1:
                            break;
                        case 2:
                            StreamWriter writer2 = new StreamWriter(new FileStream(@"..\..\..\Models\Saves\Worlds.txt", FileMode.Create));
                            for (int i = 0; i < WorldNames.Length; i++)
                            {
                                if (i != itemRight && i != WorldNames.Length - 1) writer2.Write(WorldNames[i] + ",");
                                else if (i == WorldNames.Length - 1) writer2.Write(WorldNames[i]);
                            }
                            writer2.Close();
                            break;
                    }
                    StreamReader Saves = new StreamReader(new FileStream(@"..\..\..\Models\Saves\Worlds.txt", FileMode.Open));
                    WorldNames = Saves.ReadToEnd().Split(new char[] { ',' });
                    Saves.Close();
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

            static string NewWorldName;
            static int CursorPos;
            static string Seed;
            static bool ReturnToMainMenu;
            static private void NewWorld()
            {
                if (GetKey(Key.TAB).bPressed)
                {
                    CursorPos = CursorPos == 0 ? 1 : 0;
                    draw = true;
                }
                if (ReadKey() != '\0')
                {
                    switch (CursorPos)
                    {
                        case 0:
                            if (NewWorldName.Length < 20) NewWorldName += ReadKey().ToString();
                            break;
                        case 1:
                            if (Seed.Length < 10) Seed += ReadKey().ToString();
                            break;
                    }

                    draw = true;
                }
                if (GetKey(Key.BACKSPACE).bPressed || GetKey(Key.BACKSPACE).bHeld)
                {
                    switch (CursorPos)
                    {
                        case 0:
                            if (NewWorldName.Length > 0) NewWorldName = NewWorldName.Remove(NewWorldName.Length - 1);
                            break;
                        case 1:
                            if (Seed.Length > 0) Seed = Seed.Remove(Seed.Length - 1);
                            break;
                    }
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
                    DrawLine(221, 130, 395, 130, Pixel.BLACK);
                    DrawText(65, 200, "Press TAB to switch", Pixel.WHITE, 20, 0);
                    DrawText(65, 595, "Press Enter to Save", Pixel.WHITE, 20, 0);
                    if (CursorPos == 0)
                    {
                        DrawText(65, 140, "Enter World Name: " + NewWorldName + Mul('.', 20 - NewWorldName.Length), Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                        DrawText(65, 170, "Enter Seed: " + Seed + Mul('.', 10 - Seed.Length), Pixel.WHITE, 20, 0);
                    }
                    else
                    {
                        DrawText(65, 140, "Enter World Name: " + NewWorldName + Mul('.', 20 - NewWorldName.Length), Pixel.WHITE, 20, 0);
                        DrawText(65, 170, "Enter Seed: " + Seed + Mul('.', 10 - Seed.Length), Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                    }
                    draw = false;
                }
                if (GetKey(Key.TAB).bPressed && isShifted)
                {
                    CurrentDrawingFunc = new Action(OldWorlds);
                    CursorPos = 0;
                    draw = true;
                }
                if (GetKey(Key.ENTER).bPressed)
                {
                    StreamWriter writer = new StreamWriter(new FileStream(@"..\..\..\Models\Saves\Worlds.txt", FileMode.Append));
                    writer.Write("," + NewWorldName);
                    writer.Close();
                    ReturnToMainMenu = true;
                    draw = true;
                }
            }

            static private string Mul(char x, int num)
            {
                string ret = "";
                for (int i = 0; i < num; i++)
                {
                    ret += x;
                }
                return ret;
            }

            public IGameElement NewElement()
            {
                if (ReturnToMainMenu)
                {
                    return new MainMenu(1);
                }
                if (GetKey(Key.ESCAPE).bPressed)
                {
                    return new MainMenu(1);
                }
                return this;
            }
        }
    }
}
