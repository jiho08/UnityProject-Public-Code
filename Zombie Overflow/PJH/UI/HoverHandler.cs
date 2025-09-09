using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Vector3 hoverScale = new(1.25f, 1.25f);
        [SerializeField] private Vector3 hoverRotation = new(0f, 0f, 0f);
        [SerializeField] private float animationDuration = 0.2f;

        private Vector3 _originalScale;
        private Vector3 _originalRotation;
        private Sequence _sequence;
        
        private void Start()
        {
            _originalScale = transform.localScale;
            _originalRotation = transform.localEulerAngles;
            _sequence = DOTween.Sequence();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            StopAllCoroutines();
            
            if (_sequence.IsActive() && _sequence.IsPlaying())
                _sequence.Kill();
            
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOScale(hoverScale, animationDuration))
                .Append(transform.DOLocalRotate(hoverRotation, animationDuration))
                .SetEase(Ease.OutBack).OnComplete(() =>
                {
                    transform.localScale = hoverScale;
                    transform.localEulerAngles = hoverRotation;
                });
            
            _sequence.Play();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            
            if (_sequence.IsActive() && _sequence.IsPlaying())
                _sequence.Kill();
            
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOScale(_originalScale, animationDuration))
                .Append(transform.DOLocalRotate(_originalRotation, animationDuration))
                .SetEase(Ease.OutBack).OnComplete(() =>
                {
                    transform.localScale = _originalScale;
                    transform.localEulerAngles = _originalRotation;
                });
            
            _sequence.Play();
        }
    }
}