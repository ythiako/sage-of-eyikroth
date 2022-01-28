using System;
using System.Collections.Generic;
using Behavoiurs;
using Cinemachine;
using Models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

public class DecisionPhaseController : MonoBehaviour
{
    private static DecisionPhaseController _instance;
    public static DecisionPhaseController Instance =>
        _instance ? _instance : FindObjectOfType<DecisionPhaseController>();
    
    public static event Action<Decision> DecisionMade;

    [SerializeField] private CinemachineVirtualCamera stageVirtualCamera;
    [SerializeField] private PlayableDirector director;

    [Title("Text Displays")] 
    [SerializeField] private TextAnimatorInputBehaviour textInputMediator;
    [SerializeField] private GameTextDisplay leftTextDisplay;
    [SerializeField] private GameTextDisplay rightTextDisplay;

    [Title("Characters")] 
    [SerializeField] private Transform leftCharacterRoot;
    [SerializeField] private Transform rightCharacterRoot;

    [Title("Decisions")] 
    [SerializeField] private DecisionBehaviour leftDecision;
    [SerializeField] private DecisionBehaviour rightDecision;
    [SerializeField] private List<DecisionBehaviour> optionalDecisions;

    private Conflict _conflict;
    private bool _rightSummaryShown;
    private bool _leftSummaryShown;

    private FactionRepresentativeBehaviour _leftRepresentative;
    private FactionRepresentativeBehaviour _rightRepresentative;

    public void BeginDecisionPhase(Conflict conflict)
    {
        _conflict = conflict;
        _leftSummaryShown = false;
        _rightSummaryShown = false;

        textInputMediator.enabled = false;
        leftTextDisplay.gameObject.SetActive(false);
        rightTextDisplay.gameObject.SetActive(false);
        
        // Spawn faction representatives
        
        director.Play();
    }

    public void DisplayDialogues()
    {
        leftTextDisplay.DisplayText(_conflict.aSummary.line);
        rightTextDisplay.DisplayText(_conflict.aSummary.line);

        leftTextDisplay.Shown = () =>
        {
            _leftSummaryShown = true;
            if (_leftSummaryShown && _rightSummaryShown)
            {
                DisplayDecisions();
                textInputMediator.enabled = true;

                leftTextDisplay.Next = leftTextDisplay.Close;
                rightTextDisplay.Next = rightTextDisplay.Close;
            }
        };
        
        rightTextDisplay.Shown = () =>
        {
            _rightSummaryShown = true;
            if (_leftSummaryShown && _rightSummaryShown)
            {
                DisplayDecisions();
                textInputMediator.enabled = true;

                leftTextDisplay.Next = leftTextDisplay.Close;
                rightTextDisplay.Next = rightTextDisplay.Close;
            }
        };

        leftTextDisplay.Closed = DisplayDecisions;
    }
    
    public void MakeDecision(Decision decision)
    {
        leftDecision.gameObject.SetActive(false);
        rightDecision.gameObject.SetActive(false);
        
        GlobalFlagsController.SetFlag(decision.id);
        DecisionMade?.Invoke(decision);
    }

    private void DisplayDecisions()
    {
        leftDecision.gameObject.SetActive(true);
        rightDecision.gameObject.SetActive(true);
        
        leftDecision.Set(_conflict.aDecision);
        rightDecision.Set(_conflict.bDecision);
    }
}