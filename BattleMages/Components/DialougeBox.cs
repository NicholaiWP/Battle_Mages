using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    internal class DialougeBox : Component
    {
        private Texture2D boxTexture;
        private Texture2D boxTexture_larger;
        private SpriteFont textFont;
        private string[] texts;
        private Action onDone;
        private int currentText = 0;
        private int charactersToDraw = 0;

        private float timer = 0;
        private const float charShowInterval = 0.04f;
        private const float punctationShowInterval = 0.05f;

        private CursorLockToken cursorLock;
        private MouseState prevState;

        public DialougeBox(string[] texts, Action onDone)
        {
            this.texts = texts;
            this.onDone = onDone;
            prevState = Mouse.GetState();

            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Initialize(InitializeMsg msg)
        {
            GameWorld.Camera.AllowMovement = false;
            cursorLock = GameWorld.Cursor.Lock();

            boxTexture = GameWorld.Load<Texture2D>("Textures/UI/Ingame/DialougeBox");
            boxTexture_larger = GameWorld.Load<Texture2D>("Textures/UI/Ingame/DialougeBoxLarger");
            textFont = GameWorld.Load<SpriteFont>("FontBM");
        }

        private void Update(UpdateMsg msg)
        {
            GameWorld.Cursor.SetCursor(CursorStyle.Dialouge);

            if (charactersToDraw < texts[currentText].Length)
            {
                timer -= GameWorld.DeltaTime;
                while (timer <= 0 && charactersToDraw < texts[currentText].Length - 1)
                {
                    charactersToDraw++;
                    timer += charShowInterval;
                    char latest = texts[currentText][charactersToDraw - 1];

                    if (char.IsPunctuation(latest))
                        timer += punctationShowInterval;

                    if (!char.IsWhiteSpace(latest))
                        GameWorld.SoundManager.PlaySound("DialougeSound");
                }
            }

            MouseState curState = Mouse.GetState();
            if (prevState.LeftButton == ButtonState.Released && curState.LeftButton == ButtonState.Pressed)
            {
                if (charactersToDraw < texts[currentText].Length)
                {
                    charactersToDraw = texts[currentText].Length;
                }
                else
                {
                    currentText++;
                    if (currentText >= texts.Length)
                    {
                        GameWorld.Camera.AllowMovement = true;
                        cursorLock.Unlock();
                        GameWorld.Scene.RemoveObject(GameObject);
                        onDone?.Invoke();
                    }
                }
            }
            prevState = curState;
        }

        private void Draw(DrawMsg msg)
        {
            if (currentText >= texts.Length) return;

            Texture2D textureToUse = boxTexture;
            if (GameWorld.Scene is IntroductionScene)
                textureToUse = boxTexture_larger;

            string warpedText = Utils.WarpText(texts[currentText].Substring(0, charactersToDraw), textureToUse.Width - 8, textFont);
            Vector2 pos = GameWorld.Camera.Position + new Vector2(-textureToUse.Width / 2, GameWorld.GameHeight / 2 - textureToUse.Height);
            msg.Drawer[DrawLayer.UI].Draw(textureToUse, pos, Color.White);
            msg.Drawer[DrawLayer.UI].DrawString(textFont, warpedText, pos + new Vector2(4, 2), Color.White);
        }
    }
}