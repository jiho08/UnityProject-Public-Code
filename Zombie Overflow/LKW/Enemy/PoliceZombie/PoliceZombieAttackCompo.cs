using Combat;
using GGMPool;
using UnityEngine;

namespace Enemies.PoliceZombie
{
    public class PoliceZombieAttackCompo : EnemyAttackCompo
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolTypeSO bulletType;
        [SerializeField] private Transform firePos;

        public override void Attack()
        {
            base.Attack();
            
            Projectile bullet = poolManager.Pop(bulletType) as Projectile;
            
            Vector3 direction = transform.forward;
            
            bullet.SetUpAndFire(0.8f,firePos.position, direction,whatIsPlayer);
            
            lastAttackTime = Time.time;
        }
    }
}