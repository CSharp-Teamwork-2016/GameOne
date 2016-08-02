namespace GameOne.Source.Containers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using Renderer;
    using Enumerations;
    using World;
    using Entities;
    using Events;
    using Handlers;
    using UI;
    using UI.MainMenu;
    using UI.MainMenu.SaveAndLoadGame;

    // Game contents
    // Level
    // Entities
    // Parameters
    // Main loop
    [Serializable]
    public class GameContainer
    {
        #region Fields

        // Game objects
        private Level level;
        private readonly Input input;
        private EntityHandler entityHandler;

        // initial game state
        private GameState gameState = GameState.MainMenu;

        private SaveGame saveGame;
        private LoadGame loadGame;

        //Save and Load
        //SaveManager save;
        //bool saved = false;

        #endregion Fields

        #region Constructors

        public GameContainer(KeyboardState keyboardState, MouseState mouseState)
        {
            this.level = new Level();
            DebugInfo = string.Empty;
            Console = string.Empty;
            this.input = new Input(keyboardState, mouseState, this.level);
            LevelEditor.Init(this.input, this.level);
            this.MainMenu = new MainMenu(this);
            this.saveGame = new SaveGame(this);
            this.loadGame = new LoadGame(this);
            this.entityHandler = new EntityHandler(this.level);
            this.entityHandler.Subscribe(this.level.Entities);
            this.entityHandler.SubscribeToPlayer(this.level.Player);
            this.level.Player.ExitTriggeredEvent += this.OnExitTriggered;

            //string saveFolder = "SaveManagerTest"; // put your save folder name here
            //string saveFile = "test.sav"; // put your save file name here

            //this.save = new StorageDeviceSaveManager(saveFolder, saveFile, PlayerIndex.One);
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

        public void ButtonHandler(object sender, OnButtonClickEventArgs args)
        {
            this.gameState = args.GameState;
        }

        public void SaveButtonHandler(object sender, OnButtonClickEventArgs args)
        {
            using (Stream stream = File.Open(args.Name, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, this.level);
            }
        }

        public void LoadButtonHandler(object sender, OnButtonClickEventArgs args)
        {
            using (Stream stream = File.Open(args.Name, FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();

                this.level = (Level)bin.Deserialize(stream);
            }

            LevelEditor.Init(this.input, this.level);
            this.entityHandler = new EntityHandler(this.level);
            this.entityHandler.Subscribe(this.level.Entities);
            this.entityHandler.SubscribeToPlayer(this.level.Player);
            this.level.Player.ExitTriggeredEvent += this.OnExitTriggered;

            this.gameState = GameState.Gameplay;
        }

        private void OnExitTriggered(object sender, EventArgs e)
        {
            this.level.ExitTriggered = true;
        }

        internal void Update(GameTime time, KeyboardState keyboardState, MouseState mouseState)
        {
            switch (this.gameState)
            {
                case GameState.MainMenu:
                    this.MainMenu.Update(mouseState);
                    break;
                case GameState.Gameplay:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        this.gameState = GameState.MainMenu;
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
                    }
                    else
                    {
                        LevelEditor.Update(time, Keyboard.GetState(), Mouse.GetState());
                    }

                    break;
                case GameState.EndOfGame:
                    // TODO
                    break;
                case GameState.Credits:
                    break;
                case GameState.Exit:
                    Environment.Exit(1);
                    break;
                case GameState.LoadGame:
                    this.loadGame.Update(mouseState);
                    break;
                case GameState.SaveGame:
                    this.saveGame.Update(mouseState);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

            this.entityHandler.ProcessEntities(time.ElapsedGameTime.Milliseconds/1000.0);

            // Execute tests
            foreach (Action test in Tests.ListOf.OnUpdate)
            {
                test();
            }
            // Debug info
            DebugInfo = $"Player stats:{Environment.NewLine}State: {this.level.Player.State}{Environment.NewLine}Health: {this.level.Player.Health} / {this.level.Player.MaxHealth}{Environment.NewLine}Damage: {this.level.Player.Damage}{Environment.NewLine}{Environment.NewLine}Level {this.level.Player.XpLevel}{Environment.NewLine}XP: {this.level.Player.Experience} / {this.level.Player.XpToNext}{Environment.NewLine}{Environment.NewLine}Enemies remaining: {this.level.EnemyCount}{Environment.NewLine}";
            if (ShowFPS)
            {
                DebugInfo += $"{(1000/time.ElapsedGameTime.TotalMilliseconds):f2}{Environment.NewLine}";
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
                    double hpc = (double) this.level.Player.Health/this.level.Player.MaxHealth;
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
                case GameState.Credits:
                    break;
                case GameState.Exit:
                    break;
                case GameState.LoadGame:
                    this.loadGame.Draw();
                    break;
                case GameState.SaveGame:
                    this.saveGame.Draw();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RenderLevel()
        {
            this.level.Geometry.ForEach(Primitive.DrawTile);
            foreach (var entity in this.level.Entities.Where(e => e is Model))
            {
                var model = (Model) entity;
                Primitive.DrawModel(model);
            }
        }

        private void RendeGrid()
        {
            this.level.Geometry.ForEach(Primitive.DrawGrid);
            Tile target = new Tile((int) Primitive.ToWorldX(this.input.MouseX), (int) Primitive.ToWorldY(this.input.MouseY), LevelEditor.CurrentTile);
            Primitive.DrawTile(target);
        }

        #endregion Methods
    }
}
