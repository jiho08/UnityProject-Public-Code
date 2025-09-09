using Combat;
using Players;
using UnityEngine;

namespace Enemies
{
    public class WorkerZombieAttackCompo : EnemyAttackCompo
    {
        public override void Attack()
        {
            base.Attack();
            Vector3 attackDirection = _btEnemy.transform.forward;
            Vector3 overlapCenter = transform.position + _btEnemy.transform.forward * attackDistance;

            Collider[] colliders = Physics.OverlapSphere(overlapCenter, attackRadius, whatIsPlayer);
            
            if (colliders.Length > 0)
            {
                if (colliders[0].TryGetComponent(out Player player) && player.TryGetComponent<IHittable>(out IHittable hittable))
                {
                    hittable.TakeHit(attackDirection);
                }
                else if (colliders.Length <= 0)
                {
                    Debug.Log("No player found");
                }
            }
            
            
        }
        
        
    }
}