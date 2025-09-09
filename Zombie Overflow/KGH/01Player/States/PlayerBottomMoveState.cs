using Entities;
using Animations;
using Core;
using UnityEngine;

namespace Players.States
{
    public class PlayerBottomMoveState : PlayerBottomState
    {
        public PlayerBottomMoveState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Update()
        {
            base.Update();
            var moveInput = player.PlayerInputSO.MoveInput;
            var moveDir = player.PlayerInputSO.GetDirectionBasedOnCamera(moveInput);
            movement.SetMovementDirection(moveDir);

            var zDir = Vector3.Dot(moveDir, lookDirection);
            var xDir = Vector3.Cross(moveDir, lookDirection).y;

            var dir = new Vector2(xDir, zDir).normalized;

            splitRenderer.SetMoveDir(dir);
            
            if (moveDir.magnitude < inputThreshold)
            {
                player.ChangeState("IDLE", stateType);
            }
        }
    }
}