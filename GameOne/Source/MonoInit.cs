using GameOne.Source.UI;

namespace GameOne.Source
{
    using Enumerations;
    using Sound;

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

        //Main Menu
        private MainMenu mainMenu;


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
            this.mainMenu = new MainMenu(this.Content);
        }

        protected override void LoadContent()
        {
            Renderer.Output.SetFont(this.Content.Load<SpriteFont>("Font"));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (this.gameState)
            {
                case GameState.MainMenu:
                    this.gameState = this.mainMenu.Update(gameTime);
                    break;
                case GameState.Gameplay:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        mainMenu.MainManuScreen = this.Content.Load<Texture2D>("Images/Menu/ResumeGame");
                        this.gameState = GameState.MainMenu;
                    }

                    this.loop.Update(gameTime, Keyboard.GetState(), Mouse.GetState());
                    break;
                case GameState.EndOfGame:
                    //TODO
                    break;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            switch (this.gameState)
            {
                case GameState.MainMenu:
                    this.mainMenu.Draw(this.GraphicsDevice, this.spriteBatch);
                    break;
                case GameState.Gameplay:
                    this.DrawGameplay(gameTime);
                    break;
                case GameState.EndOfGame:
                    //TODO
                    break;
            }
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
    }
}
