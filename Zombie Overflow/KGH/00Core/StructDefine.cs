using System;
using Core.Define;
using Entities.Stat;
using UnityEngine;

namespace Core
{
    public static partial class StructDefine
    {
        [Serializable]
        public struct StatSaveData
        {
            public string statName;
            public float baseValue;
        }
        [Serializable]
        public struct CharacterRenderData
        {
            public EnumDefine.PlayerType playerType;
            public GameObject characterRender;
            public GameObject gunRender;
        }
        [Serializable]
        public struct CharacterStatData
        {
            public StatSO stat;
            public float baseValue;
        }

        [Serializable]
        public struct PlayerMuzzleData
        {
            public EnumDefine.PlayerType playerType;
            public Transform gun;
        }
    }
}