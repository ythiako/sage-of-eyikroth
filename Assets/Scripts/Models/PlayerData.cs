using System.Globalization;
using UnityEngine;

public static class PlayerData
{
    public static string CurrentCulture
    {
        get => PlayerPrefs.GetString(nameof(CurrentCulture), "en-US");
        set => PlayerPrefs.SetString(nameof(CurrentCulture), value);
    }
}