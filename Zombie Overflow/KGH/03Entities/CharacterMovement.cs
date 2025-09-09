using System;
using Core;
using Core.Define;
using Entities.Stat;
using UnityEngine;

namespace Entities
{
    public class CharacterMovement : EntityMovement
    {
        public float rotationSpeed = 8f;
        [SerializeField] private float gravity = -9.8f;
        [field: SerializeField] public bool DoesAutoRotate { get; set; } = true;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private CapsuleCollider playerCollider;

        private Vector3 _autoMovement;
        public bool IsGround => characterController.isGrounded;
        private Vector3 _velocity;
        public Vector3 Velocity => _velocity;

        private float _verticalVelocity;
        private Vector3 _movementDirection;
        private Vector3 _lookPosition;

        private bool _didRotateEnd;
        public Action OnRotationEnd;

        private float _defaultRadius;
        private float _defaultHeight;

        private void Awake()
        {
            _defaultRadius = playerCollider.radius;
            _defaultHeight = playerCollider.height;
        }

        public void SetMovementDirection(Vector2 movementDirection)
        {
            _movementDirection = new Vector3(movementDirection.x, 0, movementDirection.y).normalized;
        }

        public void SetMovementDirection(Vector3 movementDirection)
        {
            _movementDirection = movementDirection.normalized;
        }

        public void SetRotationDirection(Vector3 lookPosition)
        {
            _didRotateEnd = false;
            _lookPosition = lookPosition;
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            ApplyGravity();
            Move();
        }

        private void CalculateMovement()
        {
            if (CanManualMovement)
            {
                _velocity = _movementDirection;
                _velocity *= moveSpeed * Time.fixedDeltaTime;
            }
            else
            {
                _velocity = _autoMovement * Time.fixedDeltaTime;
            }

            if (!DoesAutoRotate && _lookPosition != Vector3.zero)
            {
                var targetRot = Quaternion.LookRotation(_lookPosition);
                entity.transform.rotation = Quaternion.Lerp(entity.transform.rotation, targetRot,
                    Time.fixedDeltaTime * rotationSpeed);

                var didRot = Quaternion.Angle(entity.transform.rotation, targetRot) < ConstDefine.RotationThreshold;
                if (_didRotateEnd != didRot)
                {
                    _didRotateEnd = didRot;
                    if (_didRotateEnd)
                        OnRotationEnd?.Invoke();
                }
            }
            else if (_velocity.magnitude > 0)
            {
                var targetRot = Quaternion.LookRotation(_velocity);
                entity.transform.rotation = Quaternion.Lerp(entity.transform.rotation, targetRot,
                    Time.fixedDeltaTime * rotationSpeed);
            }
        }

        private void ApplyGravity()
        {
            if (IsGround && _verticalVelocity < 0)
            {
                _verticalVelocity = -0.03f;
            }
            else
            {
                _verticalVelocity += gravity * Time.fixedDeltaTime;
            }

            _velocity.y = _verticalVelocity;
        }

        public void SetColliderSize(float radius, float height)
        {
            playerCollider.radius = radius;
            playerCollider.height = height;
            playerCollider.center = new Vector3(0, height / 2, 0);
        }

        public void ResetColliderSize()
        {
            playerCollider.radius = _defaultRadius;
            playerCollider.height = _defaultHeight;
            playerCollider.center = new Vector3(0, _defaultHeight / 2, 0);
        }

        protected override void Move()
        {
            characterController.Move(_velocity);
        }

        public void SetAutoMovement(Vector3 movement)
        {
            _autoMovement = movement;
        }

        public override void StopImmediately()
        {
            _movementDirection = Vector3.zero;
        }
    }
}