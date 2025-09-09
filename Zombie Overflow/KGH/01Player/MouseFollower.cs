using Core.Dependencies;
using Input.InputScript;
using UnityEngine;

namespace Players
{
    public class MouseFollower : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [Range(0.001f, 10)] [SerializeField] private float distanceFromPlayer = 5f;
        [Inject] private Player _player;

        private void Update()
        {
            var mousePosition = playerInput.GetLookInput();
            var playerPosition = _player.transform.position;
            var direction = (mousePosition - playerPosition).normalized;
            var distance = Mathf.Clamp(Vector3.Distance(mousePosition, playerPosition), 0, distanceFromPlayer) / distanceFromPlayer;
            var easedDistance = (1 - Mathf.Pow(1 - distance, 4)) * distanceFromPlayer;
            var targetPosition = playerPosition + direction * easedDistance;
            targetPosition.y = playerPosition.y;
            transform.position = targetPosition;
        }
    }
}