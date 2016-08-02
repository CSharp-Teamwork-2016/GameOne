namespace GameOne.Source.UI.MainMenu.Buttons
{
    using Enumerations;

    public class BackButton : Button
    {
        private const string Text = "Back";
        private const GameState GameState = Enumerations.GameState.MainMenu;

        public BackButton(int x, int y) 
            : base(Text, x, y, GameState)
        {
        }
    }
}
