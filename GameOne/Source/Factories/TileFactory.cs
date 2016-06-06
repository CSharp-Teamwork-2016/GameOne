namespace GameOne.Source.Factories
{
    using Enumerations;
    using Level;

    public class TileFactory
    {
        public static Tile getTile(int x, int y, TileType type)
        {
            switch (type)
            {
                case TileType.Wall:
                    return new Tile(x, y, TileType.Wall, "wall");
                case TileType.Floor:
                    return new Tile(x, y, TileType.Floor, "floor");
                case TileType.Door:
                    return new Tile(x, y, TileType.Door, "door");
                default:
                    return null;
            }
        }

        public static Tile getTile(int x, int y, int type)
        {
            switch (type)
            {
                case 1:
                    return getTile(x, y, TileType.Wall);
                case 2:
                    return getTile(x, y, TileType.Floor);
                case 3:
                    return getTile(x, y, TileType.Door);
                default:
                    return null;
            }
        }
    }
}