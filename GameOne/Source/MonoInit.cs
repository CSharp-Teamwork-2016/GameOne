﻿namespace GameOne.Source
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;

    public class MonoInit : Game
    {
        // Main loop
        private Loop loop;

        // Graphics context
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Audio
        private Song backgroundMusic;

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
        }

        protected override void LoadContent()
        {
            Renderer.Output.SetFont(this.Content.Load<SpriteFont>("Font"));

            this.backgroundMusic = Content.Load<Song>("WoT-Battle-2");
            MediaPlayer.Play(this.backgroundMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        protected void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(this.backgroundMusic);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            this.loop.Update(gameTime, Keyboard.GetState(), Mouse.GetState());

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            Matrix Transform = Matrix.CreateTranslation((float)Renderer.Primitive.CameraX, (float)Renderer.Primitive.CameraY, 0);

            this.spriteBatch.Begin(transformMatrix: Transform);
            this.loop.Render();
            this.spriteBatch.End();

            this.spriteBatch.Begin();
            this.loop.RenderUI();
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
