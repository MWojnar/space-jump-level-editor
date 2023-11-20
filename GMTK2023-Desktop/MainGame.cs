using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace spaceJumpLevelEditor
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private ExtendedSpriteBatch _spriteBatch;
        public AssetManager AssetManager;
        private List<Entity> entities;
        public List<Entity> Entities { get { return entities; } }
        private List<Entity> entitiesToRemove;
        private List<Entity> entitiesToAdd;
        private Matrix viewMatrix;
        private Matrix scaleMatrix;
        private Vector2 mousePos;
        private int points;
        private int health;
        private float lastHouse = 0;
        private float startTime = 0;
        private GameTime gameTime;
        private Settings settings;
        private Phase curPhase;
        private List<Phase> phases;
        private float genIntensity = 1;
        private Random rand = new Random();
        private int curRoom = 0;

        public int Points { get { return points; } }
        public int Health { get { return health; } }
        public GameTime GameTime { get { return gameTime; } }
        public Settings Settings { get { return settings; } }
        public const int GameWidth = 960;
        public const int GameHeight = 540;
        public const int ViewScale = 2;
        public Color TextColor = new Color(200, 145, 62);
        public Vector2 CameraPos;
        public Vector2 TrueCameraPos { get {  return CameraPos / ViewScale; } }

        public Vector2 MousePos {
            get {
                mousePos.X = Mouse.GetState().X / viewMatrix.M11;
				mousePos.Y = Mouse.GetState().Y / viewMatrix.M22;
                return mousePos;
			}
        }

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GameWidth * ViewScale;
            _graphics.PreferredBackBufferHeight = GameHeight * ViewScale;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            scaleMatrix = Matrix.CreateScale(ViewScale, ViewScale, 1);
            viewMatrix = scaleMatrix;
            CameraPos = new Vector2();
            mousePos = new Vector2();
            entitiesToAdd = new List<Entity>();
            entitiesToRemove = new List<Entity>();
			using (var sr = new StreamReader("settings.json"))
			{
				string settingsData = sr.ReadToEnd();
				settings = JsonConvert.DeserializeObject<Settings>(settingsData);
			}
		}

        protected override void Initialize()
        {
            entities = new List<Entity>();
            points = 0;
            health = 3;
            lastHouse = 0;

			base.Initialize();
        }

        public void AddPoints(int amount)
        {
            points += amount;
        }

        public void RemoveHealth()
        {
            health--;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new ExtendedSpriteBatch(GraphicsDevice);
            AssetManager = new AssetManager(Content);
            AssetManager.Load();

			StartRoom(0, new GameTime());
		}

        public void StartRoom(int room, GameTime gameTime)
        {
            ClearEntities();
            CameraPos = new Vector2();
            health = 3;
            lastHouse = 0;
            startTime = (float)gameTime.TotalGameTime.TotalSeconds;
            CreateEntity(new Background(this, new Vector2(), AssetManager.GetSprite("BackgroundSky"), gameTime, .25f));
            CreateEntity(new Background(this, new Vector2(), AssetManager.GetSprite("BackgroundFence"), gameTime, .075f));
            CreateEntity(new Background(this, new Vector2(), AssetManager.GetSprite("BackgroundRoad"), gameTime, 0));
            if (room == 0)
            {
                CreateEntity(new TitleScreen(this, new Vector2(), gameTime));
            }
            else if (room == 1)
            {
                points = 0;
                phases = settings.Phases.OrderBy(e => e.TriggerAtThisManySeconds).ToList();
                if (phases.Count < 1)
                    phases.Add(new Phase());
                curPhase = phases.First();
                phases.Remove(curPhase);
                CreateEntity(new Player(this, new Vector2(100, 170), gameTime));
                initHouses();
            }
            else if (room == 2)
            {
				CreateEntity(new GameOver(this, new Vector2(), gameTime));
			}
            curRoom = room;
        }

        private void initHouses()
        {
            for (int i = 0; i < 208 * 4; i+= 208)
                CreateEntity(new House(this, new Vector2(i, 0), false, false, new GameTime()));
        }

        protected override void Update(GameTime gameTime)
		{
			this.gameTime = gameTime;

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (curRoom == 1)
            {
                CameraPos.X += 2 * ViewScale;
                updateGen(gameTime);
            }

            updateCamera();

            foreach (Entity entity in entities)
                if (!entitiesToRemove.Contains(entity))
                    entity.PreUpdate(gameTime);
            foreach (Entity entity in entities)
                if (!entitiesToRemove.Contains(entity))
                    entity.Update(gameTime);
            foreach (Entity entity in entities)
                if (!entitiesToRemove.Contains(entity))
                    entity.PostUpdate(gameTime);

            removeEntities();
            addEntities();

            base.Update(gameTime);
        }

        private void updateCamera()
        {
            Matrix translationMatrix = Matrix.CreateTranslation(-CameraPos.X, -CameraPos.Y, 0);
            viewMatrix = scaleMatrix * translationMatrix;
        }

        private void updateGen(GameTime gameTime)
        {
            if (!phases.Any())
                genIntensity += settings.FinalPhaseIntensityFactor;
            else
                if (phases.First().TriggerAtThisManySeconds < gameTime.TotalGameTime.TotalSeconds - startTime)
                {
                    curPhase = phases.First();
                    phases.Remove(curPhase);
                }
            if (rand.NextDouble() < curPhase.PumpkinGenFactor * genIntensity)
                CreateEntity(new Pumpkin(this, new Vector2(TrueCameraPos.X + GameWidth + 50, 170), gameTime));
            if (rand.NextDouble() < curPhase.TPGenFactor * genIntensity)
                CreateEntity(new ToiletPaper(this, new Vector2(TrueCameraPos.X + GameWidth + 50, 130 - (float)rand.NextDouble() * 100), gameTime));
            if (TrueCameraPos.X - lastHouse >= 208)
            {
                CreateEntity(new House(this, new Vector2(lastHouse + 208 * 4, 0), rand.NextDouble() < curPhase.HouseGenFactor * genIntensity, rand.NextDouble() < curPhase.HouseThrowDoubleFactor * genIntensity, gameTime));
                lastHouse += 208;
            }
        }

        private void addEntities()
        {
			foreach (Entity entity in entitiesToAdd)
				insertEntityByDepth(entity);
			entitiesToAdd.Clear();
		}

        private void removeEntities()
        {
			foreach (Entity entity in entitiesToRemove)
				entities.Remove(entity);
			entitiesToRemove.Clear();
		}

        public void CreateEntity(Entity entity)
        {
            if (!entities.Contains(entity) && !entitiesToAdd.Contains(entity))
                entitiesToAdd.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
			if (entities.Contains(entity) && !entitiesToRemove.Contains(entity))
				entitiesToRemove.Add(entity);
        }

        public void ClearEntities()
        {
            foreach (Entity entity in entities)
                RemoveEntity(entity);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

			_spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: viewMatrix);

			foreach (Entity entity in entities)
                entity.Draw(_spriteBatch, gameTime);

            if (curRoom == 1)
                for (int i = 1; i < 4; i++)
                    AssetManager.GetSprite("SpriteHealth").Draw(health >= i ? 0 : 1, _spriteBatch, new Vector2(TrueCameraPos.X + (i - 1) * 16, 0));

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void UpdateEntityDepth(Entity entity)
        {
            if (entities.Contains(entity))
            {
                int index = entities.IndexOf(entity);
                if (!(index - 1 < 0 || entities[index - 1].Depth <= entity.Depth) || !(index + 1 >= entities.Count || entities[index + 1].Depth >= entity.Depth))
                {
                    entities.Remove(entity);
                    insertEntityByDepth(entity);
                }
            }
        }

        private void insertEntityByDepth(Entity entity)
        {
            var lesserEntry = entities.LastOrDefault(e => e.Depth <= entity.Depth);
            if (lesserEntry == null)
                entities.Insert(0, entity);
            else
                entities.Insert(entities.IndexOf(lesserEntry) + 1, entity);
        }

        public void GameOver()
        {
            StartRoom(3, new GameTime());
        }
    }
}