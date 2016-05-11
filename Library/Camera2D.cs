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
    public class Camera2D
    {
        //Fields
        private Texture2D sprite;

        private float rotation;
        private Vector2 position;
        private Matrix viewMatrix;
        private Matrix worldMatrix;

        //Latest position of the target if it's been set, defaults to 0,0
        private Vector2 latestTargetPosition = Vector2.Zero;

        //Properties
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// The camera will position itself between this target and the mouse
        /// </summary>
        public Transform Target { get; set; }

        /// <summary>
        /// This is the matrix which the spriteBatch draws through, so it is our camera so to say
        /// </summary>
        public Matrix ViewMatrix
        {
            get
            {
                viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(new Vector3(GameWorld.MenuScreenManager.ScalingVector, 1)) *
                    Matrix.CreateTranslation(new Vector3(GameWorld.Instance.HalfViewPortWidth,
                    GameWorld.Instance.HalfViewPortHeight, 0));
                return viewMatrix;
            }
        }

        public Matrix WorldMatrix
        {
            get
            {
                worldMatrix = Matrix.Invert(viewMatrix);
                return worldMatrix;
            }
        }

        /// <summary>
        /// The constructer for the camera
        /// </summary>
        public Camera2D()
        {
            rotation = 0.0f;
            position = Vector2.Zero;
        }

        /// <summary>
        /// Loading the content of the camera so we can draw in rectangles while debugging
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Images/CollisionTexture");
        }

        public void Update(float dt)
        {
            if (Target != null)
                latestTargetPosition = Target.Position;

            var camMovespeed = 250 * dt;

            GameWorld.Cursor.CursorPictureNumber = 0;

            Vector2 mousePos = GameWorld.Cursor.Position;

            Vector2 pos = (GameWorld.Cursor.Position + latestTargetPosition) / 2;
            position = pos;

            /*if (TopRectangle.Contains(mousePos) && RightRectangle.Contains(mousePos))
            {
                Position += new Vector2(camMovespeed, -camMovespeed);
            }
            else if (TopRectangle.Contains(mousePos) && LeftRectangle.Contains(mousePos))
            {
                Position -= new Vector2(camMovespeed, camMovespeed);
            }
            else if (BottomRectangle.Contains(mousePos) && LeftRectangle.Contains(mousePos))
            {
                Position += new Vector2(-camMovespeed, camMovespeed);
            }
            else if (BottomRectangle.Contains(mousePos) && RightRectangle.Contains(mousePos))
            {
                Position += new Vector2(camMovespeed, camMovespeed);
            }
            else if (TopRectangle.Contains(mousePos))
            {
                Position -= new Vector2(0, camMovespeed);
                GameWorld.Cursor.CursorPictureNumber = 1;
            }
            else if (BottomRectangle.Contains(mousePos))
            {
                Position += new Vector2(0, camMovespeed);
            }
            else if (RightRectangle.Contains(mousePos))
            {
                Position += new Vector2(camMovespeed, 0);
            }
            else if (LeftRectangle.Contains(mousePos))
            {
                Position -= new Vector2(camMovespeed, 0);
            }*/
        }

        /// <summary>
        /// Drawing the rectangles while we debugg
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
#if FALSE

            #region topRectangle

            Rectangle topLine = new Rectangle(TopRectangle.X, TopRectangle.Y, TopRectangle.Width, 1);
            Rectangle bottomLine = new Rectangle(TopRectangle.X, TopRectangle.Y + TopRectangle.Height, TopRectangle.Width, 1);
            Rectangle rightLine = new Rectangle(TopRectangle.X + TopRectangle.Width, TopRectangle.Y, 1, TopRectangle.Height);
            Rectangle leftLine = new Rectangle(TopRectangle.X, TopRectangle.Y, 1, TopRectangle.Height);
            spriteBatch.Draw(texture: this.sprite, destinationRectangle: topLine, sourceRectangle: null, color: Color.Black, rotation: 0, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 1);
            spriteBatch.Draw(this.sprite, bottomLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, rightLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, leftLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);

            #endregion topRectangle

            #region rightRectangle

            Rectangle rightTopLine = new Rectangle(RightRectangle.X, RightRectangle.Y, RightRectangle.Width, 1);
            Rectangle rightBottomLine = new Rectangle(RightRectangle.X, RightRectangle.Y + RightRectangle.Height, RightRectangle.Width, 1);
            Rectangle rightRightLine = new Rectangle(RightRectangle.X + RightRectangle.Width, RightRectangle.Y, 1, RightRectangle.Height);
            Rectangle rightLeftLine = new Rectangle(RightRectangle.X, RightRectangle.Y, 1, RightRectangle.Height);
            spriteBatch.Draw(texture: this.sprite, destinationRectangle: rightTopLine, sourceRectangle: null, color: Color.Black, rotation: 0, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 1);
            spriteBatch.Draw(this.sprite, rightBottomLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, rightRightLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, rightLeftLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);

            #endregion rightRectangle

            #region bottomRectangle

            Rectangle bottomTopLine = new Rectangle(BottomRectangle.X, BottomRectangle.Y, BottomRectangle.Width, 1);
            Rectangle bottomBottomLine = new Rectangle(BottomRectangle.X, BottomRectangle.Y + BottomRectangle.Height, BottomRectangle.Width, 1);
            Rectangle bottomRightLine = new Rectangle(BottomRectangle.X + BottomRectangle.Width, BottomRectangle.Y, 1, BottomRectangle.Height);
            Rectangle bottomLeftLine = new Rectangle(BottomRectangle.X, BottomRectangle.Y, 1, BottomRectangle.Height);
            spriteBatch.Draw(texture: this.sprite, destinationRectangle: bottomTopLine, sourceRectangle: null, color: Color.Black, rotation: 0, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 1);
            spriteBatch.Draw(this.sprite, bottomBottomLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, bottomRightLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, bottomLeftLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);

            #endregion bottomRectangle

            #region leftRectangle

            Rectangle leftTopLine = new Rectangle(LeftRectangle.X, LeftRectangle.Y, LeftRectangle.Width, 1);
            Rectangle leftBottomLine = new Rectangle(LeftRectangle.X, LeftRectangle.Y + LeftRectangle.Height, LeftRectangle.Width, 1);
            Rectangle leftRightLine = new Rectangle(LeftRectangle.X + LeftRectangle.Width, LeftRectangle.Y, 1, LeftRectangle.Height);
            Rectangle leftLeftLine = new Rectangle(LeftRectangle.X, LeftRectangle.Y, 1, LeftRectangle.Height);
            spriteBatch.Draw(texture: this.sprite, destinationRectangle: leftTopLine, sourceRectangle: null, color: Color.Black, rotation: 0, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 1);
            spriteBatch.Draw(this.sprite, leftBottomLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, leftRightLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(this.sprite, leftLeftLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);

            #endregion leftRectangle

#endif
        }
    }
}