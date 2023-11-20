using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace spaceJumpLevelEditor
{
	public class GameOver : Entity
	{
		private SpriteFont font;
		private double startTime, width, scoreWidth;
		private bool keyDown;

		public GameOver(MainGame game, Vector2 position, GameTime gameTime, float depth = 0) : base(game, position, game.AssetManager.GetSprite("BackgroundGameOver"), gameTime, depth)
		{
			font = game.AssetManager.GetFont("FontDogicaPixelBold");
			startTime = gameTime.TotalGameTime.TotalSeconds;
			width = font.MeasureString("Press Any Key").X;
			scoreWidth = font.MeasureString($"Final Score: {game.Points}").X;
			keyDown = false;
		}

		public override void Draw(ExtendedSpriteBatch spriteBatch, GameTime gameTime)
		{
			base.Draw(spriteBatch, gameTime);
			spriteBatch.DrawString(font, $"Final Score: {game.Points}", new Vector2((float)(MainGame.GameWidth / 2 - (scoreWidth / 2)), 125), game.TextColor);
		}

		public override void Update(GameTime gameTime)
		{
			if (gameTime.TotalGameTime.TotalSeconds - startTime < 1)
				return;
			if (!(anyKeyDown() || Mouse.GetState().LeftButton == ButtonState.Pressed) && keyDown)
				game.StartRoom(0, gameTime);
			if ((anyKeyDown() || Mouse.GetState().LeftButton == ButtonState.Pressed))
				keyDown = true;
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