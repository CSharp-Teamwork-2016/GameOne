namespace GameOne.Source.Factories
{
    using Enumerations;
    using World;

    public class TileFactory
    {
        public static Tile GetTile(int x, int y, TileType type)
        {
            switch (type)
            {
                case TileType.Wall:
                    return new Tile(x, y, TileType.Wall);
                case TileType.Floor:
                    return new Tile(x, y, TileType.Floor);
                case TileType.Door:
                    return new Tile(x, y, TileType.Door);
                default:
                    return null;
            }
        }

        public static Tile GetTile(int x, int y, int type)
        {
            switch (type)
            {
                case 1:
                    return GetTile(x, y, TileType.Wall);
                case 2:
                    return GetTile(x, y, TileType.Floor);
                case 3:
                    return GetTile(x, y, TileType.Door);
                default:
                    return null;
            }
        }
    }
}