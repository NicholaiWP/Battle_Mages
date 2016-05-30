using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
        private bool canMove;
        public const int MaxHealth = 100;
        public const float MaxMana = 100;
        private float rechargeDelayTimer = 0;
        private bool deathAnimationStarted;
        public const float ManaRechargeSpeed = 30;
        public const float ManaRechargeDelay = 1;

        public int CurrentHealth { get; private set; } = MaxHealth;
        public float CurrentMana { get; private set; } = MaxMana;

        public Player(bool canUseSpells)
        {
            canMove = true;
            this.canUseSpells = canUseSpells;
            deathAnimationStarted = false;
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
            animator.CreateAnimation("WalkRight", new Animation(framesCount: 25, yPos: 0, xStartFrame: 0,
                width: 32, height: 32, fps: 25, offset: Vector2.Zero));

            animator.CreateAnimation("WalkLeft", new Animation(framesCount: 25, yPos: 32, xStartFrame: 0,
                width: 32, height: 32, fps: 25, offset: Vector2.Zero));

            animator.CreateAnimation("WalkDown", new Animation(framesCount: 14, yPos: 64, xStartFrame: 0,
                width: 32, height: 32, fps: 14, offset: Vector2.Zero));

            animator.CreateAnimation("WalkUp", new Animation(framesCount: 14, yPos: 96, xStartFrame: 0,
                width: 32, height: 32, fps: 14, offset: Vector2.Zero));

            animator.CreateAnimation("CastRight", new Animation(framesCount: 17, yPos: 128, xStartFrame: 0,
                width: 32, height: 32, fps: 40, offset: Vector2.Zero));

            animator.CreateAnimation("CastLeft", new Animation(framesCount: 17, yPos: 160, xStartFrame: 0,
                width: 32, height: 32, fps: 40, offset: Vector2.Zero));

            animator.CreateAnimation("CastDown", new Animation(framesCount: 17, yPos: 192, xStartFrame: 0,
                width: 32, height: 32, fps: 40, offset: Vector2.Zero));

            animator.CreateAnimation("CastUp", new Animation(framesCount: 16, yPos: 224, xStartFrame: 0,
                width: 32, height: 32, fps: 40, offset: Vector2.Zero));

            animator.CreateAnimation("IdleDown", new Animation(framesCount: 1, yPos: 64, xStartFrame: 0,
                width: 32, height: 32, fps: 1, offset: Vector2.Zero));

            animator.CreateAnimation("IdleLeft", new Animation(framesCount: 1, yPos: 32, xStartFrame: 0,
                width: 32, height: 32, fps: 1, offset: Vector2.Zero));

            animator.CreateAnimation("IdleRight", new Animation(framesCount: 1, yPos: 0, xStartFrame: 0,
                width: 32, height: 32, fps: 1, offset: Vector2.Zero));

            animator.CreateAnimation("IdleUp", new Animation(framesCount: 1, yPos: 96, xStartFrame: 0,
                width: 32, height: 32, fps: 1, offset: Vector2.Zero));

            animator.CreateAnimation("Death", new Animation(framesCount: 23, yPos: 384, xStartFrame: 0,
                width: 32, height: 32, fps: 12, offset: Vector2.Zero));
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
            KeyboardState kbState = Keyboard.GetState();

            //Spellcasting
            if (canUseSpells && GameWorld.Cursor.LeftButtonHeld && cooldownTimers[selectedSpell] <= 0 && CurrentMana > 0)
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
                GameObject spellObject = new GameObject(transform.Position);
                Spell spellComponent = baseRune.CreateSpell(new SpellCreationParams(attrRunes, GameWorld.Cursor.Position, character.Velocity));
                spellObject.AddComponent(spellComponent);
                GameWorld.Scene.AddObject(spellObject);

                CurrentMana -= spellComponent.ManaCost;
                cooldownTimers[selectedSpell] = spellComponent.CooldownTime;
                rechargeDelayTimer = ManaRechargeDelay;

                Vector2 vecToMouse = GameWorld.Cursor.Position - GameObject.Transform.Position;
                float angle = (float)Math.Atan2(vecToMouse.Y, vecToMouse.X);
                float degrees = MathHelper.ToDegrees(angle);

                if (degrees >= -45 && degrees <= 45)
                {
                    animator.PlayAnimation("CastRight");
                    character.FDirection = FacingDirection.Right;
                }
                else if (degrees >= -135 && degrees <= -45)
                {
                    animator.PlayAnimation("CastUp");
                    character.FDirection = FacingDirection.Up;
                }
                else if (degrees >= 135 || degrees <= -135)
                {
                    animator.PlayAnimation("CastLeft");
                    character.FDirection = FacingDirection.Left;
                }
                else if (degrees >= 45 && degrees <= 135)
                {
                    animator.PlayAnimation("CastDown");
                    character.FDirection = FacingDirection.Down;
                }
            }

            //Spellbook opening
            if (oldKbState.IsKeyUp(Keys.Tab) && kbState.IsKeyDown(Keys.Tab))
            {
                GameWorld.ChangeScene(new SpellbookScene(GameWorld.Scene));
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

            if (canMove)
            {
                //Movement
                Move(kbState);
            }

            oldKbState = kbState;
        }

        private void Move(KeyboardState kbState)
        {
            Vector2 movement = new Vector2();
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Up)))
            {
                movement.Y -= 1;
                character.FDirection = FacingDirection.Up;
            }
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Down)))
            {
                movement.Y += 1;
                character.FDirection = FacingDirection.Down;
            }
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Left)))
            {
                movement.X -= 1;
                character.FDirection = FacingDirection.Left;
            }
            if (kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Right)))
            {
                movement.X += 1;
                character.FDirection = FacingDirection.Right;
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
            if (Utils.ContainsSubstring(msg.AnimationName, "Cast"))
            {
                animator.PlayAnimation("WalkRight");
            }
            if (msg.AnimationName == "Death")
            {
                GameWorld.ChangeScene(new DeathScene());
            }
        }

        public void TakeDamage(int points)
        {
            CurrentHealth -= points;
            if (CurrentHealth <= 0 && !deathAnimationStarted)
            {
                canMove = false;
                canUseSpells = false;
                deathAnimationStarted = true;
                animator.PlayAnimation("Death");
            }
        }
    }
}