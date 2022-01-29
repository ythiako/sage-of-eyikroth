using System.Globalization;
using UnityEngine;

public static class PlayerData
{
    public static string CurrentCulture
    {
        get => PlayerPrefs.GetString(nameof(CurrentCulture), "en-US");
        set
        {
            PlayerPrefs.SetString(nameof(CurrentCulture), value);
            PlayerPrefs.Save();
        }
    }

    public static bool CanLoadGame
    {
        get => PlayerPrefs.GetInt(nameof(CanLoadGame), 0) == 1;
        set
        {
            PlayerPrefs.SetInt(nameof(CanLoadGame), value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    
    public static string Flags
    {
        get => PlayerPrefs.GetString(nameof(Flags), "");
        set
        {
            PlayerPrefs.SetString(nameof(Flags), value);
            PlayerPrefs.Save();
        }
    }

    public static string Standings
    {
        get => PlayerPrefs.GetString(nameof(Standings), "");
        set
        {
            PlayerPrefs.SetString(nameof(Standings), value);
            PlayerPrefs.Save();
        }
    }

    public static int StageIndex
    {
        get => PlayerPrefs.GetInt(nameof(StageIndex), 0);
        set
        {
            PlayerPrefs.SetInt(nameof(StageIndex), value);
            PlayerPrefs.Save();
        }
    }

    public static int ConflictIndex
    {
        get => PlayerPrefs.GetInt(nameof(ConflictIndex), 0);
        set
        {
            PlayerPrefs.SetInt(nameof(ConflictIndex), value);
            PlayerPrefs.Save();
        }
    }
}