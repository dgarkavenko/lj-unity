using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class Interactive : MonoBehaviour {

	[System.Flags]
	public enum InteractionType
	{
		gunshot = (1 << 0),
		chop = (1 << 1),
		chainsaw = (1 << 2),
		caboom = (1 << 3),
		treehit = (1 << 4),
		bite = (1 << 5)
	}

	[SerializeField] [EnumFlagsAttribute] public InteractionType ActionMask;
	public event System.Action<IInteraction> InteractionEvent;

	public void Interact(IInteraction action){
		if (InteractionEvent != null && (action.InteractionType & ActionMask) != 0) InteractionEvent(action);
	}
}


public class EnumFlagsAttribute : PropertyAttribute
{
	public EnumFlagsAttribute() { }
}

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
	{
		_property.intValue = EditorGUI.MaskField( _position, _label, _property.intValue, _property.enumNames );
	}
}