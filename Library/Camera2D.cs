﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Camera2D
    {
        private float zoom;
        private Texture2D sprite;
        private float rotation;
        private Vector2 pos;
        private Matrix viewMatrix;
        private Rectangle topRectangle;
        private Rectangle rightRectangle;
        private Rectangle bottomRectangle;
        private Rectangle leftRectangle;

        //Properties
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        /// <summary>
        /// This is the matrix which the spriteBatch draws through, so it is our camera so to say
        /// </summary>
        public Matrix GetViewMatrix
        {
            get
            {
                viewMatrix = Matrix.CreateTranslation(new Vector3(-pos, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(GameWorld.GetInstance.GetHalfViewPortWidth,
                    GameWorld.GetInstance.GetHalfViewPortHeight, 0));
                return viewMatrix;
            }
        }

        public Rectangle GetTopRectangle
        {
            get
            {
                topRectangle = new Rectangle((int)(pos.X - GameWorld.GetInstance.GetHalfViewPortWidth),
                    (int)(pos.Y - GameWorld.GetInstance.GetHalfViewPortHeight),
                    (int)GameWorld.GetInstance.GetHalfViewPortWidth * 2 - 2, 50);
                return topRectangle;
            }
        }

        public Rectangle GetRightRectangle
        {
            get
            {
                rightRectangle = new Rectangle((int)(pos.X + GameWorld.GetInstance.GetHalfViewPortWidth - 50),
                    (int)(pos.Y - GameWorld.GetInstance.GetHalfViewPortHeight),
                    50, (int)GameWorld.GetInstance.GetHalfViewPortHeight * 2 - 2);
                return rightRectangle;
            }
        }

        public Rectangle GetBottomRectangle
        {
            get
            {
                bottomRectangle = new Rectangle((int)(pos.X - GameWorld.GetInstance.GetHalfViewPortWidth),
                   (int)(pos.Y + GameWorld.GetInstance.GetHalfViewPortHeight - 52),
                   (int)GameWorld.GetInstance.GetHalfViewPortWidth * 2 - 2, 50);

                return bottomRectangle;
            }
        }

        public Rectangle GetLeftRectangle
        {
            get
            {
                leftRectangle = new Rectangle((int)(pos.X - GameWorld.GetInstance.GetHalfViewPortWidth),
                   (int)(pos.Y - GameWorld.GetInstance.GetHalfViewPortHeight),
                   50, (int)GameWorld.GetInstance.GetHalfViewPortHeight * 2 - 2);

                return leftRectangle;
            }
        }

        /// <summary>
        /// The constructer for the camera
        /// </summary>
        public Camera2D()
        {
            zoom = 1.0f;
            rotation = 0.0f;
            pos = Vector2.Zero;
        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Images/CollisionTexture");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
#if DEBUG

            #region topRectangle

            Rectangle topLine = new Rectangle(GetTopRectangle.X, GetTopRectangle.Y, GetTopRectangle.Width, 1);
            Rectangle bottomLine = new Rectangle(GetTopRectangle.X, GetTopRectangle.Y + GetTopRectangle.Height, GetTopRectangle.Width, 1);
            Rectangle rightLine = new Rectangle(GetTopRectangle.X + GetTopRectangle.Width, GetTopRectangle.Y, 1, GetTopRectangle.Height);
            Rectangle leftLine = new Rectangle(GetTopRectangle.X, GetTopRectangle.Y, 1, GetTopRectangle.Height);
            spriteBatch.Draw(texture: this.sprite, destinationRectangle: topLine, sourceRectangle: null, color: Color.Black, rotation: 0, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 1);
            spriteBatch.Draw(this.sprite, bottomLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, rightLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, leftLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);

            #endregion topRectangle

            #region rightRectangle

            Rectangle rightTopLine = new Rectangle(GetRightRectangle.X, GetRightRectangle.Y, GetRightRectangle.Width, 1);
            Rectangle rightBottomLine = new Rectangle(GetRightRectangle.X, GetRightRectangle.Y + GetRightRectangle.Height, GetRightRectangle.Width, 1);
            Rectangle rightRightLine = new Rectangle(GetRightRectangle.X + GetRightRectangle.Width, GetRightRectangle.Y, 1, GetRightRectangle.Height);
            Rectangle rightLeftLine = new Rectangle(GetRightRectangle.X, GetRightRectangle.Y, 1, GetRightRectangle.Height);
            spriteBatch.Draw(texture: this.sprite, destinationRectangle: rightTopLine, sourceRectangle: null, color: Color.Black, rotation: 0, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 1);
            spriteBatch.Draw(this.sprite, rightBottomLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, rightRightLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, rightLeftLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);

            #endregion rightRectangle

            Rectangle bottomTopLine = new Rectangle(GetBottomRectangle.X, GetBottomRectangle.Y, GetBottomRectangle.Width, 1);
            Rectangle bottomBottomLine = new Rectangle(GetBottomRectangle.X, GetBottomRectangle.Y + GetBottomRectangle.Height, GetBottomRectangle.Width, 1);
            Rectangle bottomRightLine = new Rectangle(GetBottomRectangle.X + GetBottomRectangle.Width, GetBottomRectangle.Y, 1, GetBottomRectangle.Height);
            Rectangle bottomLeftLine = new Rectangle(GetBottomRectangle.X, GetBottomRectangle.Y, 1, GetBottomRectangle.Height);
            spriteBatch.Draw(texture: this.sprite, destinationRectangle: bottomTopLine, sourceRectangle: null, color: Color.Black, rotation: 0, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 1);
            spriteBatch.Draw(this.sprite, bottomBottomLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, bottomRightLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, bottomLeftLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);

            //left
            Rectangle leftTopLine = new Rectangle(GetLeftRectangle.X, GetLeftRectangle.Y, GetLeftRectangle.Width, 1);
            Rectangle leftBottomLine = new Rectangle(GetLeftRectangle.X, GetLeftRectangle.Y + GetLeftRectangle.Height, GetLeftRectangle.Width, 1);
            Rectangle leftRightLine = new Rectangle(GetLeftRectangle.X + GetLeftRectangle.Width, GetLeftRectangle.Y, 1, GetLeftRectangle.Height);
            Rectangle leftLeftLine = new Rectangle(GetLeftRectangle.X, GetLeftRectangle.Y, 1, GetLeftRectangle.Height);
            spriteBatch.Draw(texture: this.sprite, destinationRectangle: leftTopLine, sourceRectangle: null, color: Color.Black, rotation: 0, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 1);
            spriteBatch.Draw(this.sprite, leftBottomLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, leftRightLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, leftLeftLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
#endif
        }
    }
}