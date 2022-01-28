using System;
using Febucci.UI;
using UnityEngine;
using UnityEngine.UI;

public class GameTextDisplay : MonoBehaviour
{
    [NonSerialized] public Action Next;
    
    [SerializeField] private Button button;
    [SerializeField] private TextAnimatorPlayer textPlayer;

    private bool _isComplete;

    private void OnEnable()
    {
        button.onClick.AddListener(OnButtonClicked);
        textPlayer.onTextShowed.AddListener(OnTextPlayerShowed);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClicked);
        textPlayer.onTextShowed.RemoveListener(OnTextPlayerShowed);
    }

    public void DisplayText(string text)
    {
        textPlayer.ShowText(text);
        textPlayer.StartShowingText();
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