using System;
using System.Globalization;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class LocalizedText
{
    [SerializeField, LabelWidth(30), TextArea(1, 3)] private string en;
    [SerializeField, LabelWidth(30), TextArea(1, 3)] private string tr;

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