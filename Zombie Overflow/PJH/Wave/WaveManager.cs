using System.Collections;
using System.Collections.Generic;
using Enemies;
using GGMPool;
using UnityEngine;

namespace Wave.Manager
{
    public class WaveManager : MonoBehaviour
    {
        [field: SerializeField] private List<Transform> spawnPointList;
        [field: SerializeField] private List<WaveDataSO> waveDataList;
        
        [SerializeField] private PoolManagerSO poolManager;

        private int _currentWave;

        private void Start()
        {
            StartCoroutine(SetNextWave());
        }
        
        public void StopWave() => StopAllCoroutines();
        
        private IEnumerator SetNextWave()
        {
            while (true)
            {
                if (_currentWave >= waveDataList.Count)
                    _currentWave = 0;
                
                var waveData = waveDataList[_currentWave];
                yield return new WaitForSeconds(waveData.waveDelay);
                yield return StartCoroutine(SpawnWave(waveData));

                ++_currentWave;
            }
        }

        private IEnumerator SpawnWave(WaveDataSO waveData)
        {
            foreach (var enemyInfo in waveData.enemies)
                for (int i = 0; i < enemyInfo.count; ++i)
                {
                    var enemy = poolManager.Pop(enemyInfo.enemyType) as BTEnemy;
                    enemy.transform.position = spawnPointList[Random.Range(0, spawnPointList.Count)].position;
                    yield return new WaitForSeconds(Random.Range(enemyInfo.minSpawnInterval, enemyInfo.maxSpawnInterval));
                }
        }
    }
}