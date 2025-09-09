using System;
using System.Collections;
using Combat;
using GGMPool;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Enemies
{
    public class EnemyWorkerZombie : BTEnemy
    {
        private StateChangeEvent _stateChannel;
        private BtEnemyState _state;

        private EnemyMovement _enemyMovement;

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            _enemyMovement = GetCompo<EnemyMovement>();
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

        public override void TakeHit(Vector3 direction)
        {
            base.TakeHit(direction);
            if(isDead) return;
            isDead = true;
            GetBlackboardVariable<Vector3>("KnockBackDir").Value = direction;
            _stateChannel.SendEventMessage(BtEnemyState.DEATH);
            StartCoroutine(DeadCoroutine());
        }

        #region Dead
        
        private IEnumerator DeadCoroutine()
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