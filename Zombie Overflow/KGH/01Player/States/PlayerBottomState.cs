using Core;
using Entities;
using Animations;
using Core.Define;
using UnityEngine;

namespace Players.States
{
    public abstract class PlayerBottomState : PlayerState
    {
        protected CharacterMovement movement;
        protected readonly float inputThreshold = 0.1f;
        protected Vector3 lookDirection;
        public PlayerBottomState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            movement = entity.GetCompo<CharacterMovement>(true);
            stateType = EnumDefine.StateType.Bottom;
        }

        public override void Enter()
        {
            base.Enter();
            player.PlayerInputSO.OnDodgePressed += HandleDodgeKeyPressed;
        }
        
        public override void Exit()
        {
            base.Exit();
            player.PlayerInputSO.OnDodgePressed -= HandleDodgeKeyPressed;
        }

        public override void Update()
        {
            base.Update();
            LookAtMouse();
        }

        protected void LookAtMouse()
        {
            if (!player.DoesLookAtMouse) return;
            lookDirection = player.PlayerInputSO.GetLookInput() - player.transform.position;
            lookDirection.y = 0;
            movement.SetRotationDirection(lookDirection);
        }
        
        protected virtual void HandleDodgeKeyPressed()
        {
            player.ChangeState("DODGE", stateType);
        }
    }
}