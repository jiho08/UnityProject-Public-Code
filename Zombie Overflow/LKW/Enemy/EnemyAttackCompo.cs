using System;
using Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace Enemies
{
    public class EnemyAttackCompo : MonoBehaviour , IEntityComponent, IAfterInit
    {
        [SerializeField] protected LayerMask whatIsPlayer;
        public UnityEvent AttackEvent;
        
        #region AttackSettings
        public float lastAttackTime{get; protected set;}
        [field:SerializeField] public float attackCooldown { get; private set; } = 3;
        [field:SerializeField] public float attackRadius;
        [SerializeField] protected float attackDistance;
        [field:SerializeField] public float cooldownRandomness;
        #endregion

        protected EntityAnimationTrigger _btAnimatorTrigger;
        
        protected BTEnemy _btEnemy;
        
        public void Initialize(Entity entity)
        {
            _btEnemy = entity as BTEnemy;
            Debug.Assert(_btEnemy != null, $"Not corrected entity name: {entity.name}");
            _btAnimatorTrigger = _btEnemy.GetCompo<EntityAnimationTrigger>();
        }
        
        public void AfterInit()
        {
            _btAnimatorTrigger.OnAttackTrigger += HandleAttackTrigger;
        }

        private void OnDestroy()
        {
            _btAnimatorTrigger.OnAttackTrigger -= HandleAttackTrigger;
        }

        private void HandleAttackTrigger()
        {
            Attack();
        }

        public virtual void Attack()
        {
            AttackEvent?.Invoke();
            lastAttackTime = Time.time;
        }

        
    }
}