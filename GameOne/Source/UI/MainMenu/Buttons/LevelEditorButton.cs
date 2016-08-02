namespace GameOne.Source.UI.MainMenu.Buttons
{
    using Enumerations;

    public class LevelEditorButton : Button
    {
        private const string Text = "Level Editor";
        private const GameState GameState = Enumerations.GameState.LevelEditor;

        public LevelEditorButton(int x, int y) 
            : base(Text, x, y, GameState)
        {
        }
    }
}
