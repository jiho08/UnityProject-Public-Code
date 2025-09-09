using System.Collections;
using System.Collections.Generic;
using Combat;
using Core.GameEvent;
using Enemies;
using Enemies.PoliceZombie;
using Enemies.SniperZombie;
using GGMPool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01.Script.LKW.ETC
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO enemyChannel;
        [SerializeField] private PoolManagerSO poolManager;
        
        [SerializeField] private List<Transform> centers = new List<Transform>();
        [SerializeField] private float centerRadius;
        [SerializeField] private LayerMask whatIsObstacle;
        [SerializeField] private float drumtongRadius;
        [SerializeField] private PoolTypeSO drumtongType;


        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private int summonCount = 10;
        [SerializeField] private float summonDelay = 0.2f;
        [SerializeField] private List<PoolTypeSO> enemyTypeList;

        private void Start()
        {
            enemyChannel.AddListener<BossSpawnDrumtongEvent>(HandleDrumTongSpawn);
            enemyChannel.AddListener<BossSpawnEnemyEvent>(HandleEnemySpawn);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            enemyChannel.RemoveListener<BossSpawnDrumtongEvent>(HandleDrumTongSpawn);
            enemyChannel.RemoveListener<BossSpawnEnemyEvent>(HandleEnemySpawn);
        }

        private void HandleEnemySpawn(BossSpawnEnemyEvent obj)
        {
            StartCoroutine(SummonCoroutine());
        }

        private void HandleDrumTongSpawn(BossSpawnDrumtongEvent obj)
        {
            SpawnDrumtong();
        }


        private IEnumerator SummonCoroutine()
        {
            yield return new WaitForSeconds(1.2f);
            for (int i = 0; i < summonCount; i++)
            {
                int enemyIdx = Random.Range(0, enemyTypeList.Count);
                
                ChooseType(enemyIdx);
                yield return new WaitForSeconds(summonDelay);
            }
        }

        private void ChooseType(int enemyIdx)
        {
            PoolTypeSO enemyPoolType = enemyTypeList[enemyIdx];
            
            if(enemyIdx < 3)
                Summon<EnemyWorkerZombie>(enemyPoolType);
            else if(enemyIdx <4)
                Summon<EnemySniperZombie>(enemyPoolType);
            else if(enemyIdx <5)
                Summon<EnemyPoliceZombie>(enemyPoolType);
        }

        private void Summon<T>(PoolTypeSO enemyType) where T : BTEnemy
        {
            int randIdx = Random.Range(0, spawnPoints.Count);
            T enemy =  poolManager.Pop(enemyType) as T;
            enemy.transform.position = spawnPoints[randIdx].position;
            
        }

        public void SpawnDrumtong()
        {
            while (true)
            {
                Vector3 center = centers[Random.Range(0, centers.Count)].position;

                Vector3 spawnPosition = center + Random.insideUnitSphere * centerRadius;
                spawnPosition.y = 30;
                Debug.Log(spawnPosition);
                
                if (Physics.OverlapSphere(spawnPosition, drumtongRadius, whatIsObstacle).Length <= 0)
                {
                    DrumTong drumtong = poolManager.Pop(drumtongType) as DrumTong;
                    drumtong.transform.position = spawnPosition;
                    return;
                }
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            foreach (var cen in centers)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(cen.position, centerRadius);
            }
        }
#endif
    }
}