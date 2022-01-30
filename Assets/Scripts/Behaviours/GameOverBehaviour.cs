using System;
using System.Collections;
using System.Linq;
using Controllers;
using DG.Tweening;
using Febucci.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverBehaviour : Singleton<GameOverBehaviour>
{
    [SerializeField] private Image background;
    [SerializeField] private LocalizedText victoryMessage;
    [SerializeField] private SerializableDictionary<Faction, LocalizedText> gameOverEvent;
    [SerializeField] private LocalizedText gameOver;
    [SerializeField] private LocalizedText youWin;
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private TextAnimatorPlayer textPlayer;

    private Action _onGameOverComplete;
    private bool _isGameOver;
    
    private void Awake()
    {
        var a = Instance;
        gameObject.SetActive(false);
    }

    public float BeginGameOver(Action gameOverComplete)
    {
        var txtColor = textDisplay.color;
        txtColor.a = 1;
        textDisplay.color = txtColor;
        _isGameOver = true;
        textDisplay.text = "";
        textDisplay.fontSize = 56;
        gameObject.SetActive(true);
        var bgColor = background.color;
        bgColor.a = 0;
        background.color = bgColor;
        _onGameOverComplete = gameOverComplete;
        var dict = SageStandingController.GetCriticalStandings();
        var c = dict.Count;
        var randomKey = dict.Keys.Skip(UnityEngine.Random.Range(0, c)).First();
        background.DOFade(1, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            textPlayer.enabled = true;
            textPlayer.ShowText(gameOverEvent[randomKey].GetText(LanguageController.GetCurrentCulture()));
            textPlayer.onTextShowed.AddListener(OnShownReason);
        });
        return 0.5f;
    }

    [Button]
    public void BeginVictory(Action onVictoryComplete)
    {
        var txtColor = textDisplay.color;
        txtColor.a = 1;
        textDisplay.color = txtColor;
        _isGameOver = false;
        textDisplay.text = "";
        textDisplay.fontSize = 56;
        gameObject.SetActive(true);
        var bgColor = background.color;
        bgColor.a = 0;
        background.color = bgColor;
        _onGameOverComplete = onVictoryComplete;
        background.DOFade(1, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            textPlayer.enabled = true;
            textPlayer.ShowText(victoryMessage.GetText(LanguageController.GetCurrentCulture()));
            textPlayer.onTextShowed.AddListener(OnShownReason);
        });
    }

    public void Dispose()
    {
        textPlayer.onTextShowed.RemoveListener(OnShownReason);
        gameObject.SetActive(false);
    }

    private void OnShownReason()
    {
        textPlayer.onTextShowed.RemoveListener(OnShownReason);
        StartCoroutine(DelayedFinishGameOver());
    }

    private IEnumerator DelayedFinishGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        
        textPlayer.enabled = false;

        textDisplay.DOFade(0, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            textDisplay.text = _isGameOver
                ? gameOver.GetText(LanguageController.GetCurrentCulture())
                : youWin.GetText(LanguageController.GetCurrentCulture());

            textDisplay.fontSize = 120;
            textDisplay.transform.localScale = Vector3.one * 0.85f;
            textDisplay.transform.DOScale(Vector3.one, 1.5f).SetDelay(0.5f).SetEase(Ease.Linear);
            textDisplay.DOFade(1, 1.5f).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                textDisplay.DOFade(0, 1.5f).SetDelay(2.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    _onGameOverComplete?.Invoke();
                    background.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(Dispose);
                });
            });
        });
    }
}