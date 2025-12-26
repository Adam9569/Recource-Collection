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
            Globals.WindowSize = new(1024, 768);
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
            Assets.Add("bush", Content.Load<Texture2D>("bush"));
            Assets.Add("tree1", Content.Load<Texture2D>("tree1"));
            Assets.Add("tree2", Content.Load<Texture2D>("tree2"));
            Assets.Add("twig", Content.Load<Texture2D>("twig"));
            Assets.Add("water1", Content.Load<Texture2D>("water1"));
            Assets.Add("water2", Content.Load<Texture2D>("water2"));
            Assets.Add("water", Content.Load<Texture2D>("water"));
            var tileTextures = new Dictionary<TileType, Texture2D>
            {
                {TileType.grass, Assets["grass"]},
                {TileType.Rock1, Assets["rock1"]},
                {TileType.Rock2, Assets["rock2"]},
                {TileType.Tree1, Assets["tree1"]},
                {TileType.Tree2, Assets["tree2"]},
                {TileType.Water1, Assets["water1"]},
                {TileType.Water2, Assets["water2"]}
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
                    float n = noise.Sample(x, y);

                    if (n < 0.3f)
                        tileMap.SetTile($"{x};{y}", TileType.Water1);
                    else if (n < 0.45f)
                        tileMap.SetTile($"{x};{y}", TileType.Rock1);
                    else if (n < 0.7f)
                        tileMap.SetTile($"{x};{y}", TileType.grass);
                    else
                        tileMap.SetTile($"{x};{y}", TileType.Tree1);
                }
            }
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
