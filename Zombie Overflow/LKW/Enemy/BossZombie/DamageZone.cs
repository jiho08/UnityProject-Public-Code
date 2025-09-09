using System.Collections;
using Combat;
using GGMPool;
using UnityEngine;
using IPoolable = GGMPool.IPoolable;
using DG.Tweening;

namespace Enemies.BossZombie
{
    public class DamageZone : MonoBehaviour, IPoolable
    {
        [SerializeField] private LayerMask whatIsPlayer;
        [field:SerializeField] public PoolTypeSO PoolType { get; set; }
        [SerializeField] private PoolManagerSO poolManager;
        public GameObject GameObject => gameObject;
        [SerializeField] private ParticleSystem[] effects;
        [SerializeField] private GameObject spikes;
        [SerializeField] private float lifeTime;

        private Pool _myPool;

        private void OnEnable()
        {
            foreach (var effect in effects)
                effect.Play();
            spikes.transform.DOMoveY(transform.position.y + 0.6f, 0.1f).SetEase(Ease.OutQuad);
            StartCoroutine(LifeCoroutine());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator LifeCoroutine()
        {
            yield return new WaitForSeconds(lifeTime);
            poolManager.Push(this);
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            spikes.transform.position = spikes.transform.position + new Vector3(0f, -1f, 0f);
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (((1 << collision.gameObject.layer) & whatIsPlayer) != 0)
            {
                if (collision.TryGetComponent(out IHittable hittable))
                {
                    hittable.TakeHit(collision.gameObject.transform.position - transform.position);
                }
            }
        }
    }
}