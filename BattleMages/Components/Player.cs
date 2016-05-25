using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Player : Component
    {
        private Animator animator;
        private Character character;
        private SpriteRenderer spriteRenderer;
        private Transform transform;
        private Collider collider;
        private bool canUseSpells;
        private int selectedSpell;
        private KeyboardState oldKbState;

        private float[] cooldownTimers = new float[SpellInfo.AttributeRuneSlotCount];

        public const int MaxHealth = 100;
        public const float MaxMana = 100;
        private float rechargeDelayTimer = 0;

        public const float ManaRechargeSpeed = 30;
        public const float ManaRechargeDelay = 1;

        public int CurrentHealth { get; private set; } = MaxHealth;
        public float CurrentMana { get; private set; } = MaxMana;

        public Player(GameObject gameObject, bool canUseSpells) : base(gameObject)
        {
            this.canUseSpells = canUseSpells;

            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<AnimationDoneMsg>(AnimationDone);
        }

        private void Initialize(InitializeMsg msg)
        {
            animator = GameObject.GetComponent<Animator>();
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            character = GameObject.GetComponent<Character>();
            transform = GameObject.Transform;
            collider = GameObject.GetComponent<Collider>();
            //TODO: Create animations here
        }

        private void Update(UpdateMsg msg)
        {
            //Timers
            for (int i = 0; i < cooldownTimers.Length; i++)
            {
                if (cooldownTimers[i] > 0)
                    cooldownTimers[i] -= GameWorld.DeltaTime;
            }

            rechargeDelayTimer = Math.Max(rechargeDelayTimer - GameWorld.DeltaTime / ManaRechargeDelay, 0);
            if (rechargeDelayTimer <= 0)
                CurrentMana = Math.Min(CurrentMana + GameWorld.DeltaTime * ManaRechargeSpeed, MaxMana);

            //Gather input
            MouseState mState = Mouse.GetState();
            KeyboardState kbState = Keyboard.GetState();

            //Spellcasting
            if (canUseSpells && mState.LeftButton == ButtonState.Pressed && cooldownTimers[selectedSpell] <= 0 && CurrentMana > 0)
            {
                SpellInfo spellToCast = GameWorld.State.SpellBook[GameWorld.State.SpellBar[selectedSpell]];

                //Fetch base spell and runes
                var baseRune = spellToCast.GetBaseRune();
                AttributeRune[] attrRunes = new AttributeRune[SpellInfo.AttributeRuneSlotCount];
                for (int i = 0; i < SpellInfo.AttributeRuneSlotCount; i++)
                {
                    attrRunes[i] = spellToCast.GetAttributeRune(i);
                }

                //Create spell object and add it to the world
                float manaCost;
                GameWorld.CurrentScene.AddObject(
                    ObjectBuilder.BuildSpell(transform.Position,
                    baseRune,
                    new SpellCreationParams(attrRunes, GameWorld.Cursor.Position, character.Velocity),
                    out cooldownTimers[selectedSpell],
                    out manaCost));
                CurrentMana -= manaCost;
                rechargeDelayTimer = ManaRechargeDelay;
            }

            //Spellbook opening
            if (oldKbState.IsKeyUp(Keys.Tab) && kbState.IsKeyDown(Keys.Tab))
            {
                GameWorld.ChangeScene(new SpellbookScene(GameWorld.CurrentScene));
            }

            //Spell selection
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Spell1)))
                selectedSpell = 0;
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Spell2)))
                selectedSpell = 1;
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Spell3)))
                selectedSpell = 2;
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Spell4)))
                selectedSpell = 3;

            //Movement
            Move(kbState);

            oldKbState = kbState;
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
        }

        private void AnimationDone(AnimationDoneMsg msg)
        {
        }

        public void TakeDamage(int points)
        {
            CurrentHealth -= points;
            if (CurrentHealth <= 0)
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
                GameWorld.ChangeScene(new DeathScene());
            }
        }
      
        }
    }
