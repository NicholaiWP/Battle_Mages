using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battle_Mages
{
    public class Player : Component, ICanUpdate, ICanBeLoaded, IEnterCollision, IExitCollision, ICanBeAnimated,
        IStayOnCollision
    {
        private Animator animator;
        private Character character;
        private SpriteRenderer spriteRenderer;
        private Transform transform;

        public Player(GameObject gameObject) : base(gameObject)
        {
        }

        public void LoadContent(ContentManager content)
        {
            animator = GameObject.GetComponent<Animator>();
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            character = GameObject.GetComponent<Character>();
            transform = GameObject.Transform;

            //TODO: Create animations here
        }

        public void Update()
        {
            Move();
        }

        private void Move()
        {
            KeyboardState kbState = Keyboard.GetState();

            character.Up = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Up));
            character.Down = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Down));
            character.Left = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Left));
            character.Right = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Right));

            character.Movement();
        }

        public void OnAnimationDone(string animationsName)
        {
        }

        public void OnCollisionExit(Collider other)
        {
        }

        public void OnCollisionEnter(Collider other)
        {
        }

        public void OnCollisionStay(Collider other)
        {
        }
    }
}
