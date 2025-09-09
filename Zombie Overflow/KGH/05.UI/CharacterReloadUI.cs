using System;
using Core.GameEvent;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterReloadUI : MonoBehaviour
    {
        [SerializeField] private float rotationDuration = 0.25f;
        [SerializeField] private Image reloadImage;
        [SerializeField] private GameEventChannelSO playerChannel;
        private RectTransform _rectTransform;
        private Camera _mainCamera;

        private bool _isReloading;
        private int _rotCount;
        private float _timer;

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            playerChannel.AddListener<PlayerReloadStatusEvent>(OnReloadStatusChanged);
        }
        
        private void OnDisable()
        {
            playerChannel.RemoveListener<PlayerReloadStatusEvent>(OnReloadStatusChanged);
        }

        private void OnReloadStatusChanged(PlayerReloadStatusEvent obj) => SetReloadUI(obj.isReloading);

        public void SetReloadUI(bool isActive)
        {
            _isReloading = isActive;
            reloadImage.enabled = isActive;
            _rotCount = 0;
            _timer = 0f;
        }

        private void Update()
        {
            if (!_isReloading) return;

            _timer += Time.deltaTime;

            if (_timer >= rotationDuration)
            {
                _rotCount = (_rotCount + 1) % 4;
                _timer = 0f;
            }
            
            var cameraForward = _mainCamera.transform.forward;
            var lookRot = Quaternion.LookRotation(cameraForward, Vector3.up);
            lookRot *= Quaternion.Euler(0, 0, -90 * _rotCount);

            _rectTransform.rotation = lookRot;

        }
    }
}