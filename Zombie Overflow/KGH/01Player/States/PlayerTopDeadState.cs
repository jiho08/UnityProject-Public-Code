using Animations;
using Core;
using Entities;

namespace Players.States
{
    public class PlayerTopDeadState : PlayerTopState
    {
        public PlayerTopDeadState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            skillComponent.CancelReleases();
        }
    }
}