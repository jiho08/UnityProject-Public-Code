using Combat;
using Core.GameEvent;
using Entities;
using Feedbacks.VFX;
using GGMPool;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using IPoolable = GGMPool.IPoolable;

namespace Enemies
{
    public class BTEnemy : Entity, IHittable, IPoolable
    {
        [field: SerializeField] public EntityFinderSO EntityFinder { get; protected set; }
        [SerializeField] protected PoolManagerSO poolManager;
        public BehaviorGraphAgent btAgent;
        [field:SerializeField] public float AttackRange { get;private set; }
        [SerializeField] protected float deathTime = 5f;

        [SerializeField] private PoolTypeSO hitEffectType;
        [SerializeField] private GameEventChannelSO enemyChannel;
        private Animator _animator;
        private NavMeshAgent _navAgent;
        
        private readonly int _danceHash = Animator.StringToHash("DANCE");

        protected const int enemyLayerNum = 8;
        protected const int deadLayerNum = 9;
        
        [field: SerializeField] protected bool isDead { get; set; }

        #region Init
        

        private void OnEnable()
        {
            if(_navAgent != null)
                _navAgent.enabled = true;
            enemyChannel.AddListener<EnemyDanceEvent>(HandleDanceEvent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            enemyChannel.RemoveListener<EnemyDanceEvent>(HandleDanceEvent);

            
        }

        protected override void AddComponents()
        {
            base.AddComponents();
            btAgent = GetComponent<BehaviorGraphAgent>();
            _animator = GetComponentInChildren<Animator>();
            _navAgent = GetComponentInChildren<NavMeshAgent>();
            
            enemyChannel.AddListener<EnemyDanceEvent>(HandleDanceEvent);
        }
        
        
        protected virtual void HandleDanceEvent(EnemyDanceEvent evt)
        {
            _navAgent.enabled = false;
            btAgent.enabled = false;
            foreach (var param in _animator.parameters)
            {
                switch (param.type)
                {
                    case AnimatorControllerParameterType.Float:
                        _animator.SetFloat(param.name, param.defaultFloat);
                        break;
                    case AnimatorControllerParameterType.Int:
                        _animator.SetInteger(param.name, param.defaultInt);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        _animator.SetBool(param.name, false);
                        break;
                }
            }
            Vector3 lookPos = Camera.main.transform.position;
            lookPos.y = 0;
            transform.LookAt(lookPos);
            _animator.SetBool(_danceHash, true);
            
        }

        #endregion

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public BlackboardVariable<T> GetBlackboardVariable<T>(string key)
        {
            if (btAgent.GetVariable(key, out BlackboardVariable<T> result))
            {
                return result;
            }
            return default;
        }
        
        public virtual void TakeHit(Vector3 direction)
        {
            if(isDead)return;

            gameObject.layer = deadLayerNum;
            OnHit?.Invoke();
            enemyChannel.RaiseEvent(EnemyEvent.enemyDeadEvent.Initialize());
            VFXPlayer hitEffect = poolManager.Pop(hitEffectType) as VFXPlayer;
            hitEffect.SetUpAndPlay(transform.position, Quaternion.LookRotation(direction));
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position+ new Vector3(0,1,0),AttackRange);
        }

        protected override void HandleHit()
        {
        }

        [field:SerializeField] public PoolTypeSO PoolType { get; set; }
        public GameObject GameObject => gameObject;
        private Pool _myPool;
        
        public  void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public virtual void ResetItem()
        {
            _navAgent.enabled = true;
            btAgent.enabled = true;
            gameObject.layer = enemyLayerNum;
            _animator.SetBool(_danceHash, false);

        }
    }
}
