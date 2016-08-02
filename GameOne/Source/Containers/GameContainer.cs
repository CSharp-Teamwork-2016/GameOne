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
    using UI.MainMenu;

    // Game contents
    // Level
    // Entities
    // Parameters
    // Main loop
    public class GameContainer
    {
        #region Fields

        // Game objects
        private Level level;
        private readonly Input input;
        private readonly EntityHandler entityHandler;

        // initial game state
        private GameState gameState = GameState.MainMenu;

        // Main Menu

        #endregion Fields

        #region Constructors

        public GameContainer(KeyboardState keyboardState, MouseState mouseState)
        {
            this.level = new Level();

            DebugInfo = string.Empty;
            Console = string.Empty;
            this.input = new Input(keyboardState, mouseState, this.level);
            LevelEditor.Init(this.input, this.level);
            this.MainMenu = new MainMenu();
            this.entityHandler = new EntityHandler(this.level);
            this.entityHandler.Subscribe(this.level.Entities);
            this.entityHandler.SubscribeToPlayer(this.level.Player);
            this.level.Player.ExitTriggeredEvent += this.OnExitTriggered;
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

        private void OnExitTriggered(object sender, EventArgs e)
        {
            this.level.ExitTriggered = true;
        }

        internal void Update(GameTime time, KeyboardState keyboardState, MouseState mouseState)
        {
            switch (this.gameState)
            {
                case GameState.MainMenu:
                    //this.gameState = this.MainMenu.Update(time);
                    break;
                case GameState.Gameplay:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        this.gameState = GameState.MainMenu;
                        //this.MainMenu.CurrentScreen = this.MainMenu.ResumeScreen;
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
                        //this.MainMenu.CurrentScreen = this.MainMenu.ResumeScreen;
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

        private void GameUpdate(GameTime time, KeyboardState keyboardState, MouseState mouseState)
        {
            DebugInfo = string.Empty;
            this.level.Player.Input(this.input.Update(keyboardState, mouseState));

            if (this.level.ExitOpen)
            {
                this.level.ExitOpen = false;
                this.level.SetExit();
            }

            if (this.level.ExitTriggered)
            {
                this.level.ExitTriggered = false;
                this.level.NextLevel();
                this.entityHandler.Subscribe(this.level.Entities);
            }

            this.entityHandler.ProcessEntities(time.ElapsedGameTime.Milliseconds / 1000.0);

            // Execute tests
            foreach (Action test in Tests.ListOf.OnUpdate)
            {
                test();
            }
            // Debug info
            DebugInfo = $"Player stats:{Environment.NewLine}State: {this.level.Player.State}{Environment.NewLine}Health: {this.level.Player.Health} / {this.level.Player.MaxHealth}{Environment.NewLine}Damage: {this.level.Player.Damage}{Environment.NewLine}{Environment.NewLine}Level {this.level.Player.XpLevel}{Environment.NewLine}XP: {this.level.Player.Experience} / {this.level.Player.XpToNext}{Environment.NewLine}{Environment.NewLine}Enemies remaining: {this.level.EnemyCount}{Environment.NewLine}";
            if (ShowFPS)
            {
                DebugInfo += $"{(1000 / time.ElapsedGameTime.TotalMilliseconds):f2}{Environment.NewLine}";
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
                    this.MainMenu.Draw();
                    //Output.Draw(this.MainMenu.CurrentScreen, Vector2.Zero);
                    break;
                case GameState.Gameplay:
                    double hpc = (double)this.level.Player.Health / this.level.Player.MaxHealth;
                    UserInterface.DrawSideBar(hpc, this.level.Player.HealthPotions, Level.CurrentLevel);
                    this.level.Geometry.ForEach(Primitive.DrawTileMini);
                    Primitive.DrawModelMini(this.level.Player);

                    if (this.level.Entities.Contains(this.level.ExitPortal))
                    {
                        Primitive.DrawModelMini(this.level.ExitPortal);
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

        private void RenderLevel()
        {
            this.level.Geometry.ForEach(Primitive.DrawTile);
            foreach (var entity in this.level.Entities.Where(e => e is Model))
            {
                var model = (Model)entity;
                Primitive.DrawModel(model);
            }
        }

        private void RendeGrid()
        {
            this.level.Geometry.ForEach(Primitive.DrawGrid);
            Tile target = new Tile((int)Primitive.ToWorldX(this.input.MouseX), (int)Primitive.ToWorldY(this.input.MouseY), LevelEditor.CurrentTile);
            Primitive.DrawTile(target);
        }

        #endregion Methods
    }
}
