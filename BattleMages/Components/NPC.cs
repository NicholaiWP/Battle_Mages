using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class NPC : Component
    {
        private Animator animator;
        private string imagePath;
        private int width;
        private int height;
        private int frames;
        private int fps;

        public NPC(string imagePath, Vector2 size, int frames, int fps)
        {
            this.imagePath = imagePath;
            width = (int)size.X;
            height = (int)size.Y;
            this.frames = frames;
            this.fps = fps;
            Listen<PreInitializeMsg>(Preinitialize);
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
        }

        private void Preinitialize(PreInitializeMsg message)
        {
            GameObject.AddComponent(new SpriteRenderer(imagePath, true)
            { Rectangle = new Rectangle(0, 0, width, height) });
        }

        private void Initialize(InitializeMsg message)
        {
            animator = GameObject.GetComponent<Animator>();
            animator.CreateAnimation("Idle", new Animation(priority: 0, framesCount: frames, yPos: 0, xStartFrame: 0, width: width,
                height: height, fps: fps, offset: Vector2.Zero));
        }

        private void Update(UpdateMsg message)
        {
            animator.PlayAnimation("Idle");
        }
    }
}