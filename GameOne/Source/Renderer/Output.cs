namespace GameOne.Source.Renderer
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Output
    {
        private static SpriteBatch batch;
        private static GraphicsDevice GD;
        private static SpriteFont font;
        private static int penWidth;
        private static Color penColor;
        private static Color brushColor;

		#region Properties
		/// <summary>
		/// Width of the stroke made with operations that draw outlines of shapes
		/// </summary>
		public static int PenWidth
		{
			get
			{
				return penWidth;
			}
			set
			{
				penWidth = value;
			}
		}
		/// <summary>
		/// Color of the stroke made with operations that draw outlines of shapes
		/// </summary>
		public static Color PenColor
		{
			get
			{
				return penColor;
			}
			set
			{
				penColor = value;
			}
		}
		/// <summary>
		/// Fill color used with operations that draw solid shapes
		/// </summary>
		public static Color BrushColor
		{
			get
			{
				return brushColor;
			}
			set
			{
				brushColor = value;
			}
		}
		#endregion
		///
		/// <summary>
		/// Initialize drawing surfaces
		/// </summary>
		/// <param name="spriteBatch">2D surface used for drawing</param>
		/// <param name="GraphicsDevice">Reference <see cref="GraphicsDevice"/>, used by the main <see cref="Game"/>type</param>
		public static void Init(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
		{
			batch = spriteBatch;
			GD = GraphicsDevice;
			PenWidth = 1;
			PenColor = Color.Black;
			BrushColor = Color.White;
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
			_DrawText(text, (float)x, (float)y, penColor);
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
		/// Draw solid rectangle
		/// </summary>
		private static void _FillRect(int left, int top, int width, int height, Color color)
		{
			Color[] data = new Color[width * height];
			Texture2D rect = new Texture2D(GD, width, height);
			for (int i = 0; i < data.Length; i++)
				data[i] = color;
			rect.SetData(data);
			batch.Draw(rect, new Rectangle(left, top, width, height), Color.White);
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
			_FillRect((int)left, (int)top, (int)width, (int)height, brushColor);
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
			Color[] data = new Color[width * height];
			Texture2D rect = new Texture2D(GD, width, height);
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
			_StrokeRect((int)left, (int)top, (int)width, (int)height, color, penWidth);
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
			_StrokeRect((int)left, (int)top, (int)width, (int)height, penColor, stroke);
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
			_StrokeRect((int)left, (int)top, (int)width, (int)height, penColor, penWidth);
		}
		///
		/// <summary>
		/// Draw solid circle
		/// </summary>
		private static void _FillCircle(int left, int top, int width, int height, Color color)
		{
			//Color[] data = new Color[width * height];
			//Texture2D rect = new Texture2D(GD, width, height);
			//for (int i = 0; i < data.Length; i++)
			//	data[i] = Color.Transparent;

			int xc = left + width / 2;
			int yc = top + height / 2;
			int a2 = width * width;
			int b2 = height * height;
			int fa2 = 4 * a2, fb2 = 4 * b2;
			int x, y, sigma;

			/* first half */
			for (x = 0, y = height, sigma = 2 * b2 + a2 * (1 - 2 * height); b2 * x <= a2 * y; x++)
			{
				/*
				data[Index(xc + x, yc + y, width)] = color;
				data[Index(xc - x, yc + y, width)] = color;
				data[Index(xc + x, yc - y, width)] = color;
				data[Index(xc - x, yc - y, width)] = color;
				*/
				_FillRect(xc + x, yc + y, 1, 1, Color.Black);
				_FillRect(xc - x, yc + y, 1, 1, Color.Black);
				_FillRect(xc + x, yc - y, 1, 1, Color.Black);
				_FillRect(xc - x, yc - y, 1, 1, Color.Black);
				if (sigma >= 0)
				{
					sigma += fa2 * (1 - y);
					y--;
				}
				sigma += b2 * ((4 * x) + 6);
			}

			/* second half */
			for (x = width, y = 0, sigma = 2 * a2 + b2 * (1 - 2 * width); a2 * y <= b2 * x; y++)
			{
				/*
				data[Index(xc + x, yc + y, width)] = color;
				data[Index(xc - x, yc + y, width)] = color;
				data[Index(xc + x, yc - y, width)] = color;
				data[Index(xc - x, yc - y, width)] = color;
				*/
				_FillRect(xc + x, yc + y, 1, 1, Color.Black);
				_FillRect(xc - x, yc + y, 1, 1, Color.Black);
				_FillRect(xc + x, yc - y, 1, 1, Color.Black);
				_FillRect(xc - x, yc - y, 1, 1, Color.Black);
				if (sigma >= 0)
				{
					sigma += fb2 * (1 - x);
					x--;
				}
				sigma += a2 * ((4 * y) + 6);
			}

			//rect.SetData(data);
			//batch.Draw(rect, new Rectangle(left, top, width, height), Color.White);
		}

		private static int Index(int x, int y, int width)
		{
			return y * width + x;
		}
	}
}
