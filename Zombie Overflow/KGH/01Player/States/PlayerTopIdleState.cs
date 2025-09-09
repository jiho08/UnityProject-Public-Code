using Entities;
using Animations;
using Core;
using UnityEngine;

namespace Players.States
{
    public class PlayerTopIdleState : PlayerTopState
    {
        public PlayerTopIdleState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.PlayerInputSO.OnReloadPressed += HandleReloadPressed;
            player.PlayerInputSO.OnSkillPressed += HandleSkillPressed;
            player.PlayerInputSO.OnShootPressed += HandleShootPressed;
        }

        public override void Exit()
        {
            base.Exit();
            player.PlayerInputSO.OnReloadPressed -= HandleReloadPressed;
            player.PlayerInputSO.OnSkillPressed -= HandleSkillPressed;
            player.PlayerInputSO.OnShootPressed -= HandleShootPressed;
        }

        private void HandleSkillPressed(bool isPressed)
        {
            if (!isPressed) return;
            player.ChangeState("SKILL", stateType);
        }

        private void HandleReloadPressed()
        {
            if (!attackCompo.IsFullAmmo)
            {
                player.ChangeState("RELOAD", stateType);
            }
        }

        private void HandleShootPressed()
        {
            player.ChangeState("ATTACK", stateType);
        }
    }
}