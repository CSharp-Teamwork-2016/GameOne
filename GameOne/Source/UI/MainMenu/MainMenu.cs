namespace GameOne.Source.UI.MainMenu
{
    using System;
    using Buttons;
    using Containers;
    using Interfaces.MainMenu;

    [Serializable]
    public class MainMenu : Menu
    {
        public MainMenu(GameContainer gameContainer)
        {
            this.buttons = new ButtonContainer();
            // Sad
            this.buttons.AddButton(new NewGameButton(250, 100));
            this.buttons.AddButton(new SaveGameButton(250, 150));
            this.buttons.AddButton(new LoadGameButton(250, 200));
            this.buttons.AddButton(new LevelEditorButton(250, 250));
            this.buttons.AddButton(new CreditsButton(250, 300));
            this.buttons.AddButton(new ExitButton(250, 350));

            foreach (IButton button in this.buttons)
            {
                this.OnMouseHover += button.OnMouseHover;
                this.OnMouseClick += button.OnMouseClick;
                button.OnButtonClick += gameContainer.ButtonHandler;
            }
        }
    }
}
