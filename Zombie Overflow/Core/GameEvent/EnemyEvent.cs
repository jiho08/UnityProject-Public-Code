using UnityEngine.Playables;

namespace Core.GameEvent
{
    public class EnemyEvent
    {
        public static EnemyDeadEvent enemyDeadEvent = new EnemyDeadEvent();
        public static EnemyDanceEvent enemyDanceEvent = new EnemyDanceEvent();
        public static BossSpawnDrumtongEvent bossSpawnDrumtongEvent = new BossSpawnDrumtongEvent();
        public static BossSpawnEnemyEvent bossSpawnEnemyEvent = new BossSpawnEnemyEvent();
    }

    public class EnemyDeadEvent : GameEvent
    {
        public EnemyDeadEvent Initialize()
        {
            return this;
        }
    }

    public class EnemyDanceEvent : GameEvent
    {
        public EnemyDanceEvent Initialize()
        {
            return this;
        }
    }
    public class BossSpawnDrumtongEvent : GameEvent
    {
        public BossSpawnDrumtongEvent Initialize()
        {
            return this;
        }
    }
    public class BossSpawnEnemyEvent : GameEvent
    {
        public BossSpawnEnemyEvent Initialize()
        {
            return this;
        }
    }
}