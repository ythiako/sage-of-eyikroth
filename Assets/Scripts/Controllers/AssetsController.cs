using System.Collections.Generic;
using System.Linq;
using Behavoiurs;
using Models;
using UnityEngine;

public class AssetsController : MonoBehaviour
{
    private static AssetsController _instance;
    public static AssetsController Instance =>
        _instance ? _instance : FindObjectOfType<AssetsController>();
    
    [SerializeField] private List<Conflict> conflicts;
    [SerializeField] private SerializableDictionary<Faction, FactionRepresentativeBehaviour> factionRepresentatives;

    public List<Conflict> GetConflicts()
    {
        return conflicts;
    }

    public Conflict GetConflict(string id)
    {
        return conflicts.FirstOrDefault(c => c.id == id);
    }

    public FactionRepresentativeBehaviour GetFactionRepresentativePrefab(Faction faction)
    {
        if (factionRepresentatives.TryGetValue(faction, out var representative))
            return representative;
        return default;
    }
}