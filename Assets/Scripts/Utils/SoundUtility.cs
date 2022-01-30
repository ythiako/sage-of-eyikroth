using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class SoundUtility : Singleton<SoundUtility>
    {
        public void Crossfade(AudioSource from, AudioSource to, float duration)
        {
            to.volume = 0;
            from.volume = 1;
            DOTween.To(() => from.volume, v => from.volume = v, 0, duration).SetEase(Ease.Linear).OnComplete(from.Stop);
            DOTween.To(() => to.volume, v => to.volume = v, 1, duration).SetEase(Ease.Linear);
            to.Play();
        }
    }
}