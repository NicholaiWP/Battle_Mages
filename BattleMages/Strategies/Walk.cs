using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Walk : IStrategy
    {
        private Animator animator;
        private Transform transform;
        private float speed;

        public Walk(Animator animator, Transform transform, float speed)
        {
            this.animator = animator;
            this.transform = transform;
            this.speed = speed;
        }

        public void Execute(bool movingLeft, bool movingRight, bool movingUp, bool movingDown, FacingDirection fDirection)
        {
            float moveDist = GameWorld.Instance.DeltaTime * speed;

            Collider collider = transform.GameObject.GetComponent<Collider>();
            bool collisionLeft = collider.CheckCollisionAtPosition(transform.Position + new Vector2(-moveDist, 0));
            bool collisionRight = collider.CheckCollisionAtPosition(transform.Position + new Vector2(moveDist, 0));
            bool collisionUp = collider.CheckCollisionAtPosition(transform.Position + new Vector2(0, -moveDist));
            bool collisionDown = collider.CheckCollisionAtPosition(transform.Position + new Vector2(0, moveDist));

            Vector2 translation = Vector2.Zero;
            if (movingLeft && !collisionLeft)
                translation += new Vector2(-1, 0);
            if (movingRight && !collisionRight)
                translation += new Vector2(1, 0);
            if (movingUp && !collisionUp)
                translation += new Vector2(0, -1);
            if (movingDown && !collisionDown)
                translation += new Vector2(0, 1);

            transform.Translate(translation * moveDist);
        }
    }
}