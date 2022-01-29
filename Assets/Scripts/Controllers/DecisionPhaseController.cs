using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behavoiurs;
using Cinemachine;
using Models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

public class DecisionPhaseController : Singleton<DecisionPhaseController>
{
    public static event Action<Decision> DecisionMade;
    public static event Action Completed;

    [SerializeField] private CinemachineVirtualCamera stageInitialVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera stageVirtualCamera;
    [SerializeField] private PlayableDirector director;

    [Title("Text Displays")] 
    [SerializeField] private GameTextDisplay leftTextDisplay;
    [SerializeField] private GameTextDisplay rightTextDisplay;

    [Title("Characters")] 
    [SerializeField] private Transform leftCharacterRoot;
    [SerializeField] private Transform rightCharacterRoot;

    [SerializeField] private List<DecisionBehaviour> decisions;

    private Conflict _conflict;
    private Decision _decision;
    private bool _rightTextShown;
    private bool _leftTextShown;
    private int _currentOutcomeDialogueIndex;

    private FactionRepresentativeBehaviour _leftRepresentative;
    private FactionRepresentativeBehaviour _rightRepresentative;

    [Button]
    public void BeginDecisionPhase(Conflict conflict)
    {
        gameObject.SetActive(true);
        
        _decision = null;
        _conflict = conflict;
        _leftTextShown = false;
        _rightTextShown = false;

        stageVirtualCamera.Priority = 0;
        stageInitialVirtualCamera.Priority = 10;

        leftTextDisplay.gameObject.SetActive(false);
        rightTextDisplay.gameObject.SetActive(false);

        _leftRepresentative = Instantiate(AssetsController.Instance.GetFactionRepresentativePrefab(_conflict.aSummary.faction), leftCharacterRoot);
        _rightRepresentative = Instantiate(AssetsController.Instance.GetFactionRepresentativePrefab(_conflict.bSummary.faction), rightCharacterRoot);
        
        foreach (var decision in decisions)
            decision.gameObject.SetActive(false);
        
        director.Play();
    }

    public void DisplaySummaries()
    {
        director.Pause();
        
        leftTextDisplay.DisplayText(_conflict.aSummary.line);
        rightTextDisplay.DisplayText(_conflict.bSummary.line);

        leftTextDisplay.Shown = () =>
        {
            _leftTextShown = true;
            if (_leftTextShown && _rightTextShown)
            {
                DisplayDecisions();
            }
        };
        
        rightTextDisplay.Shown = () =>
        {
            _rightTextShown = true;
            if (_leftTextShown && _rightTextShown)
            {
                DisplayDecisions();
            }
        };
    }
    
    public void MakeDecision(Decision decision)
    {
        _decision = decision;

        foreach (var d in decisions)
            d.gameObject.SetActive(false);
        
        foreach (var outcome in decision.outcomes)
        {
            var currentStanding = SageStandingController.GetStanding(outcome.Key);
            SageStandingController.SetStanding(outcome.Key, currentStanding + outcome.Value);
        }
        
        SageStandingController.SaveStanding();
        GlobalFlagsController.SetFlag(decision.id);
        DecisionMade?.Invoke(decision);
        DisplayDialogues();
    }

    public void OnRepresentativesMovingToSage()
    {
        stageVirtualCamera.Priority = 10;
        stageInitialVirtualCamera.Priority = 0;
    }

    public void OnRepresentativesQuitRoom()
    {
        Completed?.Invoke();
    }

    public void Dispose()
    {
        if (_leftRepresentative.gameObject)    Destroy(_leftRepresentative.gameObject);
        if (_rightRepresentative.gameObject)   Destroy(_rightRepresentative.gameObject);
        
        leftTextDisplay.Next = null;
        rightTextDisplay.Next = null;
        
        leftTextDisplay.Closed = null;
        rightTextDisplay.Closed = null;
        
        gameObject.SetActive(false);
    }

    private void DisplayDecisions()
    {
        decisions[0].gameObject.SetActive(true);
        decisions[1].gameObject.SetActive(true);
        
        decisions[0].Set(_conflict.aDecision);
        decisions[1].Set(_conflict.bDecision);

        var optionalDecisionCount = _conflict.optionalDecisions.Count(od => od.IsUnlocked);
        
        for (int i = 0; i < Mathf.Min(decisions.Count - 2, optionalDecisionCount); i++)
        {
            decisions[i + 2].gameObject.SetActive(true);
            decisions[i + 2].Set(_conflict.optionalDecisions[i]);
        }
    }

    private void DisplayDialogues()
    {
        var left = new List<LocalizedText>();
        var right = new List<LocalizedText>();

        foreach (var outcomeDialogue in _conflict.outcomeDialogue[_decision.id].lines)
        {
            if (outcomeDialogue.faction == _conflict.aSummary.faction)
                left.Add(outcomeDialogue.line);
            else if (outcomeDialogue.faction == _conflict.bSummary.faction)
                right.Add(outcomeDialogue.line);
        }

        _currentOutcomeDialogueIndex = 0;
        ShowDialogueOutcome();


        void ShowDialogueOutcome()
        {
            if (_currentOutcomeDialogueIndex < left.Count)
            {
                _leftTextShown = false;
                leftTextDisplay.DisplayText(left[_currentOutcomeDialogueIndex]);
                leftTextDisplay.Shown = () =>
                {
                    _leftTextShown = true;
                    if (_leftTextShown && _rightTextShown)
                    {
                        StartCoroutine(Delay(() =>
                        {
                            _currentOutcomeDialogueIndex++;
                            ShowDialogueOutcome();
                        }));
                    }
                };
            }
            else
            {
                _leftTextShown = true;
            }

            if (_currentOutcomeDialogueIndex < right.Count)
            {
                _rightTextShown = false;
                rightTextDisplay.DisplayText(right[_currentOutcomeDialogueIndex]);
                rightTextDisplay.Shown = () =>
                {
                    _rightTextShown = true;
                    if (_leftTextShown && _rightTextShown)
                    {
                        StartCoroutine(Delay(() =>
                        {
                            _currentOutcomeDialogueIndex++;
                            ShowDialogueOutcome();
                        }));
                    }
                };
            }

            if (_leftTextShown && _rightTextShown)
            {
                StartCoroutine(Delay(() =>
                {
                    leftTextDisplay.Close();
                    rightTextDisplay.Close();
                    director.Resume();
                }));
            }
        }
    }

    private IEnumerator Delay(Action callback)
    {
        yield return new WaitForSeconds(1);
        callback?.Invoke();
    }
}