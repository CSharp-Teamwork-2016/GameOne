namespace GameOne.Source.World
{
    using System.Linq;
    using Enumerations;
    using Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Renderer;
    using Containers;

    public class LevelEditor
    {
        private static Input input;
        private static double panSpeed = 3;

        public static TileType CurrentTile { get; set; }

        public static void Init(Input newInput)
        {
            input = newInput;
            CurrentTile = TileType.Wall;
        }

        public static void Update(GameTime time, KeyboardState keyboardState, MouseState mouseState)
        {
            UserInput current = input.Update(keyboardState, mouseState);

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                RemoveTile();
            }
            else if (mouseState.LeftButton == ButtonState.Pressed)
            {
                PlaceTile();
            }

            switch (current)
            {
                case UserInput.MoveUp:
                    Primitive.PanCameraUp(panSpeed);
                    break;
                case UserInput.MoveDown:
                    Primitive.PanCameraDown(panSpeed);
                    break;
                case UserInput.MoveLeft:
                    Primitive.PanCameraLeft(panSpeed);
                    break;
                case UserInput.MoveRight:
                    Primitive.PanCameraRight(panSpeed);
                    break;
                case UserInput.DrinkPotion:
                    CycleTile();
                    break;
            }
        }

        private static void CycleTile()
        {
            if (CurrentTile == TileType.Wall)
            {
                CurrentTile = TileType.Floor;
            }
            else
            {
                CurrentTile = TileType.Wall;
            }
        }

        private static void RemoveTile()
        {
            int targetX = (int)Primitive.ToWorldX(input.MouseX);
            int targetY = (int)Primitive.ToWorldY(input.MouseY);
            Tile target = GameContainer.level.Geometry.FirstOrDefault(t => t.X == targetX && t.Y == targetY);
            if (target != null)
            {
                GameContainer.level.Geometry.Remove(target);
            }
        }

        private static void PlaceTile()
        {
            int targetX = (int)Primitive.ToWorldX(input.MouseX);
            int targetY = (int)Primitive.ToWorldY(input.MouseY);
            Tile target = GameContainer.level.Geometry.FirstOrDefault(t => t.X == targetX && t.Y == targetY);

            if (target == null)
            {
                target = TileFactory.GetTile(targetX, targetY, CurrentTile);
                GameContainer.level.Geometry.Add(target);
            }
        }
    }
}
