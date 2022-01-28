using System;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimatorInputBehaviour : MonoBehaviour
{
    [SerializeField] private Button button;
    
    private GameTextDisplay[] _gameTextDisplays;

    private void Awake()
    {
        _gameTextDisplays = GetComponentsInChildren<GameTextDisplay>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        foreach (var gameTextDisplay in _gameTextDisplays)
        {
            if (gameTextDisplay.CanContinue)
            {
                gameTextDisplay.Continue();
            }
        }
    }
}