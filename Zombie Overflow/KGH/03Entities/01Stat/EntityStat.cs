using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Entities.Stat
{
    public class EntityStat : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private StatOverride[] statOverrides;
        private StatSO[] _stats;

        public Entity Owner { get; private set; }

        public void Initialize(Entity entity)
        {
            Owner = entity;
            _stats = statOverrides.Select(stat => stat.CreateStat()).ToArray();
        }

        public StatSO GetStat(StatSO targetStat)
        {
            Debug.Assert(targetStat != null, "Stats::GetStat : target stat is null");
            return _stats.FirstOrDefault(stat => stat.statName == targetStat.statName);
        }

        public bool TryGetStat(StatSO targetStat, out StatSO stat)
        {
            Debug.Assert(targetStat != null, "Stats::GetStat : target stat is null");

            // stat = _stats.FirstOrDefault(s => s.statName == targetStat.statName);
            stat = _stats.FirstOrDefault(s =>
            {
                return s.statName == targetStat.statName;
            });
            return stat;
        }

        public void SetBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue = value;
        public float GetBaseValue(StatSO stat) => GetStat(stat).BaseValue;
        public void IncreaseBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue += value;
        public void AddModifier(StatSO stat, object key, float value) => GetStat(stat).AddModifier(key, value);
        public void RemoveModifier(StatSO stat, object key) => GetStat(stat).RemoveModifier(key);

        public void CleanAllModifiers()
        {
            foreach (var stat in _stats)
            {
                stat.ClearAllModifiers();
            }
        }



        public List<StructDefine.StatSaveData> GetSaveData()
        {
            return _stats.Aggregate(new List<StructDefine.StatSaveData>(), (saveList, stat) =>
            {
                saveList.Add(new StructDefine.StatSaveData { statName = stat.statName, baseValue = stat.BaseValue });
                return saveList;
            });
        }

        public void RestoreData(List<StructDefine.StatSaveData> loadedDataList)
        {
            foreach (var loadData in loadedDataList)
            {
                var targetStat = _stats.FirstOrDefault(stat => stat.statName == loadData.statName);
                if (targetStat != default)
                {
                    targetStat.BaseValue = loadData.baseValue;
                }
            }
        }
    }
}