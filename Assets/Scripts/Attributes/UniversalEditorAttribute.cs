using UnityEngine;

public class UberDrawAttribute : PropertyAttribute
{
    
    public readonly string Data;
    public readonly bool WithLable;
    public readonly int Lines;
    public UberDrawAttribute(string data, bool withLable = true)
    {
        Data = data;
        WithLable = withLable;
        Lines = data.Split('\n').Length;
    }
}