using Entities.Stat;
using UnityEngine;

namespace Players
{
    public class PlayerStat : EntityStat, IPlayerComponent
    {
        public void SetUpPlayer(CharacterSO character)
        {
            foreach (var statData in character.characterStatData)
                SetBaseValue(statData.stat, statData.baseValue);
        }
    }
}