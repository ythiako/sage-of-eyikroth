using System;
using Behaviours.Exploration;
using Behavoiurs;
using Controllers;
using Models;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] private float minimumThreshold;
    [SerializeField] private float maximumThreshold;
    
    private void Awake()
    {
        MainMenuBehaviour.Instance.gameObject.SetActive(true);
        DecisionPhaseController.Instance.Dispose();
        ConflictController.Instance.Dispose();
        StageController.Initialize();
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
        PlayerData.CanLoadGame = true;
        
        SageStandingController.StartNew();
        GlobalFlagsController.NewFlagCollection();
        FadeTransition.Instance.FadeIn(() =>
        {
            MainMenuBehaviour.Instance.gameObject.SetActive(false);
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
        StageController.Load();
        SageStandingController.Load();
        GlobalFlagsController.LoadFlagCollection();
        FadeTransition.Instance.FadeIn(() =>
        {
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
        if (StageController.NextConflict())
        {
            var criticalStandings = SageStandingController.GetCriticalStandings();
            if (criticalStandings.Count > 0)
            {
                GameOver();
            }
            else
            {
                if (StageController.IsComplete)
                {
                    Victory();
                }
                else
                {
                    StageChangeBehaviour.Instance.DisplayStageSuccess(ToNextConflict, ConflictController.Instance.PlayConflict);
                }
            }
        }
        else
        {
            FadeTransition.Instance.FadeIn(() =>
            {
                DecisionPhaseController.Instance.Dispose();
                ToNextConflict();
                FadeTransition.Instance.FadeOut(ConflictController.Instance.PlayConflict);
            });
        }
    }

    private void ToNextConflict()
    {
        DecisionPhaseController.Instance.Dispose();
        ConflictController.Instance.PrepareConflict(StageController.GetCurrentConflict());

        ConflictController.Instance.PhaseFinished = OnConflictPhaseFinished;
    }

    private static void GameOver()
    {
        DecisionPhaseController.Instance.Dispose();
        GameOverBehaviour.Instance.BeginGameOver(() =>
        {
            StageController.Reset();
            SageStandingController.StartNew();
            GlobalFlagsController.NewFlagCollection();
            PlayerData.CanLoadGame = false;
            MainMenuBehaviour.Instance.gameObject.SetActive(true);
        });
    }

    private static void Victory()
    {
        DecisionPhaseController.Instance.Dispose();
        GameOverBehaviour.Instance.BeginVictory(() =>
        {
            StageController.Reset();
            SageStandingController.StartNew();
            GlobalFlagsController.NewFlagCollection();
            PlayerData.CanLoadGame = false;
            MainMenuBehaviour.Instance.gameObject.SetActive(true);
        });
    }

    [Button(ButtonSizes.Large)]
    private void TestGameOver()
    {
        SageStandingController.SetStanding(Faction.Elves, 10);
        GameOver();
    }

    [Button(ButtonSizes.Large)]
    private void TestVictory()
    {
        Victory();
    }
}