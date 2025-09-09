using System.Collections.Generic;
using Core.Define;
using UnityEngine;

namespace Wave
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "SO/Wave/WaveData", order = 0)]
    public class WaveDataSO : ScriptableObject
    {
        public float waveDelay;
        public List<StructDefine.EnemySpawnInfo> enemies;
    }
}