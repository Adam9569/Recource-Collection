using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Recource_Collection.TileMap;

namespace Recource_Collection
{
    public class Game1 : Game
    {
        private Matrix _translation;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Texture2D heroTexture;
        private Hero _hero;
        private SpriteFont font;
        private WorldItems _worldItems;
        private Dictionary<string, Texture2D> Assets;
        NoiseGen noise;
        TileMap tileMap;




        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Globals.WindowSize = new(1600, 1600);
            _graphics.PreferredBackBufferWidth = Globals.WindowSize.X;
            _graphics.PreferredBackBufferHeight = Globals.WindowSize.Y;
            _graphics.ApplyChanges();
          

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
            
            noise = new NoiseGen(seed:1234);

            font = Content.Load<SpriteFont>("Font");
            _worldItems = new WorldItems(_spriteBatch);
            _worldItems.LoadContent(Content);

            Assets = new Dictionary<string, Texture2D>();
            Assets.Add("grass", Content.Load<Texture2D>("grass"));
            Assets.Add("rock", Content.Load<Texture2D>("rock"));
            Assets.Add("rock1", Content.Load<Texture2D>("rock1"));
            Assets.Add("rock2", Content.Load<Texture2D>("rock2"));
            Assets.Add("enemy", Content.Load<Texture2D>("enemy"));
            Assets.Add("bush1", Content.Load<Texture2D>("bush1"));
            Assets.Add("tree1", Content.Load<Texture2D>("tree1"));
            Assets.Add("tree2", Content.Load<Texture2D>("tree2"));
            Assets.Add("twig", Content.Load<Texture2D>("twig"));
            Assets.Add("water1", Content.Load<Texture2D>("water1"));
            Assets.Add("water2", Content.Load<Texture2D>("water2"));
            Assets.Add("water", Content.Load<Texture2D>("water"));
            Assets.Add("sand1", Content.Load<Texture2D>("sand"));

            var tileTextures = new Dictionary<TileType, Texture2D>
            {
                {TileType.grass, Assets["grass"]},
                {TileType.Rock1, Assets["rock1"]},
                {TileType.Rock2, Assets["rock2"]},
                {TileType.Tree1, Assets["tree1"]},
                {TileType.Tree2, Assets["tree2"]},
                {TileType.Water1, Assets["water1"]},
                {TileType.Water2, Assets["water2"]},
                {TileType.sand1, Assets["sand1"]},
                {TileType.bush1, Assets["bush1"] }
            };
            tileMap = new TileMap(tileTextures);

            heroTexture = Content.Load<Texture2D>("hero");
            _hero = new Hero(heroTexture, new Vector2(100, 100));
            WorldGen();
    }
        private void TranslationCalc()
        {
            _translation = Matrix.CreateTranslation(-_hero.Position.X + Globals.WindowSize.X / 2, -_hero.Position.Y + Globals.WindowSize.Y / 2, 0f);

        }
        void WorldGen()
        {
            for (int x = 0; x < TileMap.Width; x++)
            {
                for (int y = 0; y < TileMap.Height; y++)
                {
                    string pos = $"{x};{y}";

                    float n = noise.Sample(x, y);
                    n = MathF.Pow(n, 0.75f);
                    if (n < 0.10f)
                        tileMap.SetTile(pos, TileType.Water1);
                    else
                        tileMap.SetTile(pos, TileType.grass);
                }
            }
            for (int x = 1; x < TileMap.Width - 1; x++)
            {
                for (int y = 1; y < TileMap.Height - 1; y++)
                {
                    string pos = $"{x};{y}";

                    if (tileMap.GetTile(pos) == TileType.grass && nearWater(x, y))
                        tileMap.SetTile(pos, TileType.sand1);
                }
            }
            for (int x = 0; x < TileMap.Width; x++)
            {
                for (int y = 0; y < TileMap.Height; y++)
                {
                    string pos = $"{x};{y}";
                    if (tileMap.GetTile(pos) == TileType.grass)
                    {
                        float r = noise.Sample(x + 2000, y + 2000);
                        r = MathF.Pow(r, 0.6f);
                        if (r > 0.60f)
                            tileMap.SetTile(pos, TileType.Rock1);
                    }
                }
            }
            for (int x = 0; x < TileMap.Width; x++)
            {
                for (int y = 0; y < TileMap.Height; y++)
                {
                    string pos = $"{x};{y}";
                    if (tileMap.GetTile(pos) == TileType.grass)
                    {
                        float f = noise.Sample(x + 1000, y + 1000);
                        f = MathF.Pow(f, 0.65f);
                        if (f > 0.48f)
                            tileMap.SetTile(pos, TileType.Tree1);
                    }
                }
            }
        }

        bool nearWater(int x, int y)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (tileMap.GetTile($"{x + dx};{y + dy}") == TileType.Water1)
                        return true;
                }
            }
            return false;
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Globals.Update(gameTime);
            var keyboardState = Keyboard.GetState();
            _hero.Update();
            TranslationCalc();
            _worldItems.Update(_hero);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(transformMatrix: _translation);
            
            _worldItems.Draw();
            
            InputManager.Update();


            for (int x = 0; x < TileMap.Width; x++)
            {
                for (int y = 0; y < TileMap.Height; y++)
                {
                    string pos = $"{x};{y}";
                    TileType type = tileMap.GetTile(pos);

                    // get the texture for that tile type
                    Texture2D tex = tileMap.Assets[type];

                    _spriteBatch.Draw(
                        tex,
                        new Rectangle(x * TileMap.tilesize, y * TileMap.tilesize, TileMap.tilesize, TileMap.tilesize),
                        Color.White
                    );
                }
            }
            _hero.Draw();

            int i = 0;
            foreach (var item in _hero.Inventory)
            {
                _spriteBatch.DrawString(font, item.Key + ": " + item.Value, new Vector2(i * 70, 10), Color.Black);
                i++;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
