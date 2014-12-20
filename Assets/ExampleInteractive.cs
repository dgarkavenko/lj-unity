using UnityEngine;
using System.Collections;

public class ExampleInteractive : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var i = gameObject.GetComponent<Interactive> ();
		if (i != null)
			i.InteractionEvent += (IInteraction obj) => {
			Debug.Log(obj as ChopAction);
		};
	}
	

}
