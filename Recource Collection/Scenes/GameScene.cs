using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Security.Cryptography.X509Certificates;

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

        private List<Enemy> enemies = new List<Enemy>();
        private SpawningEnemies _spawningEnemies;
        private Texture2D enemyTexture;

        public GameScene(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Font");
            _noise = new NoiseGen(seed: 44321);
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
            _hero = new Hero(100,heroTexture, new Vector2(100, 100));
            Globals.hero = _hero;

            _worldItems = new WorldItems(Globals.SpriteBatch);
            _worldItems.LoadContent(content);

            enemyTexture = content.Load<Texture2D>("FinalEnemy");
            _spawningEnemies = new SpawningEnemies(enemyTexture);
            _spawningEnemies.Difficulty = Difficulty.Hard;

            pixel = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public override void OnSwitch()
        {
            Texture2D newTexture = _content.Load<Texture2D>(Globals.selectedHero);
            _hero.Texture = newTexture;
        }
        public override void Update()
        {
            _hero.Update(_tileMap);
            _worldItems.Update(_hero);
            previousState = Keyboard.GetState();
            OpenInventory();
            OpenMenu();
            _spawningEnemies.Update(_tileMap, _hero, enemies);
            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(_tileMap, _hero);
            }

            _camera = Matrix.CreateTranslation(
                -_hero.Position.X + Globals.WindowSize.X / 2,
                -_hero.Position.Y + Globals.WindowSize.Y / 2,
                0f
            );

            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                SceneManager.SwitchScene(SceneName.MainMenu);
            }
                
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
                _spriteBatch.Draw(pixel, _hero.AttackHitBox, Color.Red * 0.5f);
            }

            _spriteBatch.End();
        }
    }

}
