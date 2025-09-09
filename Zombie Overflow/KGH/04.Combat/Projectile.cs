using Core;
using Core.GameEvent;
using Feedbacks.VFX;
using GGMPool;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private GameEventChannelSO playerChannel;

        [Header("Projectile Settings")] [SerializeField]
        private float defaultSpeed = 10f;

        [SerializeField] private float lifeTime = 2f;
        [Header("Effect")] [SerializeField] private PoolTypeSO hitEffect;
        [SerializeField] private TrailRenderer trailRenderer;

        private LayerMask _hitLayer;
        private LayerMask _enemyLayer;
        private float _timer;
        private Vector3 _direction;
        private Rigidbody _rb;
        private Vector3 _previousPosition;

        private float _fireSpeed;

        #region Pool

        [Header("Pool")] [SerializeField] private PoolManagerSO poolManager;
        [field: SerializeField] public PoolTypeSO PoolType { get; set; }
        public GameObject GameObject => gameObject;
        private Pool _pool;

        #endregion

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public virtual void SetUpAndFire(float speedMultiplier, Vector3 position, Vector3 direction, LayerMask hitLayer,
            LayerMask enemyLayer = default)
        {
            _hitLayer = hitLayer;
            _enemyLayer = enemyLayer;
            _fireSpeed = defaultSpeed * speedMultiplier;

            transform.position = position;
            trailRenderer.Clear();

            FireProjectile(direction);
        }

        protected void FireProjectile(Vector3 direction)
        {
            _timer = 0;
            _rb.linearVelocity = direction.normalized * _fireSpeed;
        }


        private void FixedUpdate()
        {
            _previousPosition = transform.position; // Store the position at the start of physics frame.

            _timer += Time.fixedDeltaTime;
            if (_timer >= lifeTime)
            {
                _pool?.Push(this);
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if ((_hitLayer.value & 1 << other.gameObject.layer) == 0)
                return;

            var movementDirection = (transform.position - _previousPosition).normalized;
            var movementDistance = Vector3.Distance(transform.position, _previousPosition);

            var origin = _previousPosition - movementDirection;

            var ray = new Ray(origin, movementDirection);

            if (Physics.Raycast(ray, out var hit, movementDistance * 2, _hitLayer))
            {
                var hittable = hit.collider.GetComponent<IHittable>();
                HandleRayHit(hit, movementDirection, hittable);
            }
        }

        protected virtual void HandleRayHit(RaycastHit hit, Vector3 movementDirection, IHittable hittable)
        {
            if (hitEffect != null)
            {
                var effect = poolManager.Pop(hitEffect) as VFXPlayer;
                var rotation = Quaternion.LookRotation(hit.normal);
                effect?.SetUpAndPlay(hit.point, rotation);
            }

            if (hittable != null)
            {
                //compare hit.collider.gameObject.layer with enemyLayer
                if ((1 << hit.transform.gameObject.layer & _enemyLayer.value) != 0)
                    playerChannel?.RaiseEvent(PlayerEvents.playerSkillGaugeEvent.AddGauge(0.1f));
                hittable.TakeHit(movementDirection);
            }

            _pool.Push(this);
        }

        #region Pool

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public void ResetItem()
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            _hitLayer = default;
            _enemyLayer = default;
        }

        #endregion
    }
}