namespace GameOne.Source.UI.MainMenu
{
    using System;
    using Buttons;
    using Containers;
    using Enumerations;
    using Events;
    using Interfaces.MainMenu;
    using Microsoft.Xna.Framework.Input;

    public delegate void OnMouseClickEventHandler(object sender, MousePositionEventArgs args);

    public delegate void OnMouseHoverEventHandler(object sender, MousePositionEventArgs args);

    [Serializable]
    public class MainMenu
    {
        #region Properties

        private ButtonContainer buttons;

        #endregion Properties

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

        public event OnMouseHoverEventHandler OnMouseHover;

        public event OnMouseClickEventHandler OnMouseClick;

        #region Methods

        public void Draw()
        {
            this.buttons.Draw();
        }

        public void Update(MouseState mouseState)
        {
            this.OnMouseHover?.Invoke(null, new MousePositionEventArgs(mouseState.X, mouseState.Y, mouseState));
            this.OnMouseClick?.Invoke(null, new MousePositionEventArgs(mouseState.X, mouseState.Y, mouseState));
        }

        #endregion Methods
    }
}
