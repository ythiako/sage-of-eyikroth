using DG.Tweening;
using UnityEngine;

namespace Behaviours.Exploration
{
    public class KingdomMapElement : MonoBehaviour
    {
        public Faction _factionType;
        [SerializeField] private SpriteRenderer _outline;

        private Tweener _highlightTweener;
        public void Highlight()
        {
            _highlightTweener = _outline.DOFade(0.5f, 0.5f)
                .From(0)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void StopHighlight()
        {
            if (_highlightTweener != null)
            {
                _highlightTweener.Kill();
                _highlightTweener = null;

                var outlineColor = _outline.color;
                outlineColor.a = 0;
                _outline.color = outlineColor;
            }
        }
    }
}
