//using Models.WorldGen;
using System;
using Wrapper;

/*
 * Make Civs create large cities
 * Fix Region Creator
 * Start NPC creation
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
        public override void OnUpdate(TimeStep time)
        {
            Texture2D texture = new Texture2D(@"assets/Textures/Test.bmp");
            Texture2D texture2 = new Texture2D(@"assets/Textures/Test2.png");
            Renderer.Clear();
            Renderer2D.BeginScene(Camera);
            Renderer2D.DrawQuad(new Vec2(0), new Vec2(0.5f), texture2);
            Renderer2D.DrawQuad(new Vec2(0), new Vec2(100), texture, Colors.Beige, 10.0f);
            Renderer2D.EndScene();
            texture.Dispose();
            texture2.Dispose();
        }
    }

    class Game : Application
    {
        public Game() : base("demo", 1280, 720)
        {
            PushLayer(new Test());
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
