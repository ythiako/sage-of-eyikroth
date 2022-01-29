using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

public class AssetsController : Singleton<AssetsController>
{
    [SerializeField] private List<List<Conflict>> stages;
    [SerializeField] private SerializableDictionary<Faction, FactionRepresentativeBehaviour> factionRepresentatives;

    public List<Conflict> GetStage(int index)
    {
        return stages[index];
    }

    public FactionRepresentativeBehaviour GetFactionRepresentativePrefab(Faction faction)
    {
        if (factionRepresentatives.TryGetValue(faction, out var representative))
            return representative;
        return default;
    }
}