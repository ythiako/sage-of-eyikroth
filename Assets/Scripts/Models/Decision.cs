using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Decision
{
    public string id;
    public string text;
    public bool unlocked;
    public SerializableDictionary<Faction, int> outcomes;
}
