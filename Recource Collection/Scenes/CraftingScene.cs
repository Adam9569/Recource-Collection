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
            RefreshInventoryView();
        }

        
        public override void OnSwitch()
        {
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

            RefreshInventoryView();
            Layout();
            ValidateSelection();
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
            RefreshInventoryView();
            UpdateOutput();

            bool escPressed = keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape);
            if (escPressed)
            {
                SceneManager.SwitchScene(SceneName.Game);
                previousKeyboardState = keyboardState;
                previousMouseState = mouse;
                return;
            }

            bool zeroPressed = keyboardState.IsKeyDown(Keys.D0) && !previousKeyboardState.IsKeyDown(Keys.D0);
            if (zeroPressed)
            {
                ClearCraftTable(returnItemsToInventory: true);
            }

            bool enterPressed = keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter);
            if (enterPressed)
            {
                CraftOutput();
            }
            bool leftClickPressed = mouse.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;

            if (leftClickPressed)
            {
                SelectItem(m);
            }

            bool cPressed = keyboardState.IsKeyDown(Keys.C) && !previousKeyboardState.IsKeyDown(Keys.C);
            bool qPressed = keyboardState.IsKeyDown(Keys.Q) && !previousKeyboardState.IsKeyDown(Keys.Q);

            if (cPressed)
            {
                PlaceInCraftingTable();
            }

            if (qPressed)
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
            _spriteBatch.DrawString(_font, "CRAFTING", new Vector2(40, 20), Color.White);
            _spriteBatch.DrawString(_font, "c = place in crafting table , q = drop item from inv", new Vector2(40, 90), Color.White);
            _spriteBatch.DrawString(_font, "Selected: " + (selectedItem?.ToString() ?? "None"), new Vector2(40, 120), Color.White);

            DrawInventoryPanel(_spriteBatch);
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

            int leftX = 40;
            int topY = 170;
            int boxW = 300;
            int boxH = 58;
            int gap = 8;

            int outSize = 90;
            int gap2 = 14;

            int gridLeft = craftSlots[0].X;
            int gridRight = craftSlots[2].Right;
            int gridBottom = craftSlots[8].Bottom;
            int outX = gridLeft + ((gridRight - gridLeft) / 2) - (outSize / 2);
            int outY = gridBottom + gap2;
            CraftViewBox = new Rectangle(outX, outY, outSize, outSize);

            int rows = 12;
            for (int i = 0; i < rows; i++)
                invBoxes.Add(new Rectangle(leftX, topY + i * (boxH + gap), boxW, boxH));

            int gridStartX = Globals.WindowSize.X - 420;
            int gridStartY = 220;
            int slotSize = 90;
            int slotGap = 10;

            for (int i = 0; i < 9; i++)
            {
                int gx = i % 3;
                int gy = i / 3;

                craftSlots[i] = new Rectangle(gridStartX + gx * (slotSize + slotGap),gridStartY + gy * (slotSize + slotGap),slotSize,slotSize);
            }
        }

        private void UpdateOutput()
        {
            int twigCount = 0;
            int pebbleCount = 0;
            int berryCount = 0;
            int RockCount = 0;

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

            CraftViewItem = null;
        }

        private void CraftOutput()
        {
            if (!CraftViewItem.HasValue)return;

            _hero.addToInv(CraftViewItem.Value);

            for (int i = 0; i < craftItems.Length; i++)
                craftItems[i] = null;

            RefreshInventoryView();
            ValidateSelection();
            UpdateOutput();
        }
        private void DrawInventoryPanel(SpriteBatch _spritebatch)
        {
            _spritebatch.DrawString(_font, "Inventory", new Vector2(invBoxes[0].X, invBoxes[0].Y - 30), Color.White);

            for (int i = 0; i < invBoxes.Count; i++)
            {
                Rectangle r = invBoxes[i];
                bool selected = (i == selectedInvNum);

                _spritebatch.Draw(_pixel, r, selected ? Color.Green : Color.Black * 0.6f);

                if (i < invView.Count)
                {
                    var (item, count) = invView[i];
                    Texture2D icon = CollectableItems.itemTextures[item];

                    _spritebatch.Draw(icon, new Rectangle(r.X + 6, r.Y + 6, 46, 46), Color.White);
                    _spritebatch.DrawString(_font, $"{item}  x{count}", new Vector2(r.X + 62, r.Y + 16), Color.White);
                }
            }
        }

        private void DrawCraftGrid(SpriteBatch _spritebatch)
        {
            _spritebatch.DrawString(_font, "Crafting Table", new Vector2(craftSlots[0].X, craftSlots[0].Y - 30), Color.White);

            for (int i = 0; i < 9; i++)
            {
                Rectangle r = craftSlots[i];

                _spritebatch.Draw(_pixel, r, Color.Black);
                _spritebatch.DrawString(_font, (i + 1).ToString(), new Vector2(r.X + 6, r.Y + 4), Color.Gray);

                if (craftItems[i].HasValue)
                {
                    Items it = craftItems[i].Value;
                    Texture2D icon = CollectableItems.itemTextures[it];
                    _spritebatch.Draw(icon, new Rectangle(r.X + 18, r.Y + 18, r.Width - 36, r.Height - 36), Color.White);
                }
            }
        }

        private void RefreshInventoryView()
        {
            invView = _hero.Inventory
                .Where(kv => kv.Value > 0)
                .OrderBy(kv => kv.Key.ToString())
                .Select(kv => (kv.Key, kv.Value))
                .ToList();
        }

        private void ValidateSelection()
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

            RefreshInventoryView();
            ValidateSelection();
        }

        private void DropSelectedFromInventory()
        {
            if (!selectedItem.HasValue) return;

            Items item = selectedItem.Value;

            if (!_hero.Inventory.TryGetValue(item, out int count) || count <= 0) return;

            // spawn in world in lil bit
            _hero.removeFromInv(item);

            RefreshInventoryView();
            ValidateSelection();
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
                

            RefreshInventoryView();
            ValidateSelection();
        }

    }
}
