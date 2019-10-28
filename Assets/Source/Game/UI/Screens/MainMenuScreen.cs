namespace Laser.Game.UI.Screens
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
