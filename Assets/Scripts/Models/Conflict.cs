using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu]
    public class Conflict : ScriptableObject
    {
        [LabelWidth(30)] public string id;
        [LabelWidth(120)] public bool genericConflict;
        public List<DialogueLine> descriptionLines;
        
        [HorizontalGroup("Sides"), BoxGroup("Sides/Side A"), FoldoutGroup("Sides/Side A/Summary"), InlineProperty, HideLabel] public DialogueLine aSummary;
        [HorizontalGroup("Sides"), BoxGroup("Sides/Side A"), FoldoutGroup("Sides/Side A/Decision"), InlineProperty, HideLabel] public Decision aDecision;
        
        [HorizontalGroup("Sides"), BoxGroup("Sides/Side B"), FoldoutGroup("Sides/Side B/Summary"), InlineProperty, HideLabel] public DialogueLine bSummary;
        [HorizontalGroup("Sides"), BoxGroup("Sides/Side B"), FoldoutGroup("Sides/Side B/Decision"), InlineProperty, HideLabel] public Decision bDecision;
        
        public List<Decision> optionalDecisions;
        public SerializableDictionary<string, DialogueLines> outcomeDialogue;
    }

    [System.Serializable]
    public class DialogueLine
    {
        public Faction faction;
        
        [InlineProperty, HideLabel, BoxGroup("Line")]
        public LocalizedText line;
    }

    [System.Serializable]
    public class DialogueLines
    {
        [InlineProperty, HideLabel]
        public List<DialogueLine> lines;
    }
}