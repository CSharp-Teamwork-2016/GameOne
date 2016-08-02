namespace GameOne.Source
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Sound;

    public class MonoInit : Game
    {
        // Main loop
        private Containers.GameContainer loop;

        // Graphics context
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Audio
        private readonly AudioManager audioManager = new AudioManager();

        public MonoInit()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.loop = new Containers.GameContainer(Keyboard.GetState(), Mouse.GetState());
            Renderer.Output.Init(this.spriteBatch, this.GraphicsDevice);
            this.audioManager.PlayBackgroundMusic(this.Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Renderer.Output.SetFont(this.Content.Load<SpriteFont>("Font"));

            if (this.loop.MainMenu == null)
            {
                throw new ArgumentException("Main Menu not initialized");
            }

            //this.loop.MainMenu.LoadTextures(
            //    this.Content.Load<Texture2D>("Images/Menu/MainMenu"),
            //    this.Content.Load<Texture2D>("Images/Menu/ResumeGame"),
            //    this.Content.Load<Texture2D>("Images/Menu/Credits"),
            //    this.Content.Load<Texture2D>("Images/Menu/WePromise"));
            Renderer.Primitive.FloorTile = this.Content.Load<Texture2D>("Images/floor");
            Renderer.Primitive.WallTile = this.Content.Load<Texture2D>("Images/wall");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.loop.Update(gameTime, Keyboard.GetState(), Mouse.GetState());
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            Matrix transform = Matrix.CreateTranslation((float)Renderer.Primitive.CameraX, (float)Renderer.Primitive.CameraY, 0);

            this.spriteBatch.Begin(transformMatrix: transform);
            this.loop.Render();
            this.spriteBatch.End();

            this.spriteBatch.Begin();
            this.loop.RenderUI();
            this.spriteBatch.End();
        }
    }
}
