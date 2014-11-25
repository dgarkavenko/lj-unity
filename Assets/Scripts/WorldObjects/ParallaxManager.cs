using UnityEngine;
using System;
using System.Collections;


public class ParallaxManager : MonoBehaviour
{
	[Serializable]
	public class Layer
	{
		public Transform container;
		public float distance;
		public Vector2 pivotPoint;
	};

	public CameraController cameraController;
	public Layer[] layers;
	

	void Update()
	{
		var cameraCenter = new Vector2(cameraController.transform.position.x, cameraController.transform.position.y);

		foreach (var layer in layers) {
			var layerShift = cameraCenter / (layer.distance + 1f);
			layer.container.position = new Vector3(layerShift.x, layer.container.position.y, layer.container.position.z);
		}
	}
}
