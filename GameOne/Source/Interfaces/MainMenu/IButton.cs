namespace GameOne.Source.Interfaces.MainMenu
{
    using Events;

    public interface IButton
    {
        void OnMouseHover(object sender, MousePositionEventArgs args);

        void Draw();
    }
}
