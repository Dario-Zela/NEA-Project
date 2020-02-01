using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_Engine
{
    class TileSetEditorDrawer : Engine
    {
        TileSet tileSet;
        int CurrentState = 0;
        int CurrentTile = 0;
        Pixel CurrentColour = Pixel.WHITE;
        int CurrentChannel = 0;
        Sprite CurrentSprite = null;

        public TileSetEditorDrawer(string Path, int TileWidth = 0)
        {
            tileSet = new TileSet(Path);
            tileSet.TileWidth = TileWidth == 0 ? tileSet.TileWidth :  TileWidth;
        }

        public override bool OnUserCreate()
        {
            return true;
        }

        public override bool onUserUpdate(float fElapsedTime)
        {
            Clear(Pixel.BLACK);
            if (CurrentState == 0)
            {
                if (GetKey(Key.R).bPressed)
                {
                    tileSet.SetName(tileSet.Names[CurrentTile], Console.ReadLine());
                }
                else if (GetKey(Key.RIGHT).bPressed)
                {
                    CurrentTile++;
                    CurrentTile = CurrentTile == tileSet.Names.Length ? 0 : CurrentTile;
                }
                else if (GetKey(Key.LEFT).bPressed)
                {
                    CurrentTile--;
                    CurrentTile = CurrentTile == -1 ? tileSet.Names.Length - 1 : CurrentTile;
                }
                else if (GetKey(Key.ENTER).bPressed)
                {
                    CurrentState++;
                    CurrentSprite = tileSet.GetTile(tileSet.Names[CurrentTile]);
                }
                else if (GetKey(Key.NP_ADD).bPressed) tileSet.AddTile(" ", new Sprite(tileSet.TileWidth, tileSet.TileWidth));
                else if (GetKey(Key.NP_SUB).bPressed)
                {
                    tileSet.RemoveTile(tileSet.Names[CurrentTile]);
                    CurrentTile = CurrentTile == tileSet.Names.Length ? CurrentTile - 1 : CurrentTile;
                }

                DrawString(0, 0, "Viewing Mode, Press Enter to Switch to Edit Mode\nPress the Arrow Keys to Move Around", Pixel.WHITE);
                DrawString(500, 0, "Name :" + tileSet.Names[CurrentTile], Pixel.WHITE);
                DrawSprite(200, 150, tileSet.GetTile(tileSet.Names[CurrentTile]), 25);
            }
            else if (CurrentState == 1)
            {
                if (GetMouse(0).bHeld)
                {
                    int x = (GetMouseX() - 300) / 25;
                    int y = (GetMouseY() - 150) / 25;

                    CurrentSprite.SetPixel(x, y, CurrentColour);
                }
                else if (GetKey(Key.UP).bHeld)
                {
                    switch (CurrentChannel)
                    {
                        case 0:
                            CurrentColour.R = (int)CurrentColour.R + 2 > 255 ? (byte)255: CurrentColour.R;
                            break;
                        case 1:
                            CurrentColour.G = (int)CurrentColour.R + 2 > 255 ? (byte)255 : CurrentColour.R;
                            break;
                        case 2:
                            CurrentColour.B = (int)CurrentColour.R + 2 > 255 ? (byte)255 : CurrentColour.R;
                            break;
                        case 4:
                            CurrentColour.A = (int)CurrentColour.R + 2 > 255 ? (byte)255 : CurrentColour.R;
                            break;
                    }
                }
                else if (GetKey(Key.DOWN).bHeld)
                {
                    switch (CurrentChannel)
                    {
                        case 0:
                            CurrentColour.R--;
                            break;
                        case 1:
                            CurrentColour.G--;
                            break;
                        case 2:
                            CurrentColour.B--;
                            break;
                        case 4:
                            CurrentColour.A--;
                            break;
                    }
                }
                else if (GetKey(Key.R).bHeld) CurrentChannel = 0;
                else if (GetKey(Key.G).bHeld)
                    CurrentChannel = 1;
                else if (GetKey(Key.B).bHeld) CurrentChannel = 2;
                else if (GetKey(Key.A).bHeld) CurrentChannel = 3;

                DrawString(0, 0, "Drawing Mode, Press Enter to Switch to Edit Mode\nPress the Mouse to Select the Draw Over The Point" +
                    "\nPress The R, G, B, A Keys to Start Editing the Colour of the Pixel\n Then Use The Mouse Wheel to Edit it", Pixel.WHITE);

                DrawSprite(800, 0, new Sprite(100, 100, CurrentColour));

                DrawSprite(300, 150, CurrentSprite, 25);
                DrawRect(300, 150, CurrentSprite.Width * 25, CurrentSprite.Height * 25, Pixel.BLUE);
            }
            return true;
        }

    }

    public class TileSetEditor
    {
        public TileSetEditor(string path, int TileWidth = 0)
        {
            TileSetEditorDrawer t = new TileSetEditorDrawer(path, TileWidth);
            if (t.Construct(900, 600, 1, 1,false, true))
            {
                t.Start();
            }
        }
    }
}
