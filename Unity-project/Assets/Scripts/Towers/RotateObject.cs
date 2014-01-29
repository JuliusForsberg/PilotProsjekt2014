using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {
	public float rotationSpeed = 100.0f;
	
	void Update () {
		transform.Rotate (new Vector3(0,rotationSpeed* Time.deltaTime, 0));
	}
}
