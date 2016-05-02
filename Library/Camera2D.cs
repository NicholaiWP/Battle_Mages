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
        private Rectangle topRectangle;
        private Rectangle rightRectangle;
        private Rectangle bottomRectangle;
        private Rectangle leftRectangle;

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
        /// This is the matrix which the spriteBatch draws through, so it is our camera so to say
        /// </summary>
        public Matrix ViewMatrix
        {
            get
            {
                viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(new Vector3(GameWorld.Instance.MenuScreenManager.ScalingVector, 1)) *
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
        /// Setting the topRectangle to be in the top corner of the screen, and moving it with 2% of the screen
        /// and setting height to 55
        /// </summary>
        public Rectangle TopRectangle
        {
            get
            {
                topRectangle = ViewCalculator.CalculateRectangle(new Vector2(-2, -2),
                    (int)GameWorld.Instance.HalfViewPortWidth * 2 + 2,
                    (int)GameWorld.Instance.HalfViewPortHeight / 10);
                return topRectangle;
            }
        }

        /// <summary>
        /// Setting the rightRectangle to be in the top corner of the screen, and moving it with 2% of the screen
        /// and setting width to 55
        /// </summary>
        public Rectangle RightRectangle
        {
            get
            {
                rightRectangle = ViewCalculator.CalculateRectangle(
                    new Vector2(GameWorld.Instance.HalfViewPortWidth * 2 -
                    (int)GameWorld.Instance.HalfViewPortWidth / 13, -2),
                    (int)GameWorld.Instance.HalfViewPortWidth / 12,
                    (int)GameWorld.Instance.HalfViewPortHeight * 2 + 2);
                return rightRectangle;
            }
        }

        /// <summary>
        /// Setting the bottomRectangle to be in the top corner of the screen, and moving it with 2% of the screen
        /// and setting height to 55
        /// </summary>
        public Rectangle BottomRectangle
        {
            get
            {
                bottomRectangle = ViewCalculator.CalculateRectangle(new Vector2(-2,
                    GameWorld.Instance.HalfViewPortHeight * 2 - (int)GameWorld.Instance.HalfViewPortHeight / 11),
                    (int)GameWorld.Instance.HalfViewPortWidth * 2 + 2,
                    (int)GameWorld.Instance.HalfViewPortHeight / 10);

                return bottomRectangle;
            }
        }

        /// <summary>
        /// Setting the leftRectangle to be in the top corner of the screen, and moving it with 2% of the screen
        /// and setting width to 55
        /// </summary>
        public Rectangle LeftRectangle
        {
            get
            {
                leftRectangle = ViewCalculator.CalculateRectangle(new Vector2(-2, -2),
                    (int)GameWorld.Instance.HalfViewPortWidth / 12,
                    (int)GameWorld.Instance.HalfViewPortHeight * 2 + 2);

                return leftRectangle;
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

        /// <summary>
        /// Drawing the rectangles while we debugg
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
#if DEBUG

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