using System;
using System.Collections;
using DG.Tweening;
using Febucci.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameTextDisplay : MonoBehaviour
{
    [NonSerialized] public Action Next;
    [NonSerialized] public Action Shown;
    [NonSerialized] public Action Opened;
    [NonSerialized] public Action Closed;

    [SerializeField] private TextAnimatorPlayer textPlayer;

    private bool _isOpen;
    private bool _isComplete;
    private bool _isAnimating;
    private LocalizedText _currentText;

    public bool ShownText => _isComplete;

    public bool CanContinue => !_isComplete && _isOpen && !_isAnimating;

    private void OnEnable()
    {
        textPlayer.onTextShowed.AddListener(OnTextPlayerShowed);
    }

    private void OnDisable()
    {
        textPlayer.onTextShowed.RemoveListener(OnTextPlayerShowed);
    }

    public void DisplayText(LocalizedText text)
    {
        _isComplete = false;
        _currentText = text;

        if (_isOpen)
        {
            textPlayer.ShowText(_currentText.GetText(LanguageController.GetCurrentCulture()));
            textPlayer.StartShowingText();
        }

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            
            _isAnimating = true;
        
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                _isOpen = true;
                _isAnimating = false;
            
                textPlayer.ShowText(_currentText.GetText(LanguageController.GetCurrentCulture()));
                textPlayer.StartShowingText();
                Opened?.Invoke();
            });
        }
    }

    public void Continue()
    {
        if (_isComplete)
        {
            Next?.Invoke();
            Debug.Log("Next");
        }
        else
        {
            textPlayer.SkipTypewriter();
            Debug.Log("Skip typewriter");
        }
    }

    public void Close()
    {
        _isAnimating = true;
        
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            _isOpen = false;
            _isAnimating = false;
            
            gameObject.SetActive(false);
            Closed?.Invoke();
        });
    }

    private void OnTextPlayerShowed()
    {
        _isComplete = true;
        Shown?.Invoke();
    }
}