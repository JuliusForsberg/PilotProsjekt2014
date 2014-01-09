using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		gameObject.tag = "Pickup";

		SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
		sphereCollider.radius = 3.0f;
		sphereCollider.isTrigger = true;
	}

	public Texture2D icon;
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter () {
		Player.SendMessage(
	}
}
