using Animations;
using Core.GameEvent;
using Entities;
using Entities.Stat;
using UnityEngine;

namespace Players.States
{
    public class PlayerTopReloadState : PlayerTopState
    {
        private StatSO _moveSpeedStat;
        
        private float _defaultReloadSpeed;
        private float _reloadSpeed;

        private bool _reloadTrigger;

        public PlayerTopReloadState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            if (entityStat.TryGetStat(player.ReloadSpeedStat, out var stat))
            {
                _defaultReloadSpeed = stat.Value;
                _reloadSpeed = _defaultReloadSpeed;
                stat.OnValueChange += HandleReloadSpeedValueChange;
            }
            else
            {
                Debug.LogError($"Player {entity.name} does not have a ReloadSpeedStat.");
            }
            
            _moveSpeedStat = entityStat.GetStat(player.MoveSpeedStat);
        }

        public override void Destroy()
        {
            base.Destroy();
            if (entityStat.TryGetStat(player.ReloadSpeedStat, out var stat))
            {
                stat.OnValueChange -= HandleReloadSpeedValueChange;
            }
        }
        
        public override void Enter()
        {
            base.Enter();
            _reloadTrigger = false;
            combineAnimationTrigger.OnReloadTrigger += HandleReloadTrigger;
            player.PlayerInputSO.OnShootPressed += HandleShootPressed;
            player.PlayerInputSO.OnDodgePressed += HandleDodgePressed;

            _moveSpeedStat.AddModifier(this, -2.5f);
            player.PlayerEventChannel.RaiseEvent(PlayerEvents.playerReloadStatusEvent.Initialize(true));
        }
        
        public override void Exit()
        {
            base.Exit();
            combineAnimationTrigger.OnReloadTrigger -= HandleReloadTrigger;
            player.PlayerInputSO.OnShootPressed -= HandleShootPressed;
            player.PlayerInputSO.OnDodgePressed -= HandleDodgePressed;
            
            _moveSpeedStat.RemoveModifier(this);
            player.PlayerEventChannel.RaiseEvent(PlayerEvents.playerReloadStatusEvent.Initialize(false));
        }

        public override void Update()
        {
            base.Update();
            if (!_reloadTrigger) return;
            attackCompo.Reload();
            _reloadTrigger = false;
            if (attackCompo.IsFullAmmo)
            {
                player.ChangeState("IDLE", stateType);
            }
        }
        
        private void HandleReloadSpeedValueChange(StatSO stat, float current, float previous)
        {
            _reloadSpeed = current;
            splitRenderer.SetParam(player.ReloadSpeedParam.hashValue, _reloadSpeed / _defaultReloadSpeed, stateType);
        }
        private void HandleReloadTrigger() => _reloadTrigger = true;
        
        private void HandleShootPressed()
        {
            player.ChangeState("ATTACK", stateType);
        }

        private void HandleDodgePressed() => player.ChangeState("IDLE", stateType);
    }
}
