using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
    internal class HouseThrower : Entity
    {
        private bool isThrown = false;
        private bool isThrowDouble = false;

        public HouseThrower(MainGame game, Vector2 position, GameTime gameTime, bool isThrowDouble, float depth = 0) : base(game, position, game.AssetManager.GetSprite("SpriteCandyThrow"), gameTime, depth)
        {
            this.isThrowDouble = isThrowDouble;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!isThrown && animation.GetCurrentFrame(gameTime) > 4)
            {
                if (!isThrowDouble)
                    game.CreateEntity(new Candy(game, GetPos() + new Vector2(60, 70), gameTime));
                else
                {
                    game.CreateEntity(new Candy(game, GetPos() + new Vector2(50, 70), gameTime));
                    game.CreateEntity(new Candy(game, GetPos() + new Vector2(70, 70), gameTime));
                }
                isThrown = true;
            }
            if (animation.IsOver(gameTime))
                Remove();
        }
    }
}
