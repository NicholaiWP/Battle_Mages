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
        private SpriteFont textFont;
        private string[] texts;
        private Action onDone;
        private int currentText = 0;
        private int charactersToDraw = 0;

        private float timer = 0;
        private const float charShowInterval = 0.035f;
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

            boxTexture = GameWorld.Load<Texture2D>("Textures/UI/Ingame/DialogueBox");
            textFont = GameWorld.Load<SpriteFont>("FontBM");
        }

        private void Update(UpdateMsg msg)
        {
            GameWorld.Cursor.SetCursor(CursorStyle.Dialouge);

            if (charactersToDraw < texts[currentText].Length)
            {
                timer -= GameWorld.DeltaTime;
                if (timer <= 0 && charactersToDraw < texts[currentText].Length)
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
                    charactersToDraw = 0;
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

            string warpedText = Utils.WarpText(texts[currentText].Substring(0, charactersToDraw), boxTexture.Width - 12, textFont);
            Vector2 pos = GameWorld.Camera.Position + new Vector2(-boxTexture.Width / 2, GameWorld.GameHeight / 2 - boxTexture.Height);
            msg.Drawer[DrawLayer.UI].Draw(boxTexture, pos, Color.White);
            msg.Drawer[DrawLayer.UI].DrawString(textFont, warpedText, pos + new Vector2(6, 6), Color.White);
        }
    }
}