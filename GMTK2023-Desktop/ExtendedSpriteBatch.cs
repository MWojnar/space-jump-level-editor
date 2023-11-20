using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
	public class ExtendedSpriteBatch : SpriteBatch
	{
		private Texture2D pixel;

		public ExtendedSpriteBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice)
		{
			pixel = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
			pixel.SetData(new[] { Color.White });
		}

		public void DrawLine(Vector2 start, Vector2 end, Color color, int lineWidth = 1)
		{
			Vector2 edge = end - start;
			float angle = (float)Math.Atan2(edge.Y, edge.X);

			Draw(
				pixel,
				new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), lineWidth),
				null,
				color,
				angle,
				new Vector2(0, 0),
				SpriteEffects.None,
				0);
		}
	}
}
