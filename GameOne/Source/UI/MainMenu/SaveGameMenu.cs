namespace GameOne.Source.UI.MainMenu
{
    using System;
    using Buttons;
    using Containers;
    using Interfaces.MainMenu;

    [Serializable]
    public class SaveGameMenu : Menu
    {
        public SaveGameMenu(GameContainer gameContainer)
        {
            this.buttons = new ButtonContainer();

            // Sad
            this.buttons.AddButton(new SaveSlotButton("Game1", 250, 100));
            this.buttons.AddButton(new SaveSlotButton("Game2", 250, 150));
            this.buttons.AddButton(new SaveSlotButton("Game3", 250, 200));

            foreach (IButton button in this.buttons)
            {
                this.OnMouseHover += button.OnMouseHover;
                this.OnMouseClick += button.OnMouseClick;
                button.OnButtonClick += gameContainer.SaveButtonHandler;
            }

            var backButton = new BackButton(250, 350);

            this.OnMouseHover += backButton.OnMouseHover;
            this.OnMouseClick += backButton.OnMouseClick;
            backButton.OnButtonClick += gameContainer.ButtonHandler;

            this.buttons.AddButton(backButton);
        }
    }
}
