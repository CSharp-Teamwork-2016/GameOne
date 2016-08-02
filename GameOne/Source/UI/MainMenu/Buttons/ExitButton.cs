namespace GameOne.Source.UI.MainMenu.Buttons
{
    using System;
    using Events;
    using Renderer;

    public class ExitButton : Button
    {
        private const string Text = "Exit";

        public ExitButton(int x, int y) 
            : base(Text, x, y)
        {

        }

        public override void OnMouseClick(object sender, MousePositionEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
