using System;
using System.Collections.Generic;
using Models.WorldGen;
using Models.Sprites;
using Pixel_Engine;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

/*
 * Make Civs create large cities
 * Fix Region Creator
 * Start NPC creation
*/

namespace UI
{
    interface IGameElement
    {
        void Draw();
        IGameElement NewElement();
    }

    partial class Game : Engine
    {
        public Game()
        {
            sAppName = "demo";
        }

        IGameElement CurrentElement;
        public override bool OnUserCreate()
        {
            CurrentElement = new WorldMenu();
            return true;
        }

        public override bool onUserUpdate(float fElapsedTime)
        {
            CurrentElement.Draw();
            CurrentElement = CurrentElement.NewElement();
            return true;
        }
    }

    class Test : Engine
    {
        public Test()
        {
            sAppName = "World";
        }

        public override bool OnUserCreate()
        {
            return true;
        }

        public override bool onUserUpdate(float fElapsedTime)
        {
            if (GetMouse(0).bPressed)
            {
                Console.WriteLine(GetMouseX() + " " + GetMouseY());
            }
            Clear(Pixel.BLUE);
            return true;
        }
    }

    class Start
    {
        static void Main()
        {
            Engine demo = new Game();
            if (demo.Construct(700,700, 1, 1, true, true))
            {
                demo.Start();
            }
        }

    }
}
