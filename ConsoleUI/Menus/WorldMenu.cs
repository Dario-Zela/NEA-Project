/*
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Wrapper;

namespace UI
{
    class WorldMenu : Layer
    {
        public WorldMenu() : base("WorldMenu")
        {
            BinaryReader reader = new BinaryReader(new FileStream(@"..\..\..\Models\Saves\Worlds.dat", FileMode.Open));
            if (reader.BaseStream.Length == 0) goto Skip2;
            List<Tuple<string, short>> temp = new List<Tuple<string, short>>();
            while(reader.BaseStream.Position != reader.BaseStream.Length)
            {
                temp.Add(new Tuple<string, short>(reader.ReadString(), reader.ReadInt16()));
            }
            WorldNames = new string[temp.Count];
            WorldSeeds = new short[temp.Count];
            for (int i = 0; i < temp.Count; i++)
            {
                WorldNames[i] = temp[i].Item1;
                WorldSeeds[i] = temp[i].Item2;
            }
            reader.Close();
            Skip2:
            CurrentDrawingFunc = new Action(OldWorlds);
            draw = true;
            left = true;
            itemLeft = 0;
            itemRight = 0;
            NewWorldName = "";
            CursorPos = 0;
            NewSeed = "";
            ReturnToMainMenu = false;
        }

        static Action CurrentDrawingFunc;
        static string[] WorldNames = new string[0];
        static short[] WorldSeeds = new short[0];
        static readonly string[] Settings = new[] { "Copy", "Reset", "Delete" };
        static bool draw;
        static bool left;
        static int itemLeft;
        static int itemRight;
        static bool Error = false;

        public override void OnUpdate(TimeStep time)
        {
            CurrentDrawingFunc.Invoke();
        }

        static private void OldWorlds()
        {
            if (GetKey(Key.RIGHT).bPressed && left && WorldNames.Length > 0)
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
                        foreach (string WorldName in WorldNames)
                        {
                            if (WorldNames[itemLeft] + " Copy" == WorldName)
                            {
                                Error = true;
                                goto Skip;
                            }
                        }
                        BinaryWriter writer = new BinaryWriter(new FileStream(@"..\..\..\Models\Saves\Worlds.dat", FileMode.Append));
                        writer.Write(WorldNames[itemLeft] + " Copy");
                        writer.Write(WorldSeeds[itemLeft]);
                        writer.Close();
                        break;
                    case 1:
                        break;
                    case 2:
                        BinaryWriter writer2 = new BinaryWriter(new FileStream(@"..\..\..\Models\Saves\Worlds.dat", FileMode.Create));
                        for (int i = 0; i < WorldNames.Length; i++)
                        {
                            if (i == itemRight) continue;
                            else
                            {
                                writer2.Write(WorldNames[i]);
                                writer2.Write(WorldSeeds[i]);
                            }
                        }
                        writer2.Close();
                        itemLeft--;
                        break;
                }
                Skip:
                BinaryReader reader = new BinaryReader(new FileStream(@"..\..\..\Models\Saves\Worlds.dat", FileMode.Open));
                List<Tuple<string, short>> temp = new List<Tuple<string, short>>();
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    temp.Add(new Tuple<string, short>(reader.ReadString(), reader.ReadInt16()));
                }
                WorldNames = new string[temp.Count];
                WorldSeeds = new short[temp.Count];
                for (int i = 0; i < temp.Count; i++)
                {
                    WorldNames[i] = temp[i].Item1;
                    WorldSeeds[i] = temp[i].Item2;
                }
                reader.Close();
                left = true;
                draw = true;
            }
            if (draw)
            {
                Clear(Pixel.BLACK);
                DrawRect(10, 10, 680, 680, Pixel.WHITE);
                DrawText(65, 100, "Current Worlds", Pixel.WHITE, 20, 0);
                DrawRect(60, 90, 155, 40, Pixel.WHITE);
                DrawRect(220, 90, 180, 40, Pixel.WHITE);
                DrawText(225, 100, "Create New World", Pixel.WHITE, 20, 0);
                DrawRect(50, 130, 600, 500, Pixel.WHITE);
                DrawLine(61, 130, 215, 130, Pixel.BLACK);
                DrawLine(275, 130, 275, 630, Pixel.WHITE);
                if (Error)
                {
                    DrawText(290, 230, "Error: The name is already present", Pixel.DARK_RED, 20, 0);
                    Error = false;
                }
                for (int i = 0; i < WorldNames.Length; i++)
                {
                    if (WorldNames[i].Length < 21)
                    {
                        if (i == itemLeft) DrawText(65, 140 + 30 * i, WorldNames[i], Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                        else DrawText(65, 140 + 30 * i, WorldNames[i], Pixel.WHITE, 20, 0);
                    }
                    else
                    {
                        string ReducedName = ToString(WorldNames[i].Take(20));
                        if (i == itemLeft) DrawText(65, 140 + 30 * i, ReducedName, Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                        else DrawText(65, 140 + 30 * i, ReducedName, Pixel.WHITE, 20, 0);
                    }
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
        static string NewSeed;
        static bool ReturnToMainMenu;
        static bool Valid = false;
        static private void NewWorld()
        {
            if (GetKey(Key.DOWN).bPressed)
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
                        if (NewSeed.Length < 10) NewSeed += ReadKey().ToString();
                        break;
                }
                Valid = true;
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
                        if (NewSeed.Length > 0) NewSeed = NewSeed.Remove(NewSeed.Length - 1);
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
                DrawRect(220, 90, 180, 40, Pixel.WHITE);
                DrawText(225, 100, "Create New World", Pixel.WHITE, 20, 0);
                DrawRect(50, 130, 600, 500, Pixel.WHITE);
                DrawLine(221, 130, 395, 130, Pixel.BLACK);
                DrawText(65, 595, "Press Enter to Save", Pixel.WHITE, 20, 0);
                if (CursorPos == 0)
                {
                    DrawText(65, 140, "Enter World Name: " + NewWorldName + Mul('.', 20 - NewWorldName.Length), Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                    DrawText(65, 170, "Enter Seed: " + NewSeed + Mul('.', 10 - NewSeed.Length), Pixel.WHITE, 20, 0);
                }
                else
                {
                    DrawText(65, 140, "Enter World Name: " + NewWorldName + Mul('.', 20 - NewWorldName.Length), Pixel.WHITE, 20, 0);
                    DrawText(65, 170, "Enter Seed: " + NewSeed + Mul('.', 10 - NewSeed.Length), Pixel.WHITE, 20, 0, Pixel.DARK_BLUE);
                }
                foreach (string WorldName in WorldNames)
                {
                    if (NewWorldName == WorldName)
                    {
                        DrawText(65, 200, "Error: The name is already present", Pixel.DARK_RED, 20, 0);
                        Valid = false;
                    }
                }
                draw = false;
            }
            if (GetKey(Key.TAB).bPressed)
            {
                CurrentDrawingFunc = new Action(OldWorlds);
                CursorPos = 0;
                draw = true;
            }
            if (GetKey(Key.ENTER).bPressed && Valid)
            {
                BinaryWriter writer = new BinaryWriter(new FileStream(@"..\..\..\Models\Saves\Worlds.dat", FileMode.Append));
                writer.Write(NewWorldName);
                writer.Write(Seed(NewSeed));
                writer.Close();
                ReturnToMainMenu = true;
                draw = true;
            }
        }

        static private short Seed(string s)
        {
            short ToRet = 0;
            foreach (var @char in s)
            {
                ToRet += (short)@char;
            }
            return ToRet;
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
        static private string ToString(IEnumerable<char> s)
        {
            string ToRet = "";
            foreach (char c in s)
            {
                ToRet += c;
            }
            return ToRet;
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

*/