using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BattleMages
{
    public class NPC : Component
    {
        private Animator animator;
        private string imagePath;
        private string animationName;
        private int width;
        private int height;
        private int frames;
        private int fps;
        private bool isDoor;
        private CursorLockToken token;
        private int ypos;
        private DrawLayer layer;

        public NPC(string imagePath, Vector2 size, int frames, int fps, bool isDoor = false, int ypos = 0, DrawLayer layer = DrawLayer.Gameplay)
        {
            this.ypos = ypos;
            this.imagePath = imagePath;
            width = (int)size.X;
            height = (int)size.Y;
            this.frames = frames;
            this.fps = fps;
            this.isDoor = isDoor;
            this.layer = layer;
            Listen<PreInitializeMsg>(Preinitialize);
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<AnimationDoneMsg>(AnimationDone);
        }

        private void Preinitialize(PreInitializeMsg message)
        {
            GameObject.AddComponent(new SpriteRenderer(imagePath, true, layer)
            { Rectangle = new Rectangle(0, 0, width, height) });
        }

        private void Initialize(InitializeMsg message)
        {
            animationName = "Idle";

            animator = GameObject.GetComponent<Animator>();
            animator.CreateAnimation(animationName, new Animation(priority: 0, framesCount: frames, yPos: ypos, xStartFrame: 0, width: width,
                height: height, fps: fps, offset: Vector2.Zero));

            if (isDoor)
            {
                foreach (string name in StaticData.challenges.Keys)
                {
                    animator.CreateAnimation(name, new Animation(priority: 0, framesCount: 47, yPos: ypos, xStartFrame: 0,
                        width: width, height: height, fps: fps, offset: Vector2.Zero));
                    ypos += height;
                }
            }
        }

        public void ChangeAnimation(string animationName)
        {
            if (animator.Animations.ContainsKey(animationName))
            {
                token = GameWorld.Cursor.Lock();
                this.animationName = animationName;
            }
        }

        private void Update(UpdateMsg message)
        {
            animator.PlayAnimation(animationName);
        }

        private void AnimationDone(AnimationDoneMsg message)
        {
            if (isDoor)
            {
                foreach (string name in StaticData.challenges.Keys)
                {
                    if (message.AnimationName == name)
                    {
                        token.Unlock();
                        GameWorld.ChangeScene(new HallwayScene(name));
                    }
                }
            }
        }
    }
}