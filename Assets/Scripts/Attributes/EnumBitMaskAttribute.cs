using UnityEngine;

public class EnumBitMaskAttribute : PropertyAttribute
{
    public System.Type propType;
    public EnumBitMaskAttribute(System.Type aType)
    {
        propType = aType;
    }
}
