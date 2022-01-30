﻿using System;
using Controllers;
using DG.Tweening;
using Febucci.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenuBehaviour : Singleton<MainMenuBehaviour>
{
    [Title("Intro")] 
    [SerializeField] private LocalizedText intro;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private TextAnimatorPlayer introPlayer;

    [Title("Menu Layout")]
    [SerializeField] private Ease ease;
    [SerializeField] private float delay;
    [SerializeField] private float duration;
    [SerializeField] private Transform initialTransform; 
    [SerializeField] private Transform desiredTransform;
    [SerializeField] private Transform menu;

    [Title("References")] 
    [SerializeField] private PlayableDirector credits;
    [SerializeField] private Button languageButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Title("Texts")] 
    [SerializeField, BoxGroup("New Game Text"), InlineProperty, HideLabel] private LocalizedText newGameText;
    [SerializeField, BoxGroup("Continue Game Text"), InlineProperty, HideLabel] private LocalizedText continueGameText;
    [SerializeField, BoxGroup("Credits Text"), InlineProperty, HideLabel] private LocalizedText creditsText;
    [SerializeField, BoxGroup("Quit Text"), InlineProperty, HideLabel] private LocalizedText quitText;

    private bool _playingIntro;
    
    private void Awake()
    {
        _playingIntro = true;
        newGameButton.GetComponentInChildren<TextMeshProUGUI>().text = newGameText.GetText(LanguageController.GetCurrentCulture());
        continueGameButton.GetComponentInChildren<TextMeshProUGUI>().text = continueGameText.GetText(LanguageController.GetCurrentCulture());
        creditsButton.GetComponentInChildren<TextMeshProUGUI>().text = creditsText.GetText(LanguageController.GetCurrentCulture());
        quitButton.GetComponentInChildren<TextMeshProUGUI>().text = quitText.GetText(LanguageController.GetCurrentCulture());
        introText.text = intro.GetText(LanguageController.GetCurrentCulture());
    }

    private void Update()
    {
        if (_playingIntro && Input.GetMouseButtonDown(0))
        {
            introPlayer.SkipTypewriter();
        }
    }

    private void OnEnable()
    {
        continueGameButton.interactable = PlayerData.CanLoadGame;
        var tmpro = continueGameButton.GetComponentInChildren<TextMeshProUGUI>();
        var c = tmpro.color;
        c.a = continueGameButton.interactable ? 1 : 0.5f;
        tmpro.color = c;
        
        languageButton.GetComponentInChildren<TextMeshProUGUI>().text = PlayerData.CurrentCulture switch
        {
            "en-US" => "EN",
            "tr-TR" => "TR",
            _       => ""
        };
        
        introPlayer.onTextShowed.AddListener(OnIntroPlayed);
        newGameButton.onClick.AddListener(GameController.Instance.StartNewGame);
        continueGameButton.onClick.AddListener(GameController.Instance.ContinueGame);
        quitButton.onClick.AddListener(GameController.Instance.QuitGame);
        languageButton.onClick.AddListener(GameController.Instance.ToggleLanguage);
        creditsButton.onClick.AddListener(ShowCredits);
        
        GameController.LanguageChanged += OnLanguageChanged;
        
        if (!_playingIntro) AnimateMenuIn();
    }

    private void OnDisable()
    {
        if (introPlayer) introPlayer.onTextShowed.RemoveListener(OnIntroPlayed);
        
        if (GameController.Instance)
        {
            newGameButton.onClick.RemoveListener(GameController.Instance.StartNewGame);
            continueGameButton.onClick.RemoveListener(GameController.Instance.ContinueGame);
            quitButton.onClick.RemoveListener(GameController.Instance.QuitGame);
            languageButton.onClick.RemoveListener(GameController.Instance.ToggleLanguage);
            creditsButton.onClick.RemoveListener(ShowCredits);
        }
        
        GameController.LanguageChanged -= OnLanguageChanged;
    }

    private void OnIntroPlayed()
    {
        _playingIntro = false;
        introText.DOFade(0, 1.5f).SetEase(Ease.Linear).SetDelay(2f).OnComplete(AnimateMenuIn);
    }

    private void AnimateMenuIn()
    {
        menu.localPosition = initialTransform.localPosition;
        var tweener = menu.DOLocalMove(desiredTransform.localPosition, duration).SetEase(ease);

        if (delay > 0)
            tweener.SetDelay(delay);
    }

    private void ShowCredits()
    {
        menu.gameObject.SetActive(false);
        credits.gameObject.SetActive(true);
        credits.Play();
        credits.played += OnCreditsPlayed;
    }

    private void OnCreditsPlayed(PlayableDirector director)
    {
        director.time = 0;
        credits.gameObject.SetActive(false);
        credits.played -= OnCreditsPlayed;
        menu.gameObject.SetActive(true);
        AnimateMenuIn();
    }

    private void OnLanguageChanged()
    {
        newGameButton.GetComponentInChildren<TextMeshProUGUI>().text = newGameText.GetText(LanguageController.GetCurrentCulture());
        continueGameButton.GetComponentInChildren<TextMeshProUGUI>().text = continueGameText.GetText(LanguageController.GetCurrentCulture());
        creditsButton.GetComponentInChildren<TextMeshProUGUI>().text = creditsText.GetText(LanguageController.GetCurrentCulture());
        quitButton.GetComponentInChildren<TextMeshProUGUI>().text = quitText.GetText(LanguageController.GetCurrentCulture());
        introText.text = intro.GetText(LanguageController.GetCurrentCulture());
        
        languageButton.GetComponentInChildren<TextMeshProUGUI>().text = PlayerData.CurrentCulture switch
        {
            "en-US" => "EN",
            "tr-TR" => "TR",
            _       => ""
        };
    }
}