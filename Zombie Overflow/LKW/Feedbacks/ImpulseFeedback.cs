using Feedbacks;
using Feedbacks.SFX;
using Unity.Cinemachine;
using UnityEngine;

namespace _01.Script.LKW.Feedbacks
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class ImpulseFeedback : MonoBehaviour ,IFeedback
    {
        [SerializeField] private CinemachineImpulseSource impulseSource;
        
        public void PlayFeedback(Transform trm)
        {
            impulseSource.GenerateImpulse(trm.position);
        }
    }
}