namespace GameOne.Source.UI.MainMenu.Buttons
{
    using Enumerations;

    public class LoadGameButton : Button
    {
        private const string Text = "Load Game";
        private const GameState GameState = Enumerations.GameState.LoadGame;

        public LoadGameButton(int x, int y) 
            : base(Text, x, y, GameState)
        {
        }
    }
}
