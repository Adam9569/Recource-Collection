using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text.Json;
using System;
using System.Diagnostics;

namespace Recource_Collection
{
    internal class GameScene : Scene
    {
        private Hero _hero;
        private TileMap _tileMap;
        private WorldItems _worldItems;
        private NoiseGen _noise;
        private Texture2D heroTexture;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        private Matrix _camera;
        private SpriteFont _font;
        private KeyboardState previousState;
        private Texture2D pixel;
        private DayNightCycle _dayNight;


        private SpawningEnemies _spawning;
        private SpawningEnemies _spawningEvilRabbits;
        private Texture2D EvilRabbitTexture;
        private Texture2D GoblinTexture;

        private int _worldSeed = 6767;
        private int totalKills = 0;
        private const string SaveFilePath = "Saves/save1.json";

        public Boss _boss;
        Texture2D spriteSheet;

        List<Enemy> enemies = new List<Enemy>();

        public GameScene(ContentManager content)
        {
          
            _dayNight = new DayNightCycle();
            _font = content.Load<SpriteFont>("Font");
            _worldSeed = 6767;
            _noise = new NoiseGen(seed: _worldSeed);
            var tileTextures = new Dictionary<TileMap.TileType, Texture2D>
            {
                {TileMap.TileType.grass, content.Load<Texture2D>("grass")},
                {TileMap.TileType.Rock1, content.Load<Texture2D>("rock1")},
                {TileMap.TileType.Rock2, content.Load<Texture2D>("rock2")},
                {TileMap.TileType.Tree1, content.Load<Texture2D>("tree1")},
                {TileMap.TileType.Tree2, content.Load<Texture2D>("tree2")},
                {TileMap.TileType.Water1, content.Load<Texture2D>("water1")},
                {TileMap.TileType.Water2, content.Load<Texture2D>("water2")},
                {TileMap.TileType.sand1, content.Load<Texture2D>("sand")},
                {TileMap.TileType.bush1, content.Load<Texture2D>("bush1")},
            };
            _content = content;

            _tileMap = new TileMap(tileTextures);
            WorldGen.MapCreation(_tileMap, _noise);

            heroTexture = content.Load<Texture2D>(Globals.selectedHero);
            _hero = new Hero(100,100, heroTexture, new Vector2(TileMap.Width * TileMap.tilesize / 2f,TileMap.Height * TileMap.tilesize / 2f));
            Globals.hero = _hero;
            
            _worldItems = new WorldItems(Globals.SpriteBatch);
            _worldItems.LoadContent(content);

            GoblinTexture = content.Load<Texture2D>("FinalEnemy");
            EvilRabbitTexture = content.Load<Texture2D>("EvilRabbit");
            _spawning = new SpawningEnemies(GoblinTexture,EvilRabbitTexture);
            _spawning.Difficulty = Globals.SelectedDifficulty;

            spriteSheet = content.Load<Texture2D>("treeantSpriteSheet1");
            _boss = new Boss(500, spriteSheet, new Vector2(5000, 5000));
            _boss.LoadContent(content);
            _spawning.AddType(EnemyType.Goblin);
            _spawning.AddType(EnemyType.Rabbit);

            pixel = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            if (FileManager.FileExists("Saves/save1.json"))
            {
                LoadGame();
            }
            else
            {
                NewWorld();
            }
        }
        private bool LoadGame()
        {
            if (!FileManager.FileExists(SaveFilePath))
                return false;

            string json = FileManager.ReadData(SaveFilePath);
            StoringData save = JsonSerializer.Deserialize<StoringData>(json);
            if (save == null) return false;

            _worldSeed = save.WorldSeed;
            _noise = new NoiseGen(seed: _worldSeed);
            WorldGen.MapCreation(_tileMap, _noise);

            if (save.HeroX != 0 || save.HeroY != 0)
            {
                _hero.SetPosition(new Vector2(save.HeroX, save.HeroY));
            }
            int h = save.CurrentHunger <= 0 ? _hero.MaxHunger : save.CurrentHunger;
            int t = save.CurrentThirst <= 0 ? _hero.MaxThirst : save.CurrentThirst;

            _hero.SetStats(save.CurrentHealth, h, t);

            _hero.Inventory.Clear();
            if (save.Inventory != null)
            {
                foreach (var kv in save.Inventory)
                {
                    if (Enum.TryParse<Items>(kv.Key, out var item))
                        _hero.Inventory[item] = kv.Value;
                }
            }
            totalKills = save.EnemiesKilled;
            _hero.QuestionsCorrect = save.QuestionsCorrect;
            _hero.QuestionsIncorrect = save.QuestionsIncorrect;

            return true;
        }
        public override void OnSwitch()
        {
            Texture2D newTexture = _content.Load<Texture2D>(Globals.selectedHero);
            _hero.Texture = newTexture;
        }

        private void SaveGame()
        {
            StoringData save = new StoringData
            {
                WorldSeed = _worldSeed,
                HeroX = _hero.Position.X,
                HeroY = _hero.Position.Y,
                EnemiesKilled = totalKills,
                QuestionsCorrect = _hero.QuestionsCorrect,
                QuestionsIncorrect = _hero.QuestionsIncorrect,
                CurrentHealth = _hero.CurrentHealth,
                CurrentHunger = _hero.CurrentHunger,
                CurrentThirst = _hero.CurrentThirst,
                DayCount = _dayNight.DayCount,
                TimeOfDay = _dayNight.CurrentTime,

                Inventory = _hero.Inventory.ToDictionary(k => k.Key.ToString(), v => v.Value)
                
            };
            string json = JsonSerializer.Serialize(save, new JsonSerializerOptions { WriteIndented = true });
            FileManager.SaveData("Saves", "save1.json", json);

        }

