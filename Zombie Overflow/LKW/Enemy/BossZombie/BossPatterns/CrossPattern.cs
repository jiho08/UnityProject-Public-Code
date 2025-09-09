using System;
using System.Collections;
using GGMPool;
using JetBrains.Annotations;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Enemies.BossZombie.BossPatterns
{
    public class CrossPattern : BossPattern
    {

        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private LayerMask _whatIsWall;
        [SerializeField] private PoolTypeSO rangeType;
        [SerializeField] private PoolTypeSO horizontalZoneType;
        [SerializeField] private PoolTypeSO verticalZoneType;
        [CanBeNull] private GameObject outRangeView;
        [CanBeNull] private GameObject inRangeView;
        
        private float gravity = -9.81f;
        [SerializeField] private float jumpSpeed;
        [SerializeField] private float jumpTime;

        private CrossRange hitRange;
        
        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public override void EnablePattern()
        {
            animatorTrigger.OnJumpStartTrigger += HandleJumpStart;
            animatorTrigger.OnAttackTrigger += HandleAttack;
        }
       

        public override void DisablePattern()
        {
            if(hitRange != null)
                poolManager.Push(hitRange);
            animatorTrigger.OnJumpStartTrigger -= HandleJumpStart;
            animatorTrigger.OnAttackTrigger -= HandleAttack;
            
            StopAllCoroutines();
        }

        private void HandleJumpStart()
        {
            hitRange = poolManager.Pop(rangeType) as CrossRange;
            outRangeView = hitRange.outRange;
            inRangeView = hitRange.inRange;
            
            outRangeView.SetActive(true);
            inRangeView.SetActive(true);
            
            Vector3 direction = targetTrm.position - transform.position;
            direction.y = 0;
            Vector3 endPos;
            
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 6f, _whatIsWall))
                endPos = hit.transform.position + (direction*-1).normalized*3f;
            else
                endPos = targetTrm.position;
            
            endPos.y = 0;
            outRangeView.transform.parent.position = endPos + new Vector3(0,0.2f,0);
            
            StartCoroutine(JumpToCoroutine(endPos));
        }


        private void HandleAttack()
        {
            AttackEvent?.Invoke();
            DamageZone horizontalZone = poolManager.Pop(horizontalZoneType) as DamageZone;
            DamageZone verticalZone = poolManager.Pop(horizontalZoneType) as DamageZone;
            verticalZone.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0)) * horizontalZone.transform.rotation;
            horizontalZone.transform.position = inRangeView.transform.position;
            verticalZone.transform.position = inRangeView.transform.position;
        }

        private IEnumerator JumpToCoroutine(Vector3 endPos)
        {

            Vector3 start = enemy.transform.position;
            Vector3 end = endPos;

            float currentTime = 0;
            float percent = 0;

            float v0 = (end - start).y* (end-start).y - gravity; 

            while (percent < 1)
            {
                currentTime += Time.deltaTime;
                percent = currentTime / jumpTime;

                Vector3 pos = Vector3.Lerp(start, end, percent);

                pos.y = start.y + (v0 * percent) + (gravity * percent * percent);
                inRangeView.transform.localScale = Vector3.one * 3.6f * percent;

                enemy.transform.position = pos;
                yield return null;
            }
            
        }
    }
}