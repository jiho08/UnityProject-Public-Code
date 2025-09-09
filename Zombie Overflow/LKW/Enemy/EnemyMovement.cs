using System.IO;
using Entities;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.InputSystem;

namespace Enemies
{
    public class EnemyMovement : EntityMovement
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float stopOffset = 0.05f;
        [SerializeField] private float  knockBackDistance = 0.05f;
        [SerializeField] private float  knockBackPower = 2f;
        [SerializeField] private float  knockBackTime = 1.2f;
        [SerializeField] private Ease  ease;
        
        public float RotateSpeed = 10f;

        
        private Vector3 _destination;
        public bool IsArrived => !agent.pathPending && agent.remainingDistance < agent.stoppingDistance + stopOffset;
        public float RemainDistance => agent.pathPending ? -1 : agent.remainingDistance;

        protected override void Move()
        {
            agent.SetDestination(_destination);
        }

        public void KnockBack(Vector3 direction)
        {
            Debug.unityLogger.Log(direction);
            SetStop(true);
            Vector3 landingPoint = transform.position + direction.normalized * knockBackDistance;
            
            Debug.Log(landingPoint);

            transform.parent.DOJump(landingPoint, knockBackPower, 1, knockBackTime, false)
                .SetEase(ease);
        }

        public void SetStop(bool isStop) => agent.isStopped = isStop;
        public void SetVelocity(Vector3 velocity) => agent.velocity = velocity; 
        public void SetSpeed(float speed) => agent.speed = speed;

        public void SetDestination(Vector3 destination)
        {
            _destination = destination;
            Move();
        }
        
        public Quaternion LookAtTarget(Vector3 target, bool isSmooth = true)
        {
            Vector3 nextPosition = agent.steeringTarget;
            Vector3 direction = target - entity.transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            if (isSmooth)
            {
                entity.transform.rotation = Quaternion.Slerp(entity.transform.rotation, lookRotation, Time.deltaTime * RotateSpeed);
            }
            else
            {
                entity.transform.rotation = lookRotation;
            }
            return lookRotation;
        }


        public override void StopImmediately()
        {
        }
    }
}