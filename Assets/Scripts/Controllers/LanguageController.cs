using System.Globalization;
using UnityEngine;

public static class LanguageController
{
    public static CultureInfo GetCurrentCulture()
    {
        var cultureCode = PlayerData.CurrentCulture;

        if (string.IsNullOrWhiteSpace(cultureCode))
        {
            var currentCulture = CultureInfo.CurrentCulture;
            PlayerData.CurrentCulture = currentCulture.IetfLanguageTag;
            return currentCulture;
        }

        return CultureInfo.CreateSpecificCulture(cultureCode);
    }

    public static void SetCurrentCulture(string languageTag)
    {
        if (languageTag == "en-US" || languageTag == "tr-TR")
        {
            PlayerData.CurrentCulture = languageTag;
        }
        else
        {
            Debug.LogError($"Invalid language tag: {languageTag}");
        }
    }
}