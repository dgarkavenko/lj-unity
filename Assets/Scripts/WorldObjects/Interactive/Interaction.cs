using UnityEngine;
using System.Collections;

public class Interaction : ScriptableObject {

    public enum InteractionType
    {
        gunshot,
        chop,
        chainsaw,
        caboom,
        treehit,
        bite
    }

    public InteractionType type;
    public float value;
   
    
}
