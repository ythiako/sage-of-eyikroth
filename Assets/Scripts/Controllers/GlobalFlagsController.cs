using System.Collections.Generic;
using System.Linq;

public static class GlobalFlagsController
{
    private static List<string> _flags;
    
    public static void Initialize()
    {
        _flags = PlayerData.Flags.Split(',').ToList();
    }

    public static bool IsFlagSet(string flag)
    {
        return _flags.Contains(flag);
    }
}