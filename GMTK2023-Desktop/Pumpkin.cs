using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
    public class Pumpkin : Entity
    {
        public Pumpkin(MainGame game, Vector2 position, GameTime gameTime, float depth = 0) : base(game, position, game.AssetManager.GetSprite("SpritePumpkin"), gameTime, depth)
        {
            game.CreateEntity(new Shadow(game, position, gameTime, this));
        }
    }
}
