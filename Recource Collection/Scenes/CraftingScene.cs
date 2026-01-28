using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Recource_Collection
{
    public class CraftingScene : Scene
    {
        private readonly ContentManager _content;
        private readonly Hero _hero;

        private SpriteFont _font;
        private Texture2D _pixel;
        private List<Rectangle> invBoxes = new List<Rectangle>();
        private List<(Items item, int count)> invView = new List<(Items item, int count)>();
        private Rectangle[] craftSlots = new Rectangle[9];
        private int selectedInvNum = -1;
        private Items?[] craftItems = new Items?[9];
        private Items? selectedItem = null;

        private Rectangle CraftViewBox;
        private Items? CraftViewItem = null;

        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;

        public CraftingScene(ContentManager content, Hero hero)
        {
            _content = content;
            _hero = hero;

            _font = content.Load<SpriteFont>("Font");

            _pixel = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            Layout();
            RefreshInv();
        }

        
        public override void OnSwitch()
        {
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

            RefreshInv();
            Layout();
            Validate();
            UpdateOutput();
        }
        public void CloseInventory()
        {
            if (SceneManager.CurrentScene is CraftingScene)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.L) && previousKeyboardState.IsKeyDown(Keys.L))
                {
                    SceneManager.SwitchScene(SceneName.Game);
                }
            }
        }


        public override void Update()
        {
            var keyboardState = Keyboard.GetState();
            var mouse = Mouse.GetState();
            Point m = mouse.Position;
            CloseInventory();
            RefreshInv();
            UpdateOutput();

            if (keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
            {
                SceneManager.SwitchScene(SceneName.Game);
                previousKeyboardState = keyboardState;
                previousMouseState = mouse;
                return;
            }

            if (keyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E) && selectedItem.HasValue && _hero.CurrentHunger != _hero.MaxHunger)
            {
                Items item = selectedItem.Value;

                if (_hero.Inventory.TryGetValue(item, out int count) && count > 0 && CollectableItems.Food.ContainsKey(item))
                {
                    _hero.Eating(item);

                    if (!_hero.Inventory.TryGetValue(item, out count) || count <= 0)
                    {
                        selectedItem = null;
                    }
                }
            }
            if (keyboardState.IsKeyDown(Keys.E) &&!previousKeyboardState.IsKeyDown(Keys.E) &&selectedItem.HasValue && _hero.CurrentThirst != _hero.MaxThirst)
            {
                Items item = selectedItem.Value;

                if (_hero.Inventory.TryGetValue(item, out int count) &&count > 0 &&CollectableItems.Drink.ContainsKey(item))
                {
                    _hero.Drinking(item);

                    if (!_hero.Inventory.TryGetValue(item, out count) || count <= 0)
                    {
                        selectedItem = null;
                    }
                }
            }

            if (keyboardState.IsKeyDown(Keys.D0) && !previousKeyboardState.IsKeyDown(Keys.D0))
            {
                ClearCraftTable(returnItemsToInventory: true);
            }

            
            if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
            {
                CraftOutput();
            }

            if (mouse.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                SelectItem(m);
            }

            if (keyboardState.IsKeyDown(Keys.C) && !previousKeyboardState.IsKeyDown(Keys.C))
            {
                PlaceInCraftingTable();
            }

            if (keyboardState.IsKeyDown(Keys.Q) && !previousKeyboardState.IsKeyDown(Keys.Q))
            {
                DropSelectedFromInventory();
            }

            previousKeyboardState = keyboardState;
            previousMouseState = mouse;
        }

        public override void Draw()
        {
            var _spriteBatch = Globals.SpriteBatch;
            _spriteBatch.Begin();

            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, Globals.WindowSize.X, Globals.WindowSize.Y), Color.Black * 0.75f);
            _spriteBatch.DrawString(_font, "c = place in crafting table , q = drop item from inv", new Vector2(40, 90), Color.White);
            _spriteBatch.DrawString(_font, "Selected: " + (selectedItem?.ToString() ?? "None"), new Vector2(40, 120), Color.White);

            DrawInv(_spriteBatch);
            DrawCraftGrid(_spriteBatch);
            DrawOutputBox(_spriteBatch);

            _spriteBatch.End();
        }
        private void DrawOutputBox(SpriteBatch _spritebatch)
        {
            _spritebatch.DrawString(_font, "Output", new Vector2(CraftViewBox.X, CraftViewBox.Y - 30), Color.White);

            _spritebatch.Draw(_pixel, CraftViewBox, Color.Black);

            if (CraftViewItem.HasValue)
            {
                Texture2D icon = CollectableItems.itemTextures[CraftViewItem.Value];
                _spritebatch.Draw(icon, new Rectangle(CraftViewBox.X + 10, CraftViewBox.Y + 10, CraftViewBox.Width - 20, CraftViewBox.Height - 20), Color.White);
            }
        }



        private void Layout()
        {
            invBoxes.Clear();


            int gridLeft = craftSlots[0].X;
            int gridRight = craftSlots[2].Right;
            int gridBottom = craftSlots[8].Bottom;
            int outX = gridLeft + ((gridRight - gridLeft) / 2) - (90 / 2);

            CraftViewBox = new Rectangle(gridLeft + ((gridRight - gridLeft) / 2) - (90 / 2), gridBottom + 15, 90, 90);

            int rows = 12;
            for (int i = 0; i < rows; i++)
            {
                invBoxes.Add(new Rectangle(50, 180 + i * (50 + 10), 300, 50));
            }

            for (int i = 0; i < 9; i++)
            {
                craftSlots[i] = new Rectangle(Globals.WindowSize.X - 400 + i % 3 * (100),220 + i / 3 * (100),90,90);
            }
        }

        private void UpdateOutput()
        {
            int twigCount = 0;
            int pebbleCount = 0;
            int berryCount = 0;
            int RockCount = 0;
            int FleshCount = 0;

            for (int i = 0; i < craftItems.Length; i++)
            {
                if (!craftItems[i].HasValue) continue;

                switch (craftItems[i].Value)
                {
                    case Items.twigs:
                        twigCount++;
                        break;
                    case Items.pebble:
                        pebbleCount++;
                        break;
                    case Items.berry:
                        berryCount++;
                        break;
                    case Items.Rock:
                        RockCount++;
                        break;
                    case Items.RabbiFlesh:
                        FleshCount++;
                        break;

                }
            }


            if (twigCount == 2 && pebbleCount == 3)
            {
                CraftViewItem = Items.RockPickaxe;
                return;
            }
            if (twigCount == 5)
            {
                CraftViewItem = Items.TwigPickaxe;
                return;
            }
            if (berryCount == 8)
            {
                CraftViewItem = Items.BerryBundle;
                return;
            }
            if(pebbleCount == 4)
            {
                CraftViewItem = Items.Rock;
                return;
            }
            if(RockCount == 3 && twigCount == 2)
            {
                CraftViewItem = Items.RockPickaxe;
                return;
            }
            if(FleshCount ==1&& twigCount ==2)
            {
                CraftViewItem = Items.CookedRabbitFlesh;
                return;
            }
            CraftViewItem = null;
        }

        private void CraftOutput()
        {
            if (!CraftViewItem.HasValue)return;

            _hero.addToInv(CraftViewItem.Value);

            for (int i = 0; i < craftItems.Length; i++)
                craftItems[i] = null;

            RefreshInv();
            Validate();
            UpdateOutput();
        }
        private void DrawInv(SpriteBatch _spritebatch)
        {
            _spritebatch.DrawString(_font, "Inventory", new Vector2(invBoxes[0].X, invBoxes[0].Y - 30), Color.White);

            for (int i = 0; i < invBoxes.Count; i++)
            {
                Rectangle rectangle = invBoxes[i];
                bool selected = (i == selectedInvNum);

                _spritebatch.Draw(_pixel, rectangle, selected ? Color.Green : Color.Black * 0.6f);

                if (i < invView.Count)
                {
                    var (item, count) = invView[i];
                    Texture2D icon = CollectableItems.itemTextures[item];

                    _spritebatch.Draw(icon, new Rectangle(rectangle.X + 6, rectangle.Y + 6, 46, 46), Color.White);
                    _spritebatch.DrawString(_font, $"{item}  x{count}", new Vector2(rectangle.X + 62, rectangle.Y + 16), Color.White);
                }
            }
        }

        private void DrawCraftGrid(SpriteBatch _spritebatch)
        {
            _spritebatch.DrawString(_font, "Crafting Table", new Vector2(craftSlots[0].X, craftSlots[0].Y - 30), Color.White);

            for (int i = 0; i < 9; i++)
            {
                Rectangle rectangle = craftSlots[i];

                _spritebatch.Draw(_pixel, rectangle, Color.Black);
                _spritebatch.DrawString(_font, (i + 1).ToString(), new Vector2(rectangle.X + 6, rectangle.Y + 4), Color.Gray);

                if (craftItems[i].HasValue)
                {
                    Items it = craftItems[i].Value;
                    Texture2D icon = CollectableItems.itemTextures[it];
                    _spritebatch.Draw(icon, new Rectangle(rectangle.X + 18, rectangle.Y + 18, rectangle.Width - 36, rectangle.Height - 36), Color.White);
                }
            }
        }

        private void RefreshInv()
        {
            invView = _hero.Inventory.Where(keyValue => keyValue.Value > 0).OrderBy(kv => kv.Key.ToString()).Select(kv => (kv.Key, kv.Value)).ToList();
        }

        private void Validate()
        {
            if (!selectedItem.HasValue) return;

            Items item = selectedItem.Value;
            if (!_hero.Inventory.TryGetValue(item, out int count) || count <= 0)
            {
                selectedItem = null;
                selectedInvNum = -1;
            }
        }

        private void SelectItem(Point mousePos)
        {
            selectedInvNum = -1;
            selectedItem = null;

            for (int i = 0; i < invBoxes.Count; i++)
            {
                if (invBoxes[i].Contains(mousePos) && i < invView.Count)
                {
                    selectedInvNum = i;
                    selectedItem = invView[i].item;
                    break;
                }
            }
        }

        private void PlaceInCraftingTable()
        {
            if (!selectedItem.HasValue) return;

            Items item = selectedItem.Value;

            if (!_hero.Inventory.TryGetValue(item, out int count) || count <= 0) return;
            int slot = Array.FindIndex(craftItems, s => !s.HasValue);
            if (slot == -1) return;
            _hero.removeFromInv(item);
            craftItems[slot] = item;

            RefreshInv();
            Validate();
        }

        private void DropSelectedFromInventory()
        {
            if (!selectedItem.HasValue) return;
            Items item = selectedItem.Value;
            if (!_hero.Inventory.TryGetValue(item, out int count) || count <= 0) return;

            // spawn in world in lil bit
            _hero.removeFromInv(item);

            RefreshInv();
            Validate();
        }


        private void ClearCraftTable(bool returnItemsToInventory)
        {
            if (returnItemsToInventory)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (craftItems[i].HasValue)
                    {
                        _hero.addToInv(craftItems[i].Value);
                    }
                        
                }
            }

            for (int i = 0; i < 9; i++)
            {
                craftItems[i] = null;
            }
            RefreshInv();
            Validate();
        }

    }
}
