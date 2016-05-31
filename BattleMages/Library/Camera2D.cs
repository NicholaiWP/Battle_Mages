using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    public class Camera2D
    {
        private Vector2 position;
        private Matrix viewMatrix;
        private Matrix worldMatrix;

        //Latest position of the target if it's been set, defaults to 0,0
        private Vector2 latestTargetPosition = Vector2.Zero;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// The camera will position itself between this target and the mouse
        /// </summary>
        public Transform Target { get; set; }

        public bool AllowMovement { get; set; }

        /// <summary>
        /// This is the matrix which the spriteBatch draws through, so it is our camera so to say
        /// </summary>
        public Matrix ViewMatrix
        {
            get
            {
                viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0)) *
                    Matrix.CreateScale(new Vector3(GameWorld.Instance.ScalingVector, 1)) *
                    Matrix.CreateTranslation(new Vector3(GameWorld.Instance.HalfViewPortWidth, GameWorld.Instance.HalfViewPortHeight, 0));
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
            position = Vector2.Zero;
        }

        public void Update(float dt)
        {
            if (Target != null)
                latestTargetPosition = Target.Position;

            if (AllowMovement)
            {
                var camMovespeed = 250 * dt;

                Vector2 pos = (GameWorld.Cursor.Position + latestTargetPosition) / 2;
                position = pos;
            }
        }
    }
}