//using Models.WorldGen;
using System;
using System.Linq;
using Wrapper;

/*
 * Make Civs create large cities
 * Fix Region Creator
 * Start NPC creation
 * Fix Manual Usage of OpenGL
*/

namespace UI
{
    class Test : Layer
    {
        public Test() : base("Test")
        {
            Camera = new OrthographicCamera(-1.6f, 1.6f, -1, 1);
        }

        public override void OnAttach()
        {

        }

        public override void OnDetach()
        {

        }

        public override void OnEvent(Event e)
        {
            EventDispatcher dispatcher = new EventDispatcher(e);
            dispatcher.Dispatch(OnMouseScrooled);
        }

        public override void OnImGUIRender()
        {
            ImGUI.Begin("Settings");
            ImGUI.ColorEdit3("Color", col);
            ImGUI.End();
        }
        private bool OnMouseScrooled(MouseScrolledEvent e)
        {
            zoomLevel -= e.GetMouseYOffset() * 0.25f;
            zoomLevel = Math.Max(zoomLevel, 0.25f);
            Camera.SetProjection(-1.6f * zoomLevel, 1.6f * zoomLevel, -zoomLevel, zoomLevel);
            return false;
        }

        float zoomLevel = 1.0f;
        OrthographicCamera Camera;
        Texture2D mTexture;
        Texture2D mTexture2;
        float[] col = { 0.03f, 0.2f, 1.0f };

        public override void OnUpdate(TimeStep time)
        {
            VertexArray va = new VertexArray();

            float[] vertecies =
            {
                 -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
                  0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
                  0.5f,  0.5f, 0.0f, 1.0f, 1.0f,
                 -0.5f,  0.5f, 0.0f, 0.0f, 1.0f,
            };
            VertexBuffer vb = new VertexBuffer((uint)vertecies.Length * sizeof(float), vertecies);

            System.Collections.Generic.List<BufferElement> elements = new System.Collections.Generic.List<BufferElement> { new BufferElement("aPosition", ShaderDataType.VecF3),
                                                                                                                           new BufferElement("aTexCoord", ShaderDataType.VecF2)};

            BufferLayout bl = new BufferLayout(elements);
            vb.Test();
            vb.SetLayout(bl);
            va.AddVertexBuffer(vb);

            uint[] indicies = {0, 1, 2, 2, 3, 0};
            IndexBuffer ib = new IndexBuffer((uint)indicies.Length, indicies);
            va.SetIndexBuffer(ib);
            
            mTexture2 = new Texture2D("assets/Textures/Test2.png");

            mTexture = new Texture2D("assets/Textures/Test.bmp");
            mTexture.Bind(0);

            Shader shader = new Shader(@"assets/Shaders/Texture.glsl");
            shader.SetInt("uTexture", 0);

            Renderer.Clear();
            Renderer.BeginScene(Camera);
            Renderer.Submit(va, shader);
            mTexture2.Bind(0);
            Renderer.Submit(va, shader);
            Renderer.EndScene();

            mTexture.Dispose();
        }
    }

    class Game : Application
    {
        public Game() : base("demo", 1280, 720)
        {
            PushLayer(new MainMenu());
        }
    }

    class Start
    {
        static void Main()
        {
            EntryPoint entry = new EntryPoint();
            entry.Enter(new Game());
        }
    }
}
