using System;
using System.Collections;
using Combat;
using DG.Tweening;
using GGMPool;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemies.SniperZombie
{
    public class SniperZombieAttackCompo : EnemyAttackCompo
    {
        [SerializeField] private float trackTime = 3f;
        [SerializeField] private float shootRandomness = 3f;
        [SerializeField] private PoolTypeSO bulletType;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private LineRenderer lineRenderer;
        private bool isTracking = false;
        public float rotationSpeed = 2f;

        [SerializeField] private GameObject fireRouteTrm;
        [SerializeField] private Transform firePos;
        public override void Attack()
        {
            StartCoroutine(ShootBullet());
            lastAttackTime = Time.time;
        }

        public void StartTracking()
        {
            if (!isTracking)
            {
                StartCoroutine(TrackTarget());
            }
        }


        IEnumerator TrackTarget()
        {
            fireRouteTrm.SetActive(true);
            isTracking = true;
            float timer = 0f;

            while (timer < trackTime)
            {
                lineRenderer.SetPosition(0, transform.position);
                Vector3 targetPos = _btEnemy.EntityFinder.target.transform.position;
                float distance = Vector3.Distance(transform.position, targetPos);
                Vector3 direction = targetPos - transform.parent.position;

                if (direction != Vector3.zero)
                {
                    lineRenderer.SetPosition(1, transform.position + transform.forward * distance);
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                timer += Time.deltaTime;
                yield return null;
            }

            isTracking = false;
            fireRouteTrm.SetActive(false);
            
        }

        public void StopTracking()
        {
            isTracking = false;
            fireRouteTrm.SetActive(false);
        }

        private IEnumerator ShootBullet()
        {
            AttackEvent?.Invoke();
            Projectile bullet = poolManager.Pop(bulletType) as Projectile;
            bullet.SetUpAndFire(1.4f,firePos.position, transform.forward, whatIsPlayer);
                yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < 2; i++)
            {
                AttackEvent?.Invoke();
                Vector3 bulletDirection = Quaternion.Euler(0,Random.Range(-shootRandomness,shootRandomness),0)  *transform.forward;
                bullet = poolManager.Pop(bulletType) as Projectile;
                bullet.SetUpAndFire(2f,firePos.position, bulletDirection, whatIsPlayer);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}