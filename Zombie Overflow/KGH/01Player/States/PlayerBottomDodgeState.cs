using Entities;
using Animations;
using Core;
using Entities.Stat;
using UnityEngine;

namespace Players.States
{
    public class PlayerBottomDodgeState : PlayerBottomState
    {
        private readonly RigController _rigController;
        private readonly float _defaultRigWeight;
        private readonly int _defaultLayer;
        private readonly float _defaultDodgeSpeed;

        private Vector3 _dodgeDirection;
        private float _dodgeSpeed;

        public PlayerBottomDodgeState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _rigController = entity.GetCompo<RigController>();
            _defaultRigWeight = _rigController.GetRigWeight();
            _defaultLayer = entity.gameObject.layer;

            if (entityStat.TryGetStat(player.DodgeSpeedStat, out var stat))
            {
                _defaultDodgeSpeed = stat.Value;
                _dodgeSpeed = _defaultDodgeSpeed;
                stat.OnValueChange += HandleDodgeSpeedValueChange;
            }
            else
            {
                UnityLogger.LogError($"Player {entity.name} does not have a DodgeSpeedStat.");
            }
        }

        public override void Enter()
        {
            base.Enter();
            splitRenderer.Dodge();
            PrepareForDodge();
        }

        public override void Exit()
        {
            base.Exit();
            ResetAfterDodge();
        }

        public override void Destroy()
        {
            base.Destroy();
            if (entityStat.TryGetStat(player.DodgeSpeedStat, out var stat))
            {
                stat.OnValueChange -= HandleDodgeSpeedValueChange;
            }
        }

        private void PrepareForDodge()
        {
            _rigController.SetRigWeight(0);
            player.gameObject.layer = player.DodgeLayerMask;

            var lastMoveInput = player.PlayerInputSO.LastMoveInput;
            _dodgeDirection = player.PlayerInputSO.GetDirectionBasedOnCamera(lastMoveInput) * _dodgeSpeed;

            movement.CanManualMovement = false;
            player.DoesLookAtMouse = false;
            movement.SetRotationDirection(_dodgeDirection);
            movement.SetAutoMovement(Vector3.zero);

            animatorTrigger.OnRollingStatusChange += HandleRollingStatusChange;
        }

        private void ResetAfterDodge()
        {
            _rigController.SetRigWeight(_defaultRigWeight);
            player.gameObject.layer = _defaultLayer;

            movement.CanManualMovement = true;
            player.DoesLookAtMouse = true;

            animatorTrigger.OnRollingStatusChange -= HandleRollingStatusChange;
        }

        private void HandleRollingStatusChange(bool isRolling)
        {
            if (isRolling)
            {
                movement.SetAutoMovement(_dodgeDirection);
                movement.SetColliderSize(1f, 1f);
            }
            else
            {
                movement.SetAutoMovement(Vector3.zero);
                player.ChangeState("IDLE", stateType);
                movement.ResetColliderSize();
            }
        }

        private void HandleDodgeSpeedValueChange(StatSO stat, float current, float previous)
        {
            _dodgeSpeed = current;
            animator.SetParam(player.DodgeSpeedParam.hashValue, _dodgeSpeed / _defaultDodgeSpeed);
        }

        protected override void HandleDodgeKeyPressed()
        {
        }
    }
}