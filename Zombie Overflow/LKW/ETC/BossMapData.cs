using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Script.LKW.ETC
{
    [CreateAssetMenu(fileName = "MapData", menuName = "SO/Map/MapData", order = 0)]
    public class BossMapData : ScriptableObject
    {
        
        [Header("spawn")]
        public List<Vector3> spawnPoints;
        public  List<Vector3> mapCenter;
        public  float centerRadius;
    }
}