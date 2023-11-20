using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
    public class Shadow : Entity
    {
        private Entity followEntity;

        public Shadow(MainGame game, Vector2 position, GameTime gameTime, Entity followEntity, float depth = -1) : base(game, position + new Vector2(0, 1), game.AssetManager.GetSprite("SpriteShadow"), gameTime, depth)
        {
            this.followEntity = followEntity;
        }

        public override void PostUpdate(GameTime gameTime)
        {
            base.PostUpdate(gameTime);
            SetPos(followEntity.GetPos().X, GetPos().Y);
        }
    }
}
