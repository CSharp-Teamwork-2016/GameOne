namespace GameOne.Source.UI.MainMenu.Buttons
{
    using Enumerations;

    public class NewGameButton : Button
    {
        private const string Text = "New Game";
        private const GameState GameState = Enumerations.GameState.Gameplay;

        public NewGameButton(int x, int y) 
            : base(Text, x, y, GameState)
        {
        }
    }
}
