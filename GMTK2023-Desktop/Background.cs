using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
    public class Background : Entity
    {
        private float parallax;

        public Background(MainGame game, Vector2 position, Sprite sprite, GameTime gameTime, float parallax, float depth = -100) : base(game, position, sprite, gameTime, depth)
        {
            this.parallax = parallax;
        }

        public override void Draw(ExtendedSpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 displacement = -game.CameraPos * parallax + GetPos();

            int startX = (int)displacement.X % animation.Sprite.Width;
            int startY = (int)displacement.Y % animation.Sprite.Height;

            int tilesX = MainGame.GameWidth / animation.Sprite.Width + 2;
            int tilesY = MainGame.GameHeight / animation.Sprite.Height + 2;

            int offsetX = (int)((game.TrueCameraPos.X) / MainGame.GameWidth) * MainGame.GameWidth;
            int offsetY = (int)((game.TrueCameraPos.Y) / MainGame.GameHeight) * MainGame.GameHeight;

            for (int x = -1; x < tilesX; x++)
            {
                for (int y = -1; y < tilesY; y++)
                {
                    Vector2 position = new Vector2(x * animation.Sprite.Width - startX + offsetX, y * animation.Sprite.Height - startY + offsetY);
                    animation.Sprite.Draw(0, spriteBatch, position);
                }
            }
        }
    }
}
