using Input.InputScript;
using UnityEngine;
using UnityEngine.Events;

namespace Combat.Skills.ShowDown
{
    public class DefaultShowDown : ShowDownSkill
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private LayerMask targetLayer;

        public UnityEvent<float> OnTimerChanged;
        protected override void Update()
        {
            base.Update();
            if (!IsHolding) return;
            OnTimerChanged?.Invoke((1 - timer / showDownData.maxDuration) * 360);
        }

        public override void GetTargets()
        {
            if (playerInput.GetLookInput(targetLayer, out var hitPoint, out var col))
            {
                if (Targets.Contains(col.transform)) return;
                Targets.Add(col.transform);
            }
        }
    }
}