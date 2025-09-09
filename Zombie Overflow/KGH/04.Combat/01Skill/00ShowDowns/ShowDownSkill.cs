using Core;
using Core.Dependencies;
using Entities;
using GGMPool;
using Players;
using Players.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace Combat.Skills.ShowDown
{
    public abstract class ShowDownSkill : Skill, IReleasable
    {
        [SerializeField] protected PoolTypeSO projectileType;
        public ShowDownData showDownData;

        [Inject] protected Player _player;
        protected CharacterMovement _movement;
        protected SplitEntityRenderer _renderer;
        protected PlayerAttackComponent _attackComponent;

        private bool _rotationTrigger;
        public bool IsHolding { get; private set; }
        protected float timer;
        
        public UnityEvent<bool> OnSkillStatusChanged;
        public UnityEvent OnShowDown;
        
        protected float _defaultRotSpeed;
        protected bool _stopSkill;

        protected virtual void Start()
        {
            _movement = _player.GetCompo<CharacterMovement>();
            _renderer = _player.GetCompo<SplitEntityRenderer>();
            _attackComponent = _player.GetCompo<PlayerAttackComponent>();
            _movement.OnRotationEnd += HandleRotationEnd;
            
        }

        private void OnDestroy()
        {
            _movement.OnRotationEnd -= HandleRotationEnd;
        }

        private void HandleRotationEnd()
        {
            _rotationTrigger = true;
        }

        protected override void UseSkill()
        {
            base.UseSkill();
            _stopSkill = false;
            _defaultRotSpeed = _movement.rotationSpeed;
            _movement.rotationSpeed /= showDownData.timeMultiplier;
            OnSkillStatusChanged?.Invoke(true);
            IsHolding = true;
            _rotationTrigger = false;
            Time.timeScale = showDownData.timeMultiplier;
            timer = 0f;
        }

        protected virtual void Update()
        {
            if (!IsHolding) return;
            GetTargets();
            timer += Time.unscaledDeltaTime;
            if (timer >= showDownData.maxDuration)
            {
                ReleaseSkill();
            }
        }

        public void ReleaseSkill()
        {
            if (!IsHolding) return;
            Time.timeScale = 1f;
            IsHolding = false;
            ShowDown();
        }

        protected virtual async void ShowDown()
        {
            if (ResetStopSkill()) return;
            
            if (Targets.Count == 0)
            {
                CancelSkill();
                return;
            }
            OnShowDown?.Invoke();
            
            _player.DoesLookAtMouse = false;
            _movement.rotationSpeed = _defaultRotSpeed * Targets.Count;

            foreach (var target in Targets)
            {
                if (ResetStopSkill()) return;

                await RotateTowardsTarget(target);

                if (ResetStopSkill()) return;

                HandleProjectileAttack(target);
                _renderer.Attack();
            }

            FinalizeShowDown();
        }

        private bool ResetStopSkill()
        {
            if (_stopSkill)
            {
                _stopSkill = false;
                return true;
            }
            return false;
        }

        private async System.Threading.Tasks.Task RotateTowardsTarget(Transform target)
        {
            _rotationTrigger = false;
            while (!_rotationTrigger)
            {
                if (ResetStopSkill()) return;

                Vector3 direction = target.position - _player.transform.position;
                direction.y = 0;
                _movement.SetRotationDirection(direction);
                await Awaitable.NextFrameAsync();
            }
        }

        private void HandleProjectileAttack(Transform target)
        {
            var bullets = _attackComponent.AttackWithoutAmmo(target, projectileType);
            foreach (var bullet in bullets)
            {
                if (bullet is ShowDownProjectile projectile)
                {
                    projectile.SetTarget(target);
                }
            }
        }

        private void FinalizeShowDown()
        {
            _attackComponent.ReloadAll();
            _player.DoesLookAtMouse = true;
            _movement.rotationSpeed = _defaultRotSpeed;
            OnSkillEnd?.Invoke();
            OnSkillStatusChanged?.Invoke(false);
        }

        public virtual void CancelSkill()
        {
            Time.timeScale = 1f;
            IsHolding = false;
            _player.DoesLookAtMouse = true;
            OnSkillStatusChanged?.Invoke(false);
            _stopSkill = true;
        }
        
        public override float SetSkillGauge(float gauge)
        {
            base.SetSkillGauge(gauge);
            remainSkillEnergy = Mathf.Clamp(remainSkillEnergy, 0, showDownData.maxSkillEnergy);
            return Mathf.Clamp(gauge, 0, showDownData.maxSkillEnergy);
        }

        public abstract void GetTargets();
    }
}