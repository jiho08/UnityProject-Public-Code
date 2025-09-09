using System.Collections;
using GGMPool;
using UnityEngine;

namespace Feedbacks.VFX
{
    public class VFXPlayer : MonoBehaviour, IPoolable
    {
        public PoolTypeSO PoolType { get; set; }
        public GameObject GameObject => gameObject;
        private Pool _pool;
        
        private ParticleSystem _particleSystem;

        private float _particleDuration;
        
        public void SetUpPool(Pool pool)
        {
            _pool = pool;
            
            _particleSystem = GetComponent<ParticleSystem>();
            _particleDuration = _particleSystem.main.duration;
        }

        public void ResetItem()
        {
            _particleSystem.Stop();
            _particleSystem.Clear();
        }
        public void SetUpAndPlay(Vector3 position, Quaternion rotation = default)
        {
            transform.position = position;
            transform.rotation = rotation;
            _particleSystem.Play();
            StartCoroutine(WaitAndBackToPool());
        }

        private IEnumerator WaitAndBackToPool()
        {
            yield return new WaitForSeconds(_particleDuration);
            _pool.Push(this);
        }
    }
}