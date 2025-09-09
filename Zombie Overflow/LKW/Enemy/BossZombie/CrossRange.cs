using GGMPool;
using UnityEngine;

namespace Enemies.BossZombie
{
    public class CrossRange : MonoBehaviour, IPoolable
    {
        public GameObject inRange;
        public GameObject outRange;
        [field:SerializeField] public PoolTypeSO PoolType { get; set; }
        public GameObject GameObject => gameObject;
        
        private Pool _pool;
        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public void ResetItem()
        {
            outRange.SetActive(false);
            inRange.SetActive(false);
        }
    }
}