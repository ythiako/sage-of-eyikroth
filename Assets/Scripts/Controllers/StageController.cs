using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

public static class StageController
{
    private static int _index;
    private static int _conflictIndex;
    private static List<Conflict> _conflicts;

    public static bool IsComplete => AssetsController.Instance.GetStage(_index) == null;

    public static void Initialize()
    {
        _conflicts = AssetsController.Instance.GetStage(_index);
    }

    public static void Load()
    {
        _index = PlayerData.StageIndex;
        _conflictIndex = PlayerData.ConflictIndex;
        _conflicts = AssetsController.Instance.GetStage(_index);
    }
    
    public static void PrepareStage()
    {
        _conflicts = AssetsController.Instance.GetStage(_index);
        _conflictIndex = 0;
        PlayerData.StageIndex = _index;
        PlayerData.ConflictIndex = _conflictIndex;
    }

    public static bool NextConflict()
    {
        if (_conflictIndex >= _conflicts.Count - 1)
        {
            _index++;
            PrepareStage();
            return true;
        }
        
        _conflictIndex++;
        PlayerData.ConflictIndex = _conflictIndex;
        return false;
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

    public static int GetCurrentStageNo()
    {
        return _index + 1;
    }
}