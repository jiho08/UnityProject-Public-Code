using System;
using System.Net.NetworkInformation;
using Core.GameEvent;
using DG.Tweening;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace _01.Script.LKW.ETC
{
    public class ZombieDance : MonoBehaviour
    {
        public UnityEvent ZombieDanceEnd;
        [SerializeField] private GameEventChannelSO enemyChannel;
        [SerializeField] private CinemachineCamera cam;
        [SerializeField] private CinemachineCamera cam2;
        private readonly float originScale = 1;
        private readonly float sleepScale = 0.3f;

        private void OnDestroy()
        {
            DOTween.Kill(this);
        }

        public void StartCutScene()
        {
            cam.Priority = 10;
            DOVirtual.DelayedCall(0.2f, () => Time.timeScale = sleepScale);
            DOVirtual.DelayedCall(2f, () =>
            {
                cam2.Priority = 11;
            }).OnComplete(() =>
            {
                DOVirtual.DelayedCall(4, () =>
                {
                    enemyChannel.RaiseEvent(EnemyEvent.enemyDanceEvent);
                    Time.timeScale = originScale;
                }).OnComplete(() =>
                {
                    DOVirtual.DelayedCall(4, () =>
                    {
                        ZombieDanceEnd?.Invoke();
                    });
                });
            });
           
            
            
        }
    }
}