//using Models.WorldGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public TestLayer(string name = "Layer") : base(name)
        {
            mCamera = new OrthographicCamera(1280.0f / 720.0f, -1280.0f / 720.0f, -1, +1);
        }

        public override void OnAttach()
        {
            mTexture = new Texture2D("assets/Textures/Test.bmp");
        }

        public override void OnDetach() { }
        public override void OnUpdate(TimeStep time) 
        {
            Console.Title = "FPS = " + (1 / time);
             {
                 if (Input.IsKeyPressed((int)Key.A))
                 {
                     mCameraPosition.SetX(mCameraPosition.GetX() - 5.0f * time);
                 }

                 if (Input.IsKeyPressed((int)Key.W))
                 {
                     mCameraPosition.SetY(mCameraPosition.GetY() + 5.0f * time);
                 }

                 if (Input.IsKeyPressed((int)Key.D))
                 {
                     mCameraPosition.SetX(mCameraPosition.GetX() + 5.0f * time);
                 }

                 if (Input.IsKeyPressed((int)Key.S))
                 {
                     mCameraPosition.SetY(mCameraPosition.GetY() - 5.0f * time);
                 }

                 mCamera.SetPosition(mCameraPosition);
             }

            Renderer2D.BeginScene(mCamera);
            for (float i = 0; i < num; i += 0.5f)
                Renderer2D.DrawQuad(new Vec2(-1.0f, 0.0f), new Vec2(0.8f, 0.8f), new Vec4(0.8f, 0.2f, 0.3f, 1.0f), i);
            Renderer2D.DrawQuad(new Vec3(0.0f, 0.0f, -0.1f), new Vec2(10.0f, 10.0f), mTexture, new Vec4(1.0f, 0.8f, 0.2f, 1.0f), 100.0f, 10.0f);
            Renderer2D.EndScene();
        }

        public override void OnEvent(Event e)
        {
            
        }

        public override void OnImGUIRender()
        {
            ImGUI.Begin("Settings");
            ImGUI.DragInt("Number", ref num, 1.0f, 10, 10000);
            ImGUI.End();
        }

        int num = 1;
        OrthographicCamera mCamera;
        Texture2D mTexture;
        Vec3 mCameraPosition = new Vec3(0.0f);
    }

    class SandBox : Application
    {
        public SandBox(string Title) : base(Title, 1280, 720)
        {
            
            PushLayer(new TestLayer());
        }
    }

    class Start
    {
        static void Main()
        {
            EntryPoint entry = new EntryPoint();
            entry.Enter(new SandBox("SandBox"));
        }
    }
}