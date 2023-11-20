using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
    public class Sprite
    {
        private Texture2D texture;
        private int frameWidth;
        private int frameHeight;
        private float animationRate;
        private Vector2 offset;
		public int FrameWidth { get { return frameWidth; } }
		public int FrameHeight { get { return frameHeight; } }
		public int Width { get { return texture.Width; } }
		public int Height { get { return texture.Height; } }
        public Vector2 Offset { get { return offset; } }

		public Sprite(Texture2D texture)
        {
            this.texture = texture;
            this.frameWidth = texture.Width;
            this.frameHeight = texture.Height;
            this.animationRate = 60;
            this.offset = new Vector2(0, 0);
        }

        public Sprite(Texture2D texture, int frames, float animationRate, Vector2 offset = new Vector2())
        {
            this.texture = texture;
            this.frameWidth = texture.Width / frames;
            this.frameHeight = texture.Height;
            this.animationRate = animationRate;
            this.offset = offset;
        }

		public float AnimationRate { get { return animationRate; } }

        public Rectangle GetFrameRect(int frame)
        {
            int frameX = frame * frameWidth;
            return new Rectangle(frameX, 0, frameWidth, frameHeight);
        }

        public Rectangle GetFrameRectCollision()
        {
            return new Rectangle(0, 0, frameWidth + (int)offset.X * 2, frameHeight + (int)offset.X * 2);
        }

        private Vector2 zero = new Vector2();

        public void Draw(int frame, SpriteBatch spriteBatch, Vector2 position, Color? color = null, SpriteEffects effects = SpriteEffects.None)
        {
            var sourceRect = GetFrameRect(frame);
            spriteBatch.Draw(Texture, position + offset, sourceRect, color.HasValue ? color.Value : Color.White, 0, zero, 1, effects, 0);
        }

		public void Draw(Rectangle sourceRect, SpriteBatch spriteBatch, Vector2 position, Color? color = null, SpriteEffects effects = SpriteEffects.None)
		{
			spriteBatch.Draw(Texture, position + offset, sourceRect, color.HasValue ? color.Value : Color.White, 0, zero, 1, effects, 0);
		}

        internal int GetFrames()
        {
            return texture.Width / frameWidth;
        }

        public Texture2D Texture { get { return texture; } }
    }
}
