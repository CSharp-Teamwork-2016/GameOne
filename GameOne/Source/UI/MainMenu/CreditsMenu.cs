namespace GameOne.Source.UI.MainMenu
{
    using System;
    using Buttons;
    using Containers;

    [Serializable]
    public class CreditsMenu : Menu
    {
        public CreditsMenu(GameContainer gameContainer)
        {
            this.buttons = new ButtonContainer();

            var backButton = new BackButton(250, 350);

            this.OnMouseHover += backButton.OnMouseHover;
            this.OnMouseClick += backButton.OnMouseClick;
            backButton.OnButtonClick += gameContainer.ButtonHandler;

            this.buttons.AddButton(backButton);
        }
    }
}
