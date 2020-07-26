//using Models.WorldGen;
using System;
using Wrapper;

/*
 * Make Civs create large cities
 * Fix Region Creator
 * Start NPC creation
*/

/*
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
            Tester tester = new Tester();
            
            sAppName = "demo";
        }

        IGameElement CurrentElement;
        public override bool OnUserCreate()
        {
            CurrentElement = new MainMenu();
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
            if (demo.Construct(700, 700, 1, 1, true, true))
            {
                demo.Start();
            }
        }

    }
}
*/

namespace Test
{
    class TestLayer : Layer
    {
        public TestLayer(string name) : base(name)
        {

        }

        string Val;

        public override void OnAttach()
        {
            Val = "Test";
            Console.WriteLine(Val);
        }

        public override void OnDetach() { }
        public override void OnUpdate(TimeStep time) 
        {
            Console.WriteLine(1/time.GetSeconds());
        }

        public override void OnEvent(Event e)
        {
            EventDispatcher dispatcher = new EventDispatcher(e);
            dispatcher.DispatchMM((ev) => { Console.WriteLine("{0}, x={1}, y={2}", Val, ev.GetMouseX(), ev.GetMouseY()); return true; });
        }

        public override void OnImGUIRender() { }
    }

    class SandBox : Application
    {
        public SandBox() : base()
        {
            PushLayer(new TestLayer("Val"));
        }
    }

    class Start
    {
        static void Main()
        {
            EntryPoint entry = new EntryPoint();
            entry.Enter(new SandBox());
        }
    }
}