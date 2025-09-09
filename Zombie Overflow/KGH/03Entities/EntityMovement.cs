using System;
using Entities.Stat;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities
{
    public abstract class EntityMovement : MonoBehaviour, IEntityComponent, IAfterInit
    {
        [SerializeField] protected StatSO moveSpeedStat;
        public bool CanManualMovement { get; set; } = true;

        protected float moveSpeed;

        protected Entity entity;
        protected EntityStat entityStat;
        public virtual void Initialize(Entity entity)
        {
            this.entity = entity;
            entityStat = entity.GetCompo<EntityStat>(true);
        }

        public virtual void AfterInit()
        {
            if (entityStat.TryGetStat(moveSpeedStat, out var stat))
            {
                moveSpeed = stat.Value;
                stat.OnValueChange += HandleMoveValueChange;
            }
            else
            {
                Debug.LogError($"EntityMovement::Initialize : {moveSpeedStat.statName} not found");
            }
        }
        
        protected virtual void OnDestroy()
        {
            if (entityStat.TryGetStat(moveSpeedStat, out var stat))
            {
                stat.OnValueChange -= HandleMoveValueChange;
            }
        }

        protected abstract void Move();
        public abstract void StopImmediately();
        
        private void HandleMoveValueChange(StatSO stat, float current, float previous) => moveSpeed = current;
    }
}