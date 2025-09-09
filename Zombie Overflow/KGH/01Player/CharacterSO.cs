using Combat;
using Combat.Skills;
using Core;
using Core.Define;
using Entities.Stat;
using Feedbacks.SFX;
using UnityEngine;
using StructDefine = Core.StructDefine;

namespace Players
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "SO/Player/CharacterData", order = 0)]
    public class CharacterSO : ScriptableObject
    {
        public EnumDefine.PlayerType playerType;
        
        public string characterName;
        public RenderTexture characterRenderTexture;
        
        public GunData gunData;
        public ShowDownData showDownData;
        public SoundDataSO deadSound;
        
        public StructDefine.CharacterStatData[] characterStatData;
    }
}