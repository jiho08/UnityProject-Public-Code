using System.Collections;
using Code.Core.EventChannel;
using Code.Core.Pool;
using Code.ETC;
using Code.Input;
using EPOOutline;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Player
{
    public class PlayerClick : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO fishInfoChannel;
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private float clickDistance = 10f;

        private PoolTypeSO _currentFeed;

        private PlayerResourceManager _playerResourceManager;
        private Camera _mainCam;
        private int _currentFeedCount;
        private bool _isFixed;

        private void Awake()
        {
            _playerResourceManager = PlayerResourceManager.Instance;
            
            playerInput.OnClickPressed += OnClickReceived;
            playerInput.OnSpacePressed += HandleFixed;
            _playerResourceManager.FeedLevel.Subscribe(UpdateFeed);
        }

        private void Start()
        {
            _mainCam = Camera.main;
        }

        private void OnDestroy()
        {
            playerInput.OnClickPressed -= OnClickReceived;
            playerInput.OnSpacePressed -= HandleFixed;
            _playerResourceManager.FeedLevel.Unsubscribe(UpdateFeed);
        }

        private void OnClickReceived() => StartCoroutine(DelayedClick());

        private IEnumerator DelayedClick()
        {
            yield return null; // 1 프레임 대기

            if (EventSystem.current.IsPointerOverGameObject()) // UI 위에 클릭했을 때
                yield break;

            HandleClick();
        }

        private void HandleClick()
        {
            var ray = _mainCam.ScreenPointToRay(playerInput.ScreenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out AquaticEntities.AquaticEntity aquaticEntity))
                {
                    fishInfoChannel.RaiseEvent(AquaticEntityInfoUIEvents.AquaticEntityClickEvent.Initialize(aquaticEntity));
                    return;
                }

                if (hit.collider.gameObject.TryGetComponent(out Coin coin))
                    coin.GetCoin();
                else
                    SpawnFeed(hit.point);
            }
            else
            {
                var clickPos = ray.origin + ray.direction * clickDistance;
                SpawnFeed(clickPos);
            }

            if (!_isFixed)
                fishInfoChannel.RaiseEvent(AquaticEntityInfoUIEvents.HideAquaticEntityInfoEvent);
        }

        private void SpawnFeed(Vector3 pos)
        {
            if (_currentFeedCount >= PlayerResourceManager.Instance.feedMultiCount)
            {
                // 사운드 재생 등
                return;
            }
            
            if (PlayerResourceManager.Instance.Money.Value < 0)
            {
                return;
            }
            
            var feed = poolManager.Pop(_currentFeed) as Feed.Feed;
            feed.transform.position = pos;
            feed.OnDisableFeed += OnFeedDisable;

            PlayerResourceManager.Instance.Money.Value -= feed.FeedInfo.feedPrice;
            ++_currentFeedCount;
        }

        private void OnFeedDisable()
        {
            --_currentFeedCount;

            if (_currentFeedCount < 0)
                _currentFeedCount = 0;
        }

        private void HandleFixed() => _isFixed = !_isFixed;

        private void UpdateFeed(int value)
        {
            _currentFeed = PlayerResourceManager.Instance.FeedPoolTypes[value - 1];
        }
    }
}