using UnityEngine;
using System.Collections;

public enum resourceEnum { Rock, Wood };

public class Pickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		gameObject.tag = "Pickup";

	}

	public Texture2D icon;

    public resourceEnum resource;
	// Update is called once per frame
	void Update () {
	
	}
	
}
