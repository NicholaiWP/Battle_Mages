using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Player : Component, ICanUpdate, ICanBeLoaded, ICanBeAnimated
    {
        private Animator animator;
        private Character character;
        private SpriteRenderer spriteRenderer;
        private Transform transform;
        private Collider collider;
        private int health;

        private float spellCooldownTimer;

        public Player(GameObject gameObject) : base(gameObject)
        {
            health = 100;
        }

        public void LoadContent(ContentManager content)
        {
            animator = GameObject.GetComponent<Animator>();
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            character = GameObject.GetComponent<Character>();
            transform = GameObject.Transform;
            collider = GameObject.GetComponent<Collider>();
            //TODO: Create animations here
        }

        public void Update()
        {
            if (spellCooldownTimer > 0)
            {
                spellCooldownTimer -= GameWorld.DeltaTime;
            }
            MouseState mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Pressed && spellCooldownTimer <= 0)
            {
                var spellInfo = StaticData.Spells.FirstOrDefault();
                var runeInfo = StaticData.Runes.FirstOrDefault();

                GameObject spellGo = new GameObject(transform.Position);
                Spell s = spellInfo.CreateSpell(spellGo, GameWorld.Cursor.Position, new RuneInfo[] { runeInfo });
                spellGo.AddComponent(s);
                GameWorld.CurrentScene.AddObject(spellGo);
                spellCooldownTimer = s.CooldownTime;
            }

            Move();

            if (health <= 0)
                GameWorld.CurrentScene.RemoveObject(GameObject);
        }

        

        private void OnCollision(Collider coll)
        {
            Enemy enemy = coll.GameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                if (enemy.IsAttacking)
                {
                    health -= enemy.Damage;
                }
            }
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
    }
}