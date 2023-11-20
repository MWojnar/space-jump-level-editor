using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
    public class AssetManager
    {
        private Dictionary<string, Sprite> sprites;
        private Dictionary<string, Song> music;
        private Dictionary<string, SoundEffect> sounds;
		private Dictionary<string, SpriteFont> fonts;
        private ContentManager contentManager;

        public AssetManager(ContentManager content)
        {
            sprites = new Dictionary<string, Sprite>();
			music = new Dictionary<string, Song>();
			sounds = new Dictionary<string, SoundEffect>();
			fonts = new Dictionary<string, SpriteFont>();
            contentManager = content;
        }

        public Sprite GetSprite(string assetName)
        {
            if (!sprites.TryGetValue(assetName, out var sprite))
            {
                sprite = new Sprite(contentManager.Load<Texture2D>(assetName));
                sprites.Add(assetName, sprite);
            }

            return sprite;
        }

		public Song GetMusic(string assetName)
		{
			if (!music.TryGetValue(assetName, out var song))
			{
				song = contentManager.Load<Song>(assetName);
				music.Add(assetName, song);
			}

			return song;
		}

		public SoundEffect GetSound(string assetName)
		{
			if (!sounds.TryGetValue(assetName, out var sound))
			{
				sound = contentManager.Load<SoundEffect>(assetName);
				sounds.Add(assetName, sound);
			}

			return sound;
		}

		public SpriteFont GetFont(string assetName)
		{
			if (!fonts.TryGetValue(assetName, out var font))
			{
				font = contentManager.Load<SpriteFont>(assetName);
				fonts.Add(assetName, font);
			}

			return font;
		}

		public void Load()
        {
            sprites.Clear();
            sprites.Add("SpriteHouseClosed", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_house_empty"), 1, 5));
            sprites.Add("SpriteHouseOpen", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_house_open"), 1, 5));
            sprites.Add("SpriteShadow", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_shadow"), 1, 5));
            sprites.Add("SpriteSkater", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_skater_strip2"), 2, 5));
            sprites.Add("SpriteCandyStart", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_candy_start_strip4"), 4, 5));
            sprites.Add("SpriteCandy", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_candy_strip4"), 4, 5));
            sprites.Add("SpriteCandyThrow", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_candy_throw_strip8"), 8, 5));
            sprites.Add("SpriteHealth", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_health_strip2"), 2, 0));
            sprites.Add("SpritePumpkin", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_jack_o_lantern"), 1, 5));
            sprites.Add("SpriteSkateboard", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_skateboard_strip2"), 2, 5));
            sprites.Add("SpriteSkateboardThrown", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_skateboard_thrown_strip4"), 4, 10));
            sprites.Add("SpriteSkaterJump", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_skater_jump_strip3"), 3, 5, new Vector2(-16, -16)));
            sprites.Add("SpriteToiletPaper", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_TP_strip2"), 2, 10));
            sprites.Add("SpriteSkaterDie", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_skater_die_strip4"), 4, 5));
            sprites.Add("SpriteSkaterHurt", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_skater_hurt_strip2"), 2, 5, new Vector2(-16, -16)));
            sprites.Add("SpriteSkaterKick", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_skater_kick_strip4"), 4, 0));
            sprites.Add("SpriteSkaterNoBoardJump", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_skater_noboard_jump"), 1, 5));
            sprites.Add("SpriteSkaterNoBoardWalk", new Sprite(contentManager.Load<Texture2D>("Sprites/spr_skater_noboard_walk_strip4"), 4, 5));

            sprites.Add("BackgroundFence", new Sprite(contentManager.Load<Texture2D>("Backgrounds/bg_fence"), 1, 5));
            sprites.Add("BackgroundRoad", new Sprite(contentManager.Load<Texture2D>("Backgrounds/bg_road"), 1, 5));
            sprites.Add("BackgroundSky", new Sprite(contentManager.Load<Texture2D>("Backgrounds/bg_sky"), 1, 5));
            sprites.Add("BackgroundGameOver", new Sprite(contentManager.Load<Texture2D>("Backgrounds/bg_gameover"), 1, 5));
            sprites.Add("BackgroundTitle", new Sprite(contentManager.Load<Texture2D>("Backgrounds/bg_title"), 1, 5));

            music.Clear();
			//music.Add("MusicMain", contentManager.Load<Song>("Music/msc_Invasion_final_mix"));

			sounds.Clear();
            sounds.Add("SoundCollect", contentManager.Load<SoundEffect>("Sounds/snd_candy_collect"));
            sounds.Add("SoundHurt", contentManager.Load<SoundEffect>("Sounds/snd_hurt"));
            sounds.Add("SoundJump", contentManager.Load<SoundEffect>("Sounds/snd_jump"));
            sounds.Add("SoundKick", contentManager.Load<SoundEffect>("Sounds/snd_kick"));
            sounds.Add("SoundLand", contentManager.Load<SoundEffect>("Sounds/snd_land"));
            sounds.Add("SoundLoss", contentManager.Load<SoundEffect>("Sounds/snd_candy_loss"));
            sounds.Add("SoundTPDestroy", contentManager.Load<SoundEffect>("Sounds/snd_TP_destroy"));
            /*sounds.Add("SoundBarrierDamage", contentManager.Load<SoundEffect>("Sounds/snd_barrier_damage"));
			sounds.Add("SoundExplosion", contentManager.Load<SoundEffect>("Sounds/snd_bomb_explosion"));
			sounds.Add("SoundEnemyDeath", contentManager.Load<SoundEffect>("Sounds/snd_enemy_death"));
			sounds.Add("SoundEnemyShoot", contentManager.Load<SoundEffect>("Sounds/snd_enemy_shot"));
			sounds.Add("SoundInvaderDeath", contentManager.Load<SoundEffect>("Sounds/snd_invader_death"));
			sounds.Add("SoundInvaderShoot", contentManager.Load<SoundEffect>("Sounds/snd_invader_shot"));*/

            fonts.Clear();
			fonts.Add("FontDogica", contentManager.Load<SpriteFont>("Fonts/dogica"));
			fonts.Add("FontDogicaBold", contentManager.Load<SpriteFont>("Fonts/dogicabold"));
			fonts.Add("FontDogicaPixel", contentManager.Load<SpriteFont>("Fonts/dogicapixel"));
			fonts.Add("FontDogicaPixelBold", contentManager.Load<SpriteFont>("Fonts/dogicapixelbold"));
		}
    }
}
