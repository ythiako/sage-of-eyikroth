using UnityEngine.Serialization;

namespace Models
{
    [System.Serializable]
    public class DecisionOutcome
    {
        public int outcomeId;
        public SerializableDictionary<Faction, int> factionAffection;
    }
}