using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
    public class Animation
    {
        private Sprite sprite;
        private double startTime;
        public float AnimationRate;
        public int? forcedFrame;
        public Sprite Sprite { get { return sprite; } }

        public Animation(Sprite sprite, GameTime startTime, int? forcedFrame = null)
        {
            this.sprite = sprite;
            this.AnimationRate = sprite.AnimationRate;
            this.startTime = startTime.TotalGameTime.TotalSeconds;
            this.forcedFrame = forcedFrame;
        }
        
        public int GetCurrentFrame(GameTime gameTime)
		{
            if (forcedFrame != null)
                return forcedFrame.Value;
			float animationInterval = 1 / AnimationRate;
			double timeSinceStart = (float)gameTime.TotalGameTime.TotalSeconds - startTime;
			return (int)(timeSinceStart / animationInterval) % (sprite.Width / sprite.FrameWidth);
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None)
        {
			sprite.Draw(GetCurrentFrame(gameTime), spriteBatch, position, null, effects);
		}

        internal bool IsOver(GameTime gameTime)
        {
            float animationTime = sprite.GetFrames() / AnimationRate;
            return (gameTime.TotalGameTime.TotalSeconds - startTime) > animationTime;
        }
    }
}
