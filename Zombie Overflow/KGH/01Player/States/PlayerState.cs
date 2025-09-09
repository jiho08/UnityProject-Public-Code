using Core;
using Entities;
using Animations;
using Core.Define;
using Entities.Stat;
using FSM;
using UnityEngine;

namespace Players.States
{
    public abstract class PlayerState : EntityState
    {
        protected Player player;
        protected SplitEntityRenderer splitRenderer;
        protected CombineAnimationTrigger combineAnimationTrigger;
        protected EnumDefine.StateType stateType;
        
        protected EntityStat entityStat;
        public PlayerState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            player = entity as Player;
            splitRenderer = animator as SplitEntityRenderer;
            combineAnimationTrigger = animatorTrigger as CombineAnimationTrigger;
            entityStat = player.GetCompo<EntityStat>(true);
        }

        public override void Enter()
        {
            splitRenderer.SetParam(animatorHash, true, stateType);
            isTriggerCall = false;
            animatorTrigger.OnAnimationEndTrigger += AnimationEndTrigger;
        }

        public override void Exit()
        {
            splitRenderer.SetParam(animatorHash, false, stateType);
            animatorTrigger.OnAnimationEndTrigger -= AnimationEndTrigger;
        }
    }
}