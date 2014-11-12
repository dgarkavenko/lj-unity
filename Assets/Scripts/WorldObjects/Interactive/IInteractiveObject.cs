using UnityEngine;
using System.Collections;

public interface IInteractiveObject
{
    void Interact(Interaction interaction, IInteractiveObject subject);
}
