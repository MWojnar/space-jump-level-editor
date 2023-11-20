using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
    public class ToiletPaper : Entity
    {
        private float horSpeed = -2;

        public ToiletPaper(MainGame game, Vector2 position, GameTime gameTime, float depth = 0) : base(game, position, game.AssetManager.GetSprite("SpriteToiletPaper"), gameTime, depth)
        {
            horSpeed = game.Settings.TPSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            SetPos(GetPos().X + horSpeed, GetPos().Y);
        }
    }
}
