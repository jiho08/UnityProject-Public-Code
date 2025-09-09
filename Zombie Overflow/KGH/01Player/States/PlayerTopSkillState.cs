using Animations;
using Combat.Skills;
using Core.GameEvent;
using Entities;
using UnityEngine;

namespace Players.States
{
    public class PlayerTopSkillState : PlayerTopState
    {
        private IReleasable _releasable;
        private Skill _activeSkill;
        private bool _isReleasable;
        
        public PlayerTopSkillState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            _activeSkill = skillComponent.activeSkill;
            if (!_activeSkill.AttemptUseSkill())
            {
                player.ChangeState("IDLE", stateType);
                return;
            }
            player.PlayerEventChannel.RaiseEvent(PlayerEvents.playerSkillGaugeEvent.AddGauge(-1));

            _releasable = _activeSkill as IReleasable;
            _isReleasable = _releasable != null;

            player.PlayerInputSO.OnSkillPressed += HandleSkillPressed;
            _activeSkill.OnSkillEnd += HandleSkillEnd;
        }


        public override void Exit()
        {
            player.PlayerInputSO.OnSkillPressed -= HandleSkillPressed;
            _activeSkill.OnSkillEnd -= HandleSkillEnd;
        }

        private void HandleSkillEnd()
        {
            player.ChangeState("IDLE", stateType);
        }

        private void HandleSkillPressed(bool isPressed)
        {
            if (!isPressed)
            {
                if (_isReleasable)
                {
                    _releasable.ReleaseSkill();
                }
            }
        }
    }
}