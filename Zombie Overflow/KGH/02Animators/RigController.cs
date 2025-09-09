using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Animations
{
    public class RigController : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Rig rig;
        [SerializeField] private List<Transform> rigTargets;

        private readonly List<Vector3> _offsetPositions = new List<Vector3>();
        private readonly List<Quaternion> _offsetRotations = new List<Quaternion>();

        private void Awake()
        {
            var center = Vector3.zero;
            center.x = rigTargets.Average(target => target.position.x);
            center.y = rigTargets.Average(target => target.position.y);
            center.z = rigTargets.Average(target => target.position.z);

            transform.position = center;

            foreach (var target in rigTargets)
            {
                var localOffset = Quaternion.Inverse(transform.rotation) * (target.position - transform.position);
                _offsetPositions.Add(localOffset);
                _offsetRotations.Add(Quaternion.Inverse(transform.rotation) * target.rotation);
            }
        }

        public void Initialize(Entity entity)
        {
        }

        private void Update()
        {
            for (int i = 0; i < rigTargets.Count; i++)
            {
                var target = rigTargets[i];
                var offsetPosition = _offsetPositions[i].x * transform.right +
                                     _offsetPositions[i].y * transform.up +
                                     _offsetPositions[i].z * transform.forward;
                var offsetRotation = _offsetRotations[i];

                target.position = transform.position + offsetPosition;
                target.rotation = transform.rotation * offsetRotation;
            }
        }
        public float GetRigWeight() => rig.weight;
        public void SetRigWeight(float weight) => rig.weight = weight;
    }
}