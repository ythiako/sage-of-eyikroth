using UnityEngine.Serialization;

namespace Models
{
    [System.Serializable]
    public class DecisionOutcome
    {
        public string outcomeId;
        public SerializableDictionary<Faction, int> factionAffection;
    }
}