using System;
using System.Collections;
using System.Collections.Generic;
using _01.Script.LKW.ETC;
using Core.GameEvent;
using Enemies.PoliceZombie;
using Enemies.SniperZombie;
using GGMPool;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemies.BossZombie.BossPatterns
{
    public class SummonPattern : BossPattern
    {
        [FormerlySerializedAs("mapData")] [SerializeField] private BossMapData bossMapData;
        
        [SerializeField] private UnityEvent OnScreamEvent;
        [SerializeField] private GameEventChannelSO enemyChannel;
        
        public override void EnablePattern()
        {
            OnScreamEvent?.Invoke();
            enemyChannel.RaiseEvent(EnemyEvent.bossSpawnDrumtongEvent);
            enemyChannel.RaiseEvent(EnemyEvent.bossSpawnEnemyEvent);
        }

        

        public override void DisablePattern()
        {
        }
    }
}