namespace GameOne.Source.Renderer
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Output
    {
        private static SpriteBatch batch;
        private static GraphicsDevice gd;
        private static SpriteFont font;
        private static int penWidth;
        private static Color penColor;
        private static Color brushColor;

        public static void Init(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            batch = spriteBatch;
            gd = graphicsDevice;
            penWidth = 1;
            penColor = Color.Black;
            brushColor = Color.White;
        }

        public static void SetFont(SpriteFont newFont)
        {
            font = newFont;
        }

        private static void _DrawText(string text, float x, float y, Color color)
        {
            batch.DrawString(font, text, new Vector2(x, y), color);
        }

        public static void DrawText(string text, double x, double y)
        {
            _DrawText(text, (float)x, (float)y, penColor);
        }

        public static void DrawText(string text, double x, double y, Color color)
        {
            _DrawText(text, (float)x, (float)y, color);
        }

        private static void _FillRect(int left, int top, int width, int height, Color color)
        {
            Color[] data = new Color[width * height];
            Texture2D rect = new Texture2D(gd, width, height);
            for (int i = 0; i < data.Length; i++)
                data[i] = color;
            rect.SetData(data);
            batch.Draw(rect, new Rectangle(left, top, width, height), Color.White);
        }

        public static void FillRect(double left, double top, double width, double height)
        {
            _FillRect((int)left, (int)top, (int)width, (int)height, brushColor);
        }

        public static void FillRect(double left, double top, double width, double height, Color color)
        {
            _FillRect((int)left, (int)top, (int)width, (int)height, color);
        }

        private static void _StrokeRect(int left, int top, int width, int height, Color color, int stroke)
        {
            Color[] data = new Color[width * height];
            Texture2D rect = new Texture2D(gd, width, height);
            for (int i = 0; i < data.Length; i++)
            {
                int x = i % width;
                int y = i / width;
                if (x > stroke - 1 && x < width - stroke &&
                    y > stroke - 1 && y < height - stroke)
                    data[i] = Color.Transparent;
                else
                    data[i] = color;
            }
            rect.SetData(data);
            batch.Draw(rect, new Rectangle(left, top, width, height), Color.White);
        }

        public static void StrokeRect(double left, double top, double width, double height, Color color, int stroke)
        {
            _StrokeRect((int)left, (int)top, (int)width, (int)height, color, stroke);
        }

        public static void StrokeRect(double left, double top, double width, double height, Color color)
        {
            _StrokeRect((int)left, (int)top, (int)width, (int)height, color, penWidth);
        }

        public static void StrokeRect(double left, double top, double width, double height, int stroke)
        {
            _StrokeRect((int)left, (int)top, (int)width, (int)height, penColor, stroke);
        }

        public static void StrokeRect(double left, double top, double width, double height)
        {
            _StrokeRect((int)left, (int)top, (int)width, (int)height, penColor, penWidth);
        }

        private static void _FillEllipse(int left, int top, int width, int height, Color color)
        {
            // TODO implement ellipse logic
        }
    }
}