        private void NewWorld()
        {
            _worldSeed = Random.Shared.Next(1, 10000);

            _noise = new NoiseGen(seed: _worldSeed);
            WorldGen.MapCreation(_tileMap, _noise);

            Vector2 spawn = new Vector2(TileMap.Width * TileMap.tilesize / 2f,TileMap.Height * TileMap.tilesize / 2f);
            _hero.SetPosition(spawn);

            totalKills = 0;
            _hero.QuestionsCorrect = 0;
            _hero.QuestionsIncorrect = 0;
            _hero.SetStats(_hero.MaxHealth, _hero.MaxHunger, _hero.MaxThirst);
            _hero.Inventory.Clear();
        }
        public override void Update()
        {
            _dayNight.Update((float)Globals.Time);

            if (_dayNight.JustTurnedNight)
                _spawning.OnNightStarted();

            if (_dayNight.JustTurnedDay)
                _spawning.OnDayStarted();

            _hero.Update(_tileMap);
            _worldItems.Update(_hero);
            OpenInventory();
            OpenMenu();
            _boss.Update(_hero);
            if (_boss.CurrentHealth != 0 && _boss.AnimationManager.current == AnimationManager.BossAnimations.smashAttack && _hero.HitBox.Intersects(_boss.Hitbox) && _hero.CurrentHealth > 0)
            {
                _hero.TakeDamage(_boss.Damage);
            }
            previousState = Keyboard.GetState();
            _spawning.Update(_tileMap, _hero,enemies);
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = enemies[i];

                enemy.Update(_tileMap, _hero);
                _hero.DealDamage(enemy);

                if (enemy.CurrentHealth <= 0)
                {

                    if (enemy is EvilRabbit)
                    {
                        _worldItems.SpawnItem(Items.RabbitHide,new Vector2(enemy.Position.X, enemy.Position.Y+20), new Vector2(48, 48));
                        _worldItems.SpawnItem(Items.RabbiFlesh,new Vector2(enemy.Position.X, enemy.Position.Y-10), new Vector2(48, 48));
                    }
                    totalKills++;
                    enemies.RemoveAt(i);
                    SceneManager.SwitchScene(SceneName.Question);
                    return;
                }
            }

            _camera = Matrix.CreateTranslation(-_hero.Position.X + Globals.WindowSize.X / 2,-_hero.Position.Y + Globals.WindowSize.Y / 2,0f);

                
        }
        public void OpenInventory()
        {
            if(Keyboard.GetState().IsKeyDown(Keys.P) && !previousState.IsKeyDown(Keys.P))
            {
                SceneManager.SwitchScene(SceneName.CraftingAndInv);
            }
        }
        public void OpenMenu()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !previousState.IsKeyDown(Keys.Escape))
            {
                SaveGame();
                SceneManager.SwitchScene(SceneName.MainMenu);
            }
        }

        public override void Draw()
        {
            _spriteBatch = Globals.SpriteBatch;
            _spriteBatch.Begin(transformMatrix: _camera);


            int startX = (int)((_hero.Position.X - Globals.WindowSize.X / 2) / TileMap.tilesize) - 10;
            int endX = (int)((_hero.Position.X + Globals.WindowSize.X / 2) / TileMap.tilesize) + 10;
            int startY = (int)((_hero.Position.Y - Globals.WindowSize.Y / 2) / TileMap.tilesize) - 10;
            int endY = (int)((_hero.Position.Y + Globals.WindowSize.Y / 2) / TileMap.tilesize) + 10;

            if (startX < 0) startX = 0;
            if (startY < 0) startY = 0;
            if (endX > TileMap.Width - 1) endX = TileMap.Width - 1;
            if (endY > TileMap.Height - 1) endY = TileMap.Height - 1;

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    _spriteBatch.Draw(_tileMap.Assets[_tileMap.GetTile($"{x};{y}")], new Rectangle(x * TileMap.tilesize,y * TileMap.tilesize,TileMap.tilesize,TileMap.tilesize),Color.White);
                }
            }

            if (_boss.CurrentHealth > 0)
            {
                _spriteBatch.DrawString(_font, "EVIL TREEANT HEALTH : " + _boss.CurrentHealth, new Vector2(_boss.Position.X - 150, _boss.Position.Y - 200), Color.Red);
                _hero.DealBossDamage(_boss);
                _boss.Draw(_spriteBatch);
            }
            _worldItems.Draw();
            _hero.Draw();
            for (int j = 0; j < enemies.Count; j++)
            {
                enemies[j].Draw();
            }

            _spriteBatch.Draw(pixel, _hero.HealthBar, Color.Red);
            _spriteBatch.Draw(pixel, _hero.HungerBar, Color.Brown * 0.9f);
            _spriteBatch.Draw(pixel, _hero.ThirstBar, Color.Purple * 0.9f);

            int i = 0;
            foreach (var item in _hero.Inventory)
            {
                _spriteBatch.DrawString(_font, $"{item.Key}: {item.Value}", new Vector2(i * 90, 10), Color.Black);
                i++;
            }
            if (_hero.IsAttacking)
            {
                _spriteBatch.Draw(pixel, _hero.AttackHitBox, Color.Red * 0.2f);
            }
            

            _spriteBatch.End();
        }
        
    }

}
