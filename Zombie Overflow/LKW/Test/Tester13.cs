using System;
using Combat;
using Core.GameEvent;
using Enemies;
using Enemies.BossZombie;
using GGMPool;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01.Script.LKW.Test
{
    public class Tester13 : MonoBehaviour
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolTypeSO poolType;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private GameEventChannelSO enemyChannel;
        
        private void Update()
        {
            // if (Keyboard.current.bKey.wasPressedThisFrame)
            // {
            //     impulseSource.GenerateImpulse();
            //     BossZombie enemy = poolManager.Pop(poolType) as BossZombie;
            // }
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                Time.timeScale = 0.3f;
                //impulseSource.GenerateImpulse();

            }
        }
    }
}