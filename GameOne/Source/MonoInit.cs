﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOne.Source
{
	public class MonoInit : Game
	{
		// Main loop
		Loop loop;

		// Graphics context
		private GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public MonoInit()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			base.Initialize();
			spriteBatch = new SpriteBatch(GraphicsDevice);
			loop = new Loop(Keyboard.GetState(), Mouse.GetState());
			Renderer.Output.Init(spriteBatch, GraphicsDevice);
		}

		protected override void LoadContent()
		{
			Renderer.Output.SetFont(Content.Load<SpriteFont>("Font"));
		}

		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			
			loop.Update(gameTime, Keyboard.GetState(), Mouse.GetState());

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();
			loop.Render();
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
