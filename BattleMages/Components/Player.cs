﻿using Microsoft.Xna.Framework;
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
        private bool canUseSpells;
        private int selectedSpell;
        private KeyboardState oldKbState;

        private float spell1CooldownTimer;
        private float spell2CooldownTimer;

        public Player(GameObject gameObject, bool canUseSpells) : base(gameObject)
        {
            health = 100;
            this.canUseSpells = canUseSpells;
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
            if (spell1CooldownTimer > 0)
            {
                spell1CooldownTimer -= GameWorld.DeltaTime;
            }
            else if (spell2CooldownTimer > 0)
            {
                spell2CooldownTimer -= GameWorld.DeltaTime;
            }

            MouseState mState = Mouse.GetState();
            if (canUseSpells && mState.LeftButton == ButtonState.Pressed && spell1CooldownTimer <= 0)
            {
                selectedSpell = 0;
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
                spell1CooldownTimer = s.CooldownTime;
            }

            if (canUseSpells && mState.RightButton == ButtonState.Pressed && spell2CooldownTimer <= 0)
            {
                selectedSpell = 1;
                PlayerSpell spellToCast = GameWorld.State.SpellBar[selectedSpell];
                var baseSpell = spellToCast.GetSpell();
                RuneInfo[] runes = new RuneInfo[spellToCast.RuneCount];
                for (int i = 0; i < spellToCast.RuneCount; i++)
                {
                    runes[i] = spellToCast.GetRune(i);
                }

                //Create spell object and add it to the world
                GameObject iceShard1 = new GameObject(transform.Position);
                GameObject iceShard2 = new GameObject(transform.Position);
                GameObject iceShard3 = new GameObject(transform.Position);
                Spell s1 = baseSpell.CreateSpell(iceShard1, new SpellCreationParams(runes,
                    GameWorld.Cursor.Position, character.Velocity));

                Spell s2 = baseSpell.CreateSpell(iceShard2, new SpellCreationParams(runes,
                    Utils.RotationPos(GameWorld.Cursor.Position, 30), character.Velocity));

                Spell s3 = baseSpell.CreateSpell(iceShard3, new SpellCreationParams(runes,
                    Utils.RotationPos(GameWorld.Cursor.Position, -30), character.Velocity));

                iceShard1.AddComponent(s1);
                iceShard2.AddComponent(s2);
                iceShard3.AddComponent(s3);

                GameWorld.CurrentScene.AddObject(iceShard1);
                GameWorld.CurrentScene.AddObject(iceShard2);
                GameWorld.CurrentScene.AddObject(iceShard3);
                //Set cooldown
                spell2CooldownTimer = s1.CooldownTime;
            }

            Move();
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

            if (oldKbState.IsKeyUp(Keys.Tab) && kbState.IsKeyDown(Keys.Tab))
            {
                GameWorld.ChangeScene(new SpellbookScene(GameWorld.CurrentScene));
            }

            character.Movement();
            oldKbState = kbState;
        }

        public void OnAnimationDone(string animationsName)
        {
        }

        public void DealDamage(int points)
        {
            health -= points;
            if (health <= 0)
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
                //GameWorld.ChangeScene(new DeathScene());
            }
        }
    }
}