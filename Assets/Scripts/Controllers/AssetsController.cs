using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

public class AssetsController : Singleton<AssetsController>
{
    [SerializeField] private List<Stage> stages;
    [SerializeField] private List<Conflict> randomConflicts;
    [SerializeField] private SerializableDictionary<Faction, FactionRepresentativeBehaviour> factionRepresentatives;
    [SerializeField] private SerializableDictionary<Faction, Sprite> factionLeaderSprites;

    public List<Conflict> GetStage(int index)
    {
        return stages[index].conflicts;
    }

    public Sprite GetLeaderSprite(Faction faction)
    {
        if (factionLeaderSprites.TryGetValue(faction, out Sprite sprite))
        {
            return sprite;
        }
        return null;
    }
    
    public FactionRepresentativeBehaviour GetFactionRepresentativePrefab(Faction faction)
    {
        if (factionRepresentatives.TryGetValue(faction, out var representative))
            return representative;
        return default;
    }

    
    [System.Serializable]
    public class Stage
    {
        public List<Conflict> conflicts;
    }
}