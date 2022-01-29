using System;
using Behaviours.Exploration;
using Behavoiurs;
using Controllers;
using Models;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private void Awake()
    {
        MainMenuBehaviour.Instance.gameObject.SetActive(true);
        DecisionPhaseController.Instance.Dispose();
        ConflictController.Instance.Dispose();
        ExplorationPhaseController.Instance.EndExplorationPhase();
        
        DecisionPhaseController.Completed += OnDecisionPhaseComplete;
        ExplorationPhaseController.ExplorationPhaseEnded += OnExplorationFinished;
    }

    private void OnDestroy()
    {
        DecisionPhaseController.Completed -= OnDecisionPhaseComplete;
        ExplorationPhaseController.ExplorationPhaseEnded -= OnExplorationFinished;
    }

    public void StartNewGame()
    {
        MainMenuBehaviour.Instance.gameObject.SetActive(false);
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
        MainMenuBehaviour.Instance.gameObject.SetActive(false);
        SageStandingController.Load();
        GlobalFlagsController.LoadFlagCollection();
        FadeTransition.Instance.FadeIn(() =>
        {
            //StageController.PrepareStage();
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
        FadeTransition.Instance.FadeIn(() =>
        {
            ConflictController.Instance.Dispose();
            ExplorationPhaseController.Instance.BeginExplorationPhase();
            FadeTransition.Instance.FadeOut(() => { });
        });
    }

    private void OnExplorationFinished()
    {
        FadeTransition.Instance.FadeIn(() =>
        {
            DecisionPhaseController.Instance.BeginDecisionPhase(StageController.GetCurrentConflict());
            FadeTransition.Instance.FadeOut(() => { });
        });
    }

    private void OnDecisionPhaseComplete()
    {
        FadeTransition.Instance.FadeIn(() =>
        {
            DecisionPhaseController.Instance.Dispose();
            StageController.NextConflict();
            ConflictController.Instance.PrepareConflict(StageController.GetCurrentConflict());
            
            FadeTransition.Instance.FadeOut(() =>
            {
                ConflictController.Instance.PlayConflict();
            });

            ConflictController.Instance.PhaseFinished = OnConflictPhaseFinished;
        });
    }
}