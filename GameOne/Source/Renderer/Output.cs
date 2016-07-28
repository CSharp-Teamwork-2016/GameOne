namespace GameOne.Source.Renderer
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Output
    {
        #region Fields

        private static SpriteBatch batch;
        private static GraphicsDevice graphicDevice;
        private static SpriteFont font;
        private static Texture2D pixel;

        #endregion Fields

        //===================================================================

        #region Properties

        /// <summary>
        /// Width of the stroke made with operations that draw outlines of shapes
        /// </summary>
        public static int PenWidth { get; set; }

        /// <summary>
        /// Color of the stroke made with operations that draw outlines of shapes
        /// </summary>
        public static Color PenColor { get; set; }

        /// <summary>
        /// Fill color used with operations that draw solid shapes
        /// </summary>
        public static Color BrushColor { get; set; }

        #endregion Properties

        //===================================================================

        //All static
        #region Methods

        /// <summary>
        /// Initialize drawing surfaces
        /// </summary>
        /// <param name="spriteBatch">2D surface used for drawing</param>
        /// <param name="graphicsDevice">Reference <see cref="GraphicsDevice"/>, used by the main <see cref="Game"/>type</param>
        public static void Init(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            batch = spriteBatch;
            graphicDevice = graphicsDevice;
            PenWidth = 1;
            PenColor = Color.Black;
            BrushColor = Color.White;
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        ///
        /// <summary>
        /// Change the default font used when outputting text
        /// </summary>
        /// <param name="newFont">New font to use</param>
        public static void SetFont(SpriteFont newFont)
        {
            font = newFont;
        }

        ///
        /// <summary>
        /// Draw square point
        /// </summary>
        /// <param name="x">Center left offset</param>
        /// <param name="y">Center top offset</param>
        /// <param name="size">Diameter</param>
        /// /// <param name="color">color</param>
        private static void _DrawPoint(int x, int y, int size, Color color)
        {
            Rectangle rec = new Rectangle(x - size / 2, y - size / 2, size, size);
            batch.Draw(pixel, rec, color);
        }

        ///
        /// <summary>
        /// Draw text
        /// </summary>
        private static void _DrawText(string text, float x, float y, Color color)
        {
            batch.DrawString(font, text, new Vector2(x, y), color);
        }

        ///
        /// <summary>
        /// Outputs texts to the screen on the specified location, using the default font and currently set PenColor
        /// <para><paramref name="x"/> and <paramref name="y"/> are supplied in screen units</para>
        /// <para>Origin point is the upper left corner of the resulting object</para>
        /// </summary>
        /// <param name="text">String to be displayed</param>
        /// <param name="x">Left offset</param>
        /// <param name="y">Top offset</param>
        public static void DrawText(string text, double x, double y)
        {
            _DrawText(text, (float)x, (float)y, PenColor);
        }

        ///
        /// <summary>
        /// Outputs texts to the screen on the specified location, using the default font and specified color
        /// <para><paramref name="x"/> and <paramref name="y"/> are supplied in screen units</para>
        /// <para>Origin point is the upper left corner of the resulting object</para>
        /// </summary>
        /// <param name="text">String to be displayed</param>
        /// <param name="x">Left offset</param>
        /// <param name="y">Top offset</param>
        /// <param name="color">Text color </param>
        public static void DrawText(string text, double x, double y, Color color)
        {
            _DrawText(text, (float)x, (float)y, color);
        }

        ///
        /// <summary>
        /// Draw line
        /// </summary>
        private static void _DrawLine(int x1, int y1, int x2, int y2, Color color, int width)
        {
            bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (steep)
            {
                int t;
                t = x1; // swap x0 and y0
                x1 = y1;
                y1 = t;
                t = x2; // swap x1 and y1
                x2 = y2;
                y2 = t;
            }

            if (x1 > x2)
            {
                int t;
                t = x1; // swap x0 and x1
                x1 = x2;
                x2 = t;
                t = y1; // swap y0 and y1
                y1 = y2;
                y2 = t;
            }

            int dx = x2 - x1;
            int dy = Math.Abs(y2 - y1);
            int error = dx / 2;
            int ystep = (y1 < y2) ? 1 : -1;
            int y = y1;

            for (int x = x1; x <= x2; x++)
            {
                _DrawPoint((steep ? y : x), (steep ? x : y), width, color);
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }

        ///
        /// <summary>
        /// Draw line between two points, with the specified color and thickenss
        /// </summary>
        /// <param name="x1">Starting point left offset</param>
        /// <param name="y1">Starting point top offset</param>
        /// <param name="x2">Ending point left offset</param>
        /// <param name="y2">Ending point top offset</param>
        /// <param name="color">Line color</param>
        /// <param name="width">Line thickness</param>
        public static void DrawLine(double x1, double y1, double x2, double y2, Color color, int width)
        {
            _DrawLine((int)x1, (int)y1, (int)x2, (int)y2, color, width);
        }

        ///
        /// <summary>
        /// Draw line between two points, with the specified color and current PenWidth
        /// </summary>
        /// <param name="x1">Starting point left offset</param>
        /// <param name="y1">Starting point top offset</param>
        /// <param name="x2">Ending point left offset</param>
        /// <param name="y2">Ending point top offset</param>
        /// <param name="color">Line color</param>
        public static void DrawLine(double x1, double y1, double x2, double y2, Color color)
        {
            _DrawLine((int)x1, (int)y1, (int)x2, (int)y2, color, PenWidth);
        }

        ///
        /// <summary>
        /// Draw line between two points, with the specified thickenss nad current PenColor
        /// </summary>
        /// <param name="x1">Starting point left offset</param>
        /// <param name="y1">Starting point top offset</param>
        /// <param name="x2">Ending point left offset</param>
        /// <param name="y2">Ending point top offset</param>
        /// <param name="width">Line thickness</param>
        public static void DrawLine(double x1, double y1, double x2, double y2, int width)
        {
            _DrawLine((int)x1, (int)y1, (int)x2, (int)y2, PenColor, width);
        }

        ///
        /// <summary>
        /// Draw line between two points, with the current PenColor and PenWidth
        /// </summary>
        /// <param name="x1">Starting point left offset</param>
        /// <param name="y1">Starting point top offset</param>
        /// <param name="x2">Ending point left offset</param>
        /// <param name="y2">Ending point top offset</param>
        public static void DrawLine(double x1, double y1, double x2, double y2)
        {
            _DrawLine((int)x1, (int)y1, (int)x2, (int)y2, PenColor, PenWidth);
        }

        ///
        /// <summary>
        /// Draw solid rectangle
        /// </summary>
        private static void _FillRect(int left, int top, int width, int height, Color color)
        {
            batch.Draw(pixel, new Rectangle(left, top, width, height), color);
        }

        ///
        /// <summary>
        /// Draw a solid rectangle using the currently set BrushColor
        /// </summary>
        /// <param name="left">Left offset</param>
        /// <param name="top">Top offset</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        public static void FillRect(double left, double top, double width, double height)
        {
            _FillRect((int)left, (int)top, (int)width, (int)height, BrushColor);
        }

        ///
        /// <summary>
        /// Draw a solid rectangle using the specified color
        /// </summary>
        /// <param name="left">Left offset</param>
        /// <param name="top">Top offset</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        /// <param name="color">Fill color</param>
        public static void FillRect(double left, double top, double width, double height, Color color)
        {
            _FillRect((int)left, (int)top, (int)width, (int)height, color);
        }

        ///
        /// <summary>
        /// Draw rectangle outline
        /// </summary>
        private static void _StrokeRect(int left, int top, int width, int height, Color color, int stroke)
        {
            _DrawLine(left, top, left + width - 1, top, color, stroke);
            _DrawLine(left, top + height - 1, left + width - 1, top + height - 1, color, stroke);
            _DrawLine(left, top, left, top + height - 1, color, stroke);
            _DrawLine(left + width - 1, top, left + width - 1, top + height - 1, color, stroke);
        }

        ///
        /// <summary>
        /// Draw the outline of the specified rectangle, using <paramref name="color"/> and thickness defined by <paramref name="stroke"/>
        /// </summary>
        /// <param name="left">Left offset</param>
        /// <param name="top">Top offset</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        /// <param name="color">Outline color</param>
        /// <param name="stroke">Outline thickness</param>
        public static void StrokeRect(double left, double top, double width, double height, Color color, int stroke)
        {
            _StrokeRect((int)left, (int)top, (int)width, (int)height, color, stroke);
        }

        ///
        /// <summary>
        /// Draw the outline of the specified rectangle, using <paramref name="color"/> and current PenWidth
        /// </summary>
        /// <param name="left">Left offset</param>
        /// <param name="top">Top offset</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        /// <param name="color">Outline color</param>
        public static void StrokeRect(double left, double top, double width, double height, Color color)
        {
            _StrokeRect((int)left, (int)top, (int)width, (int)height, color, PenWidth);
        }

        ///
        /// <summary>
        /// Draw the outline of the specified rectangle, using current PenColor and thickness defined by <paramref name="stroke"/>
        /// </summary>
        /// <param name="left">Left offset</param>
        /// <param name="top">Top offset</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        /// <param name="stroke">Outline thickness</param>
        public static void StrokeRect(double left, double top, double width, double height, int stroke)
        {
            _StrokeRect((int)left, (int)top, (int)width, (int)height, PenColor, stroke);
        }

        ///
        /// <summary>
        /// Draw the outline of the specified rectangle, using current PenColor and PenWidth
        /// </summary>
        /// <param name="left">Left offset</param>
        /// <param name="top">Top offset</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        public static void StrokeRect(double left, double top, double width, double height)
        {
            _StrokeRect((int)left, (int)top, (int)width, (int)height, PenColor, PenWidth);
        }

        ///
        /// <summary>
        /// Draw solid oval/ellipse
        /// </summary>
        private static void _FillOval(int left, int top, int width, int height, Color color)
        {
            int a = width / 2;
            int b = height / 2;

            int cx = left + a;
            int cy = top + b;

            int a2 = a * a;
            int b2 = b * b;

            int twoa2 = 2 * a2;
            int twob2 = 2 * b2;

            int p;
            int x = 0;
            int y = b;
            int px = 0;
            int py = twoa2 * y;

            /* Plot the initial point in each quadrant. */
            _DrawLine(cx + x, cy + y, cx - x, cy + y, color, 1);
            _DrawLine(cx + x, cy - y, cx - x, cy - y, color, 1);

            /* Region 1 */
            p = (int)(b2 - (a2 * b) + (0.25 * a2));

            while (px < py)
            {
                x++;
                px += twob2;

                if (p < 0)
                {
                    p += b2 + px;
                }
                else
                {
                    y--;
                    py -= twoa2;
                    p += b2 + px - py;
                }

                _DrawLine(cx + x, cy + y, cx - x, cy + y, color, 1);
                _DrawLine(cx + x, cy - y, cx - x, cy - y, color, 1);
            }

            /* Region 2 */
            p = (int)(b2 * (x + 0.5) * (x + 0.5) + a2 * (y - 1) * (y - 1) - a2 * b2);

            while (y > 0)
            {
                y--;
                py -= twoa2;

                if (p > 0)
                {
                    p += a2 - py;
                }
                else
                {
                    x++;
                    px += twob2;
                    p += a2 - py + px;
                }

                _DrawLine(cx + x, cy + y, cx - x, cy + y, color, 1);
                _DrawLine(cx + x, cy - y, cx - x, cy - y, color, 1);
            }
        }

        ///
        /// <summary>
        /// Draw a solid oval inside the rectangle, specified by the given coordinates and color
        /// </summary>
        /// <param name="left">Bounding rectangle left offset</param>
        /// <param name="top">Bounding rectangle top offset</param>
        /// <param name="width">Bounding rectangle width</param>
        /// <param name="height">Bounding rectangle height</param>
        /// <param name="color">Fill color</param>
        public static void FillOval(double left, double top, double width, double height, Color color)
        {
            _FillOval((int)left, (int)top, (int)width, (int)height, color);
        }

        ///
        /// <summary>
        /// Draw a solid oval inside the rectangle, specified by the given coordinates and current BrushColor
        /// </summary>
        /// <param name="left">Bounding rectangle left offset</param>
        /// <param name="top">Bounding rectangle top offset</param>
        /// <param name="width">Bounding rectangle width</param>
        /// <param name="height">Bounding rectangle height</param>
        public static void FillOval(double left, double top, double width, double height)
        {
            _FillOval((int)left, (int)top, (int)width, (int)height, BrushColor);
        }

        ///
        /// <summary>
        /// Draw oval/ellipse outline
        /// </summary>
        private static void _StrokeOval(int left, int top, int width, int height, Color color, int stroke)
        {
            int a = width / 2;
            int b = height / 2;

            int cx = left + a;
            int cy = top + b;

            int a2 = a * a;
            int b2 = b * b;

            int twoa2 = 2 * a2;
            int twob2 = 2 * b2;

            int p;
            int x = 0;
            int y = b;
            int px = 0;
            int py = twoa2 * y;

            /* Plot the initial point in each quadrant. */
            _DrawPoint(cx + x, cy + y, stroke, color);
            _DrawPoint(cx - x, cy + y, stroke, color);
            _DrawPoint(cx + x, cy - y, stroke, color);
            _DrawPoint(cx - x, cy - y, stroke, color);

            /* Region 1 */
            p = (int)(b2 - (a2 * b) + (0.25 * a2));

            while (px < py)
            {
                x++;
                px += twob2;
                if (p < 0)
                {
                    p += b2 + px;
                }
                else
                {
                    y--;
                    py -= twoa2;
                    p += b2 + px - py;
                }

                _DrawPoint(cx + x, cy + y, stroke, color);
                _DrawPoint(cx - x, cy + y, stroke, color);
                _DrawPoint(cx + x, cy - y, stroke, color);
                _DrawPoint(cx - x, cy - y, stroke, color);
            }

            /* Region 2 */
            p = (int)(b2 * (x + 0.5) * (x + 0.5) + a2 * (y - 1) * (y - 1) - a2 * b2);

            while (y > 0)
            {
                y--;
                py -= twoa2;

                if (p > 0)
                {
                    p += a2 - py;
                }
                else
                {
                    x++;
                    px += twob2;
                    p += a2 - py + px;
                }

                _DrawPoint(cx + x, cy + y, stroke, color);
                _DrawPoint(cx - x, cy + y, stroke, color);
                _DrawPoint(cx + x, cy - y, stroke, color);
                _DrawPoint(cx - x, cy - y, stroke, color);
            }
        }

        ///
        /// <summary>
        /// Draw the outline of the oval insid the rectangle, specified by the given coordinates, color and thickness
        /// </summary>
        /// <param name="left">Bounding rectangle left offset</param>
        /// <param name="top">Bounding rectangle top offset</param>
        /// <param name="width">Bounding rectangle width</param>
        /// <param name="height">Bounding rectangle height</param>
        /// <param name="color">Outline color</param>
        /// <param name="stroke">Outline thickness</param>
        public static void StrokeOval(double left, double top, double width, double height, Color color, double stroke)
        {
            _StrokeOval((int)left, (int)top, (int)width, (int)height, color, (int)stroke);
        }

        ///
        /// <summary>
        /// Draw the outline of the oval insid the rectangle, specified by the given coordinates, color and current PenWidth
        /// </summary>
        /// <param name="left">Bounding rectangle left offset</param>
        /// <param name="top">Bounding rectangle top offset</param>
        /// <param name="width">Bounding rectangle width</param>
        /// <param name="height">Bounding rectangle height</param>
        /// <param name="color">Outline color</param>
        public static void StrokeOval(double left, double top, double width, double height, Color color)
        {
            _StrokeOval((int)left, (int)top, (int)width, (int)height, color, PenWidth);
        }

        ///
        /// <summary>
        /// Draw the outline of the oval insid the rectangle, specified by the given coordinates, thickness and current PenColor
        /// </summary>
        /// <param name="left">Bounding rectangle left offset</param>
        /// <param name="top">Bounding rectangle top offset</param>
        /// <param name="width">Bounding rectangle width</param>
        /// <param name="height">Bounding rectangle height</param>
        /// <param name="stroke">Outline thickness</param>
        public static void StrokeOval(double left, double top, double width, double height, double stroke)
        {
            _StrokeOval((int)left, (int)top, (int)width, (int)height, PenColor, (int)stroke);
        }

        ///
        /// <summary>
        /// Draw the outline of the oval insid the rectangle, specified by the given coordinates and current PenColor and PenWidth
        /// </summary>
        /// <param name="left">Bounding rectangle left offset</param>
        /// <param name="top">Bounding rectangle top offset</param>
        /// <param name="width">Bounding rectangle width</param>
        /// <param name="height">Bounding rectangle height</param>
        public static void StrokeOval(double left, double top, double width, double height)
        {
            _StrokeOval((int)left, (int)top, (int)width, (int)height, PenColor, PenWidth);
        }

        /// <summary>
        /// Draw texture at specified position
        /// </summary>
        /// <param name="texture">Texture to draw</param>
        /// <param name="position">Screen coordinates</param>
        public static void Draw(Texture2D texture, Vector2 position)
        {
            batch.Draw(texture, position);
        }

        public static void Draw(Texture2D texture, Rectangle rect)
        {
            batch.Draw(texture, destinationRectangle: rect);
        }

        private static int Index(int x, int y, int width)
        {
            return y * width + x;
        }

        #endregion Methods
    }
}