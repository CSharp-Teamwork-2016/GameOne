namespace GameOne.Source.UI.MainMenu.Buttons
{
    using System;
    using Events;
    using Renderer;

    public class NewGameButton : Button
    {
        private const string Text = "New Game";

        public NewGameButton(int x, int y) 
            : base(Text, x, y)
        {

        }

        public override void OnMouseClick(object sender, MousePositionEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
