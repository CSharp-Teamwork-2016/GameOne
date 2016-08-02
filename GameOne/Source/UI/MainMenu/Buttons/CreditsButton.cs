namespace GameOne.Source.UI.MainMenu.Buttons
{
    using Enumerations;

    public class CreditsButton : Button
    {
        private const string Text = "Credits";
        private const GameState GameState = Enumerations.GameState.Credits;

        public CreditsButton(int x, int y) 
            : base(Text, x, y, GameState)
        {
        }
    }
}
