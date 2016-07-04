namespace GameOne.Source.Renderer
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Spritesheet
    {
        // Methods and information for loading and sequencing series of frames into animations
        // Individual frames and sequences may be separate classes
        public Texture2D Texture;
        public int Rows { get; set; }
        public int Cols { get; set; }

        private int currentFrame;

        private int totalFrames;

        //slow down frame animation
        private int timeSinceLastFrame;
        private int millionSecPerFrame = 50;

        public Spritesheet()
        {
            // for remove. With purpose not to throw exception in other classes
        }

        public Spritesheet(Texture2D texture, int rows, int cols)
        {
            this.Texture = texture;
            this.Rows = rows;
            this.Cols = cols;
            this.currentFrame = 0;
            this.totalFrames = this.Rows * this.Cols;
        }

        public void Update(GameTime gameTime)
        {
            this.timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (this.timeSinceLastFrame > this.millionSecPerFrame)
            {
                this.timeSinceLastFrame -= this.millionSecPerFrame;
                this.currentFrame++;
                this.timeSinceLastFrame = 0;

                if (currentFrame == totalFrames)
                {
                    currentFrame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = this.Texture.Width / this.Cols;
            int height = this.Texture.Height / this.Rows;
            int row = (int)(float)this.currentFrame / this.Cols;
            int col = this.currentFrame % this.Cols;

            Rectangle sourceRectangle = new Rectangle(width * col, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(this.Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();

        }

        // TODO
    }
}
