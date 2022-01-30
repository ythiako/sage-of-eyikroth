using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageChangeBehaviour : Singleton<StageChangeBehaviour>
{
    [SerializeField, InlineProperty, HideLabel, BoxGroup("Message")] private LocalizedText message;
    [SerializeField, InlineProperty, HideLabel, BoxGroup("Stage No")] private LocalizedText stageMessage;
    
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI textMessage;
    [SerializeField] private TextMeshProUGUI textStageNo;

    private void Awake()
    {
        var i = Instance;
        gameObject.SetActive(false);
    }

    [Button]
    public void DisplayStageSuccess(Action onDisplay, Action onComplete)
    {
        gameObject.SetActive(true);
        
        textMessage.transform.localScale = Vector3.zero;

        textMessage.text = message.GetText(LanguageController.GetCurrentCulture());
        textStageNo.text = string.Format(
            stageMessage.GetText(LanguageController.GetCurrentCulture()), 
            StageController.GetCurrentStageNo().ToString()
        );
        
        background.color = SetColorAlpha(background.color, 0);
        textMessage.color = SetColorAlpha(textMessage.color, 0);
        textStageNo.color = SetColorAlpha(textStageNo.color, 0);

        background.DOFade(1, 2).SetEase(Ease.Linear).OnComplete(() =>
        {
            onDisplay?.Invoke();
            textMessage.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutSine);
            textMessage.DOFade(1, 1).SetEase(Ease.Linear).SetDelay(1).OnComplete(() =>
            {
                textStageNo.DOFade(1, 1).SetEase(Ease.Linear).SetDelay(0.5f).OnComplete(() =>
                {
                    background.DOFade(0, 2).SetEase(Ease.Linear);
                    textMessage.DOFade(0, 2).SetEase(Ease.Linear);
                    textStageNo.DOFade(0, 2).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        onComplete?.Invoke();
                        gameObject.SetActive(false);
                    });
                });
            });
        });
    }

    private static Color SetColorAlpha(Color c, float alpha)
    {
        c.a = alpha;
        return c;
    }
}