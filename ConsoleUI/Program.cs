//using Models.WorldGen;
using System;
using System.CodeDom;
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
            mCamera = new OrthographicCamera(-(1280.0f / 720.0f) * zoomLevel, (1280.0f / 720.0f) * zoomLevel, -zoomLevel, zoomLevel);
        }

        public override void OnAttach()
        {
            Texture2D mTexture;
            mTexture = new Texture2D("assets/Textures/Test.bmp");
            mTexture.Dispose();
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

            /*
            VertexArray va = new VertexArray();
            
            float[] vertecies =
            {
                -0.5f, -0.5f, 0.0f,
                 0.5f, -0.5f, 0.0f,
                 0.0f,  0.5f, 0.0f
            };
            VertexBuffer vb = new VertexBuffer(9 * sizeof(float), vertecies);
            
            List<BufferElement> elements = new List<BufferElement>();
            elements.Add(new BufferElement("aPosition", ShaderDataType.VecF3));
            BufferLayout bl = new BufferLayout(elements);
            vb.Test();
            vb.SetLayout(bl);
            va.AddVertexBuffer(vb);

            uint[] indicies = { 0, 1, 2 };
            IndexBuffer ib = new IndexBuffer(3, indicies);
            va.SetIndexBuffer(ib);

            Shader shader = new Shader(@"assets/Shaders/FlatColor.glsl");
            Renderer.Clear();
            Renderer.BeginScene(mCamera);
            Renderer.Submit(va, shader);
            Renderer.EndScene();
            */

            Texture2D mTexture;
            mTexture = new Texture2D("assets/Fonts/Verdana.png");
            

            Renderer.Clear();
            Renderer2D.BeginScene(mCamera);
            //for (float i = 0; i < num; i += 0.5f)
            //    Renderer2D.DrawQuad(new Vec2(-1.0f, 0.0f), new Vec2(0.8f, 0.8f), new Vec4(0.8f, 0.2f, 0.3f, 1.0f), i);
            Renderer2D.DrawText("Trying this out\ndoes it work", new Vec2(0.0f, 0.0f), new Vec2(num), Font.Verdana, new Vec4(1.0f, 0.3f, 0.2f, 1.0f), c);
            //Renderer2D.DrawQuad(new Vec2(0), new Vec2(0.5f, 0.5f), mTexture);
            Renderer2D.EndScene();
            mTexture.Dispose();
        }

        public override void OnEvent(Event e)
        {
            EventDispatcher dispatcher = new EventDispatcher(e);
            dispatcher.Dispatch(OnMouseScrolled);
        }

        private bool OnMouseScrolled(MouseScrolledEvent e)
        {
            zoomLevel -= e.GetMouseYOffset() * 0.25f;
            zoomLevel = Math.Max(zoomLevel, 0.25f);
            mCamera.SetProjection(-(1280.0f / 720.0f) * zoomLevel, (1280.0f / 720.0f) * zoomLevel, -zoomLevel, zoomLevel);
            return false;
        }

        public override void OnImGUIRender()
        {
            ImGUI.Begin("Settings");
            ImGUI.DragFloat("Number", ref num, 1.0f, 10, 10000);
            ImGUI.DragFloat("char", ref c, 1f, 0, 360);
            ImGUI.End();
        }


        float num = 1.0f;
        float c = 0.0f;
        float zoomLevel = 1.0f;
        OrthographicCamera mCamera;
 
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