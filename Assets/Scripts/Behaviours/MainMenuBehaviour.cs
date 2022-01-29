using System;
using Controllers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBehaviour : Singleton<MainMenuBehaviour>
{
    [Title("References")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Title("Texts")] 
    [SerializeField, BoxGroup("New Game Text"), InlineProperty, HideLabel] private LocalizedText newGameText;
    [SerializeField, BoxGroup("Continue Game Text"), InlineProperty, HideLabel] private LocalizedText continueGameText;
    [SerializeField, BoxGroup("Credits Text"), InlineProperty, HideLabel] private LocalizedText creditsText;
    [SerializeField, BoxGroup("Quit Text"), InlineProperty, HideLabel] private LocalizedText quitText;

    private void Awake()
    {
        newGameButton.GetComponentInChildren<TextMeshProUGUI>().text = newGameText.GetText(LanguageController.GetCurrentCulture());
        continueGameButton.GetComponentInChildren<TextMeshProUGUI>().text = continueGameText.GetText(LanguageController.GetCurrentCulture());
        creditsButton.GetComponentInChildren<TextMeshProUGUI>().text = creditsText.GetText(LanguageController.GetCurrentCulture());
        quitButton.GetComponentInChildren<TextMeshProUGUI>().text = quitText.GetText(LanguageController.GetCurrentCulture());
    }

    private void OnEnable()
    {
        newGameButton.onClick.AddListener(GameController.Instance.StartNewGame);
        continueGameButton.onClick.AddListener(GameController.Instance.ContinueGame);
        creditsButton.onClick.AddListener(GameController.Instance.ShowCredits);
        quitButton.onClick.AddListener(GameController.Instance.QuitGame);
    }

    private void OnDisable()
    {
        if (GameController.Instance)
        {
            newGameButton.onClick.RemoveListener(GameController.Instance.StartNewGame);
            continueGameButton.onClick.RemoveListener(GameController.Instance.ContinueGame);
            creditsButton.onClick.RemoveListener(GameController.Instance.ShowCredits);
            quitButton.onClick.RemoveListener(GameController.Instance.QuitGame);
        }
    }
}