using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Button
    {
        //fields

        private int hoverNumber;
        private MenuButtons thisButton;
        private Texture2D[] sprites = new Texture2D[2];
        private Rectangle rectangle;
        private Vector2 position;

        /// <summary>
        /// Button class' constructor
        /// </summary>
        /// <param name="newSprite1"></param>
        /// <param name="graphics"></param>
        public Button(MenuButtons thisButton)
        {
            this.thisButton = thisButton;
            hoverNumber = 0;
        }

        public void LoadContent(ContentManager content)
        {
            switch (thisButton)
            {
                case MenuButtons.Play:
                    sprites[0] = content.Load<Texture2D>("Images/playButton");
                    sprites[1] = content.Load<Texture2D>("Images/playButtonHL");
                    position = new Vector2(-sprites[0].Width / 2, -sprites[0].Height * 1.5f);
                    break;

                case MenuButtons.Settings:
                    sprites[0] = content.Load<Texture2D>("Images/Settings");
                    sprites[1] = content.Load<Texture2D>("Images/Settings");
                    position = new Vector2(-sprites[0].Width / 2, 0);
                    break;

                case MenuButtons.Quit:
                    sprites[0] = content.Load<Texture2D>("Images/Quit");
                    sprites[1] = content.Load<Texture2D>("Images/Quit");
                    position = new Vector2(-sprites[0].Width / 2, sprites[0].Height * 1.5f);
                    break;

                case MenuButtons.ResUp:
                    sprites[0] = content.Load<Texture2D>("Images/800x600");
                    sprites[1] = content.Load<Texture2D>("Images/800x600");
                    position = new Vector2(150, -50);
                    break;

                case MenuButtons.ResDown:
                    sprites[0] = content.Load<Texture2D>("Images/800x600");
                    sprites[1] = content.Load<Texture2D>("Images/800x600");
                    position = new Vector2(-550, -50);
                    break;

                case MenuButtons.KeyBindUp:
                    sprites[0] = content.Load<Texture2D>("Images/1366x768");
                    sprites[1] = content.Load<Texture2D>("Images/1366x768");
                    position = new Vector2(-sprites[0].Width / 2, -250);
                    break;

                case MenuButtons.KeyBindLeft:
                    break;

                case MenuButtons.KeyBindDown:
                    break;

                case MenuButtons.KeyBindRight:
                    break;

                case MenuButtons.Back:
                    sprites[0] = content.Load<Texture2D>("Images/Back");
                    sprites[1] = content.Load<Texture2D>("Images/Back");
                    position = new Vector2(-sprites[0].Width / 2, sprites[0].Height * 3f);
                    break;
            }
        }

        public void Update()
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y,
                (sprites[hoverNumber].Width),
                (sprites[hoverNumber].Height));

            if (rectangle.Contains(GameWorld.Cursor.Position))
            {
                if (hoverNumber == 0)
                {
                    hoverNumber = 1;
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                    GameWorld.MenuScreenManager.MouseCanClickButton)
                {
                    GameWorld.MenuScreenManager.MouseCanClickButton = false;
                    ButtonIsClicked();
                }
            }
            else
            {
                hoverNumber = 0;
            }
        }

        private void ButtonIsClicked()
        {
            switch (thisButton)
            {
                case MenuButtons.Play:
                    GameWorld.SetState(GameState.InGame);
                    break;

                case MenuButtons.Settings:
                    GameWorld.SetState(GameState.Settings);
                    break;

                case MenuButtons.Quit:
                    GameWorld.SetState(GameState.Quit);
                    break;

                case MenuButtons.ResUp:
                    GameWorld.MenuScreenManager.ElementAtNumber++;
                    break;

                case MenuButtons.ResDown:
                    GameWorld.MenuScreenManager.ElementAtNumber--;
                    break;

                case MenuButtons.KeyBindUp:
                    GameWorld.MenuScreenManager.SwappingKeyBind = true;
                    GameWorld.MenuScreenManager.ChosenKeyToRebind = PlayerBind.Up;
                    break;

                case MenuButtons.KeyBindLeft:
                    break;

                case MenuButtons.KeyBindDown:
                    break;

                case MenuButtons.KeyBindRight:
                    break;

                case MenuButtons.Back:
                    GameWorld.SetState(GameState.MainMenu);
                    break;
            }
        }

        /// <summary>
        /// Draws the rectangle using a texture, rectangle and color
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprites[hoverNumber],
                destinationRectangle: rectangle,
                origin: Vector2.Zero,
                effects: SpriteEffects.None,
                color: Color.White);
        }
    }
}