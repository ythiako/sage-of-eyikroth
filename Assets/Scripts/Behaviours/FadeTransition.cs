using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : Singleton<FadeTransition>
{
    [SerializeField] private Image image;

    private bool _animating;
    private bool _opened;
    
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    [Button]
    public void FadeIn(Action onComplete)
    {
        if (_animating || _opened) return;
        
        gameObject.SetActive(true);
        
        var c = image.color;
        c.a = 0;
        image.color = c;

        _animating = true;

        image.DOFade(1, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            _opened = true;
            _animating = false;

            StartCoroutine(DelayedInvoke(onComplete));
        });
    }

    [Button]
    public void FadeOut(Action onComplete)
    {
        if (_animating || !_opened) return;
        
        var c = image.color;
        c.a = 1;
        image.color = c;

        _animating = true;

        image.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            _opened = false;
            _animating = false;

            StartCoroutine(DelayedInvoke(() =>
            {
                onComplete?.Invoke();
                gameObject.SetActive(false);
            }));
        });
    }

    private IEnumerator DelayedInvoke(Action callback)
    {
        yield return new WaitForSeconds(1f);
        callback?.Invoke();
    }
}