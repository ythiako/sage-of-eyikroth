using System;
using System.Collections.Generic;
using UnityEngine;

public class DecisionPhaseController : MonoBehaviour
{
    public static event Action<Decision> DecisionMade;
    
    public void MakeDecision(Decision decision)
    {
        GlobalFlagsController.SetFlag(decision.id);
        DecisionMade?.Invoke(decision);
    }
}