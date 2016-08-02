namespace GameOne.Source.UI.MainMenu.Buttons
{
    using System;
    using Events;
    using Renderer;

    public class SaveGameButton : Button
    {
        private const string Text = "Save Game";

        public SaveGameButton(int x, int y) 
            : base(Text, x, y)
        {

        }

        public override void OnMouseClick(object sender, MousePositionEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
