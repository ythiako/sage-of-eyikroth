using System;
using System.Globalization;
using UnityEngine;

[System.Serializable]
public class LocalizedText
{
    [SerializeField] private string en;
    [SerializeField] private string tr;

    public string GetText(CultureInfo culture)
    {
        return culture.IetfLanguageTag switch
        {
            "en-US" => en,
            "tr-TR" => tr,
            _       => ""
        };
    }
}