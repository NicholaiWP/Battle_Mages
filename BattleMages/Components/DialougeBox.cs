using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    internal class DialougeBox : Component
    {
        private Texture2D boxTexture;
        private Texture2D boxTexture_larger;
        private SpriteFont textFont;     
        private string text;
        private int charactersToDraw = 0;

        private float timer = 0;
        private const float charShowInterval = 0.03f;

        private GameObject closeBtnObj;

        public DialougeBox(string text)
        {
            this.text = text;

            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
            Listen<DestroyMsg>(Destroy);
        }

        private void Initialize(InitializeMsg msg)
        {
            boxTexture = GameWorld.Load<Texture2D>("Textures/UI/Ingame/DialougeBox");
            boxTexture_larger = GameWorld.Load<Texture2D>("Textures/UI/Ingame/DialougeBoxLarger");
            textFont = GameWorld.Load<SpriteFont>("FontBM");

            Texture2D closeBtnTex = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton");
            Texture2D closeBtnTex2 = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButton_Hover");
            if(GameWorld.Scene is IntroductionScene)
            {
                closeBtnObj = ObjectBuilder.BuildButton(new Vector2(60, -60), closeBtnTex, closeBtnTex2, () => { GameWorld.Scene.RemoveObject(GameObject); });
                GameWorld.Scene.AddObject(closeBtnObj);
            }
            else if (GameWorld.Scene is LobbyScene)
            {
                closeBtnObj = ObjectBuilder.BuildButton(new Vector2(-70, -90), closeBtnTex, closeBtnTex2, () => { GameWorld.Scene.RemoveObject(GameObject); });
                GameWorld.Scene.AddObject(closeBtnObj);
            }
        }

        private void Update(UpdateMsg msg)
        {
            if (charactersToDraw < text.Length)
            {
                timer -= GameWorld.DeltaTime;
                while (timer <= 0 && charactersToDraw < text.Length - 1)
                {
                    charactersToDraw++;
                    timer += charShowInterval;
                }
            }
        }

        private void Draw(DrawMsg msg)
        {
            if(GameWorld.Scene is IntroductionScene)
            {
                string warpedText = Utils.WarpText(text.Substring(0, charactersToDraw), boxTexture_larger.Width - 8, textFont);
                Vector2 pos = GameWorld.Camera.Position + new Vector2(-boxTexture_larger.Width / 2, GameWorld.GameHeight / 2 - boxTexture_larger.Height);
                msg.Drawer[DrawLayer.UI].Draw(boxTexture_larger, pos, Color.White);
                msg.Drawer[DrawLayer.UI].DrawString(textFont, warpedText, pos + new Vector2(4, 2), Color.White);               
            }
            else if(GameWorld.Scene is LobbyScene)
            {
                string warpedText = Utils.WarpText(text.Substring(0, charactersToDraw), boxTexture.Width - 8, textFont);
                Vector2 pos = GameWorld.Camera.Position + new Vector2(-boxTexture.Width / 2, GameWorld.GameHeight / 2 - boxTexture.Height);
                msg.Drawer[DrawLayer.UI].Draw(boxTexture, pos, Color.White);
                msg.Drawer[DrawLayer.UI].DrawString(textFont, warpedText, pos + new Vector2(4, 2), Color.White);

            }
        }
           
        private void Destroy(DestroyMsg msg)
        {
            GameWorld.Scene.RemoveObject(closeBtnObj);
        }
    }
}