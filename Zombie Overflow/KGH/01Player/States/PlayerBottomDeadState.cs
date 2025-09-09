using Animations;
using Entities;

namespace Players.States
{
    public class PlayerBottomDeadState : PlayerBottomState
    {
        private RigController _rigController;
        private float _rigWeight;
        
        public PlayerBottomDeadState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _rigController = entity.GetCompo<RigController>();
            _rigWeight = _rigController.GetRigWeight();
        }

        public override void Enter()
        {
            base.Enter();
            _rigController.SetRigWeight(0);
            player.gameObject.layer = player.DeadBodyLayer;
            movement.DoesAutoRotate = true;
            movement.StopImmediately();
        }

        protected override void HandleDodgeKeyPressed()
        {
        }
    }
}