using Animations;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(fileName = "StateData", menuName = "SO/FSM/StateData", order = 0)]
    public class StateSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public AnimParamSO animParam;
    }
}