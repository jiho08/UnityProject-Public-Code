using UnityEngine;

namespace Combat.Skills.ShowDown
{
    public class ShowDownProjectile : Projectile
    {
        private Transform _target;

        public void SetTarget(Transform target) => _target = target;

        protected override void OnTriggerEnter(Collider other)
        {
            if ((other.transform != _target) && _target != null)
                return;
            base.OnTriggerEnter(other);
        }

        protected override void HandleRayHit(RaycastHit hit, Vector3 movementDirection, IHittable hittable)
        {
            if (hittable == null)
                return;
            base.HandleRayHit(hit, movementDirection, hittable);
        }
    }
}