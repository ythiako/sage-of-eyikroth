using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

public static class StageController
{
    public static Action StageFinished;
    
    private static int _index;
    private static int _conflictIndex;
    private static List<Conflict> _conflicts;
    
    public static void PrepareStage()
    {
        var conflicts = AssetsController.Instance.GetStage(_index);
        _conflictIndex = 0;
        PlayerData.StageIndex = _index;
        PlayerData.ConflictIndex = _conflictIndex;
    }

    public static void NextConflict()
    {
        if (_conflictIndex >= _conflicts.Count)
        {
            _index++;
            PrepareStage();
            StageFinished?.Invoke();
        }
        else
        {
            _conflictIndex++;
            PlayerData.ConflictIndex = _conflictIndex;
        }
    }

    public static void Reset()
    {
        _index = 0;
        _conflictIndex = 0;

        PlayerData.StageIndex = 0;
        PlayerData.ConflictIndex = 0;
    }

    public static Conflict GetCurrentConflict()
    {
        return _conflicts[_conflictIndex];
    }
}