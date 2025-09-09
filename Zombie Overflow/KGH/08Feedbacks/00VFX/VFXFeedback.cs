using GGMPool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Feedbacks.VFX
{
    public class VFXFeedback : MonoBehaviour, IFeedback
    {
        [SerializeField] private PoolTypeSO effect;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private Vector3 offset;

        public void PlayFeedback(Transform trm)
        {
            var eft = poolManager.Pop(effect) as VFXPlayer;
            if (eft != null)
                eft.SetUpAndPlay(trm.position + offset);
        }
    }
}