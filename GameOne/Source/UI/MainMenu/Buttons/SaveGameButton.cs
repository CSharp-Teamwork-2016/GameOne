namespace GameOne.Source.UI.MainMenu.Buttons
{
    using Enumerations;

    public class SaveGameButton : Button
    {
        private const string Text = "Save Game";
        private const GameState GameState = Enumerations.GameState.SaveGame;

        public SaveGameButton(int x, int y) 
            : base(Text, x, y, GameState)
        {
        }
    }
}
