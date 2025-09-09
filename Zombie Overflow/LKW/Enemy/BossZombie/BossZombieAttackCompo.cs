using System;
using System.Collections.Generic;
using System.Linq;
using Enemies.BossZombie.BossPatterns;
using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies.BossZombie
{
    public enum PatternType
    {
        CrossPattern,
        CirclePattern,
        SummonPattern
    }
    
    public class BossZombieAttackCompo : EnemyAttackCompo
    {
        private Dictionary<PatternType, BossPattern> _bossStates = new Dictionary<PatternType, BossPattern>();
        [field:SerializeField] public float SummonCooldown { get; private set; }
        public float LastSummonTime { get; set; } = 0;
        
        private BossPattern _currentPattern;
        
        private void Start()
        {
            GetComponentsInChildren<BossPattern>().ToList().ForEach(pattern =>
            {
                pattern.enemy = _btEnemy;
                pattern.targetTrm = _btEnemy.EntityFinder.target.transform;
                pattern.animatorTrigger = _btEnemy.GetCompo<EntityAnimationTrigger>(); 
                _bossStates.Add(pattern.patternType, pattern);
            });
        }
        
        public void UsePattern(PatternType pattern)
        {
            _currentPattern = _bossStates[pattern];
            _currentPattern.EnablePattern();
        }

        public void DisablePattern()
        {
            lastAttackTime = Time.time + Random.Range(-cooldownRandomness, cooldownRandomness);
            if(_currentPattern!= null)
                _currentPattern.DisablePattern();
        }


        public void SetSummonTime()
        {
            LastSummonTime = Time.time;
        }
    }
}