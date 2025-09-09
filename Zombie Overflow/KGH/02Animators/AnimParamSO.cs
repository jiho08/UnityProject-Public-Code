using UnityEngine;

namespace Animations
{
    [CreateAssetMenu(fileName = "ParamSO", menuName = "SO/Animator/Params", order = 0)]
    public class AnimParamSO : ScriptableObject
    {
        public string paramererName;
        public int hashValue;
        [TextArea]
        public string description;

        private void OnValidate()
        {
            hashValue = Animator.StringToHash(paramererName);
        }
    }
}