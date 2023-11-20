using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace spaceJumpLevelEditor
{
	public class TitleScreen : Entity
	{
		private SpriteFont font;
		private double startTime, width;

		public TitleScreen(MainGame game, Vector2 position, GameTime gameTime, float depth = 0) : base(game, position, game.AssetManager.GetSprite("BackgroundTitle"), gameTime, depth)
		{
			startTime = gameTime.TotalGameTime.TotalSeconds;
		}

		public override void Update(GameTime gameTime)
		{
            if (gameTime.TotalGameTime.TotalSeconds - startTime < 1)
                return;
            if (anyKeyDown() || Mouse.GetState().LeftButton == ButtonState.Pressed)
				game.StartRoom(1, gameTime);
		}

		private bool anyKeyDown()
		{
			var keyboardState = Keyboard.GetState();
			foreach (Keys key in Enum.GetValues(typeof(Keys)))
			{
				if (keyboardState.IsKeyDown(key))
					return true;
			}
			return false;
		}
	}
}