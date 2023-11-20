using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
    public class House : Entity
    {
        private bool hasThrown = false;
        private float throwPoint = 0;
        private bool isOpen = false;
        private bool isThrowDouble = false;

        public House(MainGame game, Vector2 position, bool isOpen, bool isThrowDouble, GameTime gameTime, float depth = 0) : base(game, position, isOpen ? game.AssetManager.GetSprite("SpriteHouseOpen") : game.AssetManager.GetSprite("SpriteHouseClosed"), gameTime, depth)
        {
            this.isOpen = isOpen;
            this.isThrowDouble = isThrowDouble;
            Random rand = new Random();
            throwPoint = 450 - (float)rand.NextDouble() * 100;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (GetPos().X < game.TrueCameraPos.X - 300)
                Remove();
            if (!this.isOpen)
                return;
            if (!hasThrown && GetPos().X < throwPoint + game.TrueCameraPos.X)
            {
                hasThrown = true;
                game.CreateEntity(new HouseThrower(game, GetPos(), gameTime, isThrowDouble));
            }
        }
    }
}
