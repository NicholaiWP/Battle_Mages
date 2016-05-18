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
        private bool isMoving;
       
        public int Health { get { return health; } set { health = value; } }
        private int selectedSpell;

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
                PlayerSpell spellToCast = GameWorld.State.SpellBar[selectedSpell];

                //Fetch base spell and runes
                var baseSpell = spellToCast.GetSpell();
                RuneInfo[] runes = new RuneInfo[spellToCast.RuneCount];
                for (int i = 0; i < spellToCast.RuneCount; i++)
                {
                    runes[i] = spellToCast.GetRune(i);
                }

                //Create spell object and add it to the world
                GameObject spellGo = new GameObject(transform.Position);
                Spell s = baseSpell.CreateSpell(spellGo, new SpellCreationParams(runes,
                    GameWorld.Cursor.Position, character.Velocity));
                spellGo.AddComponent(s);
                GameWorld.CurrentScene.AddObject(spellGo);
                //Set cooldown
                spellCooldownTimer = s.CooldownTime;
            }

            Move();

            if (health <= 0)
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
                GameWorld.ChangeScene(new DeathScene());
            }
                              
        }

        private void Move()
        {
            KeyboardState kbState = Keyboard.GetState();
            character.Up = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Up));
            character.Down = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Down));
            character.Left = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Left));
            character.Right = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Right));

            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Down))
                || kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Up))
                || kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Left))
                || kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Right)))
            {
                GameWorld.SoundManager.PlaySound("walk");
            }

            character.Movement();
        }

        public void OnAnimationDone(string animationsName)
        {
        }
    }
}