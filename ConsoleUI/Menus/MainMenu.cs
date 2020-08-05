using Wrapper;
using System;

namespace UI
{
    class MainMenu : Layer
    {
        public MainMenu() : base("MainMenu")
        {
            Camera = new OrthographicCamera(0, 700, 700, 0);
        }

        public MainMenu(int CurrentIdx) : base("MainMenu")
        {
            this.CurrentIdx = CurrentIdx;
            Camera = new OrthographicCamera(0, 700, 700, 0);
        }

        OrthographicCamera Camera;
        int CurrentIdx = 0;
        readonly string[] Settings = new[] { "[Character Menu]", "[World Menu]", "[Options]", "[Quit]" };
        readonly int[] SettingX = new[] { 95, 270, 400, 500 };

        public override void OnUpdate(TimeStep time)
        {
            Renderer.Clear();
            Renderer2D.BeginScene(Camera);
            Renderer2D.DrawRect(new Vec2(10.0f, 10.0f), new Vec2(680.0f, 680.0f), 3, Colors.White);
            Renderer2D.DrawText("Welcome to", new Vec2(130.0f, 130.0f), new Vec2(50000, -50000), Font.FixedDsys, Colors.Green);
            Renderer2D.DrawText("Game", new Vec2(230.0f, 240.0f), new Vec2(70000, -70000), Font.FixedDsys, Colors.Dark_Green);
            for (int i = 0; i < Settings.Length; i++)
            {
                if (i == CurrentIdx)
                {
                    Renderer2D.DrawQuad(new Vec2(SettingX[i] + Settings[i].Length * 5, 545), new Vec2(Settings[i].Length * 10, 25), Colors.Dark_Blue);
                    Renderer2D.DrawText(Settings[i], new Vec2(SettingX[i], 550), new Vec2(10000, -10000), Font.FixedDsys, Colors.White);
                }
                else Renderer2D.DrawText(Settings[i], new Vec2(SettingX[i], 550), new Vec2(10000, -10000), Font.FixedDsys, Colors.White);
            }
            Renderer2D.EndScene();
        }

        public override void OnEvent(Event e)
        {
            EventDispatcher dispatcher = new EventDispatcher(e);
            dispatcher.Dispatch(OnKeyPressed);
            dispatcher.Dispatch(OnMouseScrooled);
        }

        private bool OnKeyPressed(KeyPressedEvent e)
        {
            if ((e.GetKeyCode() == (int)Key.Enter || e.GetKeyCode() == (int)Key.Up || e.GetKeyCode() == (int)Key.KP_enter) && e.GetRepeatedCount() == 0)
            {
                switch (CurrentIdx)
                {
                    case 0:
                        //GetApplication().PushLayer(new CharactersMenu());
                        //GetApplication().PopLayer(this);
                        break;
                    case 1:
                        //GetApplication().PushLayer(new WorldMenu());
                        //GetApplication().PopLayer(this);
                        break;
                    case 2:
                        //GetApplication().PushLayer(new OptionsMenu());
                        //GetApplication().PopLayer(this);
                        break;
                    case 3:
                        Application.GetApplication().Quit();
                        break;
                    default:
                        throw new Exception();
                }
            }

            if (e.GetKeyCode() == (int)Key.Left && e.GetRepeatedCount() == 0)
            {
                CurrentIdx = --CurrentIdx < 0 ? 3 : CurrentIdx;
            }
            if (e.GetKeyCode() == (int)Key.Right && e.GetRepeatedCount() == 0)
            {
                CurrentIdx = ++CurrentIdx > 3 ? 0 : CurrentIdx;
            }

            return true;
        }

        private bool OnMouseScrooled(MouseScrolledEvent e)
        {
            zoomLevel -= e.GetMouseYOffset() * 0.25f;
            zoomLevel = Math.Max(zoomLevel, 0.25f);
            Camera.SetProjection(0 * zoomLevel, 700 * zoomLevel, 700 * zoomLevel, 0 * zoomLevel);
            return false;
        }

        float zoomLevel = 1.0f;

        public override void OnAttach()
        {

        }

        public override void OnDetach()
        {

        }
        int Y = 0;
        public override void OnImGUIRender()
        {
            ImGUI.Begin("Settings");

            ImGUI.End();
        }
    }
}
