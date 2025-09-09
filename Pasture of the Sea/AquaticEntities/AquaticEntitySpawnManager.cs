using Code.Core;
using Code.Core.Pool;
using Code.ETC;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.AquaticEntities
{
    public class AquaticEntitySpawnManager : MonoSingleton<AquaticEntitySpawnManager>
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolTypeSO startFishPoolType;
        [SerializeField] private int startFishCount = 1;

        private void Start()
        {
            StartFishSpawn();
        }

        public void SpawnEntity(PoolTypeSO poolType)
        {
            var entity = poolManager.Pop(poolType) as AquaticEntity;
            entity.transform.position = SetRandomPosition();
        }

        private Vector3 SetRandomPosition()
        {
            var angle = Random.Range(0f, 2f * Mathf.PI);
            var radius = Random.Range(0f, MapManager.Instance.mapSize.x / 2f);
            
            var x = radius * Mathf.Cos(angle);
            var z = radius * Mathf.Sin(angle);
            var y = Random.Range(-MapManager.Instance.mapSize.y / 2f, MapManager.Instance.mapSize.y / 2f);

            return MapManager.Instance.transform.position + new Vector3(x, y, z);
        }

        private void StartFishSpawn()
        {
            for (var i = 0; i < startFishCount; ++i)
                SpawnEntity(startFishPoolType);
        }
    }
}