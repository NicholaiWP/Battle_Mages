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
        private int currentHealth;
        private bool canUseSpells;
        private int selectedSpell;
        private KeyboardState oldKbState;
        private Color barColor;

        private float cooldownTimer;

        public int Health
        {
            get
            {
                return health;
            }

            set
            {
                health = value;
            }
        }

        public int CurrentHealth
        {
            get
            {
                return currentHealth;
            }

            set
            {
                currentHealth = value;
            }
        }

        public Player(GameObject gameObject, bool canUseSpells) : base(gameObject)
        {
            health = 100;
            this.canUseSpells = canUseSpells;
            currentHealth = health;
           
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
            if (cooldownTimer > 0)
            {
                cooldownTimer -= GameWorld.DeltaTime;
            }

            MouseState mState = Mouse.GetState();
            KeyboardState kbState = Keyboard.GetState();

            if (canUseSpells && mState.LeftButton == ButtonState.Pressed && cooldownTimer <= 0)
            {
                PlayerSpell spellToCast = GameWorld.State.SpellBook[GameWorld.State.SpellBar[selectedSpell]];

                //Fetch base spell and runes
                var baseSpell = spellToCast.GetSpell();
                RuneInfo[] runes = new RuneInfo[spellToCast.RuneCount];
                for (int i = 0; i < spellToCast.RuneCount; i++)
                {
                    runes[i] = spellToCast.GetRune(i);
                }

                //Create spell object and add it to the world
                GameWorld.CurrentScene.AddObject(
                    ObjectBuilder.BuildSpell(transform.Position, baseSpell, new SpellCreationParams(runes, GameWorld.Cursor.Position, character.Velocity), out cooldownTimer));
            }

            if (oldKbState.IsKeyUp(Keys.Tab) && kbState.IsKeyDown(Keys.Tab))
            {
                GameWorld.ChangeScene(new SpellbookScene(GameWorld.CurrentScene));
            }

            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Spell1)))
                selectedSpell = 0;
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Spell2)))
                selectedSpell = 1;
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Spell3)))
                selectedSpell = 2;
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Spell4)))
                selectedSpell = 3;

                Move(kbState);
        }

        private void Move(KeyboardState kbState)
        {
            Vector2 movement = new Vector2();
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Up)))
            {
                movement.Y -= 1;
            }
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Down)))
            {
                movement.Y += 1;
            }
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Left)))
            {
                movement.X -= 1;
            }
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Right)))
            {
                movement.X += 1;
            }
            character.MoveDirection = movement;

            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Down))
                || kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Up))
                || kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Left))
                || kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Right)))
            {
                GameWorld.SoundManager.PlaySound("WalkSound");
            }

            character.Movement();
            oldKbState = kbState;
        }

        public void OnAnimationDone(string animationsName)
        {
        }

        public void DealDamage(int points)
        {          
            currentHealth -= points;
            if (currentHealth <= 0)
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
                GameWorld.ChangeScene(new DeathScene());
            }
        }
    }
}