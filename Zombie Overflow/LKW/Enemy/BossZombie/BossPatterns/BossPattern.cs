
using Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Enemies.BossZombie.BossPatterns
{
    public abstract class  BossPattern : MonoBehaviour
    {
        [HideInInspector]
        public BTEnemy enemy;
        [HideInInspector]
        public Transform targetTrm;
        [HideInInspector]
        public EntityAnimationTrigger animatorTrigger;
        public PatternType patternType;
        public UnityEvent AttackEvent;

        public abstract void EnablePattern();
        public abstract void DisablePattern();
    }
}