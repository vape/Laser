using Laser.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laser.UI.Screens
{
    public class MainMenuScreen : ScreenController
    {
        public void Play()
        {
            FindObjectOfType<GameManager>().LoadNextLevel();
            Close();
        }
    }
}
