namespace GameOne.Source.UI.MainMenu.Buttons
{
    using System;
    using Events;
    using Renderer;

    public class LoadGameButton : Button
    {
        private const string Text = "Load Game";

        public LoadGameButton(int x, int y) 
            : base(Text, x, y)
        {

        }

        public override void OnMouseClick(object sender, MousePositionEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
