using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviours.StandingIndicator
{
    public class StandingIdicatorElement : MonoBehaviour
    {
        [SerializeField] private Image _indicatorTri;
        [SerializeField] private RectTransform _background;
        
        private int _standingValue;

        public void Initialize(int standing)
        {
            _standingValue = standing;
            SetBarValue(_standingValue);
        }
        public void SetStanding(int standing)
        {
            SetBarValue(standing);
            _standingValue = standing;
        }

        public void AnimateStanding(int value)
        {
            float iterator = _standingValue;
            DOTween.To(() => iterator, f => iterator = f, value, 1f).OnUpdate(() =>
            {
                SetBarValue((int)iterator);
            });
        }

        private void SetBarValue(int value)
        {
            float width = _background.rect.width;
            float indicatorX = width * (value / 100f);

            _indicatorTri.rectTransform.anchoredPosition =
                new Vector2(indicatorX, _indicatorTri.rectTransform.anchoredPosition.y);
        }
    }
}
