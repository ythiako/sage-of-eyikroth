using System.Collections.Generic;
using System.Linq;

public static class GlobalFlagsController
{
    private static List<string> _flags = new List<string>();
    
    public static void LoadFlagCollection()
    {
        _flags = PlayerData.Flags.Split(',').ToList();
    }

    public static void NewFlagCollection()
    {
        _flags.Clear();
        PlayerData.Flags = "";
    }

    public static void SetFlag(string flag)
    {
        if (_flags.Contains(flag)) return;
        
        _flags.Add(flag);
        PlayerData.Flags = _flags.Aggregate((prev, curr) => string.IsNullOrWhiteSpace(prev) ? curr : $"{prev},{curr}");
    }

    public static bool IsFlagSet(string flag)
    {
        return _flags.Contains(flag);
    }
}