using Laser.Game;

namespace Laser.UI.Screens
{
    public class WinScreen : ScreenController
    {
        public override void Init(params object[] param)
        {
            base.Init(param);
        }

        public void Next()
        {
            FindObjectOfType<GameManager>().LoadNextLevel();
            Close();
        }

        public void Exit()
        {
            FindObjectOfType<GameManager>().OpenMainMenuScreen();
            Close();
        }
    }
}
