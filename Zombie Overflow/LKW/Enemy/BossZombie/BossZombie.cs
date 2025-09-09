using System.Collections;
using Core.GameEvent;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies.BossZombie
{
    public class BossZombie : BTEnemy
    {
        public UnityEvent OnDeadEvent;
        public UnityEvent OnScreamEvent;
        
        private BossStateChangeEvent _bossStateChannel;
        public int HitCount { get; private set; } = 0;

        private EnemyMovement _enemyMovement;
        private BossZombieAttackCompo _enemyAttackCompo;


        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            _enemyMovement = GetCompo<EnemyMovement>();
            _enemyAttackCompo = GetCompo<BossZombieAttackCompo>();
        }

        private void Start()
        {
            _bossStateChannel= GetBlackboardVariable<BossStateChangeEvent>("StateChannel").Value;
            _bossStateChannel.SendEventMessage(BossState.SCREAM);
            OnScreamEvent?.Invoke();
        }

        private void OnEnable()
        {
            if (_bossStateChannel != null)
            {
                _bossStateChannel.SendEventMessage(BossState.SCREAM);
                OnScreamEvent?.Invoke();
            }
        }

        protected override void HandleDanceEvent(EnemyDanceEvent evt)
        {
            base.HandleDanceEvent(evt);
            _enemyAttackCompo.DisablePattern();
        }

        public override void TakeHit(Vector3 direction)
        {
            //base.TakeHit(direction);
            OnHit?.Invoke();
            HitCount++;
            _enemyAttackCompo.DisablePattern();
            if (HitCount >= 2)
            {
                gameObject.layer = deadLayerNum;
                isDead = true;
                OnDeadEvent?.Invoke();
                _bossStateChannel.SendEventMessage(BossState.DEATH);
                StartCoroutine(DeadCoroutine());
                return;
            }
            _bossStateChannel.SendEventMessage(BossState.HIT);
        }

        #region Dead
        private IEnumerator DeadCoroutine()
        {
            yield return new WaitForSeconds(deathTime);
            poolManager.Push(this);
        }
        #endregion
        
        #region Pool

        public override void ResetItem()
        {
            base.ResetItem();
            isDead = false;
            HitCount = 0;
            GetBlackboardVariable<BossState>("EnemyState").Value = BossState.SCREAM;
            _enemyMovement.SetStop(false);
            
        }
        #endregion
    }
}