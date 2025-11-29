using System.Collections.Generic;

public class TempPart
{
    public EPartType Type { get; set; }
    public List<string> Modifications { get; set; }

    public TempPart(EPartType type, List<string> modifications)
    {
        Type = type;
        Modifications = modifications ?? new List<string>();
    }
}
