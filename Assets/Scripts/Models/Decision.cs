using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class Decision
{
    [LabelWidth(30)] public string id;
    [LabelWidth(120)] public string unlockingFlag;
    public LocalizedText text;
    public SerializableDictionary<Faction, int> outcomes;

    public bool IsUnlocked => string.IsNullOrWhiteSpace(unlockingFlag) || GlobalFlagsController.IsFlagSet(unlockingFlag);
}
