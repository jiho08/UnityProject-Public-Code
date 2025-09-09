using Input.InputScript;
using UnityEngine;

namespace Combat.Skills.ShowDown
{
    public class SkillFollowCircle : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float yOffset = 0.5f;
        private bool _isShow;
        public void ShowCircle(bool show)
        {
            transform.localScale = show ? Vector3.one : Vector3.zero;
            _isShow = show;
        }
        private void Update()
        {
            if (!_isShow) return;
            SetPosition();
        }
        private void SetPosition()
        {
            if (!_isShow) return;
            var input = playerInput.GetLookInput(groundLayer, out var hitPoint, out var col);
            hitPoint += Vector3.up * yOffset;
            transform.position = hitPoint;
        }
    }
}