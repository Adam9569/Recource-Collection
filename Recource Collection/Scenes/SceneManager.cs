using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Recource_Collection.Scenes;
using System;
using System.Collections.Generic;

namespace Recource_Collection
{
    public enum SceneName
    {
        MainMenu,
        Game,
        Inventory,
        CharacterSelection,
        Settings,
    }

    public static class SceneManager
    {
        private static SceneName CurrentSceneName = SceneName.MainMenu;
        private static SceneName? PreviousSceneName = null;

        public static Scene CurrentScene => Scenes[CurrentSceneName];

        private static Dictionary<SceneName, Scene> Scenes = new Dictionary<SceneName, Scene>();

        public static void LoadScenes(ContentManager Content)
        {
            Scenes.Clear();

            Scenes.Add(SceneName.Game, new GameScene(Content));
            Scenes.Add(SceneName.MainMenu, new MainMenuScene(Content));
            Scenes.Add(SceneName.Settings, new Settigns(Content));
            Scenes.Add(SceneName.CharacterSelection, new CharacterSelectionScene(Content));

            CurrentSceneName = SceneName.MainMenu;
            CurrentScene.OnSwitch();
        }

        public static void Update()
        {
            InputManager.Update();
            CurrentScene.Update();
        }

        public static void Draw()
        {
            CurrentScene.Draw();
        }

        public static void SwitchScene(SceneName newScene)
        {
            if (!Scenes.ContainsKey(newScene))
                throw new NotImplementedException();
            PreviousSceneName = CurrentSceneName;
            CurrentSceneName = newScene;
            CurrentScene.OnSwitch();
        }

        public static void BackScene()
        {
            if (PreviousSceneName == null) return;
            SwitchScene((SceneName)PreviousSceneName);
        }
    }

    public abstract class Scene
    {
        public abstract void OnSwitch();
        public abstract void Update();
        public abstract void Draw();
    }
}