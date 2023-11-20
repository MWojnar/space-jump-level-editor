using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
    internal class Board : Entity
    {
        private float verSpeed = 0;
        private float horSpeed = 0;
        private float initSpeed = 4;
        private int hangTime = 15;
        private float floor = 0;
        private float gravity = .3f;

        public Board(MainGame game, Vector2 position, GameTime gameTime, float floor, int direction, float depth = 1000) : base(game, position, game.AssetManager.GetSprite("SpriteSkateboardThrown"), gameTime, depth)
        {
            this.floor = floor;
            initSpeed = game.Settings.BoardSpeed;
            hangTime = game.Settings.BoardHangTime;
            gravity = game.Settings.BoardGravity;
            switch (direction)
            {
                case 0: verSpeed = -initSpeed; break;
                case 1: verSpeed = initSpeed; break;
                case 2: horSpeed = -initSpeed; break;
                case 3: horSpeed = initSpeed; break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (GetPos().Y > floor)
            {
                SetPos(GetPos().X, floor);
                horSpeed = 0;
                verSpeed = 0;
                SetAnimation("SpriteSkateboard", gameTime);
            }
            hangTime--;
            if (hangTime < 0)
                verSpeed += gravity;
            SetPos(GetPos().X + 2, GetPos().Y);
            SetPos(GetPos().X + horSpeed, GetPos().Y + verSpeed);
            if (GetPos().X < game.TrueCameraPos.X)
                SetPos(game.TrueCameraPos.X, GetPos().Y);
            if (GetPos().X + animation.Sprite.FrameWidth > game.TrueCameraPos.X + MainGame.GameWidth)
                SetPos(game.TrueCameraPos.X + MainGame.GameWidth - animation.Sprite.FrameWidth, GetPos().Y);
            foreach (Entity entity in game.Entities.Where(e => e is Candy && IsCollidingWithEntity(e)))
            {
                entity.Remove();
                game.AddPoints(50);
                game.AssetManager.GetSound("SoundCollect").Play();
            }
            foreach (Entity entity in game.Entities.Where(e => e is ToiletPaper && IsCollidingWithEntity(e)))
            {
                entity.Remove();
                game.AssetManager.GetSound("SoundTPDestroy").Play();
            }
        }
    }
}
