using GGMPool;

namespace Core.Define
{
    public static partial class StructDefine
    {
        public struct TestStruct
        {
            
        }
  
        [System.Serializable]
        public struct EnemySpawnInfo
        {
            public PoolTypeSO enemyType;
            public int count;
            public float minSpawnInterval;
            public float maxSpawnInterval;
        }
    }
}