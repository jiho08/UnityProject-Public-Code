using Entities;
using Animations;
using UnityEngine;

namespace Players.States
{
    public class PlayerBottomIdleState : PlayerBottomState
    {
        public PlayerBottomIdleState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            movement.SetMovementDirection(Vector2.zero);
        }

        public override void Update()
        {
            base.Update();
            var moveInput = player.PlayerInputSO.MoveInput;
            var moveDir = player.PlayerInputSO.GetDirectionBasedOnCamera(moveInput);
            movement.SetMovementDirection(moveDir);
            
            if (moveDir.magnitude >= inputThreshold)
            {
                player.ChangeState("MOVE", stateType);
            }
        }
    }
}