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
    [NonSerialized] public Action Opened;
    [NonSerialized] public Action Closed;
    
    [SerializeField] private Button button;
    [SerializeField] private TextAnimatorPlayer textPlayer;

    private bool _isOpen;
    private bool _isComplete;
    private LocalizedText _currentText;

    private void OnEnable()
    {
        button.onClick.AddListener(OnButtonClicked);
        textPlayer.onTextShowed.AddListener(OnTextPlayerShowed);

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            _isOpen = true;
            textPlayer.ShowText(_currentText.GetText(LanguageController.GetCurrentCulture()));
            textPlayer.StartShowingText();
            Opened?.Invoke();
        });
    }

    public void Close()
    {
        button.onClick.RemoveListener(OnButtonClicked);
        textPlayer.onTextShowed.RemoveListener(OnTextPlayerShowed);
        
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
            _isOpen = false;
            Closed?.Invoke();
        });
    }

    [Button]
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
            gameObject.SetActive(true);
    }

    private void OnButtonClicked()
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

    private void OnTextPlayerShowed()
    {
        _isComplete = true;
    }
}