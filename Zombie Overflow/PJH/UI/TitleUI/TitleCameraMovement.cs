using UnityEngine;

namespace UI.Title
{
    public class TitleCameraMovement : MonoBehaviour
    {
        [SerializeField] private Vector3 destination;
        [SerializeField] private float moveSpeed;

        private const float _arriveThreshold = 0.1f;
        
        private Vector3 _startPosition;
        private int _dir = 1;

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            if (IsArrived())
            {
                _dir *= -1;
                (destination, _startPosition) = (_startPosition, destination);
            }

            transform.position += Vector3.right * (_dir * Time.deltaTime * moveSpeed);
        }

        private bool IsArrived() => Mathf.Abs(transform.position.x - destination.x) <= _arriveThreshold;
    }
}