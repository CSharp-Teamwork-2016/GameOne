namespace GameOne.Source.UI.MainMenu.Buttons
{
    using Enumerations;

    public class ExitButton : Button
    {
        private const string Text = "Exit";
        private const GameState GameState = Enumerations.GameState.Exit;

        public ExitButton(int x, int y) 
            : base(Text, x, y, GameState)
        {
        }
    }
}
