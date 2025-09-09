using Core;
using Entities;
using Animations;
using Combat;
using Combat.Skills;
using Core.Define;
using Entities.Stat;
using Players.Combat;

namespace Players.States
{
    public abstract class PlayerTopState : PlayerState
    {
        protected PlayerAttackComponent attackCompo;
        protected SkillComponent skillComponent;
        public PlayerTopState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            stateType = EnumDefine.StateType.Top;
            attackCompo = entity.GetCompo<PlayerAttackComponent>();
            skillComponent = entity.GetCompo<SkillComponent>(true);
            entityStat = entity.GetCompo<EntityStat>(true);
        }
    }
}