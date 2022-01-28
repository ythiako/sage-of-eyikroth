using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu]
    public class Conflict : ScriptableObject
    {
        public string id;
        public bool genericConflict;
        public List<DialogueLine> descriptionLines;
        public DialogueLine aSummary;
        public DialogueLine bSummary;
        public Decision aDecision;
        public Decision bDecision;
        public List<Decision> optionalDecisions;
        public SerializableDictionary<string, List<DialogueLine>> outcomeDialogue;
    }

    [System.Serializable]
    public class DialogueLine
    {
        public Faction faction;
        public LocalizedText line;
    }
}