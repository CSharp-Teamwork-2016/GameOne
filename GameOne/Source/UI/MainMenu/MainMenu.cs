namespace GameOne.Source.UI.MainMenu
{
    using Buttons;
    using Containers;
    using Events;
    using Interfaces.MainMenu;
    using Microsoft.Xna.Framework.Input;

    public delegate void OnMouseClickEventHandler(object sender, MousePositionEventArgs args);

    public delegate void OnMouseHoverEventHandler(object sender, MousePositionEventArgs args);

    public class MainMenu
    {
        #region Properties

        private ButtonContainer buttons;

        #endregion Properties

        public MainMenu()
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
            }
        }

        public event OnMouseHoverEventHandler OnMouseHover;

        #region Methods

        //public GameState Update(GameTime gameTime)
        //{
        //    // Respond to user input for menu selections, etc

        //    // Start New Game or Resume curren
        //    if ((this.CurrentScreen == this.MainMenuScreen || this.CurrentScreen == this.ResumeScreen) &&
        //        Mouse.GetState().LeftButton == ButtonState.Pressed &&
        //        Mouse.GetState().X >= 250 &&
        //        Mouse.GetState().X <= 550 &&
        //        Mouse.GetState().Y >= 100 &&
        //        Mouse.GetState().Y <= 140)
        //    {
        //        return GameState.Gameplay;
        //    }
        //    // Exit Game
        //    else if ((this.CurrentScreen == this.MainMenuScreen || this.CurrentScreen == this.ResumeScreen) &&
        //        Mouse.GetState().LeftButton == ButtonState.Pressed &&
        //        Mouse.GetState().X >= 250 &&
        //        Mouse.GetState().X <= 550 &&
        //        Mouse.GetState().Y >= 350 &&
        //        Mouse.GetState().Y <= 390)
        //    {
        //        System.Environment.Exit(1);
        //        // We may add "Are you sure", "Do you want to save?" or something...
        //    }
        //    // Credits
        //    else if ((this.CurrentScreen == this.MainMenuScreen || this.CurrentScreen == this.ResumeScreen) &&
        //        Mouse.GetState().LeftButton == ButtonState.Pressed &&
        //        Mouse.GetState().X >= 250 &&
        //        Mouse.GetState().X <= 550 &&
        //        Mouse.GetState().Y >= 300 &&
        //        Mouse.GetState().Y <= 340)
        //    {
        //        this.CurrentScreen = this.CreditsScreen;
        //        return GameState.MainMenu;
        //    }
        //    // Back From Credits or Save or Load. If we start New Game, then go to main menue, then press Credits, then back, first button will be New Game, but it will actualy Resume current game. I will fix it later<=====================================================================
        //    else if ((this.CurrentScreen == this.CreditsScreen || this.CurrentScreen == this.WePromiseScreen) &&
        //        Mouse.GetState().LeftButton == ButtonState.Pressed &&
        //        Mouse.GetState().X >= 250 &&
        //        Mouse.GetState().X <= 550 &&
        //        Mouse.GetState().Y >= 400 &&
        //        Mouse.GetState().Y <= 450)
        //    {
        //        this.CurrentScreen = this.MainMenuScreen;
        //        return GameState.MainMenu;
        //    }
        //    else if ((this.CurrentScreen == this.CreditsScreen || this.CurrentScreen == this.WePromiseScreen) &&
        //        Keyboard.GetState().IsKeyDown(Keys.Escape))
        //    {
        //        this.CurrentScreen = this.MainMenuScreen;
        //        return GameState.MainMenu;
        //    }
        //    // Save TODO
        //    else if ((this.CurrentScreen == this.MainMenuScreen || this.CurrentScreen == this.ResumeScreen) &&
        //        Mouse.GetState().LeftButton == ButtonState.Pressed &&
        //        Mouse.GetState().X >= 250 &&
        //        Mouse.GetState().X <= 550 &&
        //        Mouse.GetState().Y >= 150 &&
        //        Mouse.GetState().Y <= 190)
        //    {
        //        this.CurrentScreen = this.WePromiseScreen;
        //        return GameState.MainMenu;
        //    }
        //    // Load TODO
        //    else if ((this.CurrentScreen == this.MainMenuScreen || this.CurrentScreen == this.ResumeScreen) &&
        //        Mouse.GetState().LeftButton == ButtonState.Pressed &&
        //        Mouse.GetState().X >= 250 &&
        //        Mouse.GetState().X <= 550 &&
        //        Mouse.GetState().Y >= 200 &&
        //        Mouse.GetState().Y <= 240)
        //    {
        //        this.CurrentScreen = this.WePromiseScreen;
        //        return GameState.MainMenu;
        //    }
        //    // Level Editor
        //    else if ((this.CurrentScreen == this.MainMenuScreen || this.CurrentScreen == this.ResumeScreen) &&
        //        Mouse.GetState().LeftButton == ButtonState.Pressed &&
        //        Mouse.GetState().X >= 250 &&
        //        Mouse.GetState().X <= 550 &&
        //        Mouse.GetState().Y >= 250 &&
        //        Mouse.GetState().Y <= 290)
        //    {
        //        return GameState.LevelEditor;
        //    }

        //    return GameState.MainMenu;
        //}

        //public void LoadTextures(Texture2D mainMenuScreen, Texture2D resumeScreen, Texture2D creditsScreen, Texture2D wePromiseScreen)
        //{
        //    this.MainMenuScreen = mainMenuScreen;
        //    this.ResumeScreen = resumeScreen;
        //    this.CreditsScreen = creditsScreen;
        //    this.WePromiseScreen = wePromiseScreen;

        //    this.CurrentScreen = mainMenuScreen;
        //}

        public void Draw()
        {
            this.buttons.Draw();
        }

        public void Update(MouseState mouseState)
        {
            this.OnMouseHover?.Invoke(null, new MousePositionEventArgs(mouseState.X, mouseState.Y, mouseState.LeftButton == ButtonState.Pressed));
        }

        #endregion Methods
    }
}
