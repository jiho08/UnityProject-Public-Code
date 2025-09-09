using System;
using DG.Tweening;
using Enemies.BossZombie;
using Feedbacks.VFX;
using GGMPool;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using IPoolable = GGMPool.IPoolable;

namespace Combat
{
    public class DrumTong : MonoBehaviour, IHittable, IPoolable
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolTypeSO effectType;
        [SerializeField] private LayerMask whatIsTarget;
        [SerializeField] private float explosionRadius;
        [SerializeField] private GameObject visual;
        [SerializeField] private Ease ease;
        [SerializeField] private PoolTypeSO dustEffectType;

        public UnityEvent ExplosionEvent;
        
        private Pool _myPool;
        
        public void TakeHit(Vector3 direction)
        {
            Explosion();
        }

        private void OnEnable()
        {
            transform.DOMoveY(0, 1.4f).SetEase(ease)
                .OnComplete(() =>
                {
                    VFXPlayer dustEffect = poolManager.Pop(dustEffectType) as VFXPlayer;
                    dustEffect.SetUpAndPlay(transform.position);
                });
        }

        private void OnDisable()
        {
            DOTween.Kill(transform);
        }

        private void Explosion()
        {
            VFXPlayer vfx = poolManager.Pop(effectType) as VFXPlayer;
            vfx.SetUpAndPlay(transform.position);

            ExplosionEvent?.Invoke();
            
            visual.SetActive(false);
            Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, whatIsTarget);

            if (hits.Length > 0)
            {
                foreach (Collider hit in hits)
                {
                    if (hit.TryGetComponent(out IHittable hittable))
                    {
                        hittable.TakeHit(hit.transform.position-transform.position);
                    }
                }
            }
            poolManager.Push(this);
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
#endif

        #region Pool
        [field:SerializeField]public PoolTypeSO PoolType { get; set; }
        public GameObject GameObject => gameObject;
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            visual.SetActive(true);
        }
        #endregion
    }
}