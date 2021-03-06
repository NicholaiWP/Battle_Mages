﻿using System;
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
        private const float moveSpeed = 100;
        private const float moveAccel = 1000;
        private const float dashSpeed = 750;
        private const float dashAccel = 10000;
        private const float maxDashTime = 0.05f;
        private const float dashSpellCooldown = 0.8f;
        private const float invincibleTime = 1;
        private const float blinkTime = 0.1f;
        private const float ManaRechargeSpeed = 30;
        private const float ManaRechargeDelay = 1;

        public const int MaxHealth = 100;
        public const float MaxMana = 100;

        private Animator animator;
        private Character character;
        private SpriteRenderer spriteRenderer;
        private Transform transform;
        private Collider collider;
        private bool canUseSpells;
        private int selectedSpell;
        private int latestWalkIndex = -1;
        private float currentDashTime;
        private float dashCooldown;
        private Vector2 dashVec;
        private float[] cooldownTimers = new float[GameWorld.State.SpellBar.Count];
        private float[] cooldownTimersMax = new float[GameWorld.State.SpellBar.Count];
        private bool canMove;
        private float rechargeDelayTimer = 0;
        private float invincibleTimer;
        private float blinkTimer;
        private MouseState prevMouseState = Mouse.GetState();

        public int SelectedSpell { get { return selectedSpell; } }
        public int CurrentHealth { get; private set; } = MaxHealth;
        public float CurrentMana { get; private set; } = MaxMana;
        public bool Invincible { get { return invincibleTimer > 0f; } }

        public Player(bool canUseSpells)
        {
            for (int i = 0; i < cooldownTimers.Length; i++)
            {
                cooldownTimers[i] = 1;
            }

            canMove = true;
            currentDashTime = 0;
            dashCooldown = 0;
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
            animator.CreateAnimation("WalkRight", new Animation(priority: 2, framesCount: 25, yPos: 0, xStartFrame: 0,
                width: 32, height: 32, fps: 30, offset: Vector2.Zero));
            animator.CreateAnimation("WalkLeft", new Animation(priority: 2, framesCount: 25, yPos: 32, xStartFrame: 0,
                width: 32, height: 32, fps: 30, offset: Vector2.Zero));
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
            animator.CreateAnimation("Dash", new Animation(priority: 0, framesCount: 9, yPos: 416, xStartFrame: 0,
                width: 32, height: 32, fps: 20, offset: Vector2.Zero));
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

            //Dashing on right click
            if (GameWorld.Cursor.RightButtonPressed && dashCooldown <= 0)
            {
                dashVec = Vector2.Subtract(GameWorld.Cursor.Position, GameObject.Transform.Position);
                dashVec.Normalize();
                currentDashTime = maxDashTime;
                dashCooldown = 3;

                for (int i = 0; i < cooldownTimers.Length; i++)
                {
                    if (cooldownTimers[i] < dashSpellCooldown)
                    {
                        cooldownTimers[i] = cooldownTimersMax[i] = dashSpellCooldown;
                    }
                }
            }
            else if (dashCooldown >= 0)
            {
                dashCooldown -= GameWorld.DeltaTime;
            }

            SpellInfo spellToCast = GameWorld.State.GetSpellbarSpell(selectedSpell);
            if (spellToCast != null)
            {
                SpellStats stats = spellToCast.CalcStats();

                Vector2 targetPoint = GameWorld.Cursor.Position;
                float distToTargetPoint = Vector2.Distance(transform.Position, GameWorld.Cursor.Position);

                var baseRune = spellToCast.GetBaseRune();
                if (distToTargetPoint > stats.Range && !baseRune.CanUseOutsideRange)
                {
                    GameWorld.Cursor.SetCursor(CursorStyle.OutOfRange);
                }

                //Spellcasting
                if (canUseSpells
                    && GameWorld.Cursor.LeftButtonHeld
                    && cooldownTimers[selectedSpell] <= 0
                    && CurrentMana > 0
                    && (baseRune.CanUseOutsideRange || distToTargetPoint < stats.Range))
                {
                    //Create spell object and add it to the world
                    GameObject spellObject = new GameObject(transform.Position);
                    Spell spellComponent = baseRune.CreateSpell(
                        new SpellCreationParams(spellToCast, targetPoint, character.Velocity));
                    spellObject.AddComponent(spellComponent);
                    GameWorld.Scene.AddObject(spellObject);

                    CurrentMana -= stats.ManaCost;
                    cooldownTimers[selectedSpell] = cooldownTimersMax[selectedSpell] = stats.CooldownTime;
                    rechargeDelayTimer = ManaRechargeDelay;

                    Vector2 vecToTarget = targetPoint - GameObject.Transform.Position;
                    float angle = (float)Math.Atan2(vecToTarget.Y, vecToTarget.X);
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

            MouseState mState = Mouse.GetState();

            if (mState.ScrollWheelValue < prevMouseState.ScrollWheelValue)
            {
                selectedSpell++;
                if (selectedSpell > 3)
                    selectedSpell = 0;
            }
            if (mState.ScrollWheelValue > prevMouseState.ScrollWheelValue)
            {
                selectedSpell--;
                if (selectedSpell < 0)
                    selectedSpell = 3;
            }

            prevMouseState = mState;

            if (canMove)
            {
                //Movement
                Move(kbState);
            }
        }

        private void Move(KeyboardState kbState)
        {
            if (currentDashTime > 0)
            {
                Vector2 vecToMouse = GameWorld.Cursor.Position - GameObject.Transform.Position;
                float angleRadians = (float)Math.Atan2(vecToMouse.Y, vecToMouse.X);
                animator.PlayAnimation("Dash", angleRadians);
                currentDashTime -= GameWorld.DeltaTime;
                //GameObject.Transform.Translate(dashVec * dashSpeed * GameWorld.DeltaTime, collider, 25, 2);
                character.MoveDirection = dashVec;
                character.MoveSpeed = dashSpeed;
                character.MoveAccel = dashAccel;
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

                character.MoveDirection = movement;
                character.MoveSpeed = moveSpeed;
                character.MoveAccel = moveAccel;
            }
            character.Movement();
        }

        private void AnimationDone(AnimationDoneMsg msg)
        {
            canMove = true;
            animator.PlayAnimation("Idle" + character.FDirection.ToString());
        }

        public void TakeDamage(int points)
        {
            if (Invincible) return;

            CurrentHealth -= points;
            if (CurrentHealth <= 0)
            {
                canMove = false;
                canUseSpells = false;

                MediaPlayer.Stop();
                GameWorld.ChangeScene(new DeathScene(GameObject.Transform.Position));
                GameWorld.State.Save();
            }
            else
            {
                invincibleTimer = invincibleTime;
                spriteRenderer.Opacity = 0.5f;
            }
        }

        public void Heal(int healPoints)
        {
            CurrentHealth += healPoints;
            if(CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        public float GetCooldownTimer(int slot)
        {
            if (cooldownTimersMax[slot] == 0) return 0;
            return cooldownTimers[slot] / cooldownTimersMax[slot];
        }
    }
}