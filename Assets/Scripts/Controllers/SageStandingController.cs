using System;
using System.Collections.Generic;
using UnityEngine;

public static class SageStandingController
{
    private static SerializableDictionary<Faction, int> _standings = new SerializableDictionary<Faction, int>();

    public static void StartNew()
    {
        var values = Enum.GetValues(typeof(Faction));

        foreach (var value in values)
        {
            _standings[(Faction) value] = 50;
        }
    }

    public static void Load()
    {
        var json = PlayerData.Standings;

        if (string.IsNullOrWhiteSpace(json))
        {
            StartNew();
            SaveStanding();
        }
    }

    public static void SetStanding(Faction faction, int standing)
    {
        _standings[faction] = standing;
    }

    public static void SaveStanding()
    {
        PlayerData.Standings = JsonUtility.ToJson(_standings);
    }
}