using Entities;
using Animations;

namespace FSM
{
    public abstract class EntityState
    {
        protected Entity entity;
        protected int animatorHash;
        protected EntityRenderer animator;
        protected EntityAnimationTrigger animatorTrigger;
        protected bool isTriggerCall;
        
        public EntityState(Entity entity, AnimParamSO animParam)
        {
            this.entity = entity;
            animatorHash = animParam.hashValue;
            animator = entity.GetCompo<EntityRenderer>(true);
            animatorTrigger = entity.GetCompo<EntityAnimationTrigger>(true);
        }

        public virtual void Enter()
        {
            animator.SetParam(animatorHash, true);
            isTriggerCall = false;
            animatorTrigger.OnAnimationEndTrigger += AnimationEndTrigger;
        }
        
        public virtual void Update(){}

        public virtual void Exit()
        {
            animator.SetParam(animatorHash, false);
            animatorTrigger.OnAnimationEndTrigger -= AnimationEndTrigger;
        }
        
        public virtual void AnimationEndTrigger() => isTriggerCall = true;

        public virtual void Destroy()
        {
        }
    }
}