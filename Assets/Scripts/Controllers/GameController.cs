using System;
using Controllers;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private void Awake()
    {
        MainMenuBehaviour.Instance.gameObject.SetActive(true);
        DecisionPhaseController.Instance.Dispose();
    }

    public void StartNewGame()
    {
        SageStandingController.StartNew();
        GlobalFlagsController.NewFlagCollection();
        FadeTransition.Instance.FadeIn(() =>
        {
            StageController.Reset();
            StageController.PrepareStage();
            ConflictController.Instance.PrepareConflict(StageController.GetCurrentConflict());
            
            FadeTransition.Instance.FadeOut(() =>
            {
                ConflictController.Instance.PlayConflict();
            });

            ConflictController.Instance.PhaseFinished = OnConflictPhaseFinished;
        });
    }

    public void ContinueGame()
    {
        SageStandingController.Load();
        GlobalFlagsController.LoadFlagCollection();
        FadeTransition.Instance.FadeIn(() =>
        {
            StageController.PrepareStage();
            ConflictController.Instance.PrepareConflict(StageController.GetCurrentConflict());
            
            FadeTransition.Instance.FadeOut(() =>
            {
                ConflictController.Instance.PlayConflict();
            });

            ConflictController.Instance.PhaseFinished = OnConflictPhaseFinished;
        });
    }

    public void ShowCredits()
    {
        
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.ExitPlaymode();
        }
#else
            Application.Quit();
#endif
    }

    private void OnConflictPhaseFinished()
    {
        // Check if all conflicts in stage is finished
            // If so, play stage transition
        // Otherwise
            // move on to next conflict with generic fade transition
    }
}