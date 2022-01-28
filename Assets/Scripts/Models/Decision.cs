using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class Decision
{
    public string id;
    public string unlockingFlag;
    public LocalizedText text;
    public SerializableDictionary<Faction, int> outcomes;

    public bool IsUnlocked => string.IsNullOrWhiteSpace(unlockingFlag) || GlobalFlagsController.IsFlagSet(unlockingFlag);
}
