using UnityEngine;

namespace Code.Core.Pool
{
    public class PoolManagerMono : MonoBehaviour
    {
        [SerializeField] private PoolManagerSO _poolManager;

        private void Awake()
        {
            _poolManager.InitializePool(transform);
        }
    }
}
