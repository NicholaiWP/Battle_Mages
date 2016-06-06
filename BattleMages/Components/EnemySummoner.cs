using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    internal class EnemySummoner : Component
    {
        private Animator animator;
        private Action onSummon;

        public EnemySummoner(Action onSummon)
        {
            this.onSummon = onSummon;

            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<AnimationDoneMsg>(AnimationDone);
        }

        private void Initialize(InitializeMsg msg)
        {
            animator = GameObject.GetComponent<Animator>();
        }

        private void Update(UpdateMsg msg)
        {
            animator.PlayAnimation("");

            if (animator.CurrentIndex >= 16)
            {
                onSummon?.Invoke();
                onSummon = null;
            }
        }

        private void AnimationDone(AnimationDoneMsg msg)
        {
            GameWorld.Scene.RemoveObject(GameObject);
        }
    }
}