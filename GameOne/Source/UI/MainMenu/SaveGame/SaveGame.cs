namespace GameOne.Source.UI.MainMenu.SaveGame
{
    using System;
    using Buttons;
    using Containers;
    using Events;
    using Interfaces.MainMenu;
    using Microsoft.Xna.Framework.Input;

    public delegate void OnMouseClickEventHandler(object sender, MousePositionEventArgs args);

    public delegate void OnMouseHoverEventHandler(object sender, MousePositionEventArgs args);

    [Serializable]
    public class SaveGame
    {
        #region Properties

        private ButtonContainer buttons;

        #endregion Properties

        public SaveGame(GameContainer gameContainer)
        {
            this.buttons = new ButtonContainer();
            // Sad
            this.buttons.AddButton(new SaveSlotButton("Game 1", 250, 100));
            this.buttons.AddButton(new SaveSlotButton("Game 2", 250, 150));
            this.buttons.AddButton(new SaveSlotButton("Game 3", 250, 200));

            foreach (IButton button in this.buttons)
            {
                this.OnMouseHover += button.OnMouseHover;
                this.OnMouseClick += button.OnMouseClick;
                button.OnButtonClick += gameContainer.SaveButtonHandler;
            }

            var backButton = new BackButton(250, 350);
            backButton.OnButtonClick += gameContainer.ButtonHandler;
            this.buttons.AddButton(backButton);
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
