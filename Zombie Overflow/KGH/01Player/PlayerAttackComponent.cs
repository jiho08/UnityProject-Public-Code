using System;
using System.Collections.Generic;
using Combat;
using Core.GameEvent;
using Entities;
using GGMPool;
using Input.InputScript;
using Players;
using UnityEngine;
using UnityEngine.Events;

namespace Players.Combat
{
    public class PlayerAttackComponent : MonoBehaviour, IEntityComponent, IPlayerComponent
    {
        [SerializeField] private PlayerInputSO playerInputSO;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolTypeSO bulletType;
        [SerializeField] private Transform muzzleTransform;
        [SerializeField] private LayerMask hitLayer;
        [SerializeField] private LayerMask enemyLayer;
        public GunData gunData;

        private Player _player;

        private int _currentAmmo;
        public bool IsFullAmmo => _currentAmmo >= gunData.maxAmmo;

        public UnityEvent OnAttackEvent;
        public UnityEvent OnOutOfAmmoEvent;
        public UnityEvent OnReloadEvent;

        public Action OnReloadEnded;

        public void Initialize(Entity entity)
        {
            _player = entity as Player;
        }

        public void SetUpPlayer(CharacterSO character)
        {
            gunData = character.gunData;
            _currentAmmo = gunData.maxAmmo;
        }

        public bool AttemptAttack()
        {
            if (_currentAmmo > 0)
            {
                Attack();
                return true;
            }
            else
            {
                OnOutOfAmmoEvent?.Invoke();
                return false;
            }
        }

        public void Attack()
        {
            _currentAmmo--;
            OnAttackEvent?.Invoke();
            var lookDirection = _player.PlayerInputSO.GetLookInput() - _player.transform.position;
            if (gunData.multiBullet)
                ShootMultipleBullets(lookDirection);
            else
                ShootSingleBullet(lookDirection);
            _player.PlayerEventChannel.RaiseEvent(PlayerEvents.playerBulletShootEvent);
        }

        public List<Projectile> AttackWithoutAmmo(Transform target, PoolTypeSO bulletType = null)
        {
            if (bulletType == null)
                bulletType = this.bulletType;
            var lookDirection = target.position - muzzleTransform.position;
            lookDirection.y = 0;
            OnAttackEvent?.Invoke();
            if (gunData.multiBullet)
                return ShootMultipleBullets(lookDirection, bulletType);
            else
                return new List<Projectile> { ShootSingleBullet(lookDirection, bulletType) };
        }


        public void ShootBurst(Vector3 direction, float angleStep, float spread, PoolTypeSO bulletType = null)
        {
            if (bulletType == null)
                bulletType = this.bulletType;
            
            OnAttackEvent?.Invoke();

            var count = Mathf.CeilToInt(spread / angleStep);
            for (int i = 0; i < count; i++)
            {
                var bullet = poolManager.Pop(bulletType) as Projectile;
                direction.y = 0;
                var angle = -spread / 2 + angleStep * i;
                var rotation = Quaternion.Euler(0, angle, 0);
                var rotatedDir = rotation * direction;
                bullet?.SetUpAndFire(gunData.bulletSpeedMultiplier, muzzleTransform.position, rotatedDir, hitLayer);
            }
        }

        private void ShootSingleBullet(Vector3 lookDirection)
        {
            var bullet = poolManager.Pop(bulletType) as Projectile;
            lookDirection.y = 0;
            bullet?.SetUpAndFire(gunData.bulletSpeedMultiplier, muzzleTransform.position, lookDirection, hitLayer, enemyLayer);
        }

        private Projectile ShootSingleBullet(Vector3 lookDirection, PoolTypeSO poolTypeSo)
        {
            var bullet = poolManager.Pop(poolTypeSo) as Projectile;
            lookDirection.y = 0;
            bullet?.SetUpAndFire(gunData.bulletSpeedMultiplier, muzzleTransform.position, lookDirection, hitLayer);
            return bullet;
        }

        private void ShootMultipleBullets(Vector3 lookDirection)
        {
            var angleBetweenBullets = gunData.bulletSpreadAngle / (gunData.bulletCount - 1);
            for (int i = 0; i < gunData.bulletCount; i++)
            {
                var bullet = poolManager.Pop(bulletType) as Projectile;
                lookDirection.y = 0;
                var angle = -gunData.bulletSpreadAngle / 2 + angleBetweenBullets * i;
                var rotation = Quaternion.Euler(0, angle, 0);
                var rotatedDirection = rotation * lookDirection;
                bullet?.SetUpAndFire(gunData.bulletSpeedMultiplier, muzzleTransform.position, rotatedDirection,
                    hitLayer, enemyLayer);
            }
        }

        private List<Projectile> ShootMultipleBullets(Vector3 lookDirection, PoolTypeSO poolTypeSo)
        {
            var angleBetweenBullets = gunData.bulletSpreadAngle / (gunData.bulletCount - 1);
            var bullets = new List<Projectile>();
            for (int i = 0; i < gunData.bulletCount; i++)
            {
                var bullet = poolManager.Pop(bulletType) as Projectile;
                lookDirection.y = 0;
                var angle = -gunData.bulletSpreadAngle / 2 + angleBetweenBullets * i;
                var rotation = Quaternion.Euler(0, angle, 0);
                var rotatedDirection = rotation * lookDirection;
                bullet?.SetUpAndFire(gunData.bulletSpeedMultiplier, muzzleTransform.position, rotatedDirection,
                    hitLayer);
                bullets.Add(bullet);
            }

            return bullets;
        }

        public void Reload()
        {
            _currentAmmo = Mathf.Clamp(_currentAmmo + 1, 0, gunData.maxAmmo);
            OnReloadEvent?.Invoke();
            _player.PlayerEventChannel.RaiseEvent(PlayerEvents.playerReloadEvent.Initialize(_currentAmmo));
        }

        public void ReloadAll()
        {
            _currentAmmo = gunData.maxAmmo;
            OnReloadEvent?.Invoke();
            _player.PlayerEventChannel.RaiseEvent(PlayerEvents.playerReloadEvent.Initialize(_currentAmmo));
        }
    }
}