namespace GameOne.Source.UI.MainMenu.Buttons
{
    using Enumerations;

    public class SaveSlotButton : Button
    {
        private const GameState GameState = Enumerations.GameState.SaveGame;

        public SaveSlotButton(string name, int x, int y)
            : base(name, x, y, GameState)
        {
        }
    }
}
