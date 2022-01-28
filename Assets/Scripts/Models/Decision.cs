using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class Decision
{
    public string id;
    public bool unlocked;
    public LocalizedText text;
    public SerializableDictionary<Faction, int> outcomes;
}
