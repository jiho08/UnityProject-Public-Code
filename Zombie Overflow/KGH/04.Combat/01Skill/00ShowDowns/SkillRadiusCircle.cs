using System;
using UnityEngine;

namespace Combat.Skills.ShowDown
{
    public class SkillRadiusCircle : MonoBehaviour
    {
        [SerializeField] private float outlineRadius = 0.1f;
        private float _initY;

        private void Awake()
        {
            _initY = transform.localScale.y;
        }

        public void SetRadius(float radius)
        {
            radius *= 2;
            if (radius > 0)
            {
                radius += outlineRadius;
            }

            transform.localScale = new Vector3(radius, _initY, radius);
        }
    }
}