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

        private float maxDashTime;
        private float currentDashTime;
        private float dashSpeed;
        private float dashCooldown;
        private Vector2 dashVec;

        private float[] cooldownTimers = new float[SpellInfo.AttributeRuneSlotCount];
        private bool canMove;
        public const int MaxHealth = 100;
        public const float MaxMana = 100;
        private float rechargeDelayTimer = 0;
        private bool deathAnimationStarted;
        public const float ManaRechargeSpeed = 30;
        public const float ManaRechargeDelay = 1;

        private const float invincibleTime = 1;
        private float invincibleTimer;
        private const float blinkTime = 0.1f;
        private float blinkTimer;

        public int CurrentHealth { get; private set; } = MaxHealth;
        public float CurrentMana { get; private set; } = MaxMana;
        public bool Invincible { get { return invincibleTimer > 0f; } }

        private int latestWalkIndex = -1;

        public Player(bool canUseSpells)
        {
            canMove = true;
            maxDashTime = 0.12f;
            currentDashTime = 0;
            dashCooldown = 0;
            dashSpeed = 750;
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
            animator.CreateAnimation("WalkRight", new Animation(priority: 2, framesCount: 25, yPos: 0, xStartFrame: 0,
                width: 32, height: 32, fps: 25, offset: Vector2.Zero));

            animator.CreateAnimation("WalkLeft", new Animation(priority: 2, framesCount: 25, yPos: 32, xStartFrame: 0,
                width: 32, height: 32, fps: 25, offset: Vector2.Zero));

            animator.CreateAnimation("WalkDown", new Animation(priority: 2, framesCount: 14, yPos: 64, xStartFrame: 0,
                width: 32, height: 32, fps: 14, offset: Vector2.Zero));

            animator.CreateAnimation("WalkUp", new Animation(priority: 2, framesCount: 14, yPos: 96, xStartFrame: 0,
                width: 32, height: 32, fps: 14, offset: Vector2.Zero));

            animator.CreateAnimation("CastRight", new Animation(priority: 1, framesCount: 17, yPos: 128, xStartFrame: 0,
                width: 32, height: 32, fps: 50, offset: Vector2.Zero));

            animator.CreateAnimation("CastLeft", new Animation(priority: 1, framesCount: 17, yPos: 160, xStartFrame: 0,
                width: 32, height: 32, fps: 50, offset: Vector2.Zero));

            animator.CreateAnimation("CastDown", new Animation(priority: 1, framesCount: 17, yPos: 192, xStartFrame: 0,
                width: 32, height: 32, fps: 50, offset: Vector2.Zero));

            animator.CreateAnimation("CastUp", new Animation(priority: 1, framesCount: 16, yPos: 224, xStartFrame: 0,
                width: 32, height: 32, fps: 50, offset: Vector2.Zero));

            animator.CreateAnimation("IdleDown", new Animation(priority: 2, framesCount: 1, yPos: 64, xStartFrame: 0,
                width: 32, height: 32, fps: 1, offset: Vector2.Zero));

            animator.CreateAnimation("IdleLeft", new Animation(priority: 2, framesCount: 1, yPos: 32, xStartFrame: 0,
                width: 32, height: 32, fps: 1, offset: Vector2.Zero));

            animator.CreateAnimation("IdleRight", new Animation(priority: 2, framesCount: 1, yPos: 0, xStartFrame: 0,
                width: 32, height: 32, fps: 1, offset: Vector2.Zero));

            animator.CreateAnimation("IdleUp", new Animation(priority: 2, framesCount: 1, yPos: 96, xStartFrame: 0,
                width: 32, height: 32, fps: 1, offset: Vector2.Zero));

            animator.CreateAnimation("Death", new Animation(priority: 0, framesCount: 23, yPos: 384, xStartFrame: 0,
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

            //Invincibility timer
            if (invincibleTimer > 0)
            {
                blinkTimer -= GameWorld.DeltaTime;
                if (blinkTimer <= 0)
                {
                    if (spriteRenderer.Opacity < 1)
                        spriteRenderer.Opacity = 1;
                    else
                        spriteRenderer.Opacity = 0.5f;
                    blinkTimer += blinkTime;
                }

                invincibleTimer -= GameWorld.DeltaTime;
                if (invincibleTimer <= 0)
                {
                    spriteRenderer.Opacity = 1;
                }
            }

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
                canMove = false;
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
            if (GameWorld.Cursor.RightButtonPressed && dashCooldown <= 0)
            {
                dashVec = Vector2.Subtract(GameWorld.Cursor.Position, GameObject.Transform.Position);
                dashVec.Normalize();
                currentDashTime = maxDashTime;
                dashCooldown = 9;
            }
            else if (dashCooldown >= 0)
            {
                dashCooldown -= GameWorld.DeltaTime;
            }

            if (currentDashTime > 0)
            {
                currentDashTime -= GameWorld.DeltaTime;
                GameObject.Transform.Translate(dashVec * dashSpeed * GameWorld.DeltaTime);
            }
            else
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

                if (movement != Vector2.Zero)
                {
                    if (animator.PlayingAnimationName == "WalkLeft" || animator.PlayingAnimationName == "WalkRight")
                    {
                        if ((animator.CurrentIndex == 0 && latestWalkIndex != 0) || (animator.CurrentIndex == 12 && latestWalkIndex != 12))
                        {
                            GameWorld.SoundManager.PlaySound("WalkSound");
                            latestWalkIndex = animator.CurrentIndex;
                        }
                    }
                    if (animator.PlayingAnimationName == "WalkUp" || animator.PlayingAnimationName == "WalkDown")
                    {
                        if ((animator.CurrentIndex == 0 && latestWalkIndex != 0) || (animator.CurrentIndex == 4 && latestWalkIndex != 4) || (animator.CurrentIndex == 8 && latestWalkIndex != 8))
                        {
                            GameWorld.SoundManager.PlaySound("WalkSound");
                            latestWalkIndex = animator.CurrentIndex;
                        }
                    }
                }

                character.Movement();
            }
        }

        private void AnimationDone(AnimationDoneMsg msg)
        {
            canMove = true;
            animator.PlayAnimation("Idle" + character.FDirection.ToString());

            if (msg.AnimationName == "Death")
            {
                GameWorld.ChangeScene(new DeathScene());
            }
        }

        public void TakeDamage(int points)
        {
            if (Invincible || deathAnimationStarted) return;

            CurrentHealth -= points;
            if (CurrentHealth <= 0 && !deathAnimationStarted)
            {
                GameWorld.State.SpellBar.Clear();
                GameWorld.State.SpellBook.Clear();
                canMove = false;
                canUseSpells = false;
                deathAnimationStarted = true;
                animator.PlayAnimation("Death");
            }
            else
            {
                invincibleTimer = invincibleTime;
                spriteRenderer.Opacity = 0.5f;
            }
        }
    }
}