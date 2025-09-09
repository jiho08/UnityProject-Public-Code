using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame
{
    public class SkillGaugeBarUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private Image backgroundImage;

        public void SetFillColor(Color fillColor)
        {
            fillImage.color = fillColor;
        }

        public void SetGaugeWidth(float width)
        {
            fillImage.rectTransform.sizeDelta = new Vector2(width, fillImage.rectTransform.sizeDelta.y);
            backgroundImage.rectTransform.sizeDelta = new Vector2(width, backgroundImage.rectTransform.sizeDelta.y);
        }

        public void SetGaugeValue(float value)
        {
            fillImage.fillAmount = value;
        }
    }
}