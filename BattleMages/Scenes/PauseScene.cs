using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class PauseScene : Scene
    {
        private Texture2D pauseTexture;
        private Vector2 position;

        private Scene oldScene;
        private Action onReturn;

        public PauseScene(Scene oldScene, Action onReturn = null)
        {
            this.oldScene = oldScene;
            this.onReturn = onReturn;

            pauseTexture = GameWorld.Load<Texture2D>("Textures/Backgrounds/Pause");

            //Continue button in pause screen
            var ContinueSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Menu/Continue");
            var ContinueSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Menu/Continue_Hover");
            AddObject(ObjectBuilder.BuildButton(
                GameWorld.Camera.Position + new Vector2(-ContinueSpr1.Width / 2, ContinueSpr1.Height * -1f),
                ContinueSpr1,
                ContinueSpr2,
                () => { onReturn?.Invoke(); GameWorld.ChangeScene(oldScene); }
                ));

            //Quit button for pause screen
            var QuitSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Menu/Quit");
            var QuitSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Menu/Quit_Hover");
            AddObject(ObjectBuilder.BuildButton(
                GameWorld.Camera.Position + new Vector2(-QuitSpr1.Width / 2, 0),
                QuitSpr1,
                QuitSpr2,
                () => { GameWorld.Instance.Exit(); }
                ));

            if (oldScene is GameScene)
            {
                //Forfeit button for pause screen
                var ForfeitSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Menu/Forfeit");
                var ForfeitSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Menu/Forfeit_Hover");
                AddObject(ObjectBuilder.BuildButton(
                    GameWorld.Camera.Position + new Vector2(-ForfeitSpr1.Width / 2, ForfeitSpr1.Height * 1f),
                    ForfeitSpr1,
                    ForfeitSpr2,
                    () => { GameWorld.ChangeScene(new LobbyScene()); }
                    ));
            }

            position = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2);
        }

        public override void Update()
        {
            if (GameWorld.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                onReturn?.Invoke();
                GameWorld.ChangeScene(oldScene);
            }
            base.Update();
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(pauseTexture, position, Color.White);

            base.Draw(drawer);
        }
    }
}