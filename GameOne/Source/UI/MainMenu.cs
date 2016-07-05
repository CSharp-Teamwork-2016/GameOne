﻿using GameOne.Source.Enumerations;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GameOne.Source.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class MainMenu
    {
        #region Fields

        #endregion Fields

        //===================================================================

        #region Constructors

        #endregion Constructors

        //===================================================================

        #region Properties

        public Texture2D MainMenuScreen { get; set; }
        public Texture2D ResumeScreen { get; set; }
        public Texture2D CreditsScreen { get; set; }
        public Texture2D WePromiseScreen { get; set; }

        public Texture2D CurrentScreen { get; set; }

        #endregion Properties

        //===================================================================

        #region Methods

        public GameState Update(GameTime gameTime)
        {
            // Respond to user input for menu selections, etc

            //Start New Game or Resume curren
            if ((this.CurrentScreen == this.MainMenuScreen || this.CurrentScreen == this.ResumeScreen) &&
                Mouse.GetState().LeftButton == ButtonState.Pressed &&
                Mouse.GetState().X >= 250 &&
                Mouse.GetState().X <= 550 &&
                Mouse.GetState().Y >= 100 &&
                Mouse.GetState().Y <= 140)
            {
                return GameState.Gameplay;
            }
            //Exit Game
            else if ((this.CurrentScreen == this.MainMenuScreen || this.CurrentScreen == this.ResumeScreen) &&
                Mouse.GetState().LeftButton == ButtonState.Pressed &&
                Mouse.GetState().X >= 250 &&
                Mouse.GetState().X <= 550 &&
                Mouse.GetState().Y >= 350 &&
                Mouse.GetState().Y <= 390)
            {
                System.Environment.Exit(1);
                //We may add "Are you sure", "Do you want to save?" or something...
            }
            //Credits
            else if ((this.CurrentScreen == this.MainMenuScreen || this.CurrentScreen == this.ResumeScreen) &&
                Mouse.GetState().LeftButton == ButtonState.Pressed &&
                Mouse.GetState().X >= 250 &&
                Mouse.GetState().X <= 550 &&
                Mouse.GetState().Y >= 300 &&
                Mouse.GetState().Y <= 340)
            {
                this.CurrentScreen = this.CreditsScreen;
                return GameState.MainMenu;
            }
            //Back From Credits or Save or Load. If we start New Game, then go to main menue, then press Credits, then back, first button will be New Game, but it will actualy Resume current game. I will fix it later<=====================================================================
            else if ((this.CurrentScreen == this.CreditsScreen || this.CurrentScreen == this.WePromiseScreen) &&
                Mouse.GetState().LeftButton == ButtonState.Pressed &&
                Mouse.GetState().X >= 250 &&
                Mouse.GetState().X <= 550 &&
                Mouse.GetState().Y >= 400 &&
                Mouse.GetState().Y <= 450)
            {
                this.CurrentScreen = this.MainMenuScreen;
                return GameState.MainMenu;
            }
            else if ((this.CurrentScreen == this.CreditsScreen || this.CurrentScreen == this.WePromiseScreen) &&
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.CurrentScreen = this.MainMenuScreen;
                return GameState.MainMenu;
            }
            //Save TODO
            else if ((this.CurrentScreen == this.MainMenuScreen || this.CurrentScreen == this.ResumeScreen) &&
                Mouse.GetState().LeftButton == ButtonState.Pressed &&
                Mouse.GetState().X >= 250 &&
                Mouse.GetState().X <= 550 &&
                Mouse.GetState().Y >= 150 &&
                Mouse.GetState().Y <= 190)
            {
                this.CurrentScreen = this.WePromiseScreen;
                return GameState.MainMenu;
            }
            //Load TODO
            else if ((this.CurrentScreen == this.MainMenuScreen || this.CurrentScreen == this.ResumeScreen) &&
                Mouse.GetState().LeftButton == ButtonState.Pressed &&
                Mouse.GetState().X >= 250 &&
                Mouse.GetState().X <= 550 &&
                Mouse.GetState().Y >= 200 &&
                Mouse.GetState().Y <= 240)
            {
                this.CurrentScreen = this.WePromiseScreen;
                return GameState.MainMenu;
            }

            return GameState.MainMenu;
        }

        public void LoadTextures(Texture2D MainMenuScreen, Texture2D ResumeScreen, Texture2D CreditsScreen, Texture2D WePromiseScreen)
        {
            this.MainMenuScreen = MainMenuScreen;
            this.ResumeScreen = ResumeScreen;
            this.CreditsScreen = CreditsScreen;
            this.WePromiseScreen = WePromiseScreen;

            this.CurrentScreen = MainMenuScreen;
        }

        #endregion Methods
    }
}
