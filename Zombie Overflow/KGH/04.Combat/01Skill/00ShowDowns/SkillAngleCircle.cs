using UnityEngine;
using UnityEngine.UI;

namespace Combat.Skills.ShowDown
{
    public class SkillAngleCircle : MonoBehaviour
    {
        [SerializeField] private Image circleImage;
        public void SetAngle(float angle)
        {
            circleImage.fillAmount = angle / 360f;
            circleImage.rectTransform.localRotation = Quaternion.Euler(0, 0, angle / 2);
        }
    }
}