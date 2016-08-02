namespace GameOne.Source.UI.MainMenu.Buttons
{
    using System;
    using Events;
    using Renderer;

    public class LevelEditorButton : Button
    {
        private const string Text = "Level Editor";

        public LevelEditorButton(int x, int y) 
            : base(Text, x, y)
        {

        }

        public override void OnMouseClick(object sender, MousePositionEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
