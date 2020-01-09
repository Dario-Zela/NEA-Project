using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.ViewModels
{
    public class GameSession
    {
        public Bar LeftBar { get; set; }
        public Bar RightBar { get; set; }

        public GameSession()
        {
            LeftBar = new Bar();
            RightBar = new Bar();

            LeftBar.Position = 0;
            RightBar.Position = 1;

        }
    }
}
