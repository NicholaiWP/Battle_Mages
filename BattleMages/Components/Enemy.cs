using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public abstract class Enemy : Component
    {
        //Component caching
        private SpriteRenderer spriteRenderer;
        private Transform transform;
        private Collider collider;
        protected Animator animator;
        protected Character character;

        private int health;
        private int maxHealth;
        protected bool canMove;
        protected float attackRange;
        protected float targetingRange;
        protected int damage;
        protected int moneyAmount;
        protected float attackSpeed;
        protected float cooldownTimer;
        protected List<IBehaviour> behaviours = new List<IBehaviour>();
        private float red;

        private Texture2D healthBarForeground;
        private Texture2D healthBar;
        private float healthbarEnemySize = 1f;

        public int Damage { get { return damage; } }
        public int Health { get { return health; } }
        public int MaxHealth { get; protected set; } = 100;
        public bool IsAlreadyBurned { get; set; }
        public float CooldownTimer { get { return cooldownTimer; } }

        protected int MoneyAmount
        {
            get { return moneyAmount; }
            set { moneyAmount = value; }
        }

        protected float MoveSpeed
        {
            get { return character.MoveSpeed; }
            set { character.MoveSpeed = value; }
        }

        protected float MoveAccel
        {
            get { return character.MoveAccel; }
            set { character.MoveAccel = value; }
        }

        protected Enemy()
        {
            canMove = true;
            Listen<PreInitializeMsg>(PreInitialize);
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<AnimationDoneMsg>(AnimationDone);
            Listen<DrawMsg>(Draw);
        }

        public void TakeDamage(int points)
        {
            if (health > 0)
            {
                GameWorld.Scene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position, points.ToString()));
                health -= points;
                red = 1;

                if (health <= 0)
                {
                    health = 0;
                    //Spawns a number of coins when enemy dies
                    for (int i = 0; i < moneyAmount; i++)
                    {
                        GameWorld.Scene.AddObject(ObjectBuilder.BuildCoin(transform.Position));
                    }
                    canMove = false;
                    GameObject.RemoveComponent<Collider>();
                    animator.PlayAnimation("Death" + character.FDirection.ToString());
                }
            }
        }

        protected virtual void PreInitialize(PreInitializeMsg msg)
        {
        }

        protected virtual void Initialize(InitializeMsg msg)
        {
            health = MaxHealth;

            animator = GameObject.GetComponent<Animator>();
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            character = GameObject.GetComponent<Character>();
            transform = GameObject.Transform;
            collider = GameObject.GetComponent<Collider>();

            healthBarForeground = GameWorld.Load<Texture2D>("Textures/UI/Ingame/EnemyHealthBarForeground");
            healthBar = GameWorld.Load<Texture2D>("Textures/UI/Ingame/EnemyHealthBar");
        }

        protected virtual void Update(UpdateMsg msg)
        {
            if (canMove)
            {
                Move();
            }

            if (red > 0)
            {
                red -= GameWorld.DeltaTime * 10;
                spriteRenderer.Color = new Color(1f, 1f - red, 1f - red);
            }

            healthbarEnemySize = Math.Max(0, MathHelper.Lerp(healthbarEnemySize, Health / (float)MaxHealth, GameWorld.DeltaTime * 10f));
        }

        private void Move()
        {
            foreach (IBehaviour behaviour in behaviours)
            {
                behaviour.ExecuteBehaviour();
            }

            character.Movement();
            character.MoveDirection = Vector2.Zero;
        }

        private void AnimationDone(AnimationDoneMsg msg)
        {
            if (msg.AnimationName == "DeathRight" || msg.AnimationName == "DeathLeft")
            {
                GameObject.RemoveComponent<Animator>();
                GameObject.RemoveComponent<SpriteRenderer>();
                GameWorld.Scene.RemoveObject(GameObject);
            }

            animator.PlayAnimation("Idle" + character.FDirection);
        }

        private void Draw(DrawMsg msg)
        {
            //Draw healthbar
            if (health < MaxHealth && health > 0)
            {
                Vector2 healthbarPos = GameObject.Transform.Position - Utils.HalfTexSize(healthBarForeground) + new Vector2(0, -20);
                msg.Drawer[DrawLayer.AboveUI].Draw(healthBarForeground, position: healthbarPos);
                msg.Drawer[DrawLayer.UI].Draw(healthBar, position: healthbarPos, scale: new Vector2(healthbarEnemySize, 1));
            }
        }
    }
}