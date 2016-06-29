namespace GameOne.Source
{
    using GameOne.Source.Enumerations;
    using GameOne.Source.Sound;
    using GameOne.Source.UI;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    
    public class MonoInit : Game
    {
        // Main loop
        private Loop loop;

        // Graphics context
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Audio
        private readonly AudioManager audioManager = new AudioManager();

        // initial game state
        private GameState gameState = GameState.MainMenu;

        // textures
        Texture2D mainManuScreen;


        public MonoInit()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.loop = new Loop(Keyboard.GetState(), Mouse.GetState());
            Renderer.Output.Init(this.spriteBatch, this.GraphicsDevice);
            this.audioManager.PlayBackgroundMusic(this.Content);
        }

        protected override void LoadContent()
        {
            Renderer.Output.SetFont(this.Content.Load<SpriteFont>("Font"));
            this.mainManuScreen = this.Content.Load<Texture2D>("TitleScreen");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            //this.loop.Update(gameTime, Keyboard.GetState(), Mouse.GetState());

            //base.Update(gameTime);

            base.Update(gameTime);
            switch (this.gameState)
            {
                case GameState.MainMenu:
                    this.UpdateMainMenu(gameTime);
                    break;
                case GameState.Gameplay:
                    this.UpdateGameplay(gameTime);
                    break;
                case GameState.EndOfGame:
                    this.UpdateEndOfGame(gameTime);
                    break;
            }
        }

        private void UpdateMainMenu(GameTime gameTime)
        {
            // Respond to user input for menu selections, etc
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                this.gameState = GameState.Gameplay;
            }
        }

        private void UpdateGameplay(GameTime gameTime)
        {
            // Respond to user actions in the game.
            // Update enemies
            // Handle collisions
            this.loop.Update(gameTime, Keyboard.GetState(), Mouse.GetState());
        }

        private void UpdateEndOfGame(GameTime deltaTime)
        {
            // Update scores
            // Do any animations, effects, etc for getting a high score
            // Respond to user input to restart level, or go back to main menu
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                this.gameState = GameState.MainMenu;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            //this.GraphicsDevice.Clear(Color.CornflowerBlue);
            //Matrix Transform = Matrix.CreateTranslation((float)Renderer.Primitive.CameraX, (float)Renderer.Primitive.CameraY, 0);

            //this.spriteBatch.Begin(transformMatrix: Transform);
            //this.loop.Render();
            //this.spriteBatch.End();

            //this.spriteBatch.Begin();
            //this.loop.RenderUI();
            //this.spriteBatch.End();

            //base.Draw(gameTime);

            base.Draw(gameTime);
            switch (this.gameState)
            {
                case GameState.MainMenu:
                    this.DrawMainMenu(gameTime);
                    break;
                case GameState.Gameplay:
                    this.DrawGameplay(gameTime);
                    break;
                case GameState.EndOfGame:
                    this.DrawEndOfGame(gameTime);
                    break;
            }
        }

        void DrawMainMenu(GameTime deltaTime)
        {
            // Draw the main menu, any active selections, etc
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.mainManuScreen, Vector2.Zero);
            this.spriteBatch.End();
        }

        void DrawGameplay(GameTime deltaTime)
        {
            // Draw the background the level
            // Draw enemies
            // Draw the player
            // Draw particle effects, etc
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            Matrix Transform = Matrix.CreateTranslation((float)Renderer.Primitive.CameraX, (float)Renderer.Primitive.CameraY, 0);

            this.spriteBatch.Begin(transformMatrix: Transform);
            this.loop.Render();
            this.spriteBatch.End();

            this.spriteBatch.Begin();
            this.loop.RenderUI();
            this.spriteBatch.End();
        }

        void DrawEndOfGame(GameTime deltaTime)
        {
            // Draw text and scores
            // Draw menu for restarting level or going back to main menu
        }
    }
}
