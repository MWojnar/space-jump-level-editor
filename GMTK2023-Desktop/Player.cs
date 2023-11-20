using Fall2023Jam_Desktop;
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
    public class Player : Entity
    {
        private float horSpeed = 4;
        private float horSpeedNoBoard = 2;
        private int jumpMax = 20;
        private int jumpLeft = 20;
        private int invincibilityFrames = 0;
        private bool invincible = false;
        private bool dead = false;
        private bool hasBoard = true;
        private bool kicking = false;
        private int invincibilityFramesMax = 120;
        private int playerStunFramesMax = 60;
        private int boardTouchTimer = 0;
        private int boardTouchTimerMax = 30;
        private float jumpIntensity = 4;
        private float floor = 0;
        private float verSpeed = 0;
        private float gravity = .3f;
        private Shadow shad;
        private SpriteFont font;

        public Player(MainGame game, Vector2 position, GameTime gameTime, float depth = 100) : base(game, position, game.AssetManager.GetSprite("SpriteSkater"), gameTime, depth)
        {
            font = game.AssetManager.GetFont("FontDogicaPixelBold");
            shad = new Shadow(game, position, gameTime, this);
            game.CreateEntity(shad);
            floor = position.Y;
            gravity = game.Settings.PlayerGravity;
            jumpMax = game.Settings.PlayerMaxJumpHoldFrames;
            jumpLeft = jumpMax;
            jumpIntensity = game.Settings.PlayerJumpSpeed;
            horSpeed = game.Settings.PlayerHorSpeed;
            horSpeedNoBoard = game.Settings.PlayerHorSpeedNoBoard;
            invincibilityFramesMax = game.Settings.PlayerInvincibilityFrames;
            playerStunFramesMax = game.Settings.PlayerStunFrames;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (boardTouchTimer > 0)
                boardTouchTimer--;
            if (dead)
            {
                deadUpdate(gameTime);
                return;
            }
            SetPos(GetPos().X + 2, GetPos().Y);
            if (!invincible || invincibilityFrames < playerStunFramesMax)
            {
                updateInput(gameTime);
                determineSprite(gameTime);
            }
            if (invincible)
            {
                //SetPos(GetPos().X - horSpeed / 2, GetPos().Y);
                SetVisible(((int)(gameTime.TotalGameTime.TotalSeconds * 10)) % 2 == 0);
                invincibilityFrames--;
                if (invincibilityFrames <= 0)
                {
                    invincible = false;
                }
            }
            if (jumpLeft < jumpMax)
                verSpeed += gravity;
            SetPos(GetPos().X, GetPos().Y + verSpeed);
            if (GetPos().Y >= floor)
            {
                SetPos(GetPos().X, floor);
                jumpLeft = jumpMax;
                if (verSpeed != 0)
                    game.AssetManager.GetSound("SoundLand").Play();
                verSpeed = 0;
                kicking = false;
            }
            if (GetPos().X < game.TrueCameraPos.X)
                SetPos(game.TrueCameraPos.X, GetPos().Y);
            if (GetPos().X + animation.Sprite.FrameWidth > game.TrueCameraPos.X + MainGame.GameWidth)
                SetPos(game.TrueCameraPos.X + MainGame.GameWidth - animation.Sprite.FrameWidth, GetPos().Y);
            checkCollisions(gameTime);
            if (!dead && game.Health <= 0)
            {
                dead = true;
                visible = true;
                verSpeed = -3;
                shad.Remove();
            }
        }

        public override void Draw(ExtendedSpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            string str = "Score: " + game.Points.ToString("D6");
            spriteBatch.DrawString(font, str, new Vector2((float)(game.TrueCameraPos.X + MainGame.GameWidth - (font.MeasureString(str).X)), 0), game.TextColor);
        }

        private void deadUpdate(GameTime gameTime)
        {
            SetAnimation("SpriteSkaterDie", gameTime);
            verSpeed += gravity;
            SetPos(GetPos().X, GetPos().Y + verSpeed);
            if (GetPos().Y >= 1000)
                game.StartRoom(2, gameTime);
        }

        private void determineSprite(GameTime gameTime)
        {
            SetVisible(true);
            if (GetPos().Y >= floor)
            {
                if (hasBoard)
                    SetAnimation("SpriteSkater", gameTime);
                else
                    SetAnimation("SpriteSkaterNoBoardWalk", gameTime);
            }
            else if (!kicking)
            {
                if (hasBoard)
                    SetAnimation("SpriteSkaterJump", gameTime);
                else
                    SetAnimation("SpriteSkaterNoBoardJump", gameTime);
            }
        }

        private void checkCollisions(GameTime gameTime)
        {
            if (game.Entities.Any(e => (e is Pumpkin || e is ToiletPaper) && IsCollidingWithEntity(e)))
                hit(gameTime);
            foreach (Entity entity in game.Entities.Where(e => e is Candy && IsCollidingWithEntity(e)))
            {
                entity.Remove();
                game.AddPoints(50);
                game.AssetManager.GetSound("SoundCollect").Play();
            }
            if (!hasBoard && boardTouchTimer <= 0)
                foreach (Entity entity in game.Entities.Where(e => e is Board && IsCollidingWithEntity(e)))
                {
                    entity.Remove();
                    hasBoard = true;
                }
        }

        private void updateInput(GameTime gameTime)
        {
            bool rightDown = Keyboard.GetState().IsKeyDown(Keys.D);
            bool leftDown = Keyboard.GetState().IsKeyDown(Keys.A);
            bool upDown = Keyboard.GetState().IsKeyDown(Keys.W);
            bool downDown = Keyboard.GetState().IsKeyDown(Keys.S);
            bool boardRightDown = Keyboard.GetState().IsKeyDown(Keys.Right);
            bool boardLeftDown = Keyboard.GetState().IsKeyDown(Keys.Left);
            bool boardUpDown = Keyboard.GetState().IsKeyDown(Keys.Up);
            bool boardDownDown = Keyboard.GetState().IsKeyDown(Keys.Down);
            if (leftDown)
                SetPos(GetPos().X - (hasBoard ? horSpeed : horSpeedNoBoard), GetPos().Y);
            if (rightDown)
                SetPos(GetPos().X + (hasBoard ? horSpeed : horSpeedNoBoard), GetPos().Y);
            if (!upDown && jumpLeft < jumpMax)
                jumpLeft = 0;
            if (jumpLeft > 0 && upDown)
            {
                if (jumpLeft == jumpMax)
                    game.AssetManager.GetSound("SoundJump").Play();
                verSpeed = -jumpIntensity;
                jumpLeft--;
            }
            if (jumpLeft < jumpMax && hasBoard && (boardLeftDown || boardRightDown || boardUpDown || boardDownDown))
            {
                hasBoard = false;
                kicking = true;
                boardTouchTimer = boardTouchTimerMax;
                game.AssetManager.GetSound("SoundKick").Play();
                if (boardLeftDown)
                {
                    SetAnimation("SpriteSkaterKick", gameTime, 3);
                    game.CreateEntity(new Board(game, GetPos() + new Vector2(-32, 0), gameTime, floor, 2));
                } else if (boardRightDown)
                {
                    SetAnimation("SpriteSkaterKick", gameTime, 1);
                    game.CreateEntity(new Board(game, GetPos() + new Vector2(32, 0), gameTime, floor, 3));
                } else if (boardUpDown)
                {
                    SetAnimation("SpriteSkaterKick", gameTime, 0);
                    game.CreateEntity(new Board(game, GetPos() + new Vector2(0, -32), gameTime, floor, 0));
                } else if (boardDownDown)
                {
                    SetAnimation("SpriteSkaterKick", gameTime, 2);
                    game.CreateEntity(new Board(game, GetPos() + new Vector2(0, 32), gameTime, floor, 1));
                }
            }
        }

        private void hit(GameTime gameTime)
        {
            if (invincible)
                return;
            game.AssetManager.GetSound("SoundHurt").Play();
            invincibilityFrames = invincibilityFramesMax;
            invincible = true;
            SetAnimation(new Animation(game.AssetManager.GetSprite("SpriteSkaterHurt"), gameTime));
        }
    }
}
