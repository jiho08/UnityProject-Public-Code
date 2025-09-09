using System.Collections;
using Core.GameEvent;
using GGMPool;
using UnityEngine;

namespace Enemies.SniperZombie
{
    public class EnemySniperZombie : BTEnemy
    {
        private StateChangeEvent _stateChannel;

        private SniperZombieAttackCompo _attackCompo;

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            _attackCompo = GetCompo<SniperZombieAttackCompo>();
        }

        private void Start()
        {
            _stateChannel = GetBlackboardVariable<StateChangeEvent>("StateChannel").Value;
            _stateChannel.SendEventMessage(BtEnemyState.CHASE);
        }

        private void OnEnable()
        {
            if(_stateChannel != null)
                _stateChannel.SendEventMessage(BtEnemyState.CHASE);
        }

        protected override void HandleDanceEvent(EnemyDanceEvent evt)
        {
            base.HandleDanceEvent(evt);
            _attackCompo.StopTracking();
        }

        public override void TakeHit(Vector3 direction)
        {
            base.TakeHit(direction);
            _attackCompo.StopTracking();
            if(isDead) return;
            isDead = true;
            GetBlackboardVariable<Vector3>("KnockBackDir").Value = direction;
            _stateChannel.SendEventMessage(BtEnemyState.DEATH);
            StartCoroutine(DeadCoroutine());
        }
        
        #region Dead
        
        protected IEnumerator DeadCoroutine()
        {
            yield return new WaitForSeconds(deathTime);
            poolManager.Push(this);
        }
        
        #endregion
        
        #region Pooling

        public override void ResetItem()
        {
            base.ResetItem();
            isDead = false;
        }
        
        #endregion
    }
}