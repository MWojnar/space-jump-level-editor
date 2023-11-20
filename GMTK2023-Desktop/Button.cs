using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
    public class Button : Entity
    {
        private Action clickFunc;
        private bool lastPressed;
        private Rectangle? boundaries;
        private SpriteFont font;
        private string text;

        public Button(MainGame game, Vector2 position, Sprite sprite, GameTime gameTime, Action clickFunc, SpriteFont font = null, string text = null, Rectangle? boundaries = null) : base(game, position, sprite, gameTime)
        {
            this.clickFunc = clickFunc;
            lastPressed = false;
            this.boundaries = boundaries;
            this.font = font;
            this.text = text;
            if (this.boundaries == null && this.font != null && this.text != null)
                this.boundaries = new Rectangle(new Point(), this.font.MeasureString(this.text).ToPoint());
        }

        public override void Update(GameTime gameTime)
        {
            if (!lastPressed)
            {
                if (clickFunc != null && Mouse.GetState().LeftButton == ButtonState.Pressed && (!boundaries.HasValue ? IsMouseOver() : IsMouseOver(boundaries.Value)))
                    clickFunc();
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Released)
                lastPressed = false;
            base.Update(gameTime);
        }

        public override void Draw(ExtendedSpriteBatch spriteBatch, GameTime gameTime)
        {
            if (font == null || text == null)
                base.Draw(spriteBatch, gameTime);
            else
                spriteBatch.DrawString(font, text, GetPos(), Color.White);
        }
    }
}
