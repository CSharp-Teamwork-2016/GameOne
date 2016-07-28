namespace GameOne.Source.Containers
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
    using Handlers;

    // Game contents
    // Level
    // Entities
    // Parameters
    // Main loop
    public class GameContainer
    {
        #region Fields

        // Game objects
        public static Level level;
        private readonly Input input;
        private readonly EntityHandler entityHandler;

        // initial game state
        private GameState gameState = GameState.MainMenu;

        // Main Menu

        #endregion Fields

        #region Constructors

        public GameContainer(KeyboardState keyboardState, MouseState mouseState)
        {
            level = new Level();

            DebugInfo = string.Empty;
            Console = string.Empty;
            this.input = new Input(keyboardState, mouseState);
            LevelEditor.Init(this.input);
            this.MainMenu = new MainMenu();
            this.entityHandler = new EntityHandler();
            this.entityHandler.Subscribe(level.Entities);
            this.entityHandler.SubscribeToPlayer(level.Player);
        }

        #endregion Constructors

        #region Properties

        // Debug
        public static string DebugInfo { get; set; }
        // Debug
        public static string Console { get; set; }

        public static bool ShowFPS { get; set; }

        public MainMenu MainMenu { get; }

        #endregion Properties

        #region Methods

        internal void Update(GameTime time, KeyboardState keyboardState, MouseState mouseState)
        {
            switch (this.gameState)
            {
                case GameState.MainMenu:
                    this.gameState = this.MainMenu.Update(time);
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
                    // TODO
                    break;
            }
        }

        internal void Render()
        {
            switch (this.gameState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.Gameplay:
                    this.RenderLevel();
                    break;
                case GameState.LevelEditor:
                    this.RenderLevel();
                    this.RendeGrid();
                    break;
                case GameState.EndOfGame:
                    // TODO
                    break;
            }

            // Execute tests
            foreach (Action test in Tests.ListOf.OnDraw)
            {
                test();
            }
        }

        internal void RenderUI()
        {
            switch (this.gameState)
            {
                case GameState.MainMenu:
                    Output.Draw(this.MainMenu.CurrentScreen, Vector2.Zero);
                    break;
                case GameState.Gameplay:
                    double hpc = (double)level.Player.Health / level.Player.MaxHealth;
                    UserInterface.DrawSideBar(hpc, level.Player.HealthPotions, Level.CurrentLevel);
                    level.Geometry.ForEach(Primitive.DrawTileMini);
                    Primitive.DrawModelMini(level.Player);

                    if (level.Entities.Contains(level.ExitPortal))
                    {
                        Primitive.DrawModelMini(level.ExitPortal);
                    }

                    UserInterface.DrawConsole(DebugInfo, Console);
                    break;
                case GameState.LevelEditor:
                    double tileX = Primitive.ToWorldX(this.input.MouseX);
                    double tileY = Primitive.ToWorldY(this.input.MouseY);
                    Output.DrawText($"{tileX}{Environment.NewLine}{tileY}", this.input.MouseX + 20, this.input.MouseY, Color.White);
                    break;
                case GameState.EndOfGame:
                    // TODO
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
                this.entityHandler.Subscribe(level.Entities);
            }

            this.entityHandler.ProcessEntities(level.Entities, level.Geometry, time.ElapsedGameTime.Milliseconds / 1000.0);

            // Execute tests
            foreach (Action test in Tests.ListOf.OnUpdate)
            {
                test();
            }
            // Debug info
            if (ShowFPS)
            {
                DebugInfo += $"{(1000 / time.ElapsedGameTime.TotalMilliseconds):f2}{Environment.NewLine}";
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
            Tile target = new Tile((int)Primitive.ToWorldX(this.input.MouseX), (int)Primitive.ToWorldY(this.input.MouseY), LevelEditor.CurrentTile);
            Primitive.DrawTile(target);
        }

        #endregion Methods
    }
}
