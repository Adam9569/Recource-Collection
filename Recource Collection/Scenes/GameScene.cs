using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text.Json;

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
        
        private SpawningEnemies _spawning;
        private SpawningEnemies _spawningEvilRabbits;
        private Texture2D EvilRabbitTexture;
        private Texture2D GoblinTexture;

        public Boss _boss;
        Texture2D spriteSheet;

        List<Enemy> enemies = new List<Enemy>();


        private int totalKills = 0;



        public GameScene(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Font");
            _noise = new NoiseGen(seed: 6767);
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
            _hero = new Hero(100,heroTexture, new Vector2(6000, 6000));
            Globals.hero = _hero;

            _worldItems = new WorldItems(Globals.SpriteBatch);
            _worldItems.LoadContent(content);

            GoblinTexture = content.Load<Texture2D>("FinalEnemy");
            EvilRabbitTexture = content.Load<Texture2D>("EvilRabbit");
            _spawning = new SpawningEnemies(GoblinTexture,EvilRabbitTexture);
            _spawning.Difficulty = Difficulty.Hard;

            spriteSheet = content.Load<Texture2D>("treeantSpriteSheet1");
            _boss = new Boss(500, spriteSheet, new Vector2(6000, 6000));
            _boss.LoadContent(content);
            _spawning.AddType(EnemyType.Goblin);
            _spawning.AddType(EnemyType.Rabbit);

            pixel = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
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
                WorldSeed = 6767,
                HeroX = _hero.Position.X,
                HeroY = _hero.Position.Y,
                EnemiesKilled = totalKills,
                CurrentHealth = _hero.CurrentHealth,
                CurrentHunger = _hero.CurrentHunger,
                CurrentThirst = _hero.CurrentThirst,
                Inventory = _hero.Inventory.ToDictionary(i => i.Key.ToString(),i => i.Value)
            };
            string json = JsonSerializer.Serialize(save,new JsonSerializerOptions { WriteIndented = true });

            FileManager.SaveData("Saves", "save1.json", json);
        }
        public override void Update()
        {
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
            if(Keyboard.GetState().IsKeyDown(Keys.P) && previousState.IsKeyDown(Keys.P))
            {
                SceneManager.SwitchScene(SceneName.CraftingAndInv);
            }
        }
        public void OpenMenu()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && previousState.IsKeyDown(Keys.Escape))
            {
                SceneManager.SwitchScene(SceneName.MainMenu);
                SaveGame();
            }
        }

        public override void Draw()
        {
            _spriteBatch = Globals.SpriteBatch;

            _spriteBatch.Begin(transformMatrix: _camera);

            for (int x = 0; x < TileMap.Width; x++)
            {
                for (int y = 0; y < TileMap.Height; y++)
                {
                    string pos = $"{x};{y}";
                    var type = _tileMap.GetTile(pos);
                    var tex = _tileMap.Assets[type];

                    _spriteBatch.Draw(tex,new Rectangle(x * TileMap.tilesize, y * TileMap.tilesize, TileMap.tilesize, TileMap.tilesize),Color.White);
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
