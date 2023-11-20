using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
    internal class Candy : Entity
    {
        private float verVel = 0;
        private float horVel = 0;
        private float initVel = -.5f;
        private float termVel = 2;
        private float gravity = .15f;

        public Candy(MainGame game, Vector2 position, GameTime gameTime, float depth = 0) : base(game, position, game.AssetManager.GetSprite("SpriteCandyStart"), gameTime, depth)
        {
            verVel = (float)new Random().NextDouble() * initVel - 4.5f;
            horVel = (float)new Random().NextDouble() * 2f - 1f;
            initVel = game.Settings.CandyInitVelocity;
            termVel = game.Settings.CandyTermVelocity;
            gravity = game.Settings.CandyGravity;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (animation.Sprite == game.AssetManager.GetSprite("SpriteCandyStart") && animation.IsOver(gameTime))
                SetAnimation(new Animation(game.AssetManager.GetSprite("SpriteCandy"), gameTime));
            verVel += gravity;
            if (verVel > termVel)
                verVel = termVel;
            SetPos(GetPos().X + horVel, GetPos().Y + verVel);
            if (GetPos().Y > MainGame.GameHeight)
            {
                Remove();
                game.RemoveHealth();
                game.AssetManager.GetSound("SoundLoss").Play();
            }
        }
    }
}
