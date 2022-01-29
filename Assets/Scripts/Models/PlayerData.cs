using System.Globalization;
using UnityEngine;

public static class PlayerData
{
    public static string CurrentCulture
    {
        get => PlayerPrefs.GetString(nameof(CurrentCulture), "tr-TR");
        set
        {
            PlayerPrefs.SetString(nameof(CurrentCulture), value);
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