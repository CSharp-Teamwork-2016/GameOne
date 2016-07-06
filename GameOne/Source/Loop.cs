﻿namespace GameOne.Source
{
    using System;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using Renderer;
    using Enumerations;
    using World;
    using Entities;
    using UI;

    // Game contents
    // Level
    // Entities
    // Parameters
    // Main loop
    public class Loop
    {
        #region Fields

        // Game objects
        public static Level level;
        private Input input;
        private EntityHandler entityHandler;

        // initial game state
        private GameState gameState = GameState.MainMenu;

        //Main Menu
        private MainMenu mainMenu;

        #endregion Fields

        //===================================================================

        #region Constructors

        public Loop(KeyboardState keyboardState, MouseState mouseState)
        {
            level = new Level();

            DebugInfo = string.Empty;
            Console = string.Empty;
            this.input = new Input(keyboardState, mouseState);
            LevelEditor.Init(input);
            this.mainMenu = new MainMenu();
            this.entityHandler = new EntityHandler();
            entityHandler.Subscribe(level.Entities);
            entityHandler.SubscribeToPlayer(level.Player);
        }

        #endregion Constructors

        //===================================================================

        #region Properties

        // Debug
        public static string DebugInfo { get; set; }
        // Debug
        public static string Console { get; set; }

        public static bool ShowFPS { get; set; }

        public MainMenu MainMenu
        {
            get
            {
                return this.mainMenu;
            }
        }

        #endregion Properties

        //===================================================================

        #region Methods

        internal void Update(GameTime time, KeyboardState keyboardState, MouseState mouseState)
        {
            switch (this.gameState)
            {
                case GameState.MainMenu:
                    this.gameState = this.mainMenu.Update(time);
                    break;
                case GameState.Gameplay:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        this.gameState = GameState.MainMenu;
                        this.MainMenu.CurrentScreen = this.MainMenu.ResumeScreen;
                    }
                    else
                    {
                        this.GameUpdate(time, Keyboard.GetState(), Mouse.GetState());
                    }
                    break;
                case GameState.LevelEditor:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        this.gameState = GameState.MainMenu;
                        this.MainMenu.CurrentScreen = this.MainMenu.ResumeScreen;
                    }
                    else
                    {
                        LevelEditor.Update(time, Keyboard.GetState(), Mouse.GetState());
                    }
                    break;
                case GameState.EndOfGame:
                    //TODO
                    break;
            }
        }

        private void GameUpdate(GameTime time, KeyboardState keyboardState, MouseState mouseState)
        {
            DebugInfo = string.Empty;
            level.Player.Input(this.input.Update(keyboardState, mouseState));

            if (level.ExitOpen)
            {
                level.ExitOpen = false;
                level.SetExit();
            }

            if (level.ExitTriggered)
            {
                level.ExitTriggered = false;
                level.NextLevel();
                entityHandler.Subscribe(level.Entities);
            }

            entityHandler.ProcessEntities(level.Entities, level.Geometry, time.ElapsedGameTime.Milliseconds / 1000.0);

            // Execute tests
            foreach (Action test in Tests.ListOf.OnUpdate)
            {
                test();
            }
            // Debug info
            if (ShowFPS)
            {
                DebugInfo += string.Format($"{(1000 / time.ElapsedGameTime.TotalMilliseconds):f2}{Environment.NewLine}");
            }
        }

        internal void Render()
        {
            switch (this.gameState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.Gameplay:
                    RenderLevel();
                    break;
                case GameState.LevelEditor:
                    RenderLevel();
                    RendeGrid();
                    break;
                case GameState.EndOfGame:
                    //TODO
                    break;
            }

            // Execute tests
            foreach (Action test in Tests.ListOf.OnDraw)
            {
                test();
            }
        }

        private void RenderLevel()
        {
            level.Geometry.ForEach(Primitive.DrawTile);
            foreach (var entity in level.Entities.Where(e => e is Model))
            {
                var model = (Model)entity;
                Primitive.DrawModel(model);
            }
        }

        private void RendeGrid()
        {
            level.Geometry.ForEach(Primitive.DrawGrid);
            Tile target = new Tile((int)Primitive.ToWorldX(input.MouseX), (int)Primitive.ToWorldY(input.MouseY), LevelEditor.CurrentTile);
            Primitive.DrawTile(target);
        }

        internal void RenderUI()
        {
            switch (this.gameState)
            {
                case GameState.MainMenu:
                    Output.Draw(MainMenu.CurrentScreen, Vector2.Zero);
                    break;
                case GameState.Gameplay:
                    double hpc = (double)level.Player.Health / level.Player.MaxHealth;
                    UserInterface.DrawSideBar(hpc, level.Player.HealthPotions, Level.CurrentLevel);
                    level.Geometry.ForEach(Primitive.DrawTileMini);
                    Primitive.DrawModelMini(level.Player);
                    if (level.Entities.Contains(level.ExitPortal)) Primitive.DrawModelMini(level.ExitPortal);
                    UserInterface.DrawConsole(DebugInfo, Console);
                    break;
                case GameState.LevelEditor:
                    double tileX = Primitive.ToWorldX(input.MouseX);
                    double tileY = Primitive.ToWorldY(input.MouseY);
                    Output.DrawText(string.Format($"{tileX}{Environment.NewLine}{tileY}"), input.MouseX + 20, input.MouseY, Color.White);
                    break;
                case GameState.EndOfGame:
                    //TODO
                    break;
            }
        } 

        #endregion Methods
    }
}
