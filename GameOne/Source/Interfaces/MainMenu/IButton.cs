namespace GameOne.Source.Interfaces.MainMenu
{
    using System;
    using Events;

    public interface IButton
    {
        void OnMouseHover(object sender, MousePositionEventArgs args);

        void Draw();

        void OnMouseClick(object sender, MousePositionEventArgs args);

        event EventHandler<OnButtonClickEventArgs> OnButtonClick;
    }
}
