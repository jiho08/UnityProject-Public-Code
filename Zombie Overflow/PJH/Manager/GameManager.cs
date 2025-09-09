using _01.Script.LKW.ETC;
using Core;
using Core.Define;
using Players;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Score.Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public CharacterSO CharacterSO { get; private set; }
        public BossMapData BossMapData => _mapData[(int)_selectedMap];
        
        private EnumDefine.MapType _selectedMap;
        private BossMapData[] _mapData;

        // 캐릭터 선택한 걸 저장해야 함
        // 인게임 들어갔을 때 맵 설정해서 저장
        // 플레이어 포지션 중앙으로
        
        public Vector3 SetPlayerPosition()
        {
            return Vector3.zero;
        }

        public void SelectMap()
        {
            _selectedMap = (EnumDefine.MapType)Random.Range(0, 3);

            switch (_selectedMap)
            {
                // case EnumDefine.MapType.Park:
                // {
                //     
                // }
            }
        }

        public void SelectCharacter(CharacterSO characterSO)
        {
            CharacterSO = characterSO;
        }
    }
}