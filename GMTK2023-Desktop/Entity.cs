using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace spaceJumpLevelEditor
{
    public class Entity
    {
		private Vector2 position;
        protected Animation animation;
        protected Rectangle? sourceRect;
        public Rectangle? SourceRect { get { return sourceRect; } }
        protected MainGame game;
		private float depth;
        protected Sprite baseSprite;
        public Sprite BaseSprite { get { return baseSprite; } }
        protected bool isFlipped;
        protected bool visible;

		public float Depth
		{
			get => depth;
			set
			{
                depth = value;
                game.UpdateEntityDepth(this);
			}
		}

		public Entity(MainGame game, Vector2 position, Sprite sprite, GameTime gameTime, float depth = 0)
        {
            this.game = game;
            this.position = position;
            Depth = depth;
            baseSprite = sprite;
            isFlipped = false;
            visible = true;
            if (sprite != null)
                SetAnimation(new Animation(sprite, gameTime));
        }

        public void SetAnimation(Animation animation)
        {
            this.animation = animation;
            this.sourceRect = animation.Sprite.GetFrameRectCollision();
        }

        public void SetAnimation(string spriteName, GameTime gameTime, int? forcedFrame = null)
        {
            Sprite sprite = game.AssetManager.GetSprite(spriteName);
            if (sprite != animation.Sprite)
                SetAnimation(new Animation(sprite, gameTime, forcedFrame));
        }

        public void SetVisible(bool visible)
        {
            this.visible = visible;
        }

        public virtual void Draw(ExtendedSpriteBatch spriteBatch, GameTime gameTime)
        {
            if (animation != null && visible)
                animation.Draw(gameTime, spriteBatch, position, isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }

        public virtual void PreUpdate(GameTime gameTime)
        {
            //TODO
        }

        public virtual void Update(GameTime gameTime)
        {
            //TODO
        }

        public virtual void PostUpdate(GameTime gameTime)
        {
            //TODO
        }

        public virtual bool IsMouseOver()
        {
            return IsPointColliding(game.MousePos);
        }

        public virtual bool IsMouseOver(Rectangle rect)
        {
            return IsPointColliding(game.MousePos, rect);
        }

        public bool IsPointColliding(Vector2 point)
        {
            return (sourceRect?.Contains(point.X - position.X, point.Y - position.Y)??false);
        }

        public bool IsPointColliding(Vector2 point, Rectangle rect)
        {
			return (rect.Contains(point.X - position.X, point.Y - position.Y));
		}

        public Vector2 GetPos()
        {
            return position;
        }

        public void SetAnimationSpeed(float animationSpeed)
        {
            animation.AnimationRate = animationSpeed;
        }

        public void SetPos(float x, float y)
        {
            position.X = x;
            position.Y = y;
        }

		public bool IsCollidingWithEntity(Entity entity)
		{
            return !sourceRect.HasValue || !entity.sourceRect.HasValue ? false : sourceRect.Value.OffsetBy(GetPos()).Intersects(entity.sourceRect.Value.OffsetBy(entity.GetPos()));
		}

		public bool IsCollidingWithRect(Rectangle rect)
		{
			return !sourceRect.HasValue ? false : sourceRect.Value.OffsetBy(GetPos()).Intersects(rect);
		}

        public void Remove()
        {
            game.RemoveEntity(this);
        }
    }
}
